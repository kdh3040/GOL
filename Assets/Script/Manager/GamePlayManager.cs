using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager _instance = null;
    public static GamePlayManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GamePlayManager>() as GamePlayManager;
            }
            return _instance;
        }
    }

    public int Score
    {
        get;
        private set;
    }
    private PlayerChar mPlayerChar = new PlayerChar();
    private bool mIsGamePause = false;
    private NoteSystem mNoteSystem = new NoteSystem();
    private DoorSystem mDoorSystem = new DoorSystem();
    private ItemSystem mItemSystem = new ItemSystem();
    private PageGameUI mGameUIPage;
    private Transform mNoteParentPos;

    public int[] mNormalitemArr = new int[2];
    public int mShielditem = 0;

    public float NoteSpeed
    {
        get
        {
            return mNoteSystem.NoteSpeed;
        }
    }
    public float AccumulateCreateNoteCount
    {
        get
        {
            return mNoteSystem.AccumulateCreateNoteCount;
        }
    }
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void Initialize(PlayScene scene)
    {
        mNoteSystem.Initialize(scene);
        mDoorSystem.Initialize(scene);
        mGameUIPage = scene.UIPage;
        mNoteParentPos = scene.NotesParentPos;
    }

    public void ResetGame()
    {
        SetGameNormalItemId(CommonData.ITEM_SLOT_INDEX.LEFT, PlayerData.Instance.GetItemSlotId(CommonData.ITEM_SLOT_INDEX.LEFT));
        SetGameNormalItemId(CommonData.ITEM_SLOT_INDEX.RIGHT, PlayerData.Instance.GetItemSlotId(CommonData.ITEM_SLOT_INDEX.RIGHT));
        mShielditem = PlayerData.Instance.mShielditem;

        StopAllCoroutines();
        Score = 0;
        mIsGamePause = false;
        mDoorSystem.ResetDoor();
        mGameUIPage.ResetUI();
        SkillManager.Instance.ResetGame();
    }
    public void GameExit()
    {
        ResetGame();
        mNoteSystem.GameExit();
    }
    public void GameStart()
    {
        ResetGame();
        mNoteSystem.GameStart();
        UseGameShieldItem();
        StartCoroutine(UpdateGamePlay());  
    }

    public void GameRestart()
    {
        ResetGame();
        mNoteSystem.GameRestart();
        StartCoroutine(UpdateGamePlay());
    }

    public void GamePause()
    {
       // mIsGamePause = true;
    }

    public void GameContinue()
    {
        mIsGamePause = false;
    }

    public void GameOver()
    {
        if (SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD,SkillManager.SKILL_CHECK_TYPE.TIME) != null)
            return;
        else if(SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD, SkillManager.SKILL_CHECK_TYPE.COUNT) != null)
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD, SkillManager.SKILL_CHECK_TYPE.COUNT) as GameSkill_Shield;

            if (skill.CharShield())
                return;
            // TODO 환웅 : 캐릭티 맞음
        }

        mGameUIPage.GameOver();
        mIsGamePause = true;
    }



    IEnumerator UpdateGamePlay()
    {
        while(true)
        {
            if (mIsGamePause)
            {
                yield return null;
                continue;
            }

            var time = Time.fixedDeltaTime;

            if (SkillManager.Instance.IsSkillEnable(SkillManager.SKILL_TYPE.SPEED_DOWN))
            {
                var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.SPEED_DOWN, SkillManager.SKILL_CHECK_TYPE.TIME) as GameSkill_SpeedDown;
                if (skill != null)
                {
                    mNoteSystem.NoteUpdate(skill.ConvertSpeed(time));
                }
            }
            else
            {
                mNoteSystem.NoteUpdate(time);
            }


            
            SkillManager.Instance.UpdateSkill(time);
            mGameUIPage.RefreshItemSkillUI();

            GetUserTouchEvent();


            yield return null;
        }
    }

    public Note CreateNote(string prefab)
    {
        // TODO 환웅 : 오브젝트 풀 추가 예정
        var obj = Instantiate(Resources.Load(prefab), mNoteParentPos) as GameObject;
        var note = obj.GetComponent<Note>();

        return note;
    }

    public void DeleteNote(Note note, bool score = true)
    {
        if(score)
        {
            if (note.NoteType == CommonData.NOTE_TYPE.NORMAL)
            {
                var noteNormal = note as NoteNormal;
                int defaultScore = DataManager.Instance.NoteDataDic[noteNormal.Id].Score;
                PlusScore(defaultScore);
            }
            else
            {
                var noteItem = note as NoteItem;
                PlusItem(noteItem.Id);
            }
        }
        

        DestroyImmediate(note.gameObject);
    }

    public void AddDeleteReadyNote(Note note)
    {
        mNoteSystem.NoteDeleteReady(note);
    }

    public Door CreateDoor(Transform pos)
    {
        var obj = Instantiate(Resources.Load("Prefab/Door"), pos) as GameObject;
        var door = obj.GetComponent<Door>();

        return door;
    }

    public void DeleteDoor(Door door)
    {
        DestroyImmediate(door.gameObject);
    }

    public void ClickDoor(Door door)
    {
        mNoteSystem.NoteDeleteCheck(door);
        // 라인타입
        PlayerChar.Instance.ActionDoorClose(door);
    }
    
    public void PlusScore(int score)
    {
        if (SkillManager.Instance.IsSkillEnable(SkillManager.SKILL_TYPE.SCORE_UP))
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.SCORE_UP, SkillManager.SKILL_CHECK_TYPE.TIME) as GameSkill_ScoreUP;
            if (skill != null)
            {
                Score += skill.ConvertNoteScore(score);
            }
        }
        else
            Score += score;

        mGameUIPage.RefreshUI();
    }

    public void PlusItem(int id)
    {
        var data = ItemManager.Instance.GetItemData(id);
        bool itemAdd = false;
        if (data.slot_type == CommonData.ITEM_SLOT_TYPE.NORMAL)
        {
            for (int i = 0; i < mNormalitemArr.Length; i++)
            {
                if (mNormalitemArr[i] == 0)
                {
                    mNormalitemArr[i] = id;
                    itemAdd = true;
                    break;
                }
            }

            if (itemAdd == false)
            {
                PlusScore(ConfigData.Instance.NOTE_ITEM_SCORE);
            }
            else
                mGameUIPage.RefreshItemUI();
        }
        else
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD, SkillManager.SKILL_CHECK_TYPE.COUNT);
            if(skill != null)
            {
                var shieldCount = skill.mValue1;
                if (shieldCount < ConfigData.Instance.MAX_USE_SHIELD_ITEM)
                {
                    mShielditem = id;
                    UseGameShieldItem();
                    itemAdd = true;
                }
            }
            

            if (itemAdd == false)
                PlusScore(ConfigData.Instance.NOTE_ITEM_SCORE);
        }
    }
    public void UseGameNormalItem(CommonData.ITEM_SLOT_INDEX index)
    {
        int itemId = mNormalitemArr[(int)index];
        if (itemId == 0)
            return;

        SetGameNormalItemId(index, 0);
        // TODO 환웅
        var skillName = ItemManager.Instance.GetItemSkill(itemId);
        var skill = SkillManager.Instance.AddUseSkill(skillName);

        mGameUIPage.UseItemSkill(itemId, skill);
    }

    public void UseGameShieldItem()
    {
        if (mShielditem == 0)
            return;

        var skillName = ItemManager.Instance.GetItemSkill(mShielditem);
        SkillManager.Instance.AddUseSkill(skillName);
        mShielditem = 0;
        mGameUIPage.RefreshShieldItemUI();
    }

    private void SetGameNormalItemId(CommonData.ITEM_SLOT_INDEX index, int id)
    {
        mNormalitemArr[(int)index] = id;
    }


    private void GetUserTouchEvent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var mClickDoor = hit.collider.gameObject.GetComponent<Door>();
                GamePlayManager.Instance.ClickDoor(mClickDoor);
                Debug.Log("Complete" + hit.collider.name);
            }
            else
            {
                Debug.Log("null");
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            GamePlayManager.Instance.ClickDoor(mDoorSystem.mDoorList[0]);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            GamePlayManager.Instance.ClickDoor(mDoorSystem.mDoorList[1]);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            GamePlayManager.Instance.ClickDoor(mDoorSystem.mDoorList[2]);
        }

        else if (Input.GetKey(KeyCode.Z))
        {
            GamePlayManager.Instance.ClickDoor(mDoorSystem.mDoorList[0]);
            GamePlayManager.Instance.ClickDoor(mDoorSystem.mDoorList[1]);
        }
        else if (Input.GetKey(KeyCode.X))
        {
            GamePlayManager.Instance.ClickDoor(mDoorSystem.mDoorList[1]);
            GamePlayManager.Instance.ClickDoor(mDoorSystem.mDoorList[2]);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            GamePlayManager.Instance.ClickDoor(mDoorSystem.mDoorList[0]);
            GamePlayManager.Instance.ClickDoor(mDoorSystem.mDoorList[2]);
        }

#if UNITY_ANDROID
        if(Input.touchCount >= 1)
        {
            for(int i = 0; i<Input.touchCount; i++)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var mClickDoor = hit.collider.gameObject.GetComponent<Door>();
                    GamePlayManager.Instance.ClickDoor(mClickDoor);
                    Debug.Log("Complete" + hit.collider.name);
                }                
            }
        }

#elif UNITY_EDITOR

#endif


    }
}

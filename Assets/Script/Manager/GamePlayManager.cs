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
    private PlayerChar MainChar = null;
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
        SetGameNormalItemId(CommonData.ITEM_SLOT_INDEX.LEFT, GManager.Instance.mPlayerData.GetItemSlotId(CommonData.ITEM_SLOT_INDEX.LEFT));
        SetGameNormalItemId(CommonData.ITEM_SLOT_INDEX.RIGHT, GManager.Instance.mPlayerData.GetItemSlotId(CommonData.ITEM_SLOT_INDEX.RIGHT));
        mShielditem = GManager.Instance.mPlayerData.mShielditem;

        StopAllCoroutines();
        Score = 0;
        mIsGamePause = false;
        mNoteSystem.ResetNote();
        mDoorSystem.ResetDoor();
        mGameUIPage.ResetUI();
        SkillManager.Instance.ResetGame();
    }
    public void GameExit()
    {
        ResetGame();
    }
    public void GameStart()
    { 
        ResetGame();
        UseGameShieldItem();
        StartCoroutine(UpdateGamePlay());
    }

    public void GameRestart()
    {
        ResetGame();
        StartCoroutine(UpdateGamePlay());
    }

    public void GamePause()
    {
        mIsGamePause = true;
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

            var time = Time.deltaTime;
            mNoteSystem.NoteUpdate(time);
            SkillManager.Instance.UpdateSkill(time);
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
        if (score)
        {
            if(note.NoteType == CommonData.NOTE_TYPE.NORMAL)
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
        mNoteSystem.AddDeleteReadyNote(note);
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
        mNoteSystem.DeleteCheckNote(door);
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
        var data = DataManager.Instance.ItemDataDic[id];
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
        }
        else
        {
            var shieldCount = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD, SkillManager.SKILL_CHECK_TYPE.COUNT).mValue1;
            if(shieldCount < ConfigData.Instance.MAX_USE_SHIELD_ITEM)
            {
                mShielditem = id;
                UseGameShieldItem();
                itemAdd = true;
            }
        }

        if(itemAdd == false)
        {
            PlusScore(ConfigData.Instance.NOTE_ITEM_SCORE);
        }
        else
            mGameUIPage.RefreshItemUI();
    }
    public void UseGameNormalItem(CommonData.ITEM_SLOT_INDEX index)
    {
        int itemId = mNormalitemArr[(int)index];
        if (itemId == 0)
            return;

        SetGameNormalItemId(index, 0);
        // TODO 환웅
        var itemData = DataManager.Instance.ItemDataDic[itemId];
        SkillManager.Instance.AddUseSkill(itemData.skill);
    }

    public void UseGameShieldItem()
    {
        if (mShielditem == 0)
            return;

        var itemData = DataManager.Instance.ItemDataDic[mShielditem];
        SkillManager.Instance.AddUseSkill(itemData.skill);
        mShielditem = 0;
    }

    private void SetGameNormalItemId(CommonData.ITEM_SLOT_INDEX index, int id)
    {
        mNormalitemArr[(int)index] = id;
    }
}

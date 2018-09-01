using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

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

    public class GamePlayItem
    {
        public bool IngameGet;
        public int ItemId;

        public GamePlayItem(bool ingameGet, int itemId)
        {
            IngameGet = ingameGet;
            ItemId = itemId;
        }
    }

    public int Score
    {
        get;
        private set;
    }
    private PlayerChar mPlayerChar = null;
    private bool IsGamePause = false;
    private NoteSystem mNoteSystem = new NoteSystem();
    public DoorSystem mDoorSystem = new DoorSystem();
    private PageGameUI mGameUIPage;
    public GameObject NoteDeleteObj;

    private AudioSource mAudio;
    public AudioClip[] mClip = new AudioClip[3];

    public int UseItemId = 0;
    public bool FirstStart = true;

    public float NoteSpeed
    {
        get
        {
            return mNoteSystem.NoteSpeed;
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
        mPlayerChar = scene.PlayerCharObj;
        NoteDeleteObj = scene.NoteDeleteObj;
        mPlayerChar.Initialize();
        
        mAudio = scene.gameObject.AddComponent<AudioSource>();

    }

    public void ResetGame()
    {
    
        UseItemId = PlayerData.Instance.GetUseItemId();
        PlayerData.Instance.SetUseItemId(0);
        StopAllCoroutines();
        Score = 0;
        IsGamePause = false;
        FirstStart = false;
        mGameUIPage.ResetUI();
        SkillManager.Instance.ResetGame();
    }
    public void GameExit()
    {
        ResetGame();
        mNoteSystem.GameExit();
        mDoorSystem.GameExit();
    }
    public void GameStart()
    {
    
        FirstStart = true;
        ResetGame();
        SkillManager.Instance.UseCharSkill(PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.CHAR));
        SkillManager.Instance.UseSkinSlotSkill();
        UseGameShieldItem();
        mGameUIPage.RefreshShieldItemUI();
        mNoteSystem.GameStart();
        mDoorSystem.GameStart();
        StartCoroutine(UpdateGamePlay());
    }

    public void GameRestart()
    {
        ResetGame();
        SkillManager.Instance.UseCharSkill(PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.CHAR));
        SkillManager.Instance.UseSkinSlotSkill();
        mGameUIPage.RefreshShieldItemUI();
        mNoteSystem.GameRestart();
        mDoorSystem.GameRestart();
        StartCoroutine(UpdateGamePlay());
    }

    public void GameRevival()
    {
        mDoorSystem.GameRevival();
        mNoteSystem.GameRevival();
        mGameUIPage.GameResume();
    }

    public void GamePause()
    {
       IsGamePause = true;
    }

    public void GameResumeCountStart()
    {
        mGameUIPage.GameResume();
    }
    public void GameResume()
    {
        IsGamePause = false;
    }

    public bool IsGameOver(CommonData.NOTE_LINE line)
    {
        if (SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_TIME) != null)
            return false;
        else if (SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_COUNT) != null)
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_COUNT) as GameSkill_DamageShieldCount;

            if (skill.CharShield())
            {
                mGameUIPage.RefreshShieldItemUI();
                mDoorSystem.ShowSkillEffect_Shield(line);
                return false;
            }
                
        }
        else if (SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.RESURRECTION) != null)
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.RESURRECTION) as GameSkill_Resurrection;

            if (skill.IsResurrection())
                return false;
        }

        return true;
    }

    public void GameOver()
    {
        mGameUIPage.GameOver();
        IsGamePause = true;
    }

    IEnumerator UpdateGamePlay()
    {
        while(true)
        {
            if (IsGamePause)
            {
                yield return null;
                continue;
            }

            var time = Time.fixedDeltaTime;

            if (SkillManager.Instance.IsSkillEnable(SkillManager.SKILL_TYPE.SPEED_DOWN))
            {
                var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.SPEED_DOWN) as GameSkill_SpeedDown;
                if (skill != null)
                {
                    mNoteSystem.NoteUpdate(time, skill.ConvertSpeed(time));
                }
            }
            else
            {
                mNoteSystem.NoteUpdate(time, time);
            }

            SkillManager.Instance.UpdateSkill(time);
            mGameUIPage.RefreshItemSkillUI();
            mGameUIPage.RefreshCharMsgUI(time);
            GetUserTouchEvent();
            yield return null;
        }
    }

    public void ClickDoor(Door door)
    {
        mNoteSystem.NoteDeleteCheck(door);
        // 라인타입
        mPlayerChar.ActionDoorClose(door);

        SetDoorState(door.NoteLineType, Door.DOOR_STATE.CLOSE);

        PlayDoorSound(door.NoteLineType);
    }
    
    public void PlusScore(int score)
    {
        if (SkillManager.Instance.IsSkillEnable(SkillManager.SKILL_TYPE.SCORE_UP))
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.SCORE_UP) as GameSkill_ScoreUP;
            if (skill != null)
            {
                Score += skill.ConvertNoteScore(score);
            }
        }
        else
            Score += score;

        mGameUIPage.RefreshUI();
        PlayGetItemSound();
    }

    public void PlusItem(int id)
    {
        var data = ItemManager.Instance.GetItemData(id);
        bool itemAdd = false;
        if (data.slot_type == CommonData.ITEM_SLOT_TYPE.NORMAL)
        {
            if (UseItemId != 0)
            {
                PlusScore(ConfigData.Instance.NOTE_ITEM_SCORE);
            }
            else
            {
                UseItemId = id;
                mGameUIPage.RefreshItemUI();
            }
        }
        else
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_COUNT);
            if (skill != null)
            {
                var shieldCount = skill.mCount;
                if (shieldCount < ConfigData.Instance.MAX_USE_SHIELD_ITEM)
                {
                    UseItemId = id;
                    UseGameShieldItem();
                    itemAdd = true;
                }
            }
            else
            {
                UseItemId = id;
                UseGameShieldItem();
                itemAdd = true;
            }
            

            if (itemAdd == false)
                PlusScore(ConfigData.Instance.NOTE_ITEM_SCORE);
        }

        PlayGetItemSound();
    }

    public void PlayGetItemSound()
    {
        mAudio.clip = mClip[0];
        mAudio.Play();
    }

    public void UseGameNormalItem()
    {
        if (UseItemId == 0)
            return;

        var itemData = DataManager.Instance.ItemDataDic[UseItemId];
        var skill = SkillManager.Instance.UseItemSkill(itemData.id);
        mGameUIPage.UseItemSkill(itemData.id, skill);
        mDoorSystem.StartSkillEffect(skill);

        UseItemId = 0;
        mGameUIPage.RefreshItemUI();
        PlayUseItemSound();
    }

    public void PlayUseItemSound()
    {
        mAudio.clip = mClip[2];
        mAudio.Play();
    }

    public void UseGameShieldItem()
    {
        if (UseItemId == 0)
            return;
        var itemData = DataManager.Instance.ItemDataDic[UseItemId];
        if (itemData.slot_type == CommonData.ITEM_SLOT_TYPE.SHIELD)
        {
            SkillManager.Instance.UseItemSkill(UseItemId);
            UseItemId = 0;
            mGameUIPage.RefreshShieldItemUI();
            mGameUIPage.RefreshItemUI();
        }
    }

    public void EndSkill(GameSkill skill)
    {
        mDoorSystem.EndSkillEffect(skill);
    }

    public void SetDoorState(CommonData.NOTE_LINE line, Door.DOOR_STATE state)
    {
        mDoorSystem.SetDoorState(line, state);
    }

    public void PlayDoorSound(CommonData.NOTE_LINE line)
    {
        mDoorSystem.PlaySound(line);
    } 

    public void DeleteNoteAni(Note note)
    {
        var obj = Instantiate(Resources.Load("Prefab/NoteDeleteAni"), NoteDeleteObj.transform) as GameObject;
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        //sprite.gameObject.transform.SetParent(NoteDeleteObj.transform);
        obj.gameObject.transform.localPosition = note.transform.localPosition;

        switch (note.NoteType)
        {
            case CommonData.NOTE_TYPE.NORMAL:
                var NoteData = DataManager.Instance.NoteDataDic[note.NoteId];
                sprite.sprite = (Sprite)Resources.Load(NoteData.img, typeof(Sprite));
                break;
            case CommonData.NOTE_TYPE.ITEM:
                var ItemData = ItemManager.Instance.GetItemData(note.NoteId);
                sprite.sprite = (Sprite)Resources.Load(ItemData.icon, typeof(Sprite));
                break;
            default:
                break;
        }

        Animator ani = obj.GetComponent<Animator>();
        switch (note.NoteLineType)
        {
            case CommonData.NOTE_LINE.INDEX_1:
                ani.SetTrigger("LEFT");
                StartCoroutine(Co_DeleteNoteAni(obj, true));
                break;
            case CommonData.NOTE_LINE.INDEX_2:
                if(Random.Range(0, 2) == 1)
                {
                    ani.SetTrigger("LEFT");
                    StartCoroutine(Co_DeleteNoteAni(obj, true));
                }
                else
                {
                    ani.SetTrigger("RIGHT");
                    StartCoroutine(Co_DeleteNoteAni(obj, false));
                }
                break;
            case CommonData.NOTE_LINE.INDEX_3:
                ani.SetTrigger("RIGHT");
                StartCoroutine(Co_DeleteNoteAni(obj, false));
                break;
            default:
                break;
        }
        
    }

    IEnumerator Co_DeleteNoteAni(GameObject obj, bool left)
    {
        float time = 1;
        float saveTime = 0;
        while(saveTime < time)
        {
            saveTime += Time.fixedDeltaTime;
            if(left)
                obj.transform.localPosition = new Vector3(obj.transform.localPosition.x - 0.1f, obj.transform.localPosition.y + 0.1f, obj.transform.localPosition.z);
            else
                obj.transform.localPosition = new Vector3(obj.transform.localPosition.x + 0.1f, obj.transform.localPosition.y + 0.1f, obj.transform.localPosition.z);
            yield return null;
        }
        DestroyImmediate(obj);
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
                if(mClickDoor != null)
                {
                    GamePlayManager.Instance.ClickDoor(mClickDoor);
                    Debug.Log("Complete" + hit.collider.name);
                }
                    
            }
            else
            {
                Debug.Log("null");
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            GamePlayManager.Instance.ClickDoor(mDoorSystem.DoorList[0]);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            GamePlayManager.Instance.ClickDoor(mDoorSystem.DoorList[1]);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            GamePlayManager.Instance.ClickDoor(mDoorSystem.DoorList[2]);
        }

        else if (Input.GetKey(KeyCode.Z))
        {
            GamePlayManager.Instance.ClickDoor(mDoorSystem.DoorList[0]);
            GamePlayManager.Instance.ClickDoor(mDoorSystem.DoorList[1]);
        }
        else if (Input.GetKey(KeyCode.X))
        {
            GamePlayManager.Instance.ClickDoor(mDoorSystem.DoorList[1]);
            GamePlayManager.Instance.ClickDoor(mDoorSystem.DoorList[2]);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            GamePlayManager.Instance.ClickDoor(mDoorSystem.DoorList[0]);
            GamePlayManager.Instance.ClickDoor(mDoorSystem.DoorList[2]);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            mNoteSystem.NoteSpeed += 0.1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            mNoteSystem.NoteSpeed -= 0.1f;
        }

#if UNITY_ANDROID
        if (Input.touchCount >= 1)
        {
          //  for(int i = 0; i<Input.touchCount; i++)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if(Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        var mClickDoor = hit.collider.gameObject.GetComponent<Door>();
                        GamePlayManager.Instance.ClickDoor(mClickDoor);
                        Debug.Log("Complete" + hit.collider.name);
                    }
                    
                }                
            }
        }

#elif UNITY_EDITOR

#endif


    }
}

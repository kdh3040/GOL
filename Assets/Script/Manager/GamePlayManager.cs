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
    private bool mIsGamePause = false;
    private NoteSystem mNoteSystem = new NoteSystem();
    private DoorSystem mDoorSystem = new DoorSystem();
    private PageGameUI mGameUIPage;
    private Admob mAdmob = new Admob();

    public Dictionary<CommonData.ITEM_SLOT_INDEX, GamePlayItem> ItemDic = new Dictionary<CommonData.ITEM_SLOT_INDEX, GamePlayItem>();
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
        mPlayerChar.Initialize();
        mAdmob.Init();        
    }

    public void ResetGame()
    {
        mAdmob.HideAd();

        if (FirstStart)
        {
            ItemDic.Clear();
            FirstSetItem(CommonData.ITEM_SLOT_INDEX.LEFT);
            FirstSetItem(CommonData.ITEM_SLOT_INDEX.RIGHT);
            FirstSetItem(CommonData.ITEM_SLOT_INDEX.SHIELD);
        }
        else
        {
            IngameGetSetItem(CommonData.ITEM_SLOT_INDEX.LEFT, 0);
            IngameGetSetItem(CommonData.ITEM_SLOT_INDEX.RIGHT, 0);
            IngameGetSetItem(CommonData.ITEM_SLOT_INDEX.SHIELD, 0);
        }

        StopAllCoroutines();
        Score = 0;
        mIsGamePause = false;
        FirstStart = false;
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
    
        FirstStart = true;
        ResetGame();
        SkillManager.Instance.UseCharSkill(PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.CHAR));
        SkillManager.Instance.UseSkinSlotSkill();
        UseGameShieldItem();
        mGameUIPage.RefreshShieldItemUI();
        mNoteSystem.GameStart();
        StartCoroutine(UpdateGamePlay());
    }

    public void GameRestart()
    {
        ResetGame();
        SkillManager.Instance.UseCharSkill(PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.CHAR));
        SkillManager.Instance.UseSkinSlotSkill();
        mGameUIPage.RefreshShieldItemUI();
        mNoteSystem.GameRestart();
        StartCoroutine(UpdateGamePlay());
    }

    public void GameRevival()
    {
        ResetGame();
        SkillManager.Instance.UseCharSkill(PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.CHAR));
        SkillManager.Instance.UseSkinSlotSkill();
        mGameUIPage.RefreshShieldItemUI();
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

    public bool IsGameOver(CommonData.NOTE_LINE line)
    {
        if (SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_TIME) != null)
            return false;
        else if (SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_COUNT) != null)
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_COUNT) as GameSkill_DamageShieldCount;

            if (skill.CharShield())
            {
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

        return false;
    }

    public void GameOver(int gameOverNoteId)
    {
        mGameUIPage.GameOver(gameOverNoteId);
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
                var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.SPEED_DOWN) as GameSkill_SpeedDown;
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

    public void ClickDoor(Door door)
    {
        mNoteSystem.NoteDeleteCheck(door);
        // 라인타입
        mPlayerChar.ActionDoorClose(door);

        SetDoorState(door.NoteLineType, 2);
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
    }

    public void PlusItem(int id)
    {
        var data = ItemManager.Instance.GetItemData(id);
        bool itemAdd = false;
        if (data.slot_type == CommonData.ITEM_SLOT_TYPE.NORMAL)
        {
            itemAdd = AddEmptyNormalItem(id);

            if (itemAdd == false)
            {
                PlusScore(ConfigData.Instance.NOTE_ITEM_SCORE);
            }
            else
                mGameUIPage.RefreshItemUI();
        }
        else
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_COUNT);
            if(skill != null)
            {
                var shieldCount = skill.mCount;
                if (shieldCount < ConfigData.Instance.MAX_USE_SHIELD_ITEM)
                {
                    IngameGetSetItem(CommonData.ITEM_SLOT_INDEX.SHIELD, id);
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
        var data = ItemDic[index];
        if (data.ItemId == 0)
            return;

        if(data.IngameGet == false)
        {
            PlayerData.Instance.MinusItem_Count(data.ItemId);
        }
        var skill = SkillManager.Instance.UseItemSkill(data.ItemId);
        mGameUIPage.UseItemSkill(data.ItemId, skill);
        mDoorSystem.StartSkillEffect(skill);

        IngameGetSetItem(index, 0);
    }

    public void UseGameShieldItem()
    {
        var data = ItemDic[CommonData.ITEM_SLOT_INDEX.SHIELD];
        if (data.ItemId == 0)
            return;

        if (data.IngameGet == false)
        {
            PlayerData.Instance.MinusItem_Count(data.ItemId);
        }
        SkillManager.Instance.UseItemSkill(GetItemId(CommonData.ITEM_SLOT_INDEX.SHIELD));
        mGameUIPage.RefreshShieldItemUI();

        IngameGetSetItem(CommonData.ITEM_SLOT_INDEX.SHIELD, 0);
    }

    public void EndSkill(GameSkill skill)
    {
        mDoorSystem.EndSkillEffect(skill);
    }

    public void SetDoorState(CommonData.NOTE_LINE line, int DoorState)
    {
        mDoorSystem.SetDoorState(line, DoorState);
    }

    public int ConvertScoreToCoin()
    {
        return Score / 10;
    }

    public int GetItemId(CommonData.ITEM_SLOT_INDEX type)
    {
        if (ItemDic.ContainsKey(type) == false)
            return 0;

        return ItemDic[type].ItemId;
    }
    public void FirstSetItem(CommonData.ITEM_SLOT_INDEX type)
    {
        var id = PlayerData.Instance.GetItemSlotId(type);
        if (id != 0)
            ItemDic.Add(type, new GamePlayItem(false, id));
        else
            ItemDic.Add(type, new GamePlayItem(false, id));
    }
    public bool IngameGetSetItem(CommonData.ITEM_SLOT_INDEX type, int id)
    {
        if(id == 0)
        {
            ItemDic[type].IngameGet = true;
            ItemDic[type].ItemId = 0;
            return true;
        }
        else if(ItemDic[type].ItemId == 0)
        {
            ItemDic[type].IngameGet = true;
            ItemDic[type].ItemId = id;
            return true;
        }

        return false;
    }

    public bool AddEmptyNormalItem(int id)
    {
        for (int i = (int)CommonData.ITEM_SLOT_INDEX.LEFT; i < (int)CommonData.ITEM_SLOT_INDEX.SHIELD; i++)
        {
            if (IngameGetSetItem((CommonData.ITEM_SLOT_INDEX) i, id))
                return true;
        }

        return false;
    }


    public void HideAd()
    {
        mAdmob.HideAd();
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
            mDoorSystem.DoorList[0].SetEffect("INVINCIBILITY");
            GamePlayManager.Instance.ClickDoor(mDoorSystem.DoorList[0]);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            mDoorSystem.DoorList[0].SetEffect("SHIELD");
            GamePlayManager.Instance.ClickDoor(mDoorSystem.DoorList[1]);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            mDoorSystem.DoorList[0].SetEffect("IDLE");
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

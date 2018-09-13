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
    [System.NonSerialized]
    public bool IsGamePause = false;
    private NoteSystem mNoteSystem = new NoteSystem();
    [System.NonSerialized]
    public DoorSystem mDoorSystem = new DoorSystem();
    private PageGameUI mGameUIPage;
    [System.NonSerialized]
    public GameObject NoteDeleteObj;

    public int UseItemId = 0;

    private GameObject DDongViewObj;
    private List<GameObject> DDongViewList = new List<GameObject>();
    private float DDongViewTimeSave;
    private int DDongViewPosChangeIndex = 0;

    private GameObject InGameEffect_Slow;
    private GameObject InGameEffect_Double;
    private GameObject InGameEffect_Revive;
    
    private GameObject InGameEffect_Start;
    private bool FirstStart = true;

    private bool Click = false;
    private CommonData.NOTE_LINE ClickLine = CommonData.NOTE_LINE.INDEX_1;
    [System.NonSerialized]
    public int ContinueCount = 0;
    public bool InGame = false;

    public float NoteSpeed
    {
        get
        {
            return mNoteSystem.NoteSpeed;
        }
    }

    public float PlayTime
    {
        get
        {
            return mNoteSystem.PlayTime;
        }
    }
    void Start()
    {
        DontDestroyOnLoad(this);
        AudioListener.pause =! PlayerData.Instance.GetSoundSetting();
    }

    public void Initialize(PlayScene scene)
    {
        mNoteSystem.Initialize(scene);
        mDoorSystem.Initialize(scene);
        mGameUIPage = scene.UIPage;
        mPlayerChar = scene.PlayerCharObj;
        NoteDeleteObj = scene.NoteDeleteObj;
        DDongViewObj = scene.DDongViewObj;
        mPlayerChar.Initialize();

        InGameEffect_Slow = scene.InGameEffect_Slow;
        InGameEffect_Double = scene.InGameEffect_Double;
        InGameEffect_Revive = scene.InGameEffect_Revive;

        InGameEffect_Start = scene.InGameEffect_Start;

        AdManager.Instance.RequestInterstitialAd();
        AdManager.Instance.RequestRewardBasedVideo();

      
    }

    public void ResetGame()
    {
        InGame = true;
        Click = false;
        StopAllCoroutines();
        Score = 0;
        IsGamePause = false;
        mGameUIPage.ResetUI();
        SkillManager.Instance.ResetGame();

        for (int i = 0; i < DDongViewList.Count; i++)
        {
            DestroyImmediate(DDongViewList[i]);
        }
        DDongViewList.Clear();

        DDongViewTimeSave = CommonData.DDONG_VIEW_INTERVAL;
        DDongViewPosChangeIndex = 0;
    }
    public void GameExit()
    {
        ResetGame();
        InGame = false;
        mNoteSystem.GameExit();
        mDoorSystem.GameExit();
    }
    public void GameStart()
    {
        ContinueCount = CommonData.GAME_CONTINUE_MAX_COUNT;
        FirstStart = true;
        ResetGame();
        //InGameEffect_Start.SetActive(true);
        Animator ani = InGameEffect_Start.GetComponent<Animator>();
        ani.SetTrigger("Start");

        SkillManager.Instance.UseSkinSlotSkill();
        mGameUIPage.RefreshShieldItemUI();
        mGameUIPage.RefreshItemSkillUI();
        mNoteSystem.GameStart();
        mDoorSystem.GameStart();
        StartCoroutine(UpdateGamePlay());
    }


    public void GameRevival(bool immediately = false)
    {
        mDoorSystem.GameRevival();
        mNoteSystem.GameRevival();

        if(immediately == false)
        {
            IsGamePause = true;
            mGameUIPage.GameResume();
            SkillManager.Instance.ResetGame();
            SkillManager.Instance.UseSkinSlotSkill();
        }

        InGameEffect_Revive.SetActive(true);
        StartCoroutine(Co_GameResume());
    }
    public IEnumerator Co_GameResume()
    {
        yield return new WaitForSecondsRealtime(1f);
        InGameEffect_Revive.SetActive(false);
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
        InGameEffect_Revive.SetActive(false);
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
                SetDoorState(line, Door.DOOR_STATE.OPEN);
                return false;
            }
                
        }
        else if (SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.RESURRECTION) != null)
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.RESURRECTION) as GameSkill_Resurrection;

            if (skill.IsResurrection())
            {
                GameRevival(true);
                return false;
            }
                
        }

        return true;
    }

    public bool IsAutoPlay()
    {
        if (SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_TIME) != null)
            return true;

        return false;
    }

    public void GameOver(Note note)
    {
        mGameUIPage.GameOver();
        IsGamePause = true;
        if(PlayerData.Instance.GetVibrationSetting())
        {
            Handheld.Vibrate();
        }        
        AdManager.Instance.ShowInterstitialAd();
        StartCoroutine(Co_GameOver(note));
    }

    public IEnumerator Co_GameOver(Note note)
    {
        yield return new WaitForSecondsRealtime(1f);

        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_END, new PopupGameEnd.PopupData(note.NoteId));
    }

    IEnumerator UpdateGamePlay()
    {
        while(true)
        {
            if (FirstStart)
            {
                yield return new WaitForSecondsRealtime(3.5f);                
                FirstStart = false;
                //InGameEffect_Start.SetActive(false);
            }
            if (IsGamePause)
            {
                yield return null;
                continue;
            }

            var time = Time.deltaTime;

            if(Click)
            {
                var door = mDoorSystem.DoorList[(int)ClickLine];
                var delete = mNoteSystem.NoteDeleteCheck(door);
                mPlayerChar.ActionDoorClose(door);
                SetDoorState(door.NoteLineType, Door.DOOR_STATE.CLOSE, delete);
                PlayDoorSound(door.NoteLineType);
            }

            Click = false;
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
            ShowDDong(time);

          
            yield return null;
        }
    }
    public void ClickDoorButton(int index)
    {
        ClickDoor(mDoorSystem.DoorList[index]);
    }
    public void ClickDoor(Door door)
    {
        if (FirstStart)
            return;

        if (IsGamePause)
            return;

        Click = true;
        ClickLine = door.NoteLineType;

        //    mNoteSystem.NoteDeleteCheck(door);
        //// 라인타입
        //mPlayerChar.ActionDoorClose(door);

        //SetDoorState(door.NoteLineType, Door.DOOR_STATE.CLOSE);

        //PlayDoorSound(door.NoteLineType);
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
        //PlayGetItemSound();
    }

    public void PlusItem(int id)
    {
        if (id == 0)
            return;

        var itemData = DataManager.Instance.ItemDataDic[id];
        if(itemData.slot_type == CommonData.ITEM_SLOT_TYPE.NORMAL)
        {
            var skill = SkillManager.Instance.UseItemSkill(itemData.id);
            mGameUIPage.UseItemSkill(itemData.id, skill);
            mDoorSystem.StartSkillEffect(skill);
            ViewInGameEffect(skill);
        }
        else
        {
            SkillManager.Instance.UseItemSkill(id);
            mGameUIPage.RefreshShieldItemUI();
        }

        //PlayUseItemSound();
    }
    
    public void EndSkill(GameSkill skill)
    {
        mDoorSystem.EndSkillEffect(skill);
        EndInGameEffect(skill);
    }


    public void ViewInGameEffect(GameSkill skill)
    {
        if (skill.mSkillType == SkillManager.SKILL_TYPE.SPEED_DOWN)
        {
            InGameEffect_Slow.SetActive(true);
        }
        else if (skill.mSkillType == SkillManager.SKILL_TYPE.SCORE_UP)
        {
            InGameEffect_Double.SetActive(true);
        }
    }

    public void EndInGameEffect(GameSkill skill)
    {
        if (skill.mSkillType == SkillManager.SKILL_TYPE.SPEED_DOWN)
        {
            InGameEffect_Slow.SetActive(false);
        }
        else if (skill.mSkillType == SkillManager.SKILL_TYPE.SCORE_UP)
        {
            InGameEffect_Double.SetActive(false);
        }
    }

    public void SetDoorState(CommonData.NOTE_LINE line, Door.DOOR_STATE state, bool closeMsg = true)
    {
        mDoorSystem.SetDoorState(line, state, closeMsg);
    }

    public void PlayDoorSound(CommonData.NOTE_LINE line)
    {
        if (PlayerData.Instance.GetSoundSetting())
        {
            mDoorSystem.PlaySound(line);
        }
    } 

    public void DeleteNoteAni(Note note)
    {
        var obj = Instantiate(Resources.Load("Prefab/NoteDeleteAni"), NoteDeleteObj.transform) as GameObject;
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        //sprite.gameObject.transform.SetParent(NoteDeleteObj.transform);
        obj.gameObject.transform.localPosition = new Vector3(note.transform.position.x, note.transform.position.y, 0);

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
        ani.Rebind();
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
            saveTime += Time.deltaTime;
            if(left)
                obj.transform.localPosition = new Vector3(obj.transform.localPosition.x - 0.1f, obj.transform.localPosition.y + 0.2f, obj.transform.localPosition.z);
            else
                obj.transform.localPosition = new Vector3(obj.transform.localPosition.x + 0.1f, obj.transform.localPosition.y + 0.2f, obj.transform.localPosition.z);
            yield return null;
        }
        DestroyImmediate(obj);
    }

    private void ShowDDong(float time)
    {
        DDongViewTimeSave -= time;
        if(DDongViewTimeSave < 0)
        {
            DDongViewTimeSave = CommonData.DDONG_VIEW_INTERVAL;

            if (DDongViewList.Count <= 10)
            {
                var obj = Instantiate(Resources.Load("Prefab/IngameDDongIcon"), DDongViewObj.transform) as GameObject;
                obj.gameObject.transform.localPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, -9f), -8.5f);
                DDongViewList.Add(obj);
            }
            else
            {
                DDongViewList[DDongViewPosChangeIndex].transform.localPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-4.5f, -7f), 0);
                DDongViewPosChangeIndex++;

                if (DDongViewPosChangeIndex >= 10)
                    DDongViewPosChangeIndex = 0;
            }
        }
    }

    private void GetUserTouchEvent()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        var mClickDoor = hit.collider.gameObject.GetComponent<Door>();
        //        if(mClickDoor != null)
        //        {
        //            GamePlayManager.Instance.ClickDoor(mClickDoor);
        //            Debug.Log("Complete" + hit.collider.name);
        //        }
                    
        //    }
        //    else
        //    {
        //        Debug.Log("null");
        //    }
        //}

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

    void OnApplicationPause(bool pause)
    {
        if (InGame == false)
            return;

        if (FirstStart)
            return;

        if (IsGamePause)
            return;


        if (pause)
        {
            GamePause();
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_PAUSE);
        }
    }

    void Update()
    {
        if (InGame == false)
            return;

        if (FirstStart)
            return;

        


        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Home) || Input.GetKeyUp(KeyCode.Menu))
            {
                if (PopupManager.Instance.CurrentPopupType() == PopupManager.POPUP_TYPE.GAME_PAUSE && Input.GetKeyUp(KeyCode.Escape))
                {
                    GameResumeCountStart();
                    PopupManager.Instance.DismissPopup();
                }
                else
                {
                    if (IsGamePause)
                        return;

                    GamePause();
                    PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_PAUSE);
                }
            }
        }
    }
}

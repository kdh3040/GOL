﻿using System.Collections;
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
        mPlayerChar.Initialize();
        mAdmob.Init();
        mAudio = scene.gameObject.AddComponent<AudioSource>();
        
    }

    public void ResetGame()
    {
        mAdmob.HideAd();

        UseItemId = PlayerData.Instance.GetUseItemId();
        PlayerData.Instance.SetUseItemId(0);
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
                    mNoteSystem.NoteUpdate(time, skill.ConvertSpeed(time));
                }
            }
            else
            {
                mNoteSystem.NoteUpdate(time, time);
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
            if(skill != null)
            {
                var shieldCount = skill.mCount;
                if (shieldCount < ConfigData.Instance.MAX_USE_SHIELD_ITEM)
                {
                    UseItemId = id;
                    UseGameShieldItem();
                    itemAdd = true;
                }
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
        }
    }

    public void EndSkill(GameSkill skill)
    {
        mDoorSystem.EndSkillEffect(skill);
    }

    public void SetDoorState(CommonData.NOTE_LINE line, int DoorState)
    {
        mDoorSystem.SetDoorState(line, DoorState);
    }

    public void PlayDoorSound(CommonData.NOTE_LINE line)
    {
        mDoorSystem.PlaySound(line);
    }

    public int ConvertScoreToCoin()
    {
        return Score / 10;
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

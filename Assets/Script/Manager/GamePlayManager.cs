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

    // 게임 플레이 관련 매니저
    public int Score
    {
        get;
        private set;
    }
    public int Combo
    {
        get;
        private set;
    }
    private float ComboKeepTime = 0f;
    private PlayerChar MainChar = null;
    private bool GameEnable = false;
    private bool Pause = false;
    private NoteSystem mNoteSystem = new NoteSystem();
    private DoorSystem mDoorSystem = new DoorSystem();
    private ItemSystem mItemSystem = new ItemSystem();
    private PageGameUI mGameUIPage;
    private Transform mNoteParentPos;
    public int[] mPlayItemArr = new int[2];

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
        mPlayItemArr[0] = GManager.Instance.mPlayerData.GameItemArr[0];
        mPlayItemArr[1] = GManager.Instance.mPlayerData.GameItemArr[1];

        GManager.Instance.mPlayerData.GameItemArr[0] = 0;
        GManager.Instance.mPlayerData.GameItemArr[1] = 0;

        StopAllCoroutines();
        Score = 0;
        Combo = 0;
        ComboKeepTime = ConfigData.Instance.COMBO_KEEP_TIME;
        GameEnable = true;
        Pause = false;
        mNoteSystem.ResetNote();
        mDoorSystem.ResetDoor();
        mGameUIPage.ResetUI();
        SkillManager.Instance.ResetGame();
    }

    public void GameExit()
    {
        mNoteSystem.AllDelete();
        mDoorSystem.AllDelete();
    }

    public void GameStart()
    { 
        ResetGame();
        StartCoroutine(UpdateGamePlay());
    }

    public void GameRestart()
    {
        ResetGame();
        StartCoroutine(UpdateGamePlay());
    }

    public void GamePause()
    {
        Pause = true;
    }

    public void GameContinue()
    {
        Pause = false;
    }

    public void GameEnd()
    {
        StopAllCoroutines();
    }

    public void GameOver()
    {
        // TODO 환웅 게임오버
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
        Pause = true;
    }

    IEnumerator UpdateGamePlay()
    {
        if (GameEnable == false)
            yield break;

        while(GameEnable)
        {
            if (Pause)
            {
                yield return null;
                continue;
            }

            var time = Time.deltaTime;
            mNoteSystem.NoteUpdate(time);
            SkillManager.Instance.UpdateSkill(time);

            ComboKeepTime -= time;
            if (ComboKeepTime <= 0f)
            {
                ComboKeepTime = 0f;
                RemoveCombo();
            }

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
        if (ComboKeepTime > 0f)
            PlusCombo();
    }

    public void PlusCombo()
    {
        Combo += 1;
        ComboKeepTime = ConfigData.Instance.COMBO_KEEP_TIME;
        mGameUIPage.RefreshCombo(true);
    }

    public void RemoveCombo()
    {
        // TODO 환웅 : 게임 UI의 콤보를 제거 해야함
        Combo = 0;
        ComboKeepTime = ConfigData.Instance.COMBO_KEEP_TIME;
        mGameUIPage.RefreshCombo(false);
    }

    public void PlusItem(int id)
    {
        bool itemAdd = false;
        for (int i = 0; i < GamePlayManager.Instance.mPlayItemArr.Length; i++)
        {
            if (mPlayItemArr[i] == 0)
            {
                mPlayItemArr[i] = id;
                itemAdd = true;
                break;
            }
        }

        if(itemAdd == false)
        {
            PlusScore(ConfigData.Instance.NOTE_ITEM_SCORE);
        }
        else
            mGameUIPage.RefreshItemUI();
    }
    public void UseGameItem(int index)
    {
        mPlayItemArr[index] = 0;
    }
}

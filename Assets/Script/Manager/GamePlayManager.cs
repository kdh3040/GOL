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
    public float NoteSpeed
    {
        get;
        private set;
    }
    private float ComboKeepTime = 0f;
    private PlayerChar MainChar = null;
    private bool GameEnable = false;
    private NoteSystem mNoteSystem = new NoteSystem();
    private DoorSystem mDoorSystem = new DoorSystem();
    private PageGameUI mGameUIPage;
    private Transform mNoteParentPos;

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
        Score = 0;
        Combo = 0;
        ComboKeepTime = ConfigData.Instance.COMBO_KEEP_TIME;
        GameEnable = true;
        mNoteSystem.ResetNote();
        mDoorSystem.ResetDoor();
        mGameUIPage.ResetUI();
        NoteSpeed = ConfigData.Instance.DEFAULT_NOTE_SPEED;
    }

    public void GameStart()
    {
        ResetGame();
        StartCoroutine(UpdateGamePlay());
    }

    public void GameEnd()
    {
        StopAllCoroutines();
    }

    IEnumerator UpdateGamePlay()
    {
        if (GameEnable == false)
            yield break;

        while(GameEnable)
        {
            var time = Time.deltaTime;
            mNoteSystem.NoteUpdate(time);

            ComboKeepTime -= time;
            if (ComboKeepTime <= 0f)
            {
                ComboKeepTime = 0f;
                RemoveCombo();
            }

            yield return null;
        }
    }

    public Note CreateNote()
    {
        // TODO 환웅 : 오브젝트 풀 추가 예정
        var obj = Instantiate(Resources.Load("Prefab/Note"), mNoteParentPos) as GameObject;
        var note = obj.GetComponent<Note>();

        return note;
    }

    public void DeleteNote(Note note, bool score = true)
    {
        if (score)
        {
            PlusScore(note.Id);
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

    public void ClickDoor(Door door)
    {
        mNoteSystem.DeleteCheckNote(door);
    }

    public void PlusScore(int id)
    {
        Score += DataManager.Instance.NoteDataList[id].Score;
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

    public void GameOver()
    {
        // TODO 환웅 : 게임오버체크를 하고 게임이 오버 되었을때 처리를 해야함
        // NoteManager.Instance.ResetNote();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    public static GManager _instance = null;
    public static GManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GManager>() as GManager;
            }
            return _instance;
        }
    }

    // 게임의 큰틀을 담당하는 매니저
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
    public List<Door> DoorList = new List<Door>();

    // TODO 환웅 : Page 매니저를 추가하여 게임이 시작 할때 페이지를 붙이는 형식으로 진행하자
    public GameUIPage GameUIPage;

    void Start()
    {
        DataManager.Instance.Initialize();
        NoteManager.Instance.Initialize();

        // TODO 환웅 : 메인화면에서 게임화면으로 넘어 올때 호출 해야함
        GameStart();
    }

    void Update()
    {
        if (GameEnable == false)
            return;

        var time = Time.deltaTime;
        NoteManager.Instance.NoteUpdate(time);

        ComboKeepTime -= time;
        if(ComboKeepTime <= 0f)
        {
            ComboKeepTime = 0f;
            RemoveCombo();
        }
    }

    public void ResetGame()
    {
        Score = 0;
        Combo = 0;
        ComboKeepTime = CommonData.COMBO_KEEP_TIME;
        GameEnable = true;
        NoteManager.Instance.ResetNote();
        GameUIPage.PageReset();

        for (int i = 0; i < DoorList.Count ; ++i)
        {
            DoorList[i].SetData(i + 1); // NOTE 환웅 : 데이터 세팅(임시)
        }
    }

    public void GameStart()
    {
        ResetGame();

        /*
        if (MainChar == null)
            MainChar = new PlayerChar();
        else
            MainChar.Initialize();
            */
    }

    public void PlusScore(int id)
    {
        Score += DataManager.Instance.NoteDataList[id].Score;
        GameUIPage.RefreshUI();
        if (ComboKeepTime > 0f)
            PlusCombo();
    }

    public void PlusCombo()
    {
        Combo += 1;
        ComboKeepTime = CommonData.COMBO_KEEP_TIME;
        GameUIPage.RefreshCombo(true);
    }

    public void RemoveCombo()
    {
        // TODO 환웅 : 게임 UI의 콤보를 제거 해야함
        Combo = 0;
        ComboKeepTime = CommonData.COMBO_KEEP_TIME;
        GameUIPage.RefreshCombo(false);
    }

    public void GameOver()
    {
        // TODO 환웅 : 게임오버체크를 하고 게임이 오버 되었을때 처리를 해야함
        NoteManager.Instance.ResetNote();
    }
}

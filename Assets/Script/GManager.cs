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

    // TODO 환웅 : Page 매니저를 추가하여 게임이 시작 할때 페이지를 붙이는 형식으로 진행하자
    public GameUIPage GameUIPage;

    void Start()
    {
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


        /*
        // 임시
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(wp, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                // 문을 선택 했다.
                OnClickDoor(GameDoorDic[hit.transform.name]);
                Debug.Log("Complete" + hit.collider.name);

                //this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                // Destroy(hit.collider.gameObject);
            }
        }*/
    }

    public void ResetGame()
    {
        Score = 0;
        Combo = 0;
        ComboKeepTime = CommonData.COMBO_KEEP_TIME;
        GameEnable = true;
        NoteManager.Instance.ResetNote();
        GameUIPage.PageReset();
    }

    public void GameStart()
    {
        ResetGame();

        if (MainChar == null)
            MainChar = new PlayerChar();
        else
            MainChar.Initialize();
    }

    public void PlusScore(int index)
    {
        // TODO 환웅 : 노트에 맞는 점수를 추가 해야함 (index는 note 데이터의 인덱스값)
        Score += 1;
        GameUIPage.RefreshUI();
        if (ComboKeepTime > 0f)
            PlusCombo(index);
    }

    public void PlusCombo(int index)
    {
        // TODO 환웅 : 노트에 맞는 콤보를 추가 해야함 (index는 note 데이터의 인덱스값)
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

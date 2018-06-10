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

    public List<Door> GameDoorList = new List<Door>();
    public PlayerChar GameCharacter;
    public GameUIPage GameUIPage;
    private Dictionary<string, Door> GameDoorDic = new Dictionary<string, Door>();

    void Start()
    {
        NoteManager.Instance.Initialize();

        GameStart();
    }

    void Update()
    {
        var time = Time.deltaTime;
        NoteManager.Instance.NoteUpdate(time);
        GameUIPage.PageUpdate(time);

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
        }
    }

    public void GameStart()
    {
        GameCharacter = new PlayerChar();
        GameUIPage.PageReset();

        for (int i = 0; i < GameDoorList.Count; i++)
        {
            if ((CommonData.NOTE_POS_TYPE)(i + 1) >= CommonData.NOTE_POS_TYPE.MAX)
                break;

            GameDoorList[i].SetDoorNoteType((CommonData.NOTE_POS_TYPE)(i + 1));
            GameDoorDic[GameDoorList[i].name] = GameDoorList[i];
        }
    }

    public void OnClickDoor(Door door)
    {
        GameCharacter.ActionDoorClose();
        NoteManager.Instance.CheckNote(door);
    }

    public void PlusScore(int score)
    {
        GameUIPage.PlusScore(score);
    }

    public void PlusCombo()
    {
        GameUIPage.PlusCombo();
    }
}

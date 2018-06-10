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

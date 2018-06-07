using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public PlayerChar GameCharacter;

    void Update()
    {
        // 노트 진행
        NoteManager.Instance.NoteUpdate();
    }

    public void GameStart()
    {
        GameCharacter = new PlayerChar();
    }

    public void OnClickDoor(Door door)
    {
        // 문을 선택 하였다.
        // 문닫는 모션
        GameCharacter.ActionDoorClose();
        // 붙어 있는 노트가 있는가?
        // 있으면 제거 되면서 점수 계산
        NoteManager.Instance.CheckNote(door);
    }


    public void CreateNote(Note note)
    {
        // 노트 생성
    }
    public void DeleteNote(Note note)
    {
        // 노트 제거
    }

}

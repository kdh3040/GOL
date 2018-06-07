using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour {

    public static NoteManager _instance = null;
    public static NoteManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<NoteManager>() as NoteManager;
            }
            return _instance;
        }
    }

    public Transform NoteStartPosRight;
    private List<Note> GameNoteList = new List<Note>();
    private float SaveTime;

    public NoteManager()
    {

    }

    public void NoteUpdate()
    {
        SaveTime += Time.deltaTime;

        if (SaveTime > 3f)
        {
            SaveTime = 0;
            var testt = Resources.Load("Prefab/Note");
            var test = Instantiate(testt, NoteStartPosRight) as Note;
        }
        //Time.deltaTime
        // 노트를 언제 생성 할지 체트해야함
        for (int index = 0; index < GameNoteList.Count; ++index)
        {
            GameNoteList[index].NoteUpdate();
        }
    }

    public void CheckNote(Door door)
    {
        // 선택한 문이랑 가까운 노트를 찾는다
    }
}

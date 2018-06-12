using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

    [System.NonSerialized]
    public CommonData.NOTE_POS_TYPE NotePosType = CommonData.NOTE_POS_TYPE.NONE;
    private int Id = 0;
    private NoteData data = null;
    private Transform StartPos;
    private Transform EndPos;
    private float Speed = 1.0f; // TODO 환웅 : 노트 생성시 속도를 정할 수 있게 추가
    private float SaveTime = 0;

    public void SetNoteData(CommonData.NOTE_POS_TYPE type, int id)
    {
        NotePosType = type;
        Id = id;
        data = null; // TODO 환웅 노트 데이터 추가
        var pos = NoteManager.Instance.GetNoteTypeStartEndPos(NotePosType);
        StartPos = pos[0];
        EndPos = pos[1];
        gameObject.transform.position = StartPos.position;
    }

    public void NoteUpdate(float time)
    {
        SaveTime += time;

        var pos = StartPos.position;
        pos.y = Mathf.Lerp(StartPos.position.y, EndPos.position.y, SaveTime / Speed);
        gameObject.transform.position = pos;

        if ((SaveTime / Speed) >= 1f)
        {
            NoteManager.Instance.AddDeleteReadyNote(this);
            GManager.Instance.GameOver();
        }
            
    }
}

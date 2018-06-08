using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

    public CommonData.NOTE_POS_TYPE NotePosType = CommonData.NOTE_POS_TYPE.NONE;
    public Transform StartPos;
    public Transform EndPos;
    private float Speed = 1.0f;
    private float SaveTime = 0;
    public bool DestroyReady = false;

    public void SetNoteData(CommonData.NOTE_POS_TYPE notePosType, Transform startPos, Transform endPos)
    {
        NotePosType = notePosType;
        StartPos = startPos;
        EndPos = endPos;
        gameObject.transform.position = StartPos.position;
        DestroyReady = false;
    }

    public void NoteUpdate(float time)
    {
        SaveTime += time;

        var pos = StartPos.position;
        pos.y = Mathf.Lerp(StartPos.position.y, EndPos.position.y, SaveTime / Speed);
        gameObject.transform.position = pos;

        if ((SaveTime / Speed) >= 1f)
            DestroyReady = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

    [System.NonSerialized]
    public CommonData.NOTE_LINE NoteLineType = CommonData.NOTE_LINE.NONE;
    [System.NonSerialized]
    public CommonData.NOTE_TYPE NoteType = CommonData.NOTE_TYPE.NORMAL;
    protected Transform StartPos;
    protected Transform EndPos;
    protected float SaveTime = 0;

    public virtual void NoteUpdate(float time)
    {
        SaveTime += time;

        var speed = GamePlayManager.Instance.NoteSpeed;
        var pos = StartPos.position;
        pos.y = StartPos.position.y - speed * SaveTime;
        gameObject.transform.position = pos;
    }
}

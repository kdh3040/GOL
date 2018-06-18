using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

    [System.NonSerialized]
    public CommonData.NOTE_POS_TYPE NotePosType = CommonData.NOTE_POS_TYPE.NONE;
    [System.NonSerialized]
    public int Id = 0;
    private NoteData data = null;
    private Transform StartPos;
    private Transform EndPos;
    private float SaveTime = 0;

    public void SetNoteData(CommonData.NOTE_POS_TYPE type, Transform[] pos, int id)
    {
        NotePosType = type;
        Id = id;
        data = DataManager.Instance.NoteDataList[id];
        StartPos = pos[0];
        EndPos = pos[1];
        gameObject.transform.position = StartPos.position;
    }

    public void NoteUpdate(float time)
    {
        SaveTime += time;

        var speed = GamePlayManager.Instance.NoteSpeed;
        var pos = StartPos.position;
        pos.y = StartPos.position.y - speed * SaveTime;
        gameObject.transform.position = pos;

        if (pos.y <= EndPos.position.y)
        {
            GamePlayManager.Instance.AddDeleteReadyNote(this);
            GamePlayManager.Instance.GameOver();
        }
            
    }
}

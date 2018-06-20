using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteNormal : Note
{
    public SpriteRenderer NoteImage;
    public int Id = 0;
    private NoteData data = null;

    public void SetNoteNormalData(CommonData.NOTE_POS_TYPE type, Transform[] pos, int id)
    {
        NotePosType = type;
        NoteType = CommonData.NOTE_TYPE.NORMAL;
        Id = id;
        data = DataManager.Instance.NoteDataDic[id];
        StartPos = pos[0];
        EndPos = pos[1];
        gameObject.transform.position = StartPos.position;
        NoteImage.sprite = (Sprite)Resources.Load(data.img, typeof(Sprite));
    }

    public override void NoteUpdate(float time)
    {
        base.NoteUpdate(time);
        if (gameObject.transform.position.y <= EndPos.position.y)
        {
            GamePlayManager.Instance.AddDeleteReadyNote(this);
            GamePlayManager.Instance.GameOver();
        }
    }
}

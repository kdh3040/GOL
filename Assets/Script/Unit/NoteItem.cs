using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteItem : Note
{
    public SpriteRenderer NoteImage;
    public int Id = 0;
    private ItemData data = null;

    public void SetNoteItemData(CommonData.NOTE_LINE type, Transform[] pos, int id)
    {
        NoteLineType = type;
        NoteType = CommonData.NOTE_TYPE.ITEM;
        Id = id;
        data = ItemManager.Instance.GetItemData(id);
        StartPos = pos[0];
        EndPos = pos[1];
        gameObject.transform.position = StartPos.position;
        NoteImage.sprite = (Sprite)Resources.Load(data.icon, typeof(Sprite));
    }

    public override void NoteUpdate(float time)
    {
        base.NoteUpdate(time);
        if (gameObject.transform.position.y <= EndPos.position.y)
        {
            GamePlayManager.Instance.AddDeleteReadyNote(this);
        }
    }
}

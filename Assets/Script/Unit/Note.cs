using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [System.NonSerialized]
    public CommonData.NOTE_LINE NoteLineType = CommonData.NOTE_LINE.INDEX_1;
    [System.NonSerialized]
    public CommonData.NOTE_TYPE NoteType = CommonData.NOTE_TYPE.NORMAL;
    [System.NonSerialized]
    public int NoteId = 0;
    
    public SpriteRenderer NoteImage;
    public BoxCollider2D NoteCollider;
    private NoteData NoteData = null;
    private ItemData ItemData = null;

    public void ResetNote()
    {
        NoteType = CommonData.NOTE_TYPE.NONE;
        NoteData = null;
        ItemData = null;
        NoteImage.sprite = null;
        NoteId = 0;
        NoteCollider.enabled = false;
    }

    public void SetNote(CommonData.NOTE_LINE lineType, CommonData.NOTE_TYPE type, int id)
    {
        ResetNote();
        switch (type)
        {
            case CommonData.NOTE_TYPE.NORMAL:
                SetNormalNote(id);
                break;
            case CommonData.NOTE_TYPE.ITEM:
                SetItemNote(id);
                break;
            default:
                break;
        }

        NoteLineType = lineType;
    }

    private void SetNormalNote(int id)
    {
        NoteId = id;
        NoteType = CommonData.NOTE_TYPE.NORMAL;
        NoteData = DataManager.Instance.NoteDataDic[id];
        NoteImage.sprite = (Sprite)Resources.Load(NoteData.img, typeof(Sprite));
        NoteCollider.enabled = true;
    }

    private void SetItemNote(int id)
    {
        NoteId = id;
        NoteType = CommonData.NOTE_TYPE.ITEM;
        ItemData = ItemManager.Instance.GetItemData(id);
        NoteImage.sprite = (Sprite)Resources.Load(ItemData.icon, typeof(Sprite));
        NoteCollider.enabled = true;
    }
}

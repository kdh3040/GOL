using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [System.NonSerialized]
    public CommonData.NOTE_TYPE NoteType = CommonData.NOTE_TYPE.NORMAL;

    public SpriteRenderer NoteImage;
    private NoteData NoteData = null;
    private ItemData ItemData = null;

    private void ResetNote()
    {
        NoteType = CommonData.NOTE_TYPE.NONE;
        NoteData = null;
        ItemData = null;
        NoteImage.sprite = null;
    }

    public void SetNote(CommonData.NOTE_TYPE type, int id)
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
    }

    private void SetNormalNote(int id)
    {
        NoteType = CommonData.NOTE_TYPE.NORMAL;
        NoteData = DataManager.Instance.NoteDataDic[id];
        NoteImage.sprite = (Sprite)Resources.Load(NoteData.img, typeof(Sprite));
    }

    private void SetItemNote(int id)
    {
        NoteType = CommonData.NOTE_TYPE.ITEM;
        ItemData = ItemManager.Instance.GetItemData(id);
        NoteImage.sprite = (Sprite)Resources.Load(ItemData.icon, typeof(Sprite));
    }
}

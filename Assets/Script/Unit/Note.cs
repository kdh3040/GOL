﻿using System.Collections;
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
    public SpriteRenderer ShadowImage;
    private NoteData NoteData = null;
    private ItemData ItemData = null;

    public Animator Anim = null;

    public void ResetNote()
    {
        NoteType = CommonData.NOTE_TYPE.NONE;
        NoteData = null;
        ItemData = null;
        NoteImage.sprite = null;
        ShadowImage.gameObject.SetActive(false);
        NoteId = 0;
        
        Anim.enabled = false;

        gameObject.transform.localRotation = new Quaternion();
    }


   
    public void SetNote(CommonData.NOTE_LINE lineType, CommonData.NOTE_TYPE type, int id)
    {

        
        ResetNote();

        // 밑에 클립 명 데이터화 필요
    
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
        Anim.enabled = true;
        NoteId = id;
        NoteType = CommonData.NOTE_TYPE.NORMAL;
        NoteData = DataManager.Instance.NoteDataDic[id];
        NoteImage.sprite = (Sprite)Resources.Load(NoteData.img, typeof(Sprite));
        ShadowImage.gameObject.SetActive(true);
    }

    private void SetItemNote(int id)
    {
        NoteId = id;
        NoteType = CommonData.NOTE_TYPE.ITEM;
        ItemData = ItemManager.Instance.GetItemData(id);
        NoteImage.sprite = (Sprite)Resources.Load(ItemData.note_img, typeof(Sprite));
        ShadowImage.gameObject.SetActive(true);
    }
}

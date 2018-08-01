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
    public BoxCollider NoteCollider;
    private NoteData NoteData = null;
    private ItemData ItemData = null;

    public Animator Anim = null;
    List<string> ArrAnim = new List<string>();

    public void ResetNote()
    {

        ArrAnim.Add("1");
        ArrAnim.Add("2");
        ArrAnim.Add("3");
        ArrAnim.Add("4");
        ArrAnim.Add("5");
        ArrAnim.Add("6");
        ArrAnim.Add("7");
        ArrAnim.Add("8");
        ArrAnim.Add("9");
        ArrAnim.Add("10");
        Anim.Rebind();

        NoteType = CommonData.NOTE_TYPE.NONE;
        NoteData = null;
        ItemData = null;
        NoteImage.sprite = null;
        NoteId = 0;
        NoteCollider.enabled = false;
        
        Anim.enabled = false;
        
        
    }


   
    public void SetNote(CommonData.NOTE_LINE lineType, CommonData.NOTE_TYPE type, int id)
    {

        
        ResetNote();

        // 밑에 클립 명 데이터화 필요
    
        switch (type)
        {
            case CommonData.NOTE_TYPE.NORMAL:
                SetNormalNote(id);             
                Anim.SetTrigger(ArrAnim[id-1]);
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
        Anim.enabled = true;
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

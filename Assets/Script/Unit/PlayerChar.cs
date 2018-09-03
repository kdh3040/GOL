using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : MonoBehaviour
{
    public bool touchChk = false;
    public Animator PlayerAnim;
    public SpriteRenderer PlayerImage;
    public CharData Data;
    public string CurrentTrigger;

    public void Initialize()
    {
        CurrentTrigger = "";
        Data = DataManager.Instance.CharDataDic[PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.CHAR)];
        PlayerAnim.Rebind();
        PlayerImage.sprite = (Sprite)Resources.Load(GetCharImg("_idle"), typeof(Sprite));
        PlayerAnim.SetTrigger(Data.ani_trigger);
    }

    public string GetCharImg(string add)
    {
        return Data.img + add;
    }

    public void ActionDoorClose(Door doorType)
    {
        if(CurrentTrigger != "")
            PlayerAnim.ResetTrigger(CurrentTrigger);

        if (doorType.NoteLineType == CommonData.NOTE_LINE.INDEX_1)
            CurrentTrigger = "NOTE_LINE_1";
        else if (doorType.NoteLineType == CommonData.NOTE_LINE.INDEX_2)
            CurrentTrigger = "NOTE_LINE_2";
        else if (doorType.NoteLineType == CommonData.NOTE_LINE.INDEX_3)
            CurrentTrigger = "NOTE_LINE_3";

        PlayerAnim.SetTrigger(CurrentTrigger);
    }
}

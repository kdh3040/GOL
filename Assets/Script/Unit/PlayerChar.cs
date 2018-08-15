using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : MonoBehaviour
{
    public bool touchChk = false;
    public Animator PlayerAnim;
    public SpriteRenderer PlayerImage;
    public CharData Data;

    public void Initialize()
    {
        Data = DataManager.Instance.CharDataDic[PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.CHAR)];
        PlayerImage.sprite = (Sprite)Resources.Load(GetCharImg("_idle"), typeof(Sprite));
        PlayerAnim.SetTrigger(Data.ani_trigger);
    }

    public string GetCharImg(string add)
    {
        return Data.img + add;
    }

    public void ActionDoorClose(Door doorType)
    {
        if(doorType.NoteLineType == CommonData.NOTE_LINE.INDEX_1)
            PlayerAnim.SetTrigger("NOTE_LINE_1");
        else if (doorType.NoteLineType == CommonData.NOTE_LINE.INDEX_2)
            PlayerAnim.SetTrigger("NOTE_LINE_2");
        else if (doorType.NoteLineType == CommonData.NOTE_LINE.INDEX_3)
            PlayerAnim.SetTrigger("NOTE_LINE_3");
    }


    // 문닫는 애니메이션 끝나고 Idle 애니메이션으로 복귀
    void TouchAnimEnd()
    {
        if (PlayerAnim != null)
        {
            PlayerAnim.SetTrigger("Idle");
        }
    }
}

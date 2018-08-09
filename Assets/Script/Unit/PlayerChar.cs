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
        Data = DataManager.Instance.CharDataDic[PlayerData.Instance.UseCharId];
        PlayerImage.sprite = (Sprite)Resources.Load(GetCharImg("_idle"), typeof(Sprite));
    }

    public string GetCharImg(string add)
    {
        return Data.img + add;
    }

    public void ActionDoorClose(Door doorType)
    {
        if(doorType.NoteLineType == CommonData.NOTE_LINE.INDEX_1)
            PlayerAnim.SetTrigger("1");
        else if (doorType.NoteLineType == CommonData.NOTE_LINE.INDEX_2)
            PlayerAnim.SetTrigger("2");
        else if (doorType.NoteLineType == CommonData.NOTE_LINE.INDEX_3)
            PlayerAnim.SetTrigger("3");
    }

    public void ActionDoorClose(string imgSrc)
    {
        if (PlayerAnim != null)
        {
            PlayerAnim.SetTrigger("Touch");
        }

        PlayerImage.sprite = (Sprite)Resources.Load(imgSrc, typeof(Sprite));
    }

    

    void Update()
    {
        // PC 에디터용
        // 어떤 문을 닫았는지에 대한 구분 로직 필요
        // 지금은 더미로 색깔만 변함
        /*
        if (Input.GetMouseButtonDown(0))
        {
            ActionDoorClose("dt_501");
        }

    */
    }

    // 문닫는 애니메이션 끝나고 Idle 애니메이션으로 복귀
    void TouchAnimEnd()
    {
        if (PlayerAnim != null)
        {
            PlayerAnim.SetTrigger("Idle");
        }
    }

    public void StartCharSkill()
    {

    }
}

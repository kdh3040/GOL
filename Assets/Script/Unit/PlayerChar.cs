using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : MonoBehaviour
{
    private PlayerCharData mData = null;
    public bool touchChk = false;
    public Animator PlayerAnim;


    public PlayerChar()
    {
        //mData = new PlayerCharData();
    }


    public void ActionDoorClose()
    {
        if (PlayerAnim != null)
        {
            PlayerAnim.SetTrigger("Touch");
        }
    }

    public void Initialize()
    {
        // 캐릭터 초기화
    }

    void Update()
    {
        // PC 에디터용
        // 어떤 문을 닫았는지에 대한 구분 로직 필요
        // 지금은 더미로 색깔만 변함
        if (Input.GetMouseButtonDown(0))
        {
            ActionDoorClose();
        }
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

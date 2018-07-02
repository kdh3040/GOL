using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : MonoBehaviour
{
    private PlayerCharData mData = null;
    public bool touchChk = false;
    public Animator PlayerAnim;
    public SpriteRenderer PlayerImage;
    public Transform PlayerPos;

    public static PlayerChar _instance = null;
    public static PlayerChar Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerChar>() as PlayerChar;
            }
            return _instance;
        }
    }

    public PlayerChar()
    {
        //mData = new PlayerCharData();

    }

    void Start()
    {
        PlayerImage.transform.position = PlayerPos.position;
        PlayerImage.sprite = (Sprite)Resources.Load("dt_dft", typeof(Sprite));
    }

    public void ActionDoorClose(Door doorType)
    {
        if(doorType.NoteLineType == CommonData.NOTE_LINE.INDEX_1)
            PlayerImage.sprite = (Sprite)Resources.Load("dt_501", typeof(Sprite));
        else if (doorType.NoteLineType == CommonData.NOTE_LINE.INDEX_2)
            PlayerImage.sprite = (Sprite)Resources.Load("dt_502", typeof(Sprite));
        else if (doorType.NoteLineType == CommonData.NOTE_LINE.INDEX_3)
            PlayerImage.sprite = (Sprite)Resources.Load("dt_503", typeof(Sprite));       
    }

    public void ActionDoorClose(string imgSrc)
    {
        if (PlayerAnim != null)
        {
            PlayerAnim.SetTrigger("Touch");
        }

        PlayerImage.sprite = (Sprite)Resources.Load(imgSrc, typeof(Sprite));
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DOOR_STATE
    {
        NONE,
        CLOSE,
        HALF_OPEN,
        OPEN,
    }

    public DoorData Data;
    public SpriteRenderer DoorSprite;
    public CommonData.NOTE_LINE NoteLineType;
    public DOOR_STATE DoorState = DOOR_STATE.NONE;
    public Animator DoorEffectAnim;
    public bool EffectPlay = false;

    public void SetData(CommonData.NOTE_LINE type)
    {
        Data = DataManager.Instance.DoorDataDic[PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.DOOR)];
        NoteLineType = type;
        EffectPlay = false;
        DoorState = DOOR_STATE.NONE;
        SetDoorState(DOOR_STATE.CLOSE);
    }

    public void SetDoorState(DOOR_STATE type)
    {
        if (EffectPlay)
            return;

        if (DoorState == type)
            return;

        DoorState = type;

        switch (DoorState)
        {
            case DOOR_STATE.CLOSE:
                DoorSprite.sprite = (Sprite)Resources.Load(Data.close_img, typeof(Sprite));
              
                break;
            case DOOR_STATE.HALF_OPEN:
                DoorSprite.sprite = (Sprite)Resources.Load(Data.halfopen_img, typeof(Sprite));
                break;
            case DOOR_STATE.OPEN:
                DoorSprite.sprite = (Sprite)Resources.Load(Data.open_img, typeof(Sprite));
                break;
            default:
                break;
        }
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Note"))
        {
            SetDoorState(DOOR_STATE.HALF_OPEN);
        }
    }
    */

    public void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }

    public void SetEffect(string trigger)
    {
        DoorEffectAnim.Rebind();
        DoorEffectAnim.SetTrigger(trigger);

        switch (trigger)
        {        
            case "INVINCIBILITY":
                EffectPlay = true;                
                break;
            case "SHIELD":
                EffectPlay = false;                
                break;
            default:
                EffectPlay = false;                
                break;
        } 
    }
}

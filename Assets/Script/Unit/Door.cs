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

    public void SetData(CommonData.NOTE_LINE type)
    {
        Data = DataManager.Instance.DoorDataDic[PlayerData.Instance.UseDoorId];
        NoteLineType = type;
        SetDoorState(DOOR_STATE.CLOSE);
    }

    public void SetDoorState(DOOR_STATE type)
    {
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

        if (Data.img_twist[(int)NoteLineType - 1])
            DoorSprite.flipX = true;
        else
            DoorSprite.flipX = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem
{
    public List<Door> DoorList = new List<Door>();

    public void Initialize(PlayScene scene)
    {
        DoorList = scene.DoorList;

        for (int i = 0; i < DoorList.Count; i++)
        {
            var type = (CommonData.NOTE_LINE)i;
            DoorList[i].SetData(type);
        }
    }

    public void SetDoorState(CommonData.NOTE_LINE line, int State)
    {
        var DoorNum = (int)line;
        switch(State)
        {
            case 0:
                DoorList[DoorNum].SetDoorState(Door.DOOR_STATE.OPEN);
                break;
            case 1:
                DoorList[DoorNum].SetDoorState(Door.DOOR_STATE.HALF_OPEN);
                break;
            case 2:
                DoorList[DoorNum].SetDoorState(Door.DOOR_STATE.CLOSE);
                break;

        }
        
    }
}

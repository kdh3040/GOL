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
}

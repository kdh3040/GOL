using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem
{
    public List<Door> mDoorList = new List<Door>();

    public void Initialize(PlayScene scene)
    {
        if(mDoorList.Count <= 0)
        {
            for (int i = 0; i < scene.mDoorPosList.Count; i++)
            {
                var door = GamePlayManager.Instance.CreateDoor(scene.mDoorPosList[i]);
                var type = (CommonData.NOTE_POS_TYPE)(i + 1);
                door.SetData(GManager.Instance.mPlayerData.DoorIndexId[type]);
                // 데이터 심어주기
                mDoorList.Add(door);
            }
        }
        else
        {
            for (int i = 0; i < mDoorList.Count; i++)
            {
                // 데이터 심어주기
            }
        }
        
    }

    public void ResetDoor()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem
{
    public List<Transform> mDoorPosList = new List<Transform>();
    public List<Door> mDoorList = new List<Door>();

    public void Initialize(PlayScene scene)
    {
        mDoorPosList.Clear();
        mDoorPosList = scene.mDoorPosList;
        for (int i = 0; i < mDoorPosList.Count; i++)
        {
            var door = GamePlayManager.Instance.CreateDoor(mDoorPosList[i]);
            var type = (CommonData.NOTE_LINE)(i + 1);
            door.SetData(GManager.Instance.mPlayerData.DoorIndexId[type], type);
            mDoorList.Add(door);
        }
    }

    public void ResetDoor()
    {
        //AllDelete();
    }

    public void AllDelete()
    {
        for (int i = 0; i < mDoorList.Count; i++)
        {
            GamePlayManager.Instance.DeleteDoor(mDoorList[i]);
        }

        mDoorList.Clear();
    }
}

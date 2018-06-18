using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public Dictionary<CommonData.NOTE_POS_TYPE, int> DoorIndexId = new Dictionary<CommonData.NOTE_POS_TYPE, int>();
    public int[] GameItemArr = { 1, 2 };

    public void Initialize()
    {
        DoorIndexId.Clear();
        DoorIndexId.Add(CommonData.NOTE_POS_TYPE.INDEX_1, 1);
        DoorIndexId.Add(CommonData.NOTE_POS_TYPE.INDEX_2, 2);
        DoorIndexId.Add(CommonData.NOTE_POS_TYPE.INDEX_3, 3);
    }
}

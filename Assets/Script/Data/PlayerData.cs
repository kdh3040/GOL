﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public Dictionary<CommonData.NOTE_LINE, int> DoorIndexId = new Dictionary<CommonData.NOTE_LINE, int>();
    public int[] mNormalitemArr = { 1, 3 };
    public int mShielditem = CommonData.SHIELD_ITEM_ID;


    public void Initialize()
    {
        DoorIndexId.Clear();
        DoorIndexId.Add(CommonData.NOTE_LINE.INDEX_1, 1);
        DoorIndexId.Add(CommonData.NOTE_LINE.INDEX_2, 2);
        DoorIndexId.Add(CommonData.NOTE_LINE.INDEX_3, 3);
    }

    public int GetItemSlotId(CommonData.ITEM_SLOT_INDEX index)
    {
        return mNormalitemArr[(int)index];
    }
}

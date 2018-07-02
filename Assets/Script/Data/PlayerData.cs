﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public Dictionary<CommonData.NOTE_LINE, int> DoorIndexId = new Dictionary<CommonData.NOTE_LINE, int>();
    public Dictionary<int, int> HaveItemDic = new Dictionary<int, int>();
    public int[] mNormalitemArr = { 0,0 };
    public int mShielditem = 0;
    public int Coin { get; private set; }
    public int Ddong { get; private set; }
    public void Initialize()
    {
        Coin = 1000000;
        Ddong = 100;
        DoorIndexId.Clear();
        DoorIndexId.Add(CommonData.NOTE_LINE.INDEX_1, 1);
        DoorIndexId.Add(CommonData.NOTE_LINE.INDEX_2, 2);
        DoorIndexId.Add(CommonData.NOTE_LINE.INDEX_3, 3);
        mNormalitemArr[0] = 1;
        mNormalitemArr[1] = 0;
        mShielditem = 0;

        HaveItemDic.Add(1, 3);
    }

    public int GetItemSlotId(CommonData.ITEM_SLOT_INDEX index)
    {
        return mNormalitemArr[(int)index];
    }

    public void UpdatePlayerData(float time)
    {

    }
    public void AddItem(int id)
    {
        if (HaveItemDic.ContainsKey(id) == false)
            HaveItemDic.Add(id, 1);
        else
            HaveItemDic[id] += 1;
    }
    public void RemoveItem(int id)
    {
        HaveItemDic[id] -= 1;
    }
    public void AddDdong(int value)
    {
        Ddong += value;
    }
    public void AddCoin(int value)
    {
        Coin += value;
    }
}

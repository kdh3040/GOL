using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public static PlayerData _instance = null;
    public static PlayerData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerData();
            }
            return _instance;
        }
    }

    public Dictionary<CommonData.NOTE_LINE, int> DoorIndexId = new Dictionary<CommonData.NOTE_LINE, int>();
    public Dictionary<int, int> HaveItemDic = new Dictionary<int, int>();
    public Dictionary<int, int> ItemLevelDic = new Dictionary<int, int>();
    public Dictionary<int, bool> HaveCharDic = new Dictionary<int, bool>();
    public Dictionary<int, bool> HaveDoorDic = new Dictionary<int, bool>();
    public Dictionary<int, bool> HaveBGDic = new Dictionary<int, bool>();
    public int[] mNormalitemArr = { 0,0 };
    public int mShielditem = 0;
    public int Coin { get; private set; }
    public int Ddong { get; private set; }
    public int UseCharId { get; private set; }
    public int UseDoorId { get; private set; }
    public int UseBGId { get; private set; }

    public void Initialize()
    {
        Coin = 10;
        Ddong = 100;
        DoorIndexId.Clear();
        DoorIndexId.Add(CommonData.NOTE_LINE.INDEX_1, 1);
        DoorIndexId.Add(CommonData.NOTE_LINE.INDEX_2, 2);
        DoorIndexId.Add(CommonData.NOTE_LINE.INDEX_3, 3);
        mNormalitemArr[0] = 1;
        mNormalitemArr[1] = 0;
        mShielditem = 0;

        HaveItemDic.Add(1, 1);
        HaveCharDic.Add(1, true);
        HaveDoorDic.Add(1, true);
        HaveBGDic.Add(1, true);

        ItemLevelDic.Add(1, 1);
        ItemLevelDic.Add(2, 1);
        ItemLevelDic.Add(3, 1);
        ItemLevelDic.Add(4, 1);
    }

    public int GetItemSlotId(CommonData.ITEM_SLOT_INDEX index)
    {
        return mNormalitemArr[(int)index];
    }
    public void SetItemSlotId(CommonData.ITEM_SLOT_INDEX index, int id)
    {
        mNormalitemArr[(int)index] = id;
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
    public int GetHaveItem(int id)
    {
        if (HaveItemDic.ContainsKey(id) == false)
            return 0;
        else
            return HaveItemDic[id];
    }
    public void RemoveItem(int id)
    {
        HaveItemDic[id] -= 1;
    }
    public int GetItemLevel(int id)
    {
        if (ItemLevelDic.ContainsKey(id) == false)
            return 1;
        else
            return ItemLevelDic[id];
    }
    public void ItemLevelUp(int id)
    {
        if (ItemLevelDic.ContainsKey(id) == false)
            ItemLevelDic.Add(id, 1);
        else
            ItemLevelDic[id]++;
    }
    public void AddDdong(int value)
    {
        Ddong += value;
    }
    public void AddCoin(int value)
    {
        Coin += value;
    }
    public void SubCoin(int value)
    {
        Coin -= value;
    }

    public void AddChar(int id)
    {
        if (HaveCharDic.ContainsKey(id) == false)
            HaveCharDic.Add(id, true);
    }
    public bool IsHasChar(int id)
    {
        if (HaveCharDic.ContainsKey(id) == false)
            return false;

        return true;
    }

    public void SetUseCharId(int id)
    {
        UseCharId = id;
    }

    public void AddDoor(int id)
    {
        if (HaveDoorDic.ContainsKey(id) == false)
            HaveDoorDic.Add(id, true);
    }
    public bool IsHasDoor(int id)
    {
        if (HaveDoorDic.ContainsKey(id) == false)
            return false;

        return true;
    }

    public void SetUseDoorId(int id)
    {
        UseDoorId = id;
    }

    public void AddBG(int id)
    {
        if (HaveBGDic.ContainsKey(id) == false)
            HaveBGDic.Add(id, true);
    }
    public bool IsHasBG(int id)
    {
        if (HaveBGDic.ContainsKey(id) == false)
            return false;

        return true;
    }

    public void SetUseBGId(int id)
    {
        UseBGId = id;
    }
}

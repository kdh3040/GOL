using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public Dictionary<int, int> HaveItemDic = new Dictionary<int, int>();
    public Dictionary<int, int> ItemLevelDic = new Dictionary<int, int>();
    public Dictionary<int, bool> HaveCharDic = new Dictionary<int, bool>();
    public Dictionary<int, bool> HaveDoorDic = new Dictionary<int, bool>();
    public Dictionary<int, bool> HaveBGDic = new Dictionary<int, bool>();
    public Dictionary<int, bool> HaveEndingDic = new Dictionary<int, bool>();
    public int[] mNormalitemArr = { 0,0 };
    public int mShielditem = 0;
    public int Coin { get; private set; }
    public int Ddong { get; private set; }
    public int UseCharId { get; private set; }
    public int UseDoorId { get; private set; }
    public int UseBGId { get; private set; }

    public DateTime DdongRefilTime = DateTime.MaxValue;

    public void Initialize()
    {
        Coin = 10;
        Ddong = 1;
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

        HaveEndingDic.Add(1, true);

        UseCharId = 1;
        UseDoorId = 1;
        UseBGId = 1;
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
        if(DdongRefilTime <= CommonFunc.GetCurrentTime())
        {
            var spanTime = CommonFunc.GetCurrentTime() - DdongRefilTime;
            var refileCount = 1 + (int)(spanTime.TotalSeconds / ConfigData.Instance.DDONG_REFIL_TIME);

            AddDdong(refileCount);
            if (Ddong >= ConfigData.Instance.MAX_DDONG_COUNT)
                DdongRefilTime = DateTime.MaxValue;
            else
                DdongRefilTime = DateTime.Now.AddSeconds(ConfigData.Instance.DDONG_REFIL_TIME);
        }
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
    public void SubDdong(int value)
    {
        Ddong -= value;

        if (Ddong <= 0)
        {
            DdongRefilTime = DateTime.Now;// new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            DdongRefilTime = DdongRefilTime.AddSeconds(ConfigData.Instance.DDONG_REFIL_TIME);
        }

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


    public void AddEnding(int id)
    {
        if (HaveEndingDic.ContainsKey(id) == false)
            HaveEndingDic.Add(id, true);
    }
    public bool IsHasEnding(int id)
    {
        if (HaveEndingDic.ContainsKey(id) == false)
            return false;

        return true;
    }


    public bool IsPlayEnable(bool showMsgPopup)
    {
        if (Ddong <= 0)
        {
            if(showMsgPopup)
                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("GAME_PLAY_LACK_DDONG")));

            return false;
        }

        return true;
    }
}

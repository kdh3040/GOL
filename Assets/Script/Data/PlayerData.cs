using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


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

    [System.Serializable]
    public class SaveData
    {
        public Dictionary<CommonData.SKIN_TYPE, List<int>> HaveSkin = new Dictionary<CommonData.SKIN_TYPE, List<int>>();
        public Dictionary<CommonData.SKIN_TYPE, int> UseSkin = new Dictionary<CommonData.SKIN_TYPE, int>();
        public Dictionary<int, KeyValuePair<int, int>> HaveItem_LevelCount = new Dictionary<int, KeyValuePair<int, int>>();
        public int MyCoin;
        public int MyDDong;
        public long NextDDongRefilTime;

        public int UseChar;
        public int UseDoor;
        public int UseBackground;
        public int[] UseItemArr = { 0, 0, 0 };

        public void Save()
        {
            HaveSkin = PlayerData.Instance.HaveSkin;
            UseSkin = PlayerData.Instance.UseSkin;
            HaveItem_LevelCount = PlayerData.Instance.HaveItem_LevelCount;
            MyCoin = PlayerData.Instance.MyCoin;
            MyDDong = PlayerData.Instance.MyDDong;

            UseItemArr = PlayerData.Instance.UseItemArr;
            NextDDongRefilTime = PlayerData.Instance.NextDDongRefilTime.Ticks;
        }

        public void Load()
        {
            PlayerData.Instance.HaveSkin = HaveSkin;
            PlayerData.Instance.UseSkin = UseSkin;
            PlayerData.Instance.HaveItem_LevelCount = HaveItem_LevelCount;
            PlayerData.Instance.MyCoin = MyCoin;
            PlayerData.Instance.MyDDong = MyDDong;

            PlayerData.Instance.UseItemArr = UseItemArr;
            PlayerData.Instance.NextDDongRefilTime = new DateTime(NextDDongRefilTime);
        }
    }

    private Dictionary<CommonData.SKIN_TYPE, List<int>> HaveSkin = new Dictionary<CommonData.SKIN_TYPE, List<int>>();
    private Dictionary<CommonData.SKIN_TYPE, int> UseSkin = new Dictionary<CommonData.SKIN_TYPE, int>();
    private Dictionary<int, KeyValuePair<int, int>> HaveItem_LevelCount = new Dictionary<int, KeyValuePair<int, int>>();
    public int MyCoin { get; private set; }
    private int Myddong;
    public int MyDDong {
        get { return Myddong; }
        private set
        {
            if (value <= 0)
            {
                Myddong = 0;
                NextDDongRefilTime = DateTime.Now.AddSeconds(ConfigData.Instance.DDONG_REFIL_TIME);
            }
            else
                Myddong = value;
        }
    }
    private DateTime NextDDongRefilTime;

    private int[] UseItemArr = { 0, 0, 0 };

    private SaveData MySaveData = new SaveData();
    
    public void SaveFile()
    {
        MySaveData.Save();
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(Application.dataPath + "PlayerData.ini", FileMode.Create);
        formatter.Serialize(stream, MySaveData);
        stream.Close();
    }

    public void LoadFile()
    {
        FileInfo fileInfo = new FileInfo(Application.dataPath + "PlayerData.ini");
        if (fileInfo.Exists)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.dataPath + "PlayerData.ini", FileMode.Open);
            MySaveData = (SaveData)formatter.Deserialize(stream);
            stream.Close();

            MySaveData.Load();
        }
        else
        {
            HaveSkin.Add(CommonData.SKIN_TYPE.BACKGROUND, new List<int>());
            HaveSkin[CommonData.SKIN_TYPE.BACKGROUND].Add(1);
            HaveSkin.Add(CommonData.SKIN_TYPE.CHAR, new List<int>());
            HaveSkin[CommonData.SKIN_TYPE.CHAR].Add(1);
            HaveSkin.Add(CommonData.SKIN_TYPE.DOOR, new List<int>());
            HaveSkin[CommonData.SKIN_TYPE.DOOR].Add(1);

            UseSkin.Add(CommonData.SKIN_TYPE.BACKGROUND, 1);
            UseSkin.Add(CommonData.SKIN_TYPE.CHAR, 1);
            UseSkin.Add(CommonData.SKIN_TYPE.DOOR, 1);

            MyCoin = 1000;
            MyDDong = CommonData.MAX_DDONG_COUNT;
            NextDDongRefilTime = DateTime.MaxValue;

            SaveFile();
        }
    }

    public void Initialize()
    {
        LoadFile();
    }

    public int GetItemSlotId(CommonData.ITEM_SLOT_INDEX index)
    {
        return UseItemArr[(int)index];
    }
    public void SetItemSlotId(CommonData.ITEM_SLOT_INDEX index, int id)
    {
        UseItemArr[(int)index] = id;
    }

    public bool SetItemSlotId_Normal(int id)
    {
        for (int i = 0; i < UseItemArr.Length; i++)
        {
            if ((CommonData.ITEM_SLOT_INDEX)i == CommonData.ITEM_SLOT_INDEX.SHIELD)
                return false;

            if (UseItemArr[i] == 0)
            {
                UseItemArr[i] = id;
                return true;
            }
        }

        return false;
    }

    public void UpdatePlayerData(float time)
    {
        if (NextDDongRefilTime <= CommonFunc.GetCurrentTime())
        {
            var spanTime = CommonFunc.GetCurrentTime() - NextDDongRefilTime;
            var refileCount = 1 + (int)(spanTime.TotalSeconds / ConfigData.Instance.DDONG_REFIL_TIME);

            PlusDDong(refileCount);
            if (MyDDong >= CommonData.MAX_DDONG_COUNT)
                NextDDongRefilTime = DateTime.MaxValue;
            else
            {
                NextDDongRefilTime = DateTime.Now.AddSeconds(ConfigData.Instance.DDONG_REFIL_TIME);
                SaveFile();
            }
        }
    }

    public TimeSpan GetNextDDongRefileTime()
    {
        return NextDDongRefilTime - CommonFunc.GetCurrentTime();
    }

    public void PlusItem_Count(int id)
    {
        if (HaveItem_LevelCount.ContainsKey(id) == false)
            HaveItem_LevelCount.Add(id, new KeyValuePair<int, int>(1, 1));
        else
        {
            var key = HaveItem_LevelCount[id].Key;
            var value = HaveItem_LevelCount[id].Value + 1;
            HaveItem_LevelCount[id] = new KeyValuePair<int, int>(key, value);
        }

        SaveFile();
    }

    public void MinusItem_Count(int id)
    {
        // 가지고 있는 아이템 제거하는 로직 필요
        if (HaveItem_LevelCount.ContainsKey(id))
        {
            var key = HaveItem_LevelCount[id].Key;
            var value = HaveItem_LevelCount[id].Value > 0 ? HaveItem_LevelCount[id].Value - 1 : 0;
            HaveItem_LevelCount[id] = new KeyValuePair<int, int>(key, value);
        }

        SaveFile();
    }

    public int GetItemCount(int id)
    {
        if (HaveItem_LevelCount.ContainsKey(id))
            return HaveItem_LevelCount[id].Value;

        return 0;
    }

    public void PlusItem_Level(int id)
    {
        if (HaveItem_LevelCount.ContainsKey(id) == false)
            HaveItem_LevelCount.Add(id, new KeyValuePair<int, int>(1, 1));
        else
        {
            var key = HaveItem_LevelCount[id].Key + 1;
            var value = HaveItem_LevelCount[id].Value;
            HaveItem_LevelCount[id] = new KeyValuePair<int, int>(key, value);
        }

        SaveFile();
    }

    public int GetItemLevel(int id)
    {
        if (HaveItem_LevelCount.ContainsKey(id))
            return HaveItem_LevelCount[id].Key;

        return 1;
    }

    public void PlusDDong(int count)
    {
        MyDDong += count;
        SaveFile();
    }

    public void MinusDDong()
    {
        MyDDong -= 1;
        SaveFile();
    }

    public void PlusCoin(int coin)
    {
        MyCoin += coin;
        SaveFile();
    }

    public void MinusCoin(int coin)
    {
        MyCoin -= coin;
        SaveFile();
    }

    public void AddSkin(CommonData.SKIN_TYPE type, int id)
    {
        if (HaveSkin.ContainsKey(type) == false)
            HaveSkin.Add(type, new List<int>());

        HaveSkin[type].Add(id);
        SaveFile();
    }

    public bool HasSkin(CommonData.SKIN_TYPE type, int id)
    {
        if (HaveSkin.ContainsKey(type) == false)
            return false;
        else
        {
            for (int i = 0; i < HaveSkin[type].Count; i++)
            {
                if (HaveSkin[type][i] == id)
                    return true;
            }
        }
        return false;
    }

    public void SetUseSkin(CommonData.SKIN_TYPE type, int id)
    {
        UseSkin[type] = id;
        SaveFile();
    }

    public int GetUseSkin(CommonData.SKIN_TYPE type)
    {
        return UseSkin[type];
    }

    public bool IsPlayEnable(bool showMsgPopup)
    {
        if (MyDDong <= 0)
        {
            if(showMsgPopup)
                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("GAME_PLAY_LACK_DDONG")));

            return false;
        }

        return true;
    }
}

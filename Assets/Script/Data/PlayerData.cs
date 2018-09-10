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
        public Dictionary<CommonData.SKIN_TYPE, int> SkinSlotLevel = new Dictionary<CommonData.SKIN_TYPE, int>();
        public Dictionary<int, KeyValuePair<int, int>> HaveItem_LevelCount = new Dictionary<int, KeyValuePair<int, int>>();
        private List<int> HaveEnding = new List<int>();
        public int MyCoin;
        public int MyDDong;
        public long NextDDongRefilTime;

        public int UseChar;
        public int UseDoor;
        public int UseBackground;

        public bool SoundSetting = true;
        public bool VibrationSetting = true;
        public bool AlarmSetting = true;

        // 사용 안함
        public int LastEquipItemId = 1;

        public void Save()
        {
            HaveSkin = PlayerData.Instance.HaveSkin;
            UseSkin = PlayerData.Instance.UseSkin;
            SkinSlotLevel = PlayerData.Instance.SkinSlotLevel;
            HaveItem_LevelCount = PlayerData.Instance.HaveItem_LevelCount;
            HaveEnding = PlayerData.Instance.HaveEnding;
            MyCoin = PlayerData.Instance.MyCoin;
            MyDDong = PlayerData.Instance.MyDDong;
            NextDDongRefilTime = PlayerData.Instance.NextDDongRefilTime.Ticks;
            SoundSetting = PlayerData.Instance.SoundSetting;
            VibrationSetting = PlayerData.Instance.VibrationSetting;
            AlarmSetting = PlayerData.Instance.AlarmSetting;
        }

        public void Load()
        {
            if (HaveSkin == null)
            {
                HaveSkin.Add(CommonData.SKIN_TYPE.BACKGROUND, new List<int>());
                HaveSkin[CommonData.SKIN_TYPE.BACKGROUND].Add(1);
                HaveSkin.Add(CommonData.SKIN_TYPE.CHAR, new List<int>());
                HaveSkin[CommonData.SKIN_TYPE.CHAR].Add(1);
                HaveSkin.Add(CommonData.SKIN_TYPE.DOOR, new List<int>());
                HaveSkin[CommonData.SKIN_TYPE.DOOR].Add(1);
                HaveSkin = new Dictionary<CommonData.SKIN_TYPE, List<int>>();
            }
            if (UseSkin == null)
            {
                UseSkin = new Dictionary<CommonData.SKIN_TYPE, int>();
                UseSkin.Add(CommonData.SKIN_TYPE.BACKGROUND, 1);
                UseSkin.Add(CommonData.SKIN_TYPE.CHAR, 1);
                UseSkin.Add(CommonData.SKIN_TYPE.DOOR, 1);
            }
            if (SkinSlotLevel == null)
            {
                SkinSlotLevel = new Dictionary<CommonData.SKIN_TYPE, int>();
                SkinSlotLevel.Add(CommonData.SKIN_TYPE.BACKGROUND, 1);
                SkinSlotLevel.Add(CommonData.SKIN_TYPE.CHAR, 1);
                SkinSlotLevel.Add(CommonData.SKIN_TYPE.DOOR, 1);
            }
            if (HaveItem_LevelCount == null)
                HaveItem_LevelCount = new Dictionary<int, KeyValuePair<int, int>>();
            if (HaveEnding == null)
                HaveEnding = new List<int>();

            PlayerData.Instance.HaveSkin = HaveSkin;
            PlayerData.Instance.UseSkin = UseSkin;
            PlayerData.Instance.SkinSlotLevel = SkinSlotLevel;
            PlayerData.Instance.HaveItem_LevelCount = HaveItem_LevelCount;
            PlayerData.Instance.HaveEnding = HaveEnding;
            PlayerData.Instance.MyCoin = MyCoin;
            PlayerData.Instance.MyDDong = MyDDong;
            PlayerData.Instance.NextDDongRefilTime = new DateTime(NextDDongRefilTime);
            PlayerData.Instance.SoundSetting = SoundSetting;
            PlayerData.Instance.VibrationSetting = VibrationSetting;
            PlayerData.Instance.AlarmSetting = AlarmSetting;
        }
    }

    private Dictionary<CommonData.SKIN_TYPE, List<int>> HaveSkin = new Dictionary<CommonData.SKIN_TYPE, List<int>>();
    private Dictionary<CommonData.SKIN_TYPE, int> UseSkin = new Dictionary<CommonData.SKIN_TYPE, int>();
    private Dictionary<CommonData.SKIN_TYPE, int> SkinSlotLevel = new Dictionary<CommonData.SKIN_TYPE, int>();
    private Dictionary<int, KeyValuePair<int, int>> HaveItem_LevelCount = new Dictionary<int, KeyValuePair<int, int>>();
    private List<int> HaveEnding = new List<int>();
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

    private SaveData MySaveData = new SaveData();

    public bool SoundSetting { get; private set; }
    public bool VibrationSetting { get; private set; }
    public bool AlarmSetting { get; private set; }

    public void SaveFile()
    {
        MySaveData.Save();
        BinaryFormatter formatter = new BinaryFormatter();
        string path = pathForDocumentsFile("PlayerData.ini");
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, MySaveData);
        stream.Close();
    }

    public void LoadFile()
    {
        string path = pathForDocumentsFile("PlayerData.ini");
        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Exists)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
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

            SkinSlotLevel.Add(CommonData.SKIN_TYPE.BACKGROUND, 1);
            SkinSlotLevel.Add(CommonData.SKIN_TYPE.CHAR, 1);
            SkinSlotLevel.Add(CommonData.SKIN_TYPE.DOOR, 1);

            MyCoin = 1000;
            MyDDong = CommonData.MAX_DDONG_COUNT;
            NextDDongRefilTime = DateTime.MaxValue;
            SaveFile();
        }
    }

    public string pathForDocumentsFile(string filename)
    {
#if UNITY_EDITOR
        string path_pc = Application.dataPath;
        path_pc = path_pc.Substring(0, path_pc.LastIndexOf('/'));
        return Path.Combine(path_pc, filename);
#elif UNITY_ANDROID
        string path = Application.persistentDataPath;
        path = path.Substring(0, path.LastIndexOf('/'));
        return Path.Combine(path, filename);
#elif UNITY_IOS
        string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
        path = path.Substring(0, path.LastIndexOf('/'));
        return Path.Combine(Path.Combine(path, "Documents"), filename);
#endif
    }

    public void Initialize()
    {
        LoadFile();
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

    public SkinData GetUseSkinData(CommonData.SKIN_TYPE type)
    {
        SkinData skinData = null;
        switch (type)
        {
            case CommonData.SKIN_TYPE.CHAR:
                skinData = DataManager.Instance.CharDataDic[GetUseSkin(type)];
                break;
            case CommonData.SKIN_TYPE.DOOR:
                skinData = DataManager.Instance.DoorDataDic[GetUseSkin(type)];
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                skinData = DataManager.Instance.BackGroundDataDic[GetUseSkin(type)];
                break;
            default:
                break;
        }

        return skinData;
    }

    public void SetSkinSlotLevel(CommonData.SKIN_TYPE type, int level)
    {
        if (level > DataManager.Instance.SkinSlotLevelDataList[type].Count)
            SkinSlotLevel[type] = DataManager.Instance.SkinSlotLevelDataList[type].Count;
        else
            SkinSlotLevel[type] = level;

        SaveFile();
    }

    public int GetSkinSlotLevel(CommonData.SKIN_TYPE type)
    {
        return SkinSlotLevel[type];
    }

    public string GetSkinSlotSkill(CommonData.SKIN_TYPE type)
    {
        var level = GetSkinSlotLevel(type);
        var data = DataManager.Instance.SkinSlotLevelDataList[type][level - 1];
        return data.skill;
    }

    public bool HasEnding(int id)
    {
        for (int i = 0; i < HaveEnding.Count; i++)
        {
            if (HaveEnding[i] == id)
                return true;
        }

        return false;
    }

    public void AddEnding(int id)
    {
        for (int i = 0; i < HaveEnding.Count; i++)
        {
            if (HaveEnding[i] == id)
                return;
        }

        HaveEnding.Add(id);

        SaveFile();
    }

    public bool IsPlayEnable()
    {
        if (CommonFunc.UseDDong() == false)
            return false;

        return true;
    }

    public void SetSoundSetting(bool enable)
    {
        SoundSetting = enable;
        SettingManager.Instance.SetSoundStatus(enable);
        SaveFile();
    }

    public void SetVibrationSetting(bool enable)
    {
        VibrationSetting = enable;
        SettingManager.Instance.SetVibeStatus(enable);
        SaveFile();
    }

    public void SetAlarmSetting(bool enable)
    {
        AlarmSetting = enable;
        SettingManager.Instance.SetNotiStatus(enable);
        SaveFile();
    }
}

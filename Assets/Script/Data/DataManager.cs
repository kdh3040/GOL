﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class DataManager {

    public static DataManager _instance = null;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataManager();
            }
            return _instance;
        }
    }

    public Dictionary<int, DoorData> DoorDataDic = new Dictionary<int, DoorData>();
    public Dictionary<int, NoteData> NoteDataDic = new Dictionary<int, NoteData>();
    public Dictionary<int, CharData> CharDataDic = new Dictionary<int, CharData>();
    public Dictionary<int, BackgroundData> BackGroundDataDic = new Dictionary<int, BackgroundData>();
    public Dictionary<int, ItemData> ItemDataDic = new Dictionary<int, ItemData>();
    public Dictionary<string, SkillData> SkillDataList = new Dictionary<string, SkillData>();
    public Dictionary<int, EndingData> EndingDataList = new Dictionary<int, EndingData>();
    public Dictionary<int, EndingGroupData> EndingGroupDataList = new Dictionary<int, EndingGroupData>();
    public Dictionary<int, CharMsgData> CharMsgDataList = new Dictionary<int, CharMsgData>();
    public Dictionary<string, ItemLevelUpCostData> ItemLevelUpCostDataList = new Dictionary<string, ItemLevelUpCostData>();
    public Dictionary<CommonData.SKIN_TYPE, List<SkinSlotLevelData>> SkinSlotLevelDataList = new Dictionary<CommonData.SKIN_TYPE, List<SkinSlotLevelData>>();

    private List<KeyValuePair<string, string>> LoadingDataXmlList = new List<KeyValuePair<string, string>>();

    public IEnumerator LoadingData(Slider loadingSlider, float WaitTime)
    {
        if (LoadingDataXmlList.Count <= 0)
        {
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Door", "Doors"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Note", "Notes"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("CommonData", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Localize", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Item", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Skill", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Character", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Background", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Ending", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("EndingGroup", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("CharSkinSlotLevel", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("BGSkinSlotLevel", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("DoorSkinSlotLevel", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("CharMsg", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("ItemLevelUpCost", "Datas"));

        }

        for (int i = 0; i < LoadingDataXmlList.Count; i++)
        {
            string xmlName = LoadingDataXmlList[i].Key;
            XmlNodeList list = GetXmlNodeList(LoadingDataXmlList[i].Key, LoadingDataXmlList[i].Value);


            if(xmlName == "Door")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new DoorData(child);
                        DoorDataDic.Add(data.id, data);
                    }
                }
            }
            else if (xmlName == "Note")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new NoteData(child);
                        NoteDataDic.Add(data.id, data);
                    }
                }
            }
            else if (xmlName == "CommonData")
            {
                foreach (XmlNode node in list)
                {
                    ConfigData.Instance.Initialize(node.ChildNodes);
                }
            }
            else if (xmlName == "Localize")
            {
                foreach (XmlNode node in list)
                {
                    LocalizeData.Instance.Initialize(node.ChildNodes);
                }
            }
            else if (xmlName == "Item")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new ItemData(child);
                        ItemDataDic.Add(data.id, data);
                    }
                }
            }
            else if (xmlName == "Skill")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new SkillData(child);
                        SkillDataList.Add(data.name, data);
                    }
                }
            }

            else if (xmlName == "Character")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new CharData(child);
                        CharDataDic.Add(data.id, data);
                    }
                }
            }
            else if (xmlName == "Background")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new BackgroundData(child);
                        BackGroundDataDic.Add(data.id, data);
                    }
                }
            }
            else if (xmlName == "Ending")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new EndingData(child);
                        EndingDataList.Add(data.id, data);
                    }
                }
            }

            else if (xmlName == "EndingGroup")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new EndingGroupData(child);
                        EndingGroupDataList.Add(data.id, data);
                    }
                }
            }
            

            else if (xmlName == "CharSkinSlotLevel")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new SkinSlotLevelData(child);
                        if(SkinSlotLevelDataList.ContainsKey(CommonData.SKIN_TYPE.CHAR) == false)
                            SkinSlotLevelDataList.Add(CommonData.SKIN_TYPE.CHAR, new List<SkinSlotLevelData>());

                        SkinSlotLevelDataList[CommonData.SKIN_TYPE.CHAR].Add(data);
                    }
                }

                SkinSlotLevelDataList[CommonData.SKIN_TYPE.CHAR].Sort(delegate (SkinSlotLevelData A, SkinSlotLevelData B)
                {
                    if (A.level > B.level)
                        return 1;
                    else
                        return -1;
                });
            }

            else if (xmlName == "BGSkinSlotLevel")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new SkinSlotLevelData(child);
                        if (SkinSlotLevelDataList.ContainsKey(CommonData.SKIN_TYPE.BACKGROUND) == false)
                            SkinSlotLevelDataList.Add(CommonData.SKIN_TYPE.BACKGROUND, new List<SkinSlotLevelData>());

                        SkinSlotLevelDataList[CommonData.SKIN_TYPE.BACKGROUND].Add(data);
                    }
                }

                SkinSlotLevelDataList[CommonData.SKIN_TYPE.BACKGROUND].Sort(delegate (SkinSlotLevelData A, SkinSlotLevelData B)
                {
                    if (A.level > B.level)
                        return 1;
                    else
                        return -1;
                });
            }

            else if (xmlName == "DoorSkinSlotLevel")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new SkinSlotLevelData(child);
                        if (SkinSlotLevelDataList.ContainsKey(CommonData.SKIN_TYPE.DOOR) == false)
                            SkinSlotLevelDataList.Add(CommonData.SKIN_TYPE.DOOR, new List<SkinSlotLevelData>());

                        SkinSlotLevelDataList[CommonData.SKIN_TYPE.DOOR].Add(data);
                    }
                }

                SkinSlotLevelDataList[CommonData.SKIN_TYPE.DOOR].Sort(delegate (SkinSlotLevelData A, SkinSlotLevelData B)
                {
                    if (A.level > B.level)
                        return 1;
                    else
                        return -1;
                });
            }

            else if (xmlName == "CharMsg")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new CharMsgData(child);
                        CharMsgDataList.Add(data.id, data);
                    }
                }
            }

            else if (xmlName == "ItemLevelUpCost")
            {
                foreach (XmlNode node in list)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        var data = new ItemLevelUpCostData(child);
                        ItemLevelUpCostDataList.Add(data.name, data);
                    }
                }
            }


            

            loadingSlider.value = (i + 1) / (float)LoadingDataXmlList.Count;
            yield return null;
        }

        yield return new WaitForSeconds(WaitTime);
    }

    public XmlNodeList GetXmlNodeList(string fileName, string key)
    {
        TextAsset txtAsset = (TextAsset)Resources.Load("xml/" + fileName);
        XmlDocument xmlDoc = new XmlDocument();
        Debug.Log(txtAsset.text);
        xmlDoc.LoadXml(txtAsset.text);
        XmlNodeList all_nodes = xmlDoc.SelectNodes(key);
        return all_nodes;
    }

    public SkinData GetSkinData(CommonData.SKIN_TYPE type, int id)
    {
        SkinData skinData = null;
        switch (type)
        {
            case CommonData.SKIN_TYPE.CHAR:
                skinData = DataManager.Instance.CharDataDic[id];
                break;
            case CommonData.SKIN_TYPE.DOOR:
                skinData = DataManager.Instance.DoorDataDic[id];
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                skinData = DataManager.Instance.BackGroundDataDic[id];
                break;
            default:
                break;
        }

        return skinData;
    }

    public SkinData GetSkinData(CommonData.SKIN_TYPE type, string name)
    {
        switch (type)
        {
            case CommonData.SKIN_TYPE.CHAR:
                {
                    var skinEnumerator = CharDataDic.GetEnumerator();
                    while (skinEnumerator.MoveNext())
                    {
                        if (skinEnumerator.Current.Value.name == name)
                            return skinEnumerator.Current.Value;
                    }
                }
                break;
            case CommonData.SKIN_TYPE.DOOR:
                {
                    var skinEnumerator = DoorDataDic.GetEnumerator();
                    while (skinEnumerator.MoveNext())
                    {
                        if (skinEnumerator.Current.Value.name == name)
                            return skinEnumerator.Current.Value;
                    }
                }
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                {
                    var skinEnumerator = BackGroundDataDic.GetEnumerator();
                    while (skinEnumerator.MoveNext())
                    {
                        if (skinEnumerator.Current.Value.name == name)
                            return skinEnumerator.Current.Value;
                    }
                }
                break;
            default:
                break;
        }
        return null;
    }
}

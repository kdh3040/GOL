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

    public Dictionary<int, DoorData> DoorDataList = new Dictionary<int, DoorData>();
    public Dictionary<int, NoteData> NoteDataList = new Dictionary<int, NoteData>();

    private List<KeyValuePair<string, string>> LoadingDataXmlList = new List<KeyValuePair<string, string>>();


    public IEnumerator LoadingData(Text loadingCount)
    {
        if(LoadingDataXmlList.Count <= 0)
        {
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Door", "Doors"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Note", "Notes"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("CommonData", "Datas"));
            LoadingDataXmlList.Add(new KeyValuePair<string, string>("Localize", "Datas"));
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
                        DoorDataList.Add(data.id, data);
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
                        NoteDataList.Add(data.id, data);
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
            loadingCount.text = string.Format("데이터 로딩중 입니다.({0} / {1})", i, LoadingDataXmlList.Count);
            yield return null;
        }

        yield return null;
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
}

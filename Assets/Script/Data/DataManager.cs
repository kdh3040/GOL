using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

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

    public void Initialize()
    {
        // NOTE 환웅 : 모든 데이터 파싱
        XmlNodeList list = GetXmlNodeList("Door", "Doors");

        foreach (XmlNode node in list)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                var data = new DoorData(child);
                DoorDataList.Add(data.id, data);
            }
        }

        list = GetXmlNodeList("Note", "Notes");

        foreach (XmlNode node in list)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                var data = new NoteData(child);
                NoteDataList.Add(data.id, data);
            }
        }

        list = GetXmlNodeList("CommonData", "Datas");

        foreach (XmlNode node in list)
        {
            ConfigData.Instance.Initialize(node.ChildNodes);
        }

        list = GetXmlNodeList("Localize", "Datas");

        foreach (XmlNode node in list)
        {
            LocalizeData.Instance.Initialize(node.ChildNodes);
        }
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

using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LocalizeData {

    public static LocalizeData _instance = null;
    public static LocalizeData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LocalizeData();
            }
            return _instance;
        }
    }

    public Dictionary<string, string> LocalizeList = new Dictionary<string, string>();

    public void Initialize(XmlNodeList list)
    {
        foreach (XmlNode node in list)
        {
            string key = node.Attributes.GetNamedItem("id").Value;
            // TODO 환웅 : 로컬라이징 작업
            string value = node.Attributes.GetNamedItem("kr").Value;
            LocalizeList.Add(key, value);
        }
    }

    public string GetLocalizeString(string key)
    {
        if (LocalizeList.ContainsKey(key))
            return LocalizeList[key];

        return key;
    }

    public string GetLocalizeString(string key, params object[] args)
    {
        return string.Format(GetLocalizeString(key), args);
    }
}

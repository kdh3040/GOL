using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ConfigData {

    public static ConfigData _instance = null;
    public static ConfigData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ConfigData();
            }
            return _instance;
        }
    }

    public float COMBO_KEEP_TIME;

    public void Initialize(XmlNodeList list)
    {
        foreach (XmlNode node in list)
        {
            string key = node.Attributes.GetNamedItem("id").Value;

            if (key.Equals("COMBO_KEEP_TIME"))
                COMBO_KEEP_TIME = GetFloatValue(node);
        }
    }

    public int GetIntValue(XmlNode node)
    {
        return int.Parse(node.Attributes.GetNamedItem("value").Value);
    }

    public float GetFloatValue(XmlNode node)
    {
        return float.Parse(node.Attributes.GetNamedItem("value").Value);
    }
}

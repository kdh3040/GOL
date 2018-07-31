﻿using System.Collections;
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
    public float DEFAULT_NOTE_SPEED;
    public float NOTE_CREATE_INTERVAL;
    public int NOTE_CREATE_SAMETIME_MAX_COUNT;
    public float NOTE_ITEM_CREATE_PERCENT;
    public int NOTE_ITEM_SCORE;
    public int MAX_USE_SHIELD_ITEM;
    public int MAX_DDONG_COUNT;
    public int DDONG_REFIL_TIME;
    public int REVIVAL_COST;

    public void Initialize(XmlNodeList list)
    {
        foreach (XmlNode node in list)
        {
            string key = node.Attributes.GetNamedItem("id").Value;

            if (key.Equals("COMBO_KEEP_TIME"))
                COMBO_KEEP_TIME = GetFloatValue(node);
            else if (key.Equals("DEFAULT_NOTE_SPEED"))
                DEFAULT_NOTE_SPEED = GetFloatValue(node);
            else if (key.Equals("NOTE_CREATE_INTERVAL"))
                NOTE_CREATE_INTERVAL = GetFloatValue(node);
            else if (key.Equals("NOTE_CREATE_SAMETIME_MAX_COUNT"))
                NOTE_CREATE_SAMETIME_MAX_COUNT = GetIntValue(node);
            else if (key.Equals("NOTE_ITEM_CREATE_PERCENT"))
                NOTE_ITEM_CREATE_PERCENT = GetFloatValue(node);
            else if (key.Equals("NOTE_ITEM_SCORE"))
                NOTE_ITEM_SCORE = GetIntValue(node);
            else if (key.Equals("MAX_USE_SHIELD_ITEM"))
                MAX_USE_SHIELD_ITEM = GetIntValue(node);
            else if (key.Equals("MAX_DDONG_COUNT"))
                MAX_DDONG_COUNT = GetIntValue(node);
            else if (key.Equals("DDONG_REFIL_TIME"))
                DDONG_REFIL_TIME = GetIntValue(node);
            else if (key.Equals("REVIVAL_COST"))
                REVIVAL_COST = GetIntValue(node);
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

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class ItemData
{
    public int id;
    public string name;
    public string desc;
    public string skill;
    public string icon;
    public string note_img;
    public int create_probability;
    public CommonData.ITEM_SLOT_TYPE slot_type;

    public ItemData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        desc = node.Attributes.GetNamedItem("desc").Value;
        skill = node.Attributes.GetNamedItem("skill").Value;
        icon = node.Attributes.GetNamedItem("icon").Value;
        note_img = node.Attributes.GetNamedItem("note_img").Value;
        create_probability = int.Parse(node.Attributes.GetNamedItem("create_probability").Value);

        switch(node.Attributes.GetNamedItem("slot_type").Value)
        {
            case "normal":
                slot_type = CommonData.ITEM_SLOT_TYPE.NORMAL;
                break;
            case "shield":
                slot_type = CommonData.ITEM_SLOT_TYPE.SHIELD;
                break;

        }
    }

    public string GetLocalizeName()
    {
        return LocalizeData.Instance.GetLocalizeString(name);
    }

    public string GetLocalizeDesc()
    {
        return LocalizeData.Instance.GetLocalizeString(desc);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class ItemData
{
    public int id;
    public string name;
    public string skill;
    public string icon;
    public int cost;
    public int create_probability;
    public CommonData.ITEM_SLOT_TYPE slot_type;

    public ItemData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        skill = node.Attributes.GetNamedItem("skill").Value;
        icon = node.Attributes.GetNamedItem("icon").Value;
        cost = int.Parse(node.Attributes.GetNamedItem("cost").Value);
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
}
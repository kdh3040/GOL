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
    public string slot_type;

    public ItemData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        skill = node.Attributes.GetNamedItem("skill").Value;
        icon = node.Attributes.GetNamedItem("icon").Value;
        cost = int.Parse(node.Attributes.GetNamedItem("cost").Value);
        create_probability = int.Parse(node.Attributes.GetNamedItem("create_probability").Value);
        slot_type = node.Attributes.GetNamedItem("slot_type").Value;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class CharData : SkinData
{
    public string skill;

    public CharData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        desc = node.Attributes.GetNamedItem("desc").Value;
        icon = node.Attributes.GetNamedItem("icon").Value;
        cost = int.Parse(node.Attributes.GetNamedItem("cost").Value);
        skill = node.Attributes.GetNamedItem("skill").Value;
    }
}
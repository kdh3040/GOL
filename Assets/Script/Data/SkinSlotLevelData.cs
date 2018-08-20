using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class SkinSlotLevelData
{
    public int level;
    public string skill;
    public int cost;

    public SkinSlotLevelData(XmlNode node)
    {
        level = int.Parse(node.Attributes.GetNamedItem("level").Value);
        skill = node.Attributes.GetNamedItem("skill").Value;
        cost = int.Parse(node.Attributes.GetNamedItem("cost").Value);
    }
}

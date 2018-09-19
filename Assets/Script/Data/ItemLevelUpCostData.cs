using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class ItemLevelUpCostData
{
    public string name;
    public int cost;

    public ItemLevelUpCostData(XmlNode node)
    {
        name = node.Attributes.GetNamedItem("name").Value;
        cost = int.Parse(node.Attributes.GetNamedItem("cost").Value);
    }
}
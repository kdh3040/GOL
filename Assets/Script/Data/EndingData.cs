using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class EndingData : SkinData
{
    public string img;

    public EndingData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        desc = node.Attributes.GetNamedItem("desc").Value;
        img = node.Attributes.GetNamedItem("img").Value;
        cost = int.Parse(node.Attributes.GetNamedItem("cost").Value);
    }

    public override string GetIcon()
    {
        return "";
    }
}

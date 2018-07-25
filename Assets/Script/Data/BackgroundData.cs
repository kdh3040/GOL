using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class BackgroundData : SkinData
{
    public string img_front;
    public string img_back;

    public BackgroundData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        desc = node.Attributes.GetNamedItem("desc").Value;
        icon = node.Attributes.GetNamedItem("icon").Value;
        cost = int.Parse(node.Attributes.GetNamedItem("cost").Value);
        img_front = node.Attributes.GetNamedItem("img_front").Value;
        img_back = node.Attributes.GetNamedItem("img_back").Value;
    }

    public override string GetIcon()
    {
        return icon;
    }
}
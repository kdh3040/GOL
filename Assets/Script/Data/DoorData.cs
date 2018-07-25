using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class DoorData : SkinData
{
    public string close_img;
    public string halfopen_img;
    public string open_img;
    public bool[] img_twist = new bool[3];

    public DoorData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        desc = node.Attributes.GetNamedItem("desc").Value;
        icon = node.Attributes.GetNamedItem("icon").Value;
        cost = int.Parse(node.Attributes.GetNamedItem("cost").Value);
        close_img = node.Attributes.GetNamedItem("close_img").Value;
        halfopen_img = node.Attributes.GetNamedItem("halfopen_img").Value;
        open_img = node.Attributes.GetNamedItem("open_img").Value;
        img_twist[0] = bool.Parse(node.Attributes.GetNamedItem("img_twist_1").Value);
        img_twist[1] = bool.Parse(node.Attributes.GetNamedItem("img_twist_2").Value);
        img_twist[2] = bool.Parse(node.Attributes.GetNamedItem("img_twist_3").Value);
    }

    public override string GetIcon()
    {
        return icon;
    }
}

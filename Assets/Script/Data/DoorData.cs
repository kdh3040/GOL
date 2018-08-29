using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class DoorData : SkinData
{
    public string close_img;
    public string halfopen_img;
    public string open_img;

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
    }

    public override string GetIcon()
    {
        return icon;
    }

    public override string GetSkinSlotTypeName()
    {
        return LocalizeData.Instance.GetLocalizeString("SKIN_SLOT_DOOR");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class CharData : SkinData
{
    
    public string img;
    public string ani_trigger;
    public string shopani_trigger;

    public CharData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        desc = node.Attributes.GetNamedItem("desc").Value;
        icon = node.Attributes.GetNamedItem("icon").Value;
        cost = int.Parse(node.Attributes.GetNamedItem("cost").Value);
        skill = node.Attributes.GetNamedItem("skill").Value;
        img = node.Attributes.GetNamedItem("img").Value;
        ani_trigger = node.Attributes.GetNamedItem("ani_trigger").Value;
        shopani_trigger = node.Attributes.GetNamedItem("shopani_trigger").Value;
    }

    public override string GetIcon()
    {
        return icon;
    }

    public override string GetSkinSlotTypeName()
    {
        return LocalizeData.Instance.GetLocalizeString("SKIN_SLOT_CHAR");
    }
}
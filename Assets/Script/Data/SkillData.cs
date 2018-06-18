using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class SkillData
{
    public int id;
    public string name;
    public string desc;
    public float value1;
    public float value2;
    public float value3;
    public string skilltype;
    public string checktype;

    public SkillData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        desc = node.Attributes.GetNamedItem("desc").Value;
        skilltype = node.Attributes.GetNamedItem("skilltype").Value;
        checktype = node.Attributes.GetNamedItem("checktype").Value;
        value1 = float.Parse(node.Attributes.GetNamedItem("value1").Value);
        value2 = float.Parse(node.Attributes.GetNamedItem("value2").Value);
        value3 = float.Parse(node.Attributes.GetNamedItem("value3").Value);
    }
}
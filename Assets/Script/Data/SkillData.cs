using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class SkillData
{
    public int id;
    public string name;
    public string desc;
    public float time;
    public float count;
    public float percent;
    public string skilltype;

    public SkillData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        desc = node.Attributes.GetNamedItem("desc").Value;
        skilltype = node.Attributes.GetNamedItem("skilltype").Value;
        time = float.Parse(node.Attributes.GetNamedItem("time").Value);
        count = float.Parse(node.Attributes.GetNamedItem("count").Value);
        percent = float.Parse(node.Attributes.GetNamedItem("percent").Value);
    }
}
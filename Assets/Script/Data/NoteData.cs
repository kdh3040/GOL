using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class NoteData {

    public int id = 0;
    public int Score = 1;
    public string img;
    public string endingName;
    public string ani_trigger;

    public NoteData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        Score = int.Parse(node.Attributes.GetNamedItem("score").Value);
        img = node.Attributes.GetNamedItem("img").Value;
        endingName = node.Attributes.GetNamedItem("ending").Value;
        ani_trigger = node.Attributes.GetNamedItem("ani_trigger").Value;
    }
}

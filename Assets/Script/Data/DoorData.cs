using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class DoorData {

    public int id;
    public string Img;

    public DoorData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        Img = node.Attributes.GetNamedItem("Img").Value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class CharMsgData
{
    public int id;
    public string Msg;

    public CharMsgData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        Msg = node.Attributes.GetNamedItem("msg").Value;
    }
}

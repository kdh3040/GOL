using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class EndingGroupData
{
    public int id;
    public string name;
    public List<int> ending_list = new List<int>();
    public string img;

    public EndingGroupData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;

        var endingListString = node.Attributes.GetNamedItem("ending_list").Value;
        var endingListStringArr = endingListString.Split(',');
        for (int i = 0; i < endingListStringArr.Length; i++)
        {
            ending_list.Add(int.Parse(endingListStringArr[i]));
        }
    }
}

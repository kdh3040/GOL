using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class NoteData {

    public int id = 0;
    public int Score = 1;
    public string img;
    public int Probability = 0;
    public List<string> EndDescList = new List<string>();

    public NoteData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        Score = int.Parse(node.Attributes.GetNamedItem("score").Value);
        img = node.Attributes.GetNamedItem("img").Value;
        Probability = int.Parse(node.Attributes.GetNamedItem("probability").Value);

        var endDescString = node.Attributes.GetNamedItem("end_desc").Value;
        var endDescStringArr = endDescString.Split(',');
        for (int i = 0; i < endDescStringArr.Length; i++)
        {
            EndDescList.Add(endDescStringArr[i].Replace(" ", ""));
        }
    }

    public string GetEndDesc()
    {
        return LocalizeData.Instance.GetLocalizeString(EndDescList[Random.Range(0, EndDescList.Count)]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class BackgroundData : SkinData
{
    public string img_front;
    public string img_back;
    public List<int> noteList = new List<int>();
    public List<int> endingList = new List<int>();
    public string endingTitle;

    public BackgroundData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        desc = node.Attributes.GetNamedItem("desc").Value;
        icon = node.Attributes.GetNamedItem("icon").Value;
        cost = int.Parse(node.Attributes.GetNamedItem("cost").Value);
        img_front = node.Attributes.GetNamedItem("img_front").Value;
        img_back = node.Attributes.GetNamedItem("img_back").Value;

        var noteListString = node.Attributes.GetNamedItem("note_list").Value;
        var noteListStringArr = noteListString.Split(',');
        for (int i = 0; i < noteListStringArr.Length; i++)
        {
            noteList.Add(int.Parse(noteListStringArr[i]));
        }

        var endingListString = node.Attributes.GetNamedItem("ending_list").Value;
        var endingListStringArr = noteListString.Split(',');
        for (int i = 0; i < endingListStringArr.Length; i++)
        {
            endingList.Add(int.Parse(noteListStringArr[i]));
        }

        endingTitle = node.Attributes.GetNamedItem("ending_title").Value;
    }

    public override string GetIcon()
    {
        return icon;
    }
}
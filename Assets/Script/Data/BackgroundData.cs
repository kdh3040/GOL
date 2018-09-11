using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class BackgroundData : SkinData
{
    public string img_back;
    public string img_front;
    public List<int> noteList = new List<int>();
    public List<int> endingGroupList = new List<int>();

    public BackgroundData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        desc = node.Attributes.GetNamedItem("desc").Value;
        icon = node.Attributes.GetNamedItem("icon").Value;
        cost = int.Parse(node.Attributes.GetNamedItem("cost").Value);
        img_front = node.Attributes.GetNamedItem("img_front").Value;
        img_back = node.Attributes.GetNamedItem("img_back").Value;
        skill = node.Attributes.GetNamedItem("skill").Value;

        var noteListString = node.Attributes.GetNamedItem("note_list").Value;
        var noteListStringArr = noteListString.Split(',');
        for (int i = 0; i < noteListStringArr.Length; i++)
        {
            noteList.Add(int.Parse(noteListStringArr[i]));
        }

        var endingListString = node.Attributes.GetNamedItem("ending_group_list").Value;
        var endingListStringArr = endingListString.Split(',');
        for (int i = 0; i < endingListStringArr.Length; i++)
        {
            endingGroupList.Add(int.Parse(endingListStringArr[i]));
        }
    }

    public override string GetIcon()
    {
        return icon;
    }

    public override string GetSkinSlotTypeName()
    {
        return LocalizeData.Instance.GetLocalizeString("SKIN_SLOT_BG");
    }
}
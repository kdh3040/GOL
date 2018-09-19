using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class BackgroundData : SkinData
{
    public string img_back;
    public string img_front;
    public string img_main;
    public List<int> noteList = new List<int>();
    public List<int> endingGroupList = new List<int>();
    public float speed_default;
    public float speed_interval;
    public float speed_up;
    public string buy_bg;

    public BackgroundData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        desc = node.Attributes.GetNamedItem("desc").Value;
        icon = node.Attributes.GetNamedItem("icon").Value;
        cost = int.Parse(node.Attributes.GetNamedItem("cost").Value);
        img_front = node.Attributes.GetNamedItem("img_front").Value;
        img_back = node.Attributes.GetNamedItem("img_back").Value;
        img_main = node.Attributes.GetNamedItem("img_main").Value;
        speed_default = float.Parse(node.Attributes.GetNamedItem("speed_default").Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        speed_interval = float.Parse(node.Attributes.GetNamedItem("speed_interval").Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        speed_up = float.Parse(node.Attributes.GetNamedItem("speed_up").Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        skill = node.Attributes.GetNamedItem("skill").Value;
        buy_bg = node.Attributes.GetNamedItem("buy_bg").Value;

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
            int id = int.Parse(endingListStringArr[i]);
            endingGroupList.Add(id);
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

    public string GetLocalizeNameReady()
    {
        return LocalizeData.Instance.GetLocalizeString(string.Format("{0}_READY",name));
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class EndingData : SkinData
{
    public string img;

    public EndingData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        desc = node.Attributes.GetNamedItem("desc").Value;
        img = node.Attributes.GetNamedItem("img").Value;
    }

    public override string GetIcon()
    {
        var enumerator = DataManager.Instance.NoteDataDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Value.endingName == name)
                return enumerator.Current.Value.img;
        }
        return "";
    }
}

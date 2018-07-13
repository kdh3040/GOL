using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class NoteCreateData
{
    public int id;
    public float time;
    public List<KeyValuePair<int, int>> noteCreateList = new List<KeyValuePair<int, int>>();
    public int noteCreateAllPercent = 0;

    public NoteCreateData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        time = float.Parse(node.Attributes.GetNamedItem("time").Value);

        for (int i = 1; i <= 5; i++)
        {
            var noteId = int.Parse(node.Attributes.GetNamedItem(string.Format("note_{0}", i)).Value);
            if(noteId != 0)
            {
                var noteCreatePercent = int.Parse(node.Attributes.GetNamedItem(string.Format("note_{0}_percent", i)).Value);
                noteCreateAllPercent += noteCreatePercent;

                KeyValuePair<int, int> data = new KeyValuePair<int, int>(noteId, noteCreatePercent);
                noteCreateList.Add(data);
            }
        }
    }
}
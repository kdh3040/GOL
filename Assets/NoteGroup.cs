using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGroup : MonoBehaviour {
    public Note[] NoteList = new Note[3];

    public void SetNoteData(List<KeyValuePair<CommonData.NOTE_TYPE, int>> noteIdList)
    {
        for (int i = 0; i < noteIdList.Count; i++)
        {
            NoteList[i].SetNote(noteIdList[i].Key, noteIdList[i].Value);
        }
    }

    public bool IsAliveNote()
    {
        for (int i = 0; i < NoteList.Length; i++)
        {
            if (NoteList[i].NoteType != CommonData.NOTE_TYPE.NONE)
                return true;
        }

        return false;
    }

    public void TouchNote(CommonData.NOTE_LINE type)
    {

    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGroup : MonoBehaviour {
    public Note[] NoteList = new Note[3];

    public void ResetNoteGroup()
    {
        for (int i = 0; i < NoteList.Length; i++)
        {
            NoteList[i].ResetNote();
        }
    }

    public void SetNoteData(List<KeyValuePair<CommonData.NOTE_TYPE, int>> noteIdList)
    {
        for (int i = 0; i < noteIdList.Count; i++)
        {
            NoteList[i].SetNote((CommonData.NOTE_LINE)i, noteIdList[i].Key, noteIdList[i].Value);
        }
    }

    public Note IsDoorOpenNote()
    {
        for (int i = 0; i < NoteList.Length; i++)
        {
            if (NoteList[i].NoteType != CommonData.NOTE_TYPE.NONE)
                return NoteList[i];
        }

        return null;
    }

    public bool DeleteNote(CommonData.NOTE_LINE type, bool touchDelete = true)
    {
        var note = NoteList[(int)type];
        var noteType = note.NoteType;
        var noteId = note.NoteId;

        if(noteType != CommonData.NOTE_TYPE.NONE)
        {
            switch (noteType)
            {
                case CommonData.NOTE_TYPE.NORMAL:
                    var noteData = DataManager.Instance.NoteDataDic[noteId];
                    GamePlayManager.Instance.PlusScore(noteData.Score);
                    break;
                case CommonData.NOTE_TYPE.ITEM:
                    if(touchDelete)
                        GamePlayManager.Instance.PlusItem(noteId);
                    break;
                default:
                    break;
            }

            GamePlayManager.Instance.DeleteNoteAni(note);
            note.ResetNote();

            return true;
        }


        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour {

    public static NoteManager _instance = null;
    public static NoteManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<NoteManager>() as NoteManager;
            }
            return _instance;
        }
    }


    public Transform NoteStartPos_1;
    public Transform NoteEndPos_1;
    public Transform NoteStartPos_2;
    public Transform NoteEndPos_2;
    public Transform NoteStartPos_3;
    public Transform NoteEndPos_3;

    private Dictionary<CommonData.NOTE_POS_TYPE, Transform[]> NotePosDic = new Dictionary<CommonData.NOTE_POS_TYPE, Transform[]>();
    private Dictionary<CommonData.NOTE_POS_TYPE, List<Note>> GameNoteList = new Dictionary<CommonData.NOTE_POS_TYPE, List<Note>>();
    private float SaveTime;

    public NoteManager()
    {
        
    }

    public void Initialize()
    {
        NotePosDic.Clear();
        NotePosDic.Add(CommonData.NOTE_POS_TYPE.INDEX_1, new Transform[2]);
        NotePosDic.Add(CommonData.NOTE_POS_TYPE.INDEX_2, new Transform[2]);
        NotePosDic.Add(CommonData.NOTE_POS_TYPE.INDEX_3, new Transform[2]);
        NotePosDic[CommonData.NOTE_POS_TYPE.INDEX_1][0] = NoteStartPos_1;
        NotePosDic[CommonData.NOTE_POS_TYPE.INDEX_1][1] = NoteEndPos_1;
        NotePosDic[CommonData.NOTE_POS_TYPE.INDEX_2][0] = NoteStartPos_2;
        NotePosDic[CommonData.NOTE_POS_TYPE.INDEX_2][1] = NoteEndPos_2;
        NotePosDic[CommonData.NOTE_POS_TYPE.INDEX_3][0] = NoteStartPos_3;
        NotePosDic[CommonData.NOTE_POS_TYPE.INDEX_3][1] = NoteEndPos_3;
    }

    public void NoteUpdate(float time)
    {
        SaveTime += time;

        if (SaveTime > Random.Range(1.1f, 1.3f))
        {
            SaveTime = 0;

            CreateNote((CommonData.NOTE_POS_TYPE)Random.Range((int)CommonData.NOTE_POS_TYPE.INDEX_1, (int)CommonData.NOTE_POS_TYPE.MAX));
        }

        var enumerator = GameNoteList.GetEnumerator();
        while(enumerator.MoveNext())
        {
            for (int index = 0; index < enumerator.Current.Value.Count; ++index)
            {
                enumerator.Current.Value[index].NoteUpdate(time);
            }
        }


        // 임시
        var enumerator_2 = GameNoteList.GetEnumerator();
        while (enumerator_2.MoveNext())
        {
            for (int index = 0; index < enumerator_2.Current.Value.Count; ++index)
            {
                if (enumerator_2.Current.Value[index].DestroyReady)
                {
                    // TODO 환웅 : 게임 오버
                    DeleteNote(enumerator_2.Current.Value[index], false);
                    break;
                }
            }
        }
    }

    public void CheckNote(Door door)
    {
        if (GameNoteList.ContainsKey(door.NoteType) == false)
            return;

        var list = GameNoteList[door.NoteType];
        for (int index = 0; index < list.Count; ++index)
        {
            var target = list[index].gameObject;
            var distance = Vector3.Distance(target.transform.position, door.gameObject.transform.position);

            // TODO 환웅 : 거리에 따른 점수 계산이 달라져야 할듯
            if (distance < 1.0f)
            {
                DeleteNote(list[index]);
                break;
            }
        }
    }

    public void DeleteNote(Note note, bool realDelete = true)
    {
        var type = note.NotePosType;
        if (GameNoteList.ContainsKey(type))
        {
            var list = GameNoteList[type];
            list.Remove(note);
            DestroyImmediate(note.gameObject);
            if(realDelete)
            {
                GManager.Instance.PlusScore(1);
                GManager.Instance.PlusCombo();
            }
        }
    }

    public void CreateNote(CommonData.NOTE_POS_TYPE type)
    {
        var obj = Instantiate(Resources.Load("Prefab/Note")) as GameObject;
        var note = obj.GetComponent<Note>();
        note.SetNoteData(type, NotePosDic[type][0], NotePosDic[type][1]);
        if (GameNoteList.ContainsKey(type) == false)
            GameNoteList.Add(type, new List<Note>());

        GameNoteList[type].Add(note);
    }
}

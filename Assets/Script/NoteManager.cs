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

        if (SaveTime > Random.Range(0.1f, 0.3f))
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
                if(enumerator_2.Current.Value[index].DestroyReady)
                {
                    DeleteNote(enumerator_2.Current.Value[index]);
                    break;
                }
            }
        }
    }

    public void CheckNote(Door door)
    {



        //var doorRect = new Rect(door.gameObject.transform.position.x, door.gameObject.transform.position.y, door.Collider.size.x, door.Collider.size.y);
        //// 선택한 문이랑 가까운 노트를 찾는다
        //Rect.
        //for (int index = 0; index < GameNoteList.Count; ++index)
        //{
        //    GameNoteList[index].NoteUpdate(time);
        //}
    }

    public void DeleteNote(Note note)
    {
        var type = note.NotePosType;
        if (GameNoteList.ContainsKey(type))
        {
            var list = GameNoteList[type];
            for (int index = list.Count - 1; index >= 0; --index)
            {
                if (list[index].DestroyReady)
                {
                    DestroyImmediate(note.gameObject);
                    list.RemoveAt(index);
                    GManager.Instance.PlusScore(1);
                    GManager.Instance.PlusCombo();
                }
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

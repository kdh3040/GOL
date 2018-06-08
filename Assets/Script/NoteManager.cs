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
    private List<Note> GameNoteList = new List<Note>(); // NOTE WOONG : 오브젝트 풀 만들어보자
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

            var obj = Instantiate(Resources.Load("Prefab/Note")) as GameObject;
            var note = obj.GetComponent<Note>();
            int typeIndex = Random.Range((int)CommonData.NOTE_POS_TYPE.INDEX_1, (int)CommonData.NOTE_POS_TYPE.MAX);
            var ttt = (CommonData.NOTE_POS_TYPE)typeIndex;
            var posArr = NotePosDic[ttt];
            note.SetNoteData((CommonData.NOTE_POS_TYPE)typeIndex, posArr[0], posArr[1]);
            GameNoteList.Add(note);
        }

        for (int index = 0; index < GameNoteList.Count; ++index)
        {
            GameNoteList[index].NoteUpdate(time);
        }

        for (int index = GameNoteList.Count - 1; index >= 0; --index)
        {
            if (GameNoteList[index].DestroyReady)
            {
                DestroyImmediate(GameNoteList[index].gameObject);
                GameNoteList.RemoveAt(index);
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
}

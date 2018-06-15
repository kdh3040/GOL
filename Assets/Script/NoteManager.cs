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
    
    // NOTE 환웅 : 노트와 관련된 기능들만 사용하는 매니저


    public Transform NoteStartPos_1;
    public Transform NoteEndPos_1;
    public Transform NoteStartPos_2;
    public Transform NoteEndPos_2;
    public Transform NoteStartPos_3;
    public Transform NoteEndPos_3;

    private Dictionary<CommonData.NOTE_POS_TYPE, Transform[]> NotePosDic = new Dictionary<CommonData.NOTE_POS_TYPE, Transform[]>();
    private Dictionary<CommonData.NOTE_POS_TYPE, List<Note>> NoteList = new Dictionary<CommonData.NOTE_POS_TYPE, List<Note>>();
    private List<Note> DeleteReadyNoteList = new List<Note>();
    [System.NonSerialized]
    public float Speed = 0f;
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

    public void ResetNote()
    {
        var enumerator = NoteList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            for (int index = enumerator.Current.Value.Count - 1; index >= 0 ; --index)
            {
                DeleteNote(enumerator.Current.Value[index], false);
            }

            enumerator.Current.Value.Clear();
        }

        DeleteReadyNoteList.Clear();

        Speed = ConfigData.Instance.DEFAULT_NOTE_SPEED;
    }

    public void NoteUpdate(float time)
    {
        SaveTime += time;

        // TODO 환웅 : 노트 생성의 시스템화가 필요
        if (SaveTime > Random.Range(0.1f, 0.3f))
        {
            SaveTime = 0;

            CreateNote((CommonData.NOTE_POS_TYPE)Random.Range((int)CommonData.NOTE_POS_TYPE.INDEX_1, (int)CommonData.NOTE_POS_TYPE.MAX));
        }

        var enumerator = NoteList.GetEnumerator();
        while(enumerator.MoveNext())
        {
            for (int index = 0; index < enumerator.Current.Value.Count; ++index)
            {
                enumerator.Current.Value[index].NoteUpdate(time);
            }
        }

        for (int i = 0; i < DeleteReadyNoteList.Count; i++)
        {
            DeleteNote(DeleteReadyNoteList[i], false);
        }
        DeleteReadyNoteList.Clear();
    }

    public void DeleteCheckNote(Door door)
    {
        if (NoteList.ContainsKey(door.NoteType) == false)
            return;

        var list = NoteList[door.NoteType];
        for (int index = list.Count - 1; index >= 0; --index)
        {
            var target = list[index].gameObject;
            var distance = Vector3.Distance(target.transform.position, door.gameObject.transform.position);

            // TODO 환웅 : 노트의 충돌처리에 대한 시스템화가 필요
            if (distance < 1.0f)
            {
                DeleteNote(list[index]);
            }
        }
    }

    public void AddDeleteReadyNote(Note note)
    {
        DeleteReadyNoteList.Add(note);
    }

    public void DeleteNote(Note note, bool score = true)
    {
        var type = note.NotePosType;
        if (NoteList.ContainsKey(type))
        {
            var list = NoteList[type];
            list.Remove(note);
            DestroyImmediate(note.gameObject);
            if(score)
                GManager.Instance.PlusScore(note.Id);
        }
    }

    public void CreateNote(CommonData.NOTE_POS_TYPE type)
    {
        // TODO 환웅 : 오브젝트 풀 추가 예정
        var obj = Instantiate(Resources.Load("Prefab/Note")) as GameObject;
        var note = obj.GetComponent<Note>();
        note.SetNoteData(type, Random.Range(1, DataManager.Instance.NoteDataList.Count));
        if (NoteList.ContainsKey(type) == false)
            NoteList.Add(type, new List<Note>());

        NoteList[type].Add(note);
    }

    public Transform[] GetNoteTypeStartEndPos(CommonData.NOTE_POS_TYPE type)
    {
        return NotePosDic[type];
    }
}

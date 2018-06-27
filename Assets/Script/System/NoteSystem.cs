using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSystem
{
    private Dictionary<CommonData.NOTE_LINE, Transform[]> NoteLineDic = new Dictionary<CommonData.NOTE_LINE, Transform[]>();
    private Dictionary<CommonData.NOTE_LINE, List<Note>> NoteList = new Dictionary<CommonData.NOTE_LINE, List<Note>>();
    private List<Note> DeleteReadyNoteList = new List<Note>();
    private float SaveTime;
    public float AccumulateCreateNoteCount
    {
        get;
        private set;
    }
    public float NoteSpeed
    {
        get;
        private set;
    }

    public void Initialize(PlayScene scene)
    {
        ResetNote();
        NoteLineDic.Clear();
        NoteLineDic.Add(CommonData.NOTE_LINE.INDEX_1, new Transform[2]);
        NoteLineDic.Add(CommonData.NOTE_LINE.INDEX_2, new Transform[2]);
        NoteLineDic.Add(CommonData.NOTE_LINE.INDEX_3, new Transform[2]);
        NoteLineDic[CommonData.NOTE_LINE.INDEX_1][0] = scene.NoteStartPos_1;
        NoteLineDic[CommonData.NOTE_LINE.INDEX_1][1] = scene.NoteEndPos_1;
        NoteLineDic[CommonData.NOTE_LINE.INDEX_2][0] = scene.NoteStartPos_2;
        NoteLineDic[CommonData.NOTE_LINE.INDEX_2][1] = scene.NoteEndPos_2;
        NoteLineDic[CommonData.NOTE_LINE.INDEX_3][0] = scene.NoteStartPos_3;
        NoteLineDic[CommonData.NOTE_LINE.INDEX_3][1] = scene.NoteEndPos_3;
    }


    public void ResetNote()
    {
        AccumulateCreateNoteCount = 0;
        NoteSpeed = ConfigData.Instance.DEFAULT_NOTE_SPEED;
        AllDelete();
    }

    public void AllDelete()
    {
        var enumerator = NoteList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            for (int index = enumerator.Current.Value.Count - 1; index >= 0; --index)
            {
                DeleteNote(enumerator.Current.Value[index], false);
            }

            enumerator.Current.Value.Clear();
        }

        DeleteReadyNoteList.Clear();
    }

    public void NoteUpdate(float time)
    {
        SaveTime += time;

        // TODO 환웅 : 노트 생성의 시스템화가 필요
        if (SaveTime > ConfigData.Instance.NOTE_CREATE_INTERVAL)
        {
            SaveTime = 0;
            int createCount = Random.Range(1, ConfigData.Instance.NOTE_CREATE_SAMETIME_MAX_COUNT + 1);
            List<CommonData.NOTE_LINE> createTypeList = new List<CommonData.NOTE_LINE>();

            for (int i = 0; i < createCount; i++)
            {
                var createNotePosType = (CommonData.NOTE_LINE)Random.Range((int)CommonData.NOTE_LINE.INDEX_1, (int)CommonData.NOTE_LINE.MAX);
                bool createEnable = false;

                while(createEnable == false)
                {
                    createEnable = true;
                    for (int index = 0; index < createTypeList.Count; index++)
                    {
                        if (createTypeList[index] == createNotePosType)
                        {
                            createEnable = false;
                            break;
                        }
                    }

                    if (createEnable)
                        break;
                    else
                        createNotePosType = (CommonData.NOTE_LINE)Random.Range((int)CommonData.NOTE_LINE.INDEX_1, (int)CommonData.NOTE_LINE.MAX);
                }

                var craeteItemNoteId = PickItemNoteId();
                if (Random.Range(0f, 100.0f) < ConfigData.Instance.NOTE_ITEM_CREATE_PERCENT && craeteItemNoteId != 0)
                {
                    CreateItemNote(createNotePosType, craeteItemNoteId);
                }
                else
                {
                    CreateNormalNote(createNotePosType);
                }

                createTypeList.Add(createNotePosType);
            }
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
        if (NoteList.ContainsKey(door.NoteLineType) == false)
            return;

        var list = NoteList[door.NoteLineType];
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
        var type = note.NoteLineType;
        if (NoteList.ContainsKey(type))
        {
            var list = NoteList[type];
            list.Remove(note);
            GamePlayManager.Instance.DeleteNote(note, score);
        }
    }
    public void CreateNormalNote(CommonData.NOTE_LINE type)
    {
        // TODO 환웅 : 오브젝트 풀 추가 예정
        var note = GamePlayManager.Instance.CreateNote("Prefab/NoteNormal") as NoteNormal;
        note.SetNoteNormalData(type, NoteLineDic[type],  Random.Range(1, DataManager.Instance.NoteDataDic.Count));
        if (NoteList.ContainsKey(type) == false)
            NoteList.Add(type, new List<Note>());

        NoteList[type].Add(note);
        AccumulateCreateNoteCount++;
        UpdateNoteSpeed();
    }

    public void CreateItemNote(CommonData.NOTE_LINE type, int itemId)
    {
        // TODO 환웅 : 오브젝트 풀 추가 예정
        var note = GamePlayManager.Instance.CreateNote("Prefab/NoteItem") as NoteItem;
        note.SetNoteItemData(type, NoteLineDic[type], itemId);
        if (NoteList.ContainsKey(type) == false)
            NoteList.Add(type, new List<Note>());

        NoteList[type].Add(note);
        AccumulateCreateNoteCount++;
        UpdateNoteSpeed();
    }

    public Transform[] GetNoteTypeStartEndPos(CommonData.NOTE_LINE type)
    {
        return NoteLineDic[type];
    }

    public void UpdateNoteSpeed()
    {
        if ((AccumulateCreateNoteCount % 3) == 0)
        {
            NoteSpeed += 0.1f;
        }
    }

    public int PickItemNoteId()
    {
        var list = DataManager.Instance.ItemDataList_CreateProbability;
        int value = Random.Range(0, DataManager.Instance.ItemAllCreateProbability);
        int returnValueId = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if ((i == 0 && value <= list[i].create_probability) ||
                (i > 0 && value > list[i - 1].create_probability && value <= list[i].create_probability))
            {
                returnValueId = list[i].id;
                break;
            }
        }

        return returnValueId;
    }
}

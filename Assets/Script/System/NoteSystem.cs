using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSystem
{
    private class NoteCreateData
    {
        public int Percent = 0;
        public int OriginalPercent = 0;
        public int Id = 0;
        public float EnableTime = 0;
    }

    private List<NoteGroup> NoteGroupList = new List<NoteGroup>();
    private Transform NoteGroupEndPos;
    public float NoteSpeed { get; set; }
    private List<NoteCreateData> NoteCreatePercentList = new List<NoteCreateData>();
    private List<NoteCreateData> NoteItemCreatePercentList = new List<NoteCreateData>();
    private int NoteItemAllPercentValue = 0;

    private float PlayTime = 0;
    private float NoteSpeedCheckTime = 0;
    private float ItemNoteCreatePercent = 0;

    public void Initialize(PlayScene scene)
    {
        NoteGroupList = scene.NoteGroupList;
        NoteGroupEndPos = scene.NoteGroupEndPos;
    }
    public void ResetSystem()
    {
        NoteSpeed = ConfigData.Instance.DEFAULT_NOTE_SPEED;
        PlayTime = 0;
        NoteSpeedCheckTime = 0;
        ItemNoteCreatePercent = ConfigData.Instance.NOTE_ITEM_CREATE_PERCENT;

        var endPos = NoteGroupEndPos.localPosition;
        for (int i = 0; i < NoteGroupList.Count; i++)
        {
            NoteGroupList[i].gameObject.transform.localPosition = new Vector3(0, endPos.y + (i * CommonData.NOTE_GROUP_INTERVAL));
            NoteGroupList[i].ResetNoteGroup();
        }

        if(NoteCreatePercentList.Count <= 0)
        {
            int percentValue = 0;
            var list = DataManager.Instance.NoteCreateDataList;
            for (int index_1 = 0; index_1 < list.Count; index_1++)
            {
                for (int index_2 = 0; index_2 < list[index_1].noteCreateList.Count; index_2++)
                {
                    var data = list[index_1].noteCreateList[index_2];
                    NoteCreateData createData = new NoteCreateData();
                    createData.Percent = percentValue + data.Value;
                    createData.OriginalPercent = data.Value;
                    createData.Id = data.Key;
                    createData.EnableTime = list[index_1].time;
                    percentValue += data.Value;
                    NoteCreatePercentList.Add(createData);
                }
            }
        }

        if(NoteItemCreatePercentList.Count <= 0)
        {
            NoteItemAllPercentValue = 0;
            var enumerator = DataManager.Instance.ItemDataDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                NoteCreateData createData = new NoteCreateData();
                createData.Percent = NoteItemAllPercentValue + enumerator.Current.Value.create_probability;
                createData.OriginalPercent = enumerator.Current.Value.create_probability;
                createData.Id = enumerator.Current.Key;
                NoteItemAllPercentValue += enumerator.Current.Value.create_probability;
                NoteItemCreatePercentList.Add(createData);
            }
        }
    }

    public void GameStart()
    {
        ResetSystem();

        if (SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.ITEM_CREATE) != null)
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.ITEM_CREATE) as GameSkill_ItemCreate;
            ItemNoteCreatePercent += skill.mPercent;
        }
    }

    public void GameRestart()
    {
        ResetSystem();
    }

    public void GameExit()
    {
        ResetSystem();
    }

    public void NoteUpdate(float time)
    {
        PlayTime += time;
        NoteSpeedCheckTime += time;
        for (int i = 0; i < NoteGroupList.Count; i++)
        {
            var pos = NoteGroupList[i].gameObject.transform.localPosition;
            pos.y = pos.y - (NoteSpeed * time);
            NoteGroupList[i].gameObject.transform.localPosition = pos;

            if(NoteGroupEndPos.localPosition.y > pos.y)
            {
                if(NoteGroupList[i].IsAliveNote())
                {
                    var note = NoteGroupList[i].GetGameOverCheckNote();
                    if(note != null && GamePlayManager.Instance.IsGameOver(note.NoteLineType))
                    {
                        GamePlayManager.Instance.SetDoorState(note.NoteLineType, 0);
                        GamePlayManager.Instance.GameOver(note.NoteId);
                    }
                    else
                    {
                        int posIndex = i - 1;
                        if (posIndex < 0)
                            posIndex = NoteGroupList.Count - 1;

                        var nextPos = NoteGroupList[posIndex].transform.localPosition;
                        NoteGroupList[i].gameObject.transform.localPosition = new Vector3(0, nextPos.y + CommonData.NOTE_GROUP_INTERVAL);
                        AllocateNoteGroup(i);
                    }
                }
                else
                {
                    int posIndex = i - 1;
                    if (posIndex < 0)
                        posIndex = NoteGroupList.Count - 1;

                    var nextPos = NoteGroupList[posIndex].transform.localPosition;
                    NoteGroupList[i].gameObject.transform.localPosition = new Vector3(0, nextPos.y + CommonData.NOTE_GROUP_INTERVAL);
                    AllocateNoteGroup(i);
                }
            }
        }

        UpdateNoteSpeed();
    }

    private void UpdateNoteSpeed()
    {
        if(NoteSpeedCheckTime > ConfigData.Instance.NOTE_SPEED_UP_INTERVAL)
        {
            NoteSpeedCheckTime = 0;
            NoteSpeed += ConfigData.Instance.NOTE_SPEED_UP;
        }
    }

    private void AllocateNoteGroup(int index)
    {
        // 노트 생성
        int createCount = Random.Range(1, ConfigData.Instance.NOTE_CREATE_SAMETIME_MAX_COUNT + 1);
        List<KeyValuePair<CommonData.NOTE_TYPE, int>> createNoteList = new List<KeyValuePair<CommonData.NOTE_TYPE, int>>();

        for (int i = (int)CommonData.NOTE_LINE.INDEX_1; i <= (int)CommonData.NOTE_LINE.INDEX_3; i++)
        {
            createNoteList.Add(new KeyValuePair<CommonData.NOTE_TYPE, int>(CommonData.NOTE_TYPE.NONE, 0));
        }

        for (int i = 0; i < createCount; i++)
        {
            var createNoteLine = 0;
            while (true)
            {
                createNoteLine = Random.Range((int)CommonData.NOTE_LINE.INDEX_1, (int)CommonData.NOTE_LINE.INDEX_3 + 1);
                if(createNoteList[createNoteLine].Key == CommonData.NOTE_TYPE.NONE)
                    break;
            }
            

            var createNoteId = 0;
            var createNoteType = CommonData.NOTE_TYPE.NORMAL;
            if (Random.Range(0f, 100.0f) < ItemNoteCreatePercent)
            {
                createNoteType = CommonData.NOTE_TYPE.ITEM;
                createNoteId = PickItemNoteId();

                if (createNoteId == 0)
                    createNoteType = CommonData.NOTE_TYPE.NORMAL;
            }

            if (createNoteType == CommonData.NOTE_TYPE.NORMAL)
                createNoteId = PickNormalNoteId();

            createNoteList[createNoteLine] = new KeyValuePair<CommonData.NOTE_TYPE, int>(createNoteType, createNoteId);
        }

        NoteGroupList[index].SetNoteData(createNoteList);
    }

    private int PickNormalNoteId()
    {
        var allPercentValue = 0;

        for (int i = 0; i < NoteCreatePercentList.Count; i++)
        {
            if (NoteCreatePercentList[i].EnableTime < PlayTime)
                allPercentValue += NoteCreatePercentList[i].OriginalPercent;
        }

        var percentValue = Random.Range(0, allPercentValue + 1);

        for (int i = 0; i < NoteCreatePercentList.Count; i++)
        {
            if (percentValue <= NoteCreatePercentList[i].Percent)
                return NoteCreatePercentList[i].Id;
        }

        return 1;
    }

    private int PickItemNoteId()
    {
        var percentValue = Random.Range(0, NoteItemAllPercentValue + 1);

        for (int i = 0; i < NoteItemCreatePercentList.Count; i++)
        {
            if (percentValue <= NoteItemCreatePercentList[i].Percent)
                return NoteItemCreatePercentList[i].Id;
        }

        return 1;
    }

    public bool NoteDeleteCheck(Door door)
    {
        float minDistance = float.MaxValue;
        var minDistanceIndex = 0;
        for (int i = 0; i < NoteGroupList.Count; i++)
        {
            float distance = (NoteGroupList[i].transform.position.y - door.gameObject.transform.position.y);
            //var distance = Vector3.Distance(NoteGroupList[i].transform.position.y, door.gameObject.transform.position.y);

            //Debug.Log("minDistance : " + minDistance + " i : " + i);
            //Debug.Log("NoteGroupList : " + NoteGroupList[i].transform.position + " door : " + door.gameObject.transform.position);
            if (minDistance > distance)
            {
                minDistance = distance;
                minDistanceIndex = i;
            }   
        }

        //Debug.Log("minDistance : " + minDistance + " minDistanceIndex : " + minDistanceIndex + "door : " + door.NoteLineType);
        // NoteGroupList[i].gameObject.transform.localPosition
        if (minDistance < CommonData.NOTE_TOUCH_DELETE_INTERVAL)
            return NoteGroupList[minDistanceIndex].DeleteNote(door.NoteLineType);

        return false;
    }


    /*private Dictionary<CommonData.NOTE_LINE, Transform[]> NoteLineDic = new Dictionary<CommonData.NOTE_LINE, Transform[]>();
    private Dictionary<CommonData.NOTE_LINE, List<Note>> NoteList = new Dictionary<CommonData.NOTE_LINE, List<Note>>();
    private List<KeyValuePair<int, int>> NoteCreatePercentList = new List<KeyValuePair<int, int>>();
    private List<KeyValuePair<int, int>> NoteItemCreatePercentList = new List<KeyValuePair<int, int>>();
    private float NoteCreateTime = 0;
    private int NoteCreateDataIndex = 0;
    private int AllNoteCreatePercent = 0;
    private int AllNoteItemCreatePercent = 0;
    private List<Note> DeleteReadyNoteList = new List<Note>();
    private float SaveTime;
    private float AllPlayTime = 0;
    private float NoteItemCreatePercentOffset = 0;
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

    private void ResetSystem()
    {
        NoteAllDelete();
        AccumulateCreateNoteCount = 0;
        NoteSpeed = ConfigData.Instance.DEFAULT_NOTE_SPEED;
        AllPlayTime = 0;
        AllNoteCreatePercent = 0;
        NoteCreateTime = 0;
        NoteCreateDataIndex = 0;
        NoteCreatePercentList.Clear();
        NoteItemCreatePercentOffset = 0;
    }

    public void GameStart()
    {
        ResetSystem();

        if (NoteItemCreatePercentList.Count <= 0)
        {
            var itemEnumerator = DataManager.Instance.ItemDataDic.GetEnumerator();
            while (itemEnumerator.MoveNext())
            {
                var data = itemEnumerator.Current;
                AllNoteItemCreatePercent += data.Value.create_probability;
                NoteItemCreatePercentList.Add(new KeyValuePair<int, int>(data.Key, AllNoteItemCreatePercent));
            }

            NoteItemCreatePercentList.Sort(delegate (KeyValuePair<int, int> A, KeyValuePair<int, int> B)
            {
                if (A.Value > B.Value)
                    return 1;
                else
                    return -1;
            });
        }

        if (SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.ITEM_CREATE) != null)
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.ITEM_CREATE) as GameSkill_ItemCreate;
            NoteItemCreatePercentOffset = skill.mPercent * 100;
        } 
    }

    public void GameRestart()
    {
        ResetSystem();
    }

    public void GameExit()
    {
        ResetSystem();
    }

    public void NoteUpdate(float time)
    {
        SaveTime += time;
        AllPlayTime += time;

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
                if (Random.Range(0f, 100.0f) < (ConfigData.Instance.NOTE_ITEM_CREATE_PERCENT + NoteItemCreatePercentOffset) && craeteItemNoteId != 0)
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
            NoteDelete(DeleteReadyNoteList[i], false);
        }
        DeleteReadyNoteList.Clear();
    }

    private void NoteAllDelete()
    {
        var enumerator = NoteList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            for (int index = enumerator.Current.Value.Count - 1; index >= 0; --index)
            {
                NoteDelete(enumerator.Current.Value[index], false);
            }

            enumerator.Current.Value.Clear();
        }

        DeleteReadyNoteList.Clear();
    }

    public bool NoteDeleteCheck(Door door)
    {
        if (NoteList.ContainsKey(door.NoteLineType) == false)
            return false;

        bool deleteEnable = false;
        var list = NoteList[door.NoteLineType];
        for (int index = list.Count - 1; index >= 0; --index)
        {
            var target = list[index].gameObject;
            var distance = Vector3.Distance(target.transform.position, door.gameObject.transform.position);

            // TODO 환웅 : 노트의 충돌처리에 대한 시스템화가 필요
            if (distance < 1.0f)
            {
                var note = list[index];
                NoteDelete(note);
                deleteEnable = true;
            }
        }

        return deleteEnable;
    }

    public void NoteDeleteReady(Note note)
    {
        DeleteReadyNoteList.Add(note);
    }

    private void NoteDelete(Note note, bool score = true)
    {
        var type = note.NoteLineType;
        if (NoteList.ContainsKey(type))
        {
            var list = NoteList[type];
            list.Remove(note);
            GamePlayManager.Instance.DeleteNote(note, score);
        }
    }
    private void CreateNormalNote(CommonData.NOTE_LINE type)
    {
        // TODO 환웅 : 오브젝트 풀 추가 예정
        var note = GamePlayManager.Instance.CreateNote("Prefab/NoteNormal") as NoteNormal;
        note.SetNoteNormalData(type, NoteLineDic[type], PickItemNoteId());
        if (NoteList.ContainsKey(type) == false)
            NoteList.Add(type, new List<Note>());

        NoteList[type].Add(note);
        AccumulateCreateNoteCount++;
        UpdateNoteSpeed();
    }

    private void CreateItemNote(CommonData.NOTE_LINE type, int itemId)
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

    private void UpdateNoteSpeed()
    {
        if ((AccumulateCreateNoteCount % 3) == 0)
        {
            NoteSpeed += 0.1f;
        }
    }

    private int PickNoteId()
    {
        if (NoteCreateTime < AllPlayTime && DataManager.Instance.NoteCreateDataList.Count - 1 >= NoteCreateDataIndex)
        {
            var data = DataManager.Instance.NoteCreateDataList[NoteCreateDataIndex];
            NoteCreateTime = data.time;
            for (int i = 0; i < data.noteCreateList.Count; i++)
            {
                AllNoteCreatePercent += data.noteCreateList[i].Value;
                NoteCreatePercentList.Add(new KeyValuePair<int, int>(data.noteCreateList[i].Key, AllNoteCreatePercent));
            }
            NoteCreateDataIndex += 1;
            NoteCreatePercentList.Sort(delegate (KeyValuePair<int, int> A, KeyValuePair<int, int> B)
            {
                if (A.Value < B.Value)
                    return -1;
                else
                    return 1;
            });
        }

        var list = NoteCreatePercentList;
        int value = Random.Range(0, AllNoteCreatePercent);
        for (int i = 0; i < list.Count; i++)
        {
            if (value < list[i].Value)
                return list[i].Key;
        }

        return 0;
    }

    private int PickItemNoteId()
    {
        var list = NoteItemCreatePercentList;
        int value = Random.Range(0, AllNoteItemCreatePercent);
        for (int i = 0; i < list.Count; i++)
        {
            if (value < list[i].Value)
                return list[i].Key;
        }

        return 0;
    }*/
}

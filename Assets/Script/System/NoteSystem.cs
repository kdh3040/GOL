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
    }

    private List<NoteGroup> NoteGroupList = new List<NoteGroup>();
    private Transform NoteGroupEndPos;
    private Transform NoteGroupOpenPos;
    public float NoteSpeed { get; set; }
    private List<NoteCreateData> NoteNormalCreateList = new List<NoteCreateData>();
    private int NoteNormalAllPercentValue = 0;
    private List<NoteCreateData> NoteItemCreatePercentList = new List<NoteCreateData>();
    private int NoteItemAllPercentValue = 0;

    private float PlayTime = 0;
    private float NoteSpeedCheckTime = 0;
    private float ItemNoteCreatePercent = 0;

    public void Initialize(PlayScene scene)
    {
        NoteGroupList = scene.NoteGroupList;
        NoteGroupEndPos = scene.NoteGroupEndPos;
        NoteGroupOpenPos = scene.NoteGroupOpenPos;
    }
    public void ResetSystem()
    {
        NoteSpeed = ConfigData.Instance.DEFAULT_NOTE_SPEED;
        PlayTime = 0;
        NoteSpeedCheckTime = 0;
        ItemNoteCreatePercent = ConfigData.Instance.NOTE_ITEM_CREATE_PERCENT;
        NoteNormalCreateList.Clear();

        AllDeleteNote();
        if (NoteItemCreatePercentList.Count <= 0)
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

        var backgroundData = DataManager.Instance.BackGroundDataDic[PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.BACKGROUND)];
        NoteNormalCreateList.Clear();
        NoteNormalAllPercentValue = 0;
        var notelist = backgroundData.noteList;
        for (int i = 0; i < notelist.Count; i++)
        {
            var noteData = DataManager.Instance.NoteDataDic[notelist[i]];
            NoteCreateData createData = new NoteCreateData();
            createData.Percent = NoteNormalAllPercentValue + noteData.Probability;
            createData.OriginalPercent = noteData.Probability;
            createData.Id = noteData.id;
            NoteNormalAllPercentValue += noteData.Probability;
            NoteNormalCreateList.Add(createData);
        }
    }

    public void AllDeleteNote(bool deleteAni = false)
    {
        var endPos = NoteGroupEndPos.localPosition;
        for (int i = 0; i < NoteGroupList.Count; i++)
        {
            if(deleteAni)
            {
                for (int index_1 = 0; index_1 < NoteGroupList[i].NoteList.Length; index_1++)
                {
                    if (NoteGroupList[i].NoteList[index_1].NoteType != CommonData.NOTE_TYPE.NONE)
                        GamePlayManager.Instance.DeleteNoteAni(NoteGroupList[i].NoteList[index_1]);
                }
            }

            NoteGroupList[i].gameObject.transform.localPosition = new Vector3(0, endPos.y + (i * CommonData.NOTE_GROUP_INTERVAL));
            NoteGroupList[i].ResetNoteGroup();
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

    public void GameRevival()
    {
        NoteSpeed -= NoteSpeed * (float)ConfigData.Instance.CHAR_REVIVAL_NOTE_SPEED_DOWN_PERCENT * 0.01f;
        AllDeleteNote(true);
    }

    public void NoteUpdate(float time, float speedTime)
    {
        PlayTime += time;
        NoteSpeedCheckTime += time;
        for (int i = 0; i < NoteGroupList.Count; i++)
        {
            var pos = NoteGroupList[i].gameObject.transform.localPosition;
            pos.y = pos.y - (NoteSpeed * speedTime);
            NoteGroupList[i].gameObject.transform.localPosition = pos;

            if(NoteGroupEndPos.localPosition.y > pos.y)
            {
                if(NoteGroupList[i].IsAliveNote())
                {
                    var note = NoteGroupList[i].GetGameOverCheckNote();
                    if(note != null && GamePlayManager.Instance.IsGameOver(note.NoteLineType))
                    {
                        GamePlayManager.Instance.SetDoorState(note.NoteLineType, Door.DOOR_STATE.OPEN);
                        GamePlayManager.Instance.GameOver(note);
                    }
                    else
                    {
                        if(note != null)
                            NoteGroupList[i].DeleteNote(note.NoteLineType);
                        NoteGroupReset(i);
                    }
                }
                else
                {
                    NoteGroupReset(i);
                }
            }

            else if(NoteGroupOpenPos.localPosition.y > pos.y)
            {
                var note = NoteGroupList[i].IsDoorOpenNote();
                if (note != null)
                    GamePlayManager.Instance.SetDoorState(note.NoteLineType, Door.DOOR_STATE.HALF_OPEN);
            }
        }


        UpdateNoteSpeed();
    }

    private void NoteGroupReset(int index)
    {
        int posIndex = index - 1;
        if (posIndex < 0)
            posIndex = NoteGroupList.Count - 1;

        var nextPos = NoteGroupList[posIndex].transform.localPosition;
        NoteGroupList[index].gameObject.transform.localPosition = new Vector3(0, nextPos.y + CommonData.NOTE_GROUP_INTERVAL);
        AllocateNoteGroup(index);
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
        var percentValue = Random.Range(0, NoteNormalAllPercentValue + 1);

        for (int i = 0; i < NoteNormalCreateList.Count; i++)
        {
            if (percentValue <= NoteNormalCreateList[i].Percent)
                return NoteNormalCreateList[i].Id;
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
        //float minDistance = float.MaxValue;
        //var minDistanceIndex = 0;
        for (int i = 0; i < NoteGroupList.Count; i++)
        {

            if(NoteGroupList[i].transform.position.y < NoteGroupOpenPos.localPosition.y)
                return NoteGroupList[i].DeleteNote(door.NoteLineType);

            //float distance = (NoteGroupList[i].transform.position.y - NoteGroupOpenPos.localPosition.y);
            //if (minDistance > distance)
            //{
            //    minDistance = distance;
            //    minDistanceIndex = i;
            //}   
        }
        //if (minDistance < CommonData.NOTE_TOUCH_DELETE_INTERVAL)
        //    return NoteGroupList[minDistanceIndex].DeleteNote(door.NoteLineType);

        return false;
    }
}

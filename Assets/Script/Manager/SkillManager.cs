﻿using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class SkillManager
{
    public static SkillManager _instance = null;
    public static SkillManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SkillManager();
            }
            return _instance;
        }
    }

    public enum SKILL_TYPE
    {
        NONE,
        DAMAGE_SHIELD,
        SCORE_UP
    }

    public enum SKILL_CHECK_TYPE
    {
        NONE,
        TIME,
        COUNT
    }


    public Dictionary<string, SkillData> SkillDataList = new Dictionary<string, SkillData>();
    private Dictionary<SKILL_TYPE, List<GameSkill>> mUseSkillList = new Dictionary<SKILL_TYPE, List<GameSkill>>();

    public void Initialize(XmlNodeList list)
    {
        foreach (XmlNode node in list)
        {
            var data = new SkillData(node);
            SkillDataList.Add(data.name, data);
        }
    }
    public void ResetGame()
    {
        mUseSkillList.Clear();
    }
    public GameSkill AddUseSkill(string skillName)
    {
        SkillData skillData = SkillDataList[skillName];
        SKILL_TYPE skillType = ConvertSkillType(skillData.skilltype);
        SKILL_CHECK_TYPE skillCheckType = ConvertSkillCheckType(skillData.checktype);
        GameSkill data = null;

        if(skillType == SKILL_TYPE.DAMAGE_SHIELD)
        {
            if (skillCheckType == SKILL_CHECK_TYPE.COUNT)
                data = new GameSkill_Shield(skillName);
            else if (skillCheckType == SKILL_CHECK_TYPE.TIME)
                data = new GameSkill_Invincibility(skillName);
        }
        else if (skillType == SKILL_TYPE.SCORE_UP)
        {
            if (skillCheckType == SKILL_CHECK_TYPE.TIME)
                data = new GameSkill_ScoreUP(skillName);
        }

        if(data != null)
        {
            AddUseSkill(data);
        }

        return data;
    }

    private void AddUseSkill(GameSkill data)
    {
        var skillType = data.mSkillType;
        if (mUseSkillList.ContainsKey(skillType) == false)
            mUseSkillList.Add(skillType, new List<GameSkill>());

        bool plusSameSkill = false;
        for (int i = 0; i < mUseSkillList[skillType].Count; i++)
        {
            if (mUseSkillList[skillType][i].mSkillCheckType == data.mSkillCheckType)
            {
                plusSameSkill = true;
                mUseSkillList[skillType][i].PlusSameSkill();
            }
        }

        if(plusSameSkill == false)
        {
            data.StartSkill();
            mUseSkillList[skillType].Add(data);

            mUseSkillList[skillType].Sort(delegate (GameSkill A, GameSkill B)
            {
                if (A.mSkillCheckType < B.mSkillCheckType)
                    return 1;
                else
                    return -1;
            });
        }
    }

    public void UpdateSkill(float time)
    {
        var skillEnumerator = mUseSkillList.GetEnumerator();
        while (skillEnumerator.MoveNext())
        {
            var list = skillEnumerator.Current.Value;
            for (int i = list.Count - 1; i >= 0; --i)
            {
                list[i].SkillUpdate(time);
                if (list[i].mSkillRemoveReady)
                    list.RemoveAt(i);
            }
        }
    }

    public bool IsSkillEnable(SKILL_TYPE type)
    {
        if (mUseSkillList.ContainsKey(type) == false)
            return false;

        var list = mUseSkillList[type];
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].mEnable)
                return true;
        }

        return false;
    }

    public GameSkill GetGameSkill(SKILL_TYPE type, SKILL_CHECK_TYPE checkType)
    {
        if (mUseSkillList.ContainsKey(type) == false)
            return null;

        var list = mUseSkillList[type];
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].mSkillCheckType == checkType)
                return list[i];
        }

        return null;
    }

    public SKILL_TYPE ConvertSkillType(string str)
    {
        SKILL_TYPE type = SKILL_TYPE.NONE;
        switch (str)
        {
            case "DAMAGE_SHIELD":
                type = SKILL_TYPE.DAMAGE_SHIELD;
                break;
            case "SCORE_UP":
                type = SKILL_TYPE.SCORE_UP;
                break;
            default:
                type = SKILL_TYPE.NONE;
                break;
        }

        return type;
    }
    public SKILL_CHECK_TYPE ConvertSkillCheckType(string str)
    {
        SKILL_CHECK_TYPE type = SKILL_CHECK_TYPE.NONE;
        switch (str)
        {
            case "TIME":
                type = SKILL_CHECK_TYPE.TIME;
                break;
            case "COUNT":
                type = SKILL_CHECK_TYPE.COUNT;
                break;
            default:
                type = SKILL_CHECK_TYPE.NONE;
                break;
        }

        return type;
    }
}

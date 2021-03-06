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
        DAMAGE_SHIELD_TIME,
        DAMAGE_SHIELD_COUNT,
        SCORE_UP,
        SPEED_DOWN,
        RESURRECTION,
        GAME_OVER_SCORE_BONUS,
        GAME_OVER_COIN_BONUS,
        ITEM_CREATE,
    }

    private Dictionary<SKILL_TYPE, GameSkill> mUseSkillList = new Dictionary<SKILL_TYPE, GameSkill>();

    public void ResetGame()
    {
        var enumerator = mUseSkillList.GetEnumerator();
        while (enumerator.MoveNext())
            enumerator.Current.Value.EndSkill();
        mUseSkillList.Clear();
    }
    public void UseSkinSlotSkill()
    {
        AddUseSkill(PlayerData.Instance.GetSkinSlotSkill(CommonData.SKIN_TYPE.CHAR));
        AddUseSkill(PlayerData.Instance.GetSkinSlotSkill(CommonData.SKIN_TYPE.BACKGROUND));
        AddUseSkill(PlayerData.Instance.GetSkinSlotSkill(CommonData.SKIN_TYPE.DOOR));
        AddUseSkill(PlayerData.Instance.GetSkinSkill(CommonData.SKIN_TYPE.CHAR));
        AddUseSkill(PlayerData.Instance.GetSkinSkill(CommonData.SKIN_TYPE.BACKGROUND));
        AddUseSkill(PlayerData.Instance.GetSkinSkill(CommonData.SKIN_TYPE.DOOR));
    }

    public GameSkill UseItemSkill(int itemId)
    {
        var skillName = ItemManager.Instance.GetItemSkill(itemId);
        var skill = AddUseSkill(skillName);

        return skill;
    }
    public GameSkill AddUseSkill(string skillName)
    {
        SkillData skillData = DataManager.Instance.SkillDataList[skillName];
        SKILL_TYPE skillType = ConvertSkillType(skillData.skilltype);
        GameSkill data = null;

        switch (skillType)
        {
            case SKILL_TYPE.DAMAGE_SHIELD_TIME:
                data = new GameSkill_DamageShieldTime(skillName);
                break;
            case SKILL_TYPE.DAMAGE_SHIELD_COUNT:
                data = new GameSkill_DamageShieldCount(skillName);
                break;
            case SKILL_TYPE.SCORE_UP:
                data = new GameSkill_ScoreUP(skillName);
                break;
            case SKILL_TYPE.SPEED_DOWN:
                data = new GameSkill_SpeedDown(skillName);
                break;
            case SKILL_TYPE.RESURRECTION:
                data = new GameSkill_Resurrection(skillName);
                break;
            case SKILL_TYPE.GAME_OVER_SCORE_BONUS:
                data = new GameSkill_GameOverScoreBonus(skillName);
                break;
            case SKILL_TYPE.GAME_OVER_COIN_BONUS:
                data = new GameSkill_GameOverCoinBonus(skillName);
                break;
            case SKILL_TYPE.ITEM_CREATE:
                data = new GameSkill_ItemCreate(skillName);
                break;
            default:
                break;
        }

        if (data != null)
        {
            AddUseSkill(data);
            data.StartSkill();
        }

        return data;
    }

    private void AddUseSkill(GameSkill data)
    {
        var skillType = data.mSkillType;
        if (mUseSkillList.ContainsKey(skillType) == false)
            mUseSkillList.Add(skillType, data);
        else
        {
            mUseSkillList[skillType].PlusSameSkill(data);
        }
    }

    public void UpdateSkill(float time)
    {
        var removeSkillList = new List<SKILL_TYPE>();
        var skillEnumerator = mUseSkillList.GetEnumerator();
        while (skillEnumerator.MoveNext())
        {
            var skill = skillEnumerator.Current.Value;
            skill.SkillUpdate(time);
            if (skill.mSkillRemoveReady)
                removeSkillList.Add(skill.mSkillType);
        }

        for (int i = 0; i < removeSkillList.Count; i++)
        {
            mUseSkillList.Remove(removeSkillList[i]);
        }
    }

    public bool IsSkillEnable(SKILL_TYPE type)
    {
        if (mUseSkillList.ContainsKey(type) == false)
            return false;

        return mUseSkillList[type].mEnable;
    }

    public GameSkill GetGameSkill(SKILL_TYPE type)
    {
        if (mUseSkillList.ContainsKey(type) == false)
            return null;

        return mUseSkillList[type];
    }

    public SKILL_TYPE ConvertSkillType(string str)
    {
        SKILL_TYPE type = SKILL_TYPE.NONE;
        switch (str)
        {
            case "DAMAGE_SHIELD_TIME":
                type = SKILL_TYPE.DAMAGE_SHIELD_TIME;
                break;
            case "DAMAGE_SHIELD_COUNT":
                type = SKILL_TYPE.DAMAGE_SHIELD_COUNT;
                break;
            case "SCORE_UP":
                type = SKILL_TYPE.SCORE_UP;
                break;
            case "SPEED_DOWN":
                type = SKILL_TYPE.SPEED_DOWN;
                break;
            case "RESURRECTION":
                type = SKILL_TYPE.RESURRECTION;
                break;
            case "GAME_OVER_SCORE_BONUS":
                type = SKILL_TYPE.GAME_OVER_SCORE_BONUS;
                break;
            case "ITEM_CREATE":
                type = SKILL_TYPE.ITEM_CREATE;
                break;
            case "GAME_OVER_COIN_BONUS":
                type = SKILL_TYPE.GAME_OVER_COIN_BONUS;
                break;
            default:
                break;
        }

        return type;
    }

    public SkillData GetSkillData(string skillName)
    {
        return DataManager.Instance.SkillDataList[skillName];
    }
    
    public bool IsSkillData(string skillName)
    {
        return DataManager.Instance.SkillDataList.ContainsKey(skillName);
    }

    
}

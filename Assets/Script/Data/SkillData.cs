using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class SkillData
{
    public int id;
    public string name;
    public float time;
    public float count;
    public float percent;
    public string skilltype;

    public SkillData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        skilltype = node.Attributes.GetNamedItem("skilltype").Value;
        time = float.Parse(node.Attributes.GetNamedItem("time").Value);
        count = float.Parse(node.Attributes.GetNamedItem("count").Value);
        percent = float.Parse(node.Attributes.GetNamedItem("percent").Value);
    }

    public string GetDesc()
    {
        SkillManager.SKILL_TYPE type = SkillManager.Instance.ConvertSkillType(skilltype);
        switch (type)
        {
            case SkillManager.SKILL_TYPE.DAMAGE_SHIELD_TIME:
                return string.Format(LocalizeData.Instance.GetLocalizeString("SKILL_DESC_DAMAGE_SHIELD_TIME"), time);
            case SkillManager.SKILL_TYPE.DAMAGE_SHIELD_COUNT:
                return string.Format(LocalizeData.Instance.GetLocalizeString("SKILL_DESC_DAMAGE_SHIELD_COUNT"));
            case SkillManager.SKILL_TYPE.SCORE_UP:
                return string.Format(LocalizeData.Instance.GetLocalizeString("SKILL_DESC_SCORE_UP"), time, percent);
            case SkillManager.SKILL_TYPE.SPEED_DOWN:
                return string.Format(LocalizeData.Instance.GetLocalizeString("SKILL_DESC_SPEED_DOWN"), time);
            case SkillManager.SKILL_TYPE.RESURRECTION:
                if (percent == 0)
                    return "";
                return string.Format(LocalizeData.Instance.GetLocalizeString("SKILL_DESC_RESURRECTION"), percent);
            case SkillManager.SKILL_TYPE.GAME_OVER_SCORE_BONUS:
                if (percent == 0)
                    return "";
                return string.Format(LocalizeData.Instance.GetLocalizeString("SKILL_DESC_GAME_OVER_SCORE_BONUS"), percent);
            case SkillManager.SKILL_TYPE.GAME_OVER_COIN_BONUS:
                if (percent == 0)
                    return "";
                return string.Format(LocalizeData.Instance.GetLocalizeString("SKILL_DESC_GAME_OVER_COIN_BONUS"), percent);
            case SkillManager.SKILL_TYPE.ITEM_CREATE:
                return string.Format(LocalizeData.Instance.GetLocalizeString("SKILL_DESC_ITEM_CREATE"), percent);
            default:
                break;
        }

        return "";
    }

    public string GetPlusSkillDesc(SkillData data)
    {
        SkillManager.SKILL_TYPE type = SkillManager.Instance.ConvertSkillType(skilltype);
        if (percent + data.percent == 0)
            return "";

        switch (type)
        {
            case SkillManager.SKILL_TYPE.RESURRECTION:
                return string.Format(LocalizeData.Instance.GetLocalizeString("SKILL_DESC_RESURRECTION"), percent + data.percent);
            case SkillManager.SKILL_TYPE.GAME_OVER_SCORE_BONUS:
                return string.Format(LocalizeData.Instance.GetLocalizeString("SKILL_DESC_GAME_OVER_SCORE_BONUS"), percent + data.percent);
            case SkillManager.SKILL_TYPE.GAME_OVER_COIN_BONUS:
                return string.Format(LocalizeData.Instance.GetLocalizeString("SKILL_DESC_GAME_OVER_COIN_BONUS"), percent + data.percent);
            default:
                break;
        }

        return "";
    }
}
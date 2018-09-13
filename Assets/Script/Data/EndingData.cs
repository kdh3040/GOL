using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text;

public class EndingData
{
    public enum ENDING_CONDITION
    {
        NONE,
        CHAR_SLOT_LEVEL,
        DOOR_SLOT_LEVEL,
        BG_SLOT_LEVEL,
        GAME_COIN,
        GAME_SCORE,
        HAVE_ENDING,
        CHAR_SKIN,
        DOOR_SKIN,
        BG_SKIN,
    }

    public int id;
    public string name = "";
    public string desc = "";
    public int cost = 0;
    public string img;
    public List<KeyValuePair<ENDING_CONDITION, int>> EndingConditionList = new List<KeyValuePair<ENDING_CONDITION, int>>();

    public EndingData(XmlNode node)
    {
        id = int.Parse(node.Attributes.GetNamedItem("id").Value);
        name = node.Attributes.GetNamedItem("name").Value;
        img = node.Attributes.GetNamedItem("img").Value;
        cost = int.Parse(node.Attributes.GetNamedItem("cost").Value);

        for (int i = 1; i <= 3; i++)
        {
            var type = ConvertType(node.Attributes.GetNamedItem(string.Format("condition_type{0}", i)).Value);
            if(type != ENDING_CONDITION.NONE)
            {
                var value = int.Parse(node.Attributes.GetNamedItem(string.Format("condition_value{0}", i)).Value);

                EndingConditionList.Add(new KeyValuePair<ENDING_CONDITION, int>(type, value));
            }
        }
    }

    private ENDING_CONDITION ConvertType(string type)
    {
        switch(type)
        {
            case "CHAR_SLOT_LEVEL":
                return ENDING_CONDITION.CHAR_SLOT_LEVEL;
            case "DOOR_SLOT_LEVEL":
                return ENDING_CONDITION.DOOR_SLOT_LEVEL;
            case "BG_SLOT_LEVEL":
                return ENDING_CONDITION.BG_SLOT_LEVEL;
            case "GAME_COIN":
                return ENDING_CONDITION.GAME_COIN;
            case "GAME_SCORE":
                return ENDING_CONDITION.GAME_SCORE;
            case "HAVE_ENDING":
                return ENDING_CONDITION.HAVE_ENDING;
            case "CHAR_SKIN":
                return ENDING_CONDITION.CHAR_SKIN;
            case "DOOR_SKIN":
                return ENDING_CONDITION.DOOR_SKIN;
            case "BG_SKIN":
                return ENDING_CONDITION.BG_SKIN;
        }

        return ENDING_CONDITION.NONE;
    }

    public bool IsOpenEnding(int coin, int score)
    {
        bool returnValue = false;
        for (int i = 0; i < EndingConditionList.Count; i++)
        {
            returnValue = false;
            var value = EndingConditionList[i].Value;
            switch (EndingConditionList[i].Key)
            {
                case ENDING_CONDITION.NONE:
                    continue;
                case ENDING_CONDITION.CHAR_SLOT_LEVEL:
                    if (PlayerData.Instance.GetSkinSlotLevel(CommonData.SKIN_TYPE.CHAR) >= value)
                        returnValue = true;
                    break;
                case ENDING_CONDITION.DOOR_SLOT_LEVEL:
                    if (PlayerData.Instance.GetSkinSlotLevel(CommonData.SKIN_TYPE.DOOR) >= value)
                        returnValue = true;
                    break;
                case ENDING_CONDITION.BG_SLOT_LEVEL:
                    if (PlayerData.Instance.GetSkinSlotLevel(CommonData.SKIN_TYPE.BACKGROUND) >= value)
                        returnValue = true;
                    break;
                case ENDING_CONDITION.GAME_COIN:
                    if(coin >= value)
                        returnValue = true;
                    break;
                case ENDING_CONDITION.GAME_SCORE:
                    if (score >= value)
                        returnValue = true;
                    break;
                case ENDING_CONDITION.HAVE_ENDING:
                    if (PlayerData.Instance.HasEnding(value))
                        returnValue = true;
                    break;
                case ENDING_CONDITION.CHAR_SKIN:
                    if (PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.CHAR) == value)
                        returnValue = true;
                    break;
                case ENDING_CONDITION.DOOR_SKIN:
                    if (PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.DOOR) == value)
                        returnValue = true;
                    break;
                case ENDING_CONDITION.BG_SKIN:
                    if (PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.BACKGROUND) == value)
                        returnValue = true;
                    break;
                default:
                    break;
            }

            if (returnValue == false)
                return false;
        }
        return true;
    }

    public string GetConditionDesc()
    {
        StringBuilder desc = new StringBuilder();
        for (int i = 0; i < EndingConditionList.Count; i++)
        {
            if (EndingConditionList[i].Key == ENDING_CONDITION.NONE)
                continue;

            int value = EndingConditionList[i].Value;
            switch (EndingConditionList[i].Key)
            {
                case ENDING_CONDITION.CHAR_SLOT_LEVEL:
                    desc.Append(LocalizeData.Instance.GetLocalizeString("ENDING_CONDITION_CHAR_SLOT_LEVEL", value));
                    desc.AppendLine();
                    break;
                case ENDING_CONDITION.DOOR_SLOT_LEVEL:
                    desc.Append(LocalizeData.Instance.GetLocalizeString("ENDING_CONDITION_DOOR_SLOT_LEVEL", value));
                    desc.AppendLine();
                    break;
                case ENDING_CONDITION.BG_SLOT_LEVEL:
                    desc.Append(LocalizeData.Instance.GetLocalizeString("ENDING_CONDITION_BG_SLOT_LEVEL", value));
                    desc.AppendLine();
                    break;
                case ENDING_CONDITION.GAME_COIN:
                    desc.Append(LocalizeData.Instance.GetLocalizeString("ENDING_CONDITION_GAME_COIN", CommonFunc.ConvertNumber(value)));
                    desc.AppendLine();
                    break;
                case ENDING_CONDITION.GAME_SCORE:
                    desc.Append(LocalizeData.Instance.GetLocalizeString("ENDING_CONDITION_GAME_SCORE", CommonFunc.ConvertNumber(value)));
                    desc.AppendLine();
                    break;
                case ENDING_CONDITION.HAVE_ENDING:
                    {
                        var data = DataManager.Instance.EndingDataList[value];
                        desc.Append(LocalizeData.Instance.GetLocalizeString("ENDING_CONDITION_HAVE_ENDING", data.GetLocalizeName()));
                        desc.AppendLine();
                        break;
                    }
                case ENDING_CONDITION.CHAR_SKIN:
                    {
                        var data = DataManager.Instance.CharDataDic[value];
                        desc.Append(LocalizeData.Instance.GetLocalizeString("ENDING_CONDITION_CHAR_SKIN", data.GetLocalizeName()));
                        desc.AppendLine();
                        break;
                    }
                case ENDING_CONDITION.DOOR_SKIN:
                    {
                        var data = DataManager.Instance.DoorDataDic[value];
                        desc.Append(LocalizeData.Instance.GetLocalizeString("ENDING_CONDITION_DOOR_SKIN", data.GetLocalizeName()));
                        desc.AppendLine();
                        break;
                    }
                case ENDING_CONDITION.BG_SKIN:
                    {
                        var data = DataManager.Instance.BackGroundDataDic[value];
                        desc.Append(LocalizeData.Instance.GetLocalizeString("ENDING_CONDITION_BG_SKIN", data.GetLocalizeName()));
                        desc.AppendLine();
                        break;
                    }
                default:
                    break;
            }
        }

        return desc.ToString();
    }

    public string GetLocalizeName()
    {
        return LocalizeData.Instance.GetLocalizeString(name);
    }
    public string GetLocalizeDesc()
    {
        return LocalizeData.Instance.GetLocalizeString(desc);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ItemManager
{
    public static ItemManager _instance = null;
    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemManager();
            }
            return _instance;
        }
    }

    public ItemData GetItemData(int id)
    {
        return DataManager.Instance.ItemDataDic[id];
    }

    public Sprite GetItemIcon(int id)
    {
        var data = GetItemData(id);
        return (Sprite)Resources.Load(data.icon, typeof(Sprite));
    }

    public string GetItemSkill(int id)
    {
        var data = GetItemData(id);
        // 아이템 레벨에 맞는 스킬 반환
        string skillName = string.Format("{0}{1}", data.skill, PlayerData.Instance.GetItemLevel(id));
        return skillName;
    }

    public bool IsItemLevelUp(int id)
    {
        var data = GetItemData(id);
        // 아이템 레벨에 맞는 스킬 반환
        string skillName = string.Format("{0}{1}", data.skill, PlayerData.Instance.GetItemLevel(id) + 1);
        return SkillManager.Instance.IsSkillData(skillName);
    }

    public void ItemLevelUp(int id)
    {
        if (IsItemLevelUp(id))
        {
            PlayerData.Instance.ItemLevelUp(id);
        }
    }
}
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
        return data.skill;
    }
}
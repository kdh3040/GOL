﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItemSlot : MonoBehaviour {

    public Button SlotButton;
    public GameObject SelectImg;
    public Image ItemIcon;
    public Text Count;

    [System.NonSerialized]
    public ItemData mItemData;

    public void SetItem(int itemId)
    {
        mItemData = ItemManager.Instance.GetItemData(itemId);
        ItemIcon.sprite = ItemManager.Instance.GetItemIcon(itemId);
        RefreshUI();
    }

    public void RefreshUI()
    {
        Count.text = CommonFunc.ConvertNumber(PlayerData.Instance.GetHaveItem(mItemData.id));
    }

    public void SetSelect(bool enable)
    {
        SelectImg.SetActive(enable);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public Button SlotButton;
    public Image BackgroudImg;
    public Image ItemIcon;
    public UIPointValue Cost;
    public UICountImgFont Level;
    public UICountImgFont Count;
    public Image SelectImg;

    [System.NonSerialized]
    public int ItemId;
    [System.NonSerialized]
    public bool Select;
    [System.NonSerialized]
    public bool Equip;

    public void SetItemSlot(int itemId)
    {
        ItemId = itemId;

        var itemData = DataManager.Instance.ItemDataDic[ItemId];
        CommonFunc.SetImageFile(itemData.icon, ref ItemIcon);

        Cost.SetValue(itemData.cost);
        RefreshUI();
        SetSelect(false);
        SetEquip(false);
    }
    public void SetSelect(bool enable)
    {
        Select = enable;
        SelectImg.gameObject.SetActive(enable);
    }
    public void SetEquip(bool enable)
    {
        Equip = enable;
        if (Equip)
            CommonFunc.SetImageFile("Renewal/UI/slot_bg_4", ref BackgroudImg, false);
        else
            CommonFunc.SetImageFile("Renewal/UI/slot_bg_1", ref BackgroudImg, false);
    }

    public void RefreshUI()
    {
        var level = PlayerData.Instance.GetItemLevel(ItemId);
        Level.SetValue(string.Format("+{0}", level), UICountImgFont.IMG_RANGE.RIGHT, UICountImgFont.IMG_TYPE.GREEN);
        Count.SetValue(PlayerData.Instance.GetItemCount(ItemId).ToString(), UICountImgFont.IMG_RANGE.RIGHT, UICountImgFont.IMG_TYPE.YELLOW);
    }
}

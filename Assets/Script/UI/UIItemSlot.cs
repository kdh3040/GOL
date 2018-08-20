using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour {

    public Image ItemIcon;
    public UIPointValue Cost;
    public UICountImgFont Level;
    public UICountImgFont Count;
    public Image SelectImg;

    [System.NonSerialized]
    public int ItemId;
    [System.NonSerialized]
    public bool Select;

    public void SetItemSlot(int itemId)
    {
        ItemId = itemId;

        var itemData = DataManager.Instance.ItemDataDic[ItemId];
        CommonFunc.SetImageFile(itemData.icon, ref ItemIcon);

        Cost.SetValue(itemData.cost);
        var level = PlayerData.Instance.GetItemLevel(itemId);
        Level.SetValue(string.Format("+{0}", level), UICountImgFont.IMG_RANGE.RIGHT, UICountImgFont.IMG_TYPE.GREEN);
        Count.SetValue(PlayerData.Instance.GetItemCount(itemId).ToString(), UICountImgFont.IMG_RANGE.RIGHT, UICountImgFont.IMG_TYPE.YELLOW);
        SetSelect(false);
    }
    public void SetSelect(bool enable)
    {
        Select = enable;
        SelectImg.gameObject.SetActive(enable);
    }
}

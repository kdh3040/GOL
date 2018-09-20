using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public Button SlotButton;
    public Image BackgroudImg;
    public Image ItemIcon;
    public GameObject LevelBG;
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

        RefreshUI();
        SetSelect(false);
    }
    public void SetSelect(bool enable)
    {
        Select = enable;
        SelectImg.gameObject.SetActive(enable);
    }

    public void RefreshUI()
    {
        var level = PlayerData.Instance.GetItemLevel(ItemId);
        if (level == 1)
        {
            LevelBG.gameObject.SetActive(false);
            Level.gameObject.SetActive(false);
        }
        else
        {
            LevelBG.gameObject.SetActive(true);
            Level.gameObject.SetActive(true);
            Level.SetValue(string.Format("+{0}", level - 1), UICountImgFont.IMG_RANGE.CENTER, UICountImgFont.IMG_TYPE.GREEN);
        }
    }
}

using System.Collections;
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
    [System.NonSerialized]
    public bool mSelected;

    public void SetItem(int itemId)
    {
        mItemData = DataManager.Instance.ItemDataDic[itemId];
        ItemIcon.sprite = (Sprite)Resources.Load(mItemData.icon, typeof(Sprite));
        RefreshUI();
    }

    public void RefreshUI()
    {
        Count.text = CommonFunc.ConvertNumber(GManager.Instance.mPlayerData.GetHaveItem(mItemData.id));
    }

    public void SetSelect(bool enable)
    {
        SelectImg.SetActive(enable);
    }
}

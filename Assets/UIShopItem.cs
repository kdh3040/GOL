using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour {

    public Button SlotButton;
    public GameObject SelectImg;
    public Image ItemIcon;

    [System.NonSerialized]
    public ItemData mItemData;
    [System.NonSerialized]
    public bool mSelected;

    public void SetItem(int itemId)
    {
        mItemData = DataManager.Instance.ItemDataDic[itemId];
        ItemIcon.sprite = (Sprite)Resources.Load(mItemData.icon, typeof(Sprite));
    }

    public void SetSelect(bool enable)
    {
        SelectImg.SetActive(enable);
    }
}

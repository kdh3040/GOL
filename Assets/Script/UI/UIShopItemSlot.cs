using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItemSlot : MonoBehaviour {

    public Button SlotButton;
    public GameObject CostObj;
    public Text CostValue;
    public GameObject CountObj;
    public Text CountValue;
    public Text LevelValue;
    public Image ItemIcon;
    public Image SelectImg;

    [System.NonSerialized]
    public ItemData ItemData;

    public void SetItem(int itemId)
    {
        ItemData = ItemManager.Instance.GetItemData(itemId);
        CommonFunc.SetImageFile(ItemData.icon, ref ItemIcon);
        RefreshUI();
    }

    public void RefreshUI()
    {
        int haveCount = PlayerData.Instance.GetItemCount(ItemData.id);
        if (haveCount > 0)
        {
            CostObj.SetActive(false);
            CountObj.SetActive(true);
            CountValue.text = string.Format("x{0}", CommonFunc.ConvertNumber(haveCount));
        }
        else
        {
            CostObj.SetActive(true);
            CountObj.SetActive(false);
            CostValue.text = CommonFunc.ConvertNumber(ItemData.cost);
        }

        LevelValue.text = string.Format("+{0}", PlayerData.Instance.GetItemLevel(ItemData.id));
    }

    public void SetSelect(bool enable)
    {
        SelectImg.gameObject.SetActive(enable);
    }
}

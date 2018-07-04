using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameShopChar : MonoBehaviour {

    public Image SelectCharThumbnail;
    public Text SelectCharDesc;
    public GridLayoutGroup CharGrid;

    public Button BuyButton;
    public Button EquipButton;

    public void ShowUI()
    {
        var itemIndexList = DataManager.Instance.ItemDataIndexList;
        for (int i = 0; i < itemIndexList.Count; i++)
        {
            var obj = Instantiate(Resources.Load("Prefab/UIShopItemSlot"), CharGrid.gameObject.transform) as GameObject;
            var slot = obj.GetComponent<UIShopItemSlot>();
            slot.gameObject.transform.localPosition = new Vector3(i * 200, 0);
            slot.SetItem(itemIndexList[i]);
            //slot.SlotButton.onClick.AddListener(() => { OnClickItemSlot(slot.mItemData.id); });
        }
    }
}

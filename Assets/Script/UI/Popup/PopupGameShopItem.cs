using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupGameShopItem : MonoBehaviour {

    //public Image SelectItemIcon;
    //public Text SelectItemDesc;
    //public Button MyItemSlot_1;
    //public Image MyItemSlotImg_1;
    //public Button MyItemSlot_2;
    //public Image MyItemSlotImg_2;
    //public Button MyItemSlot_Shield;
    //public Image MyItemSlotImg_Shield;
    //public List<UIShopItemSlot> ItemList = new List<UIShopItemSlot>();
    //public Button ItemBuy;
    //public Button ItemEquip;
    //public Button ItemUpgrade;

    //private int SelectItemId = 0;

    //void Awake()
    //{
    //    MyItemSlot_1.onClick.AddListener(() => { OnClickMyItemSlot(CommonData.ITEM_SLOT_INDEX.LEFT); });
    //    MyItemSlot_2.onClick.AddListener(() => { OnClickMyItemSlot(CommonData.ITEM_SLOT_INDEX.RIGHT); });
    //    MyItemSlot_Shield.onClick.AddListener(OnClickMyShieldItemSlot);
    //    ItemBuy.onClick.AddListener(OnClickItemBuy);
    //    ItemEquip.onClick.AddListener(OnClickItemEquip);
    //    ItemUpgrade.onClick.AddListener(OnClickItemUpgrade);
    //}

    //public void ShowUI()
    //{
    //    var itemIndexList = new List<int>();
    //    var itemDataDicEnumerator = DataManager.Instance.ItemDataDic.GetEnumerator();

    //    while (itemDataDicEnumerator.MoveNext())
    //    {
    //        itemIndexList.Add(itemDataDicEnumerator.Current.Key);
    //    }

    //    itemIndexList.Sort(delegate (int A, int B)
    //    {
    //        if (A > B)
    //            return 1;
    //        else
    //            return -1;
    //    });

    //    for (int i = 0; i < itemIndexList.Count; i++)
    //    {
    //        int id = itemIndexList[i];
    //        ItemList[i].SetItem(id);
    //        ItemList[i].SlotButton.onClick.AddListener(() => { OnClickItemSlot(id); });
    //    }

    //    OnClickItemSlot(ItemList[0].ItemData.id);
    //    MyItemSlotUI();
    //    RefreshItemSlot();
    //}

    //public void MyItemSlotUI()
    //{

    //    RefreshMyNormalItemSlot(CommonData.ITEM_SLOT_INDEX.LEFT, ref MyItemSlotImg_1);
    //    RefreshMyNormalItemSlot(CommonData.ITEM_SLOT_INDEX.RIGHT, ref MyItemSlotImg_2);

    //    RefreshMyShieldItemSlot();
    //}

    //private void RefreshMyNormalItemSlot(CommonData.ITEM_SLOT_INDEX index, ref Image icon)
    //{
    //    int id = PlayerData.Instance.GetItemSlotId(index);
    //    if (id == 0)
    //        icon.gameObject.SetActive(false);
    //    else
    //    {
    //        icon.gameObject.SetActive(true);
    //        var itemData = ItemManager.Instance.GetItemData(id);
    //        CommonFunc.SetImageFile(itemData.icon, ref icon);
    //    }
    //}

    //private void RefreshMyShieldItemSlot()
    //{
    //    if (PlayerData.Instance.GetItemSlotId(CommonData.ITEM_SLOT_INDEX.SHIELD) != 0)
    //    {
    //        MyItemSlotImg_Shield.gameObject.SetActive(true);
    //        var itemData = ItemManager.Instance.GetItemData(PlayerData.Instance.GetItemSlotId(CommonData.ITEM_SLOT_INDEX.SHIELD));
    //        CommonFunc.SetImageFile(itemData.icon, ref MyItemSlotImg_Shield);
    //    }
    //    else
    //        MyItemSlotImg_Shield.gameObject.SetActive(false);
    //}

    //public void RefreshItemSlot()
    //{
    //    for (int i = 0; i < ItemList.Count; i++)
    //    {
    //        ItemList[i].RefreshUI();
    //    }
    //}

    //public void OnClickItemSlot(int itemId)
    //{
    //    for (int i = 0; i < ItemList.Count; i++)
    //    {
    //        if (ItemList[i].ItemData.id == itemId)
    //            ItemList[i].SetSelect(true);
    //        else
    //            ItemList[i].SetSelect(false);
    //    }

    //    SelectItemId = itemId;
    //    var itemData = ItemManager.Instance.GetItemData(SelectItemId);
    //    var skillName = ItemManager.Instance.GetItemSkill(SelectItemId);
    //    SelectItemDesc.text = "아이템 설명 추가해야함";
    //    CommonFunc.SetImageFile(itemData.icon, ref SelectItemIcon);
    //}
    //public void OnClickMyItemSlot(CommonData.ITEM_SLOT_INDEX index)
    //{
    //    if (PlayerData.Instance.GetItemSlotId(index) != 0)
    //    {
    //        PlayerData.Instance.SetItemSlotId(index, 0);
    //        MyItemSlotUI();
    //    }
    //}
    //public void OnClickMyShieldItemSlot()
    //{
    //    PlayerData.Instance.SetItemSlotId(CommonData.ITEM_SLOT_INDEX.SHIELD, 0);
    //    MyItemSlotUI();
    //}
    //public void OnClickItemBuy()
    //{
    //    var itemData = ItemManager.Instance.GetItemData(SelectItemId);
    //    UnityAction yesAction = () =>
    //    {
    //        PopupManager.Instance.DismissPopup();
    //        if (CommonFunc.UseCoin(itemData.cost))
    //        {
    //            PlayerData.Instance.PlusItem_Count(SelectItemId);
    //            RefreshItemSlot();
    //        }
    //    };
    //    UnityAction noAction = () =>
    //    {
    //        PopupManager.Instance.DismissPopup();
    //    };
    //    var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("BUY_ITEM_TITLE"), yesAction, noAction);
    //    PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);

    //}
    //public void OnClickItemEquip()
    //{
    //    bool equipEnable = false;
    //    var itemData = ItemManager.Instance.GetItemData(SelectItemId);
    //    if(PlayerData.Instance.GetItemCount(SelectItemId) > 0)
    //    {
    //        if (itemData.slot_type == CommonData.ITEM_SLOT_TYPE.NORMAL)
    //        {
    //            equipEnable = PlayerData.Instance.SetItemSlotId_Normal(SelectItemId);
    //        }
    //        else
    //        {
    //            if (PlayerData.Instance.GetItemSlotId(CommonData.ITEM_SLOT_INDEX.SHIELD) == 0)
    //            {
    //                equipEnable = true;
    //                PlayerData.Instance.SetItemSlotId(CommonData.ITEM_SLOT_INDEX.SHIELD, SelectItemId);
    //            }
    //        }
    //        if (equipEnable)
    //            MyItemSlotUI();
    //        else
    //        {
    //            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("EQUIP_ITEM_FULL"));
    //            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
    //        }
    //    }
    //    else
    //    {
    //        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("EQUIP_ITEM_NONE"));
    //        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
    //    }
    //}
    //public void OnClickItemUpgrade()
    //{

    //    if (ItemManager.Instance.IsItemLevelUp(SelectItemId))
    //    {
    //        var itemData = ItemManager.Instance.GetItemData(SelectItemId);
    //        UnityAction yesAction = () =>
    //        {
    //            PopupManager.Instance.DismissPopup();
    //            if (CommonFunc.UseCoin(itemData.levelup_cost))
    //            {
    //                ItemManager.Instance.ItemLevelUp(SelectItemId);
    //                RefreshItemSlot();
    //            }
    //        };
    //        UnityAction noAction = () =>
    //        {
    //            PopupManager.Instance.DismissPopup();
    //        };
    //        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("LEVELUP_ITEM"), yesAction, noAction);
    //        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
    //    }
    //    else
    //    {
    //        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("ITEM_LEVELUP_MAX"));
    //        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
    //    }
    //}        
}

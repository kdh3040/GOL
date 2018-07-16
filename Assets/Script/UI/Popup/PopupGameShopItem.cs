using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupGameShopItem : MonoBehaviour {

    public List<Button> MyItemSlotLIst = new List<Button>();
    public List<Image> MyItemSlotBGLIst = new List<Image>();
    public List<Image> MyItemSlotIconLIst = new List<Image>();
    public Button MyShieldItemSlot;
    public Image MyShieldItemSlotBG;
    public Image MyShieldItemSlotIcon;
    public Transform ItemSlotListStartPos;
    public Text ItemDesc;
    public Button ItemBuy;
    public Button ItemEquip;
    public Button ItemUpgrade;

    private List<UIShopItemSlot> ItemList = new List<UIShopItemSlot>();
    private int SelectItemId = 0;

    void Awake()
    {
        for (int i = 0; i < MyItemSlotLIst.Count; ++i)
        {
            int temp = i;
            MyItemSlotLIst[i].onClick.AddListener(() => { OnClickMyItemSlot(temp); });
        }

        MyShieldItemSlot.onClick.AddListener(OnClickMyShieldItemSlot);
        ItemBuy.onClick.AddListener(OnClickItemBuy);
        ItemEquip.onClick.AddListener(OnClickItemEquip);
        ItemUpgrade.onClick.AddListener(OnClickItemUpgrade);
    }

    public void ShowUI()
    {
        if (ItemList.Count <= 0)
        {
            var itemIndexList = new List<int>();
            var itemDataDicEnumerator = DataManager.Instance.ItemDataDic.GetEnumerator();

            while(itemDataDicEnumerator.MoveNext())
            {
                itemIndexList.Add(itemDataDicEnumerator.Current.Key);
            }

            itemIndexList.Sort(delegate (int A, int B)
            {
                if (A > B)
                    return 1;
                else
                    return -1;
            });

            for (int i = 0; i < itemIndexList.Count; i++)
            {
                var obj = Instantiate(Resources.Load("Prefab/UIShopItemSlot"), ItemSlotListStartPos) as GameObject;
                var slot = obj.GetComponent<UIShopItemSlot>();
                slot.gameObject.transform.localPosition = new Vector3(i * 200, 0);
                slot.SetItem(itemIndexList[i]);
                slot.SlotButton.onClick.AddListener(() => { OnClickItemSlot(slot.mItemData.id); });
                ItemList.Add(slot);
            }
        }


        OnClickItemSlot(ItemList[0].mItemData.id);
        MyItemSlotUI();
        RefreshItemSlot();
        RefreshBottomButton();
    }

    public void MyItemSlotUI()
    {
        var normalItemArr = PlayerData.Instance.mNormalitemArr;
        for (int i = 0; i < normalItemArr.Length; i++)
        {
            if (normalItemArr[i] == 0 ||
                PlayerData.Instance.GetHaveItem(normalItemArr[i]) <= 0)
            {
                PlayerData.Instance.SetItemSlotId((CommonData.ITEM_SLOT_INDEX)i, 0);
                MyItemSlotBGLIst[i].sprite = (Sprite)Resources.Load("item_empty", typeof(Sprite));
                MyItemSlotIconLIst[i].gameObject.SetActive(false);
            }
            else
            {
                var itemData = ItemManager.Instance.GetItemData(normalItemArr[i]);
                MyItemSlotIconLIst[i].gameObject.SetActive(true);
                MyItemSlotIconLIst[i].sprite = (Sprite)Resources.Load(itemData.icon, typeof(Sprite));
                MyItemSlotBGLIst[i].sprite = (Sprite)Resources.Load("item_slot", typeof(Sprite));
            }
        }

        var shieldItmeId = PlayerData.Instance.mShielditem;
        if (shieldItmeId == 0)
        {
            MyShieldItemSlotIcon.gameObject.SetActive(false);
            MyShieldItemSlotBG.sprite = (Sprite)Resources.Load("shield_empty", typeof(Sprite));
        }
        else
        {
            MyShieldItemSlotBG.sprite = (Sprite)Resources.Load("shield_slot", typeof(Sprite));
            MyShieldItemSlotIcon.gameObject.SetActive(true);
            MyShieldItemSlotIcon.sprite = ItemManager.Instance.GetItemIcon(shieldItmeId);
        }
    }

    public void RefreshItemSlot()
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].RefreshUI();
        }
    }

    public void RefreshBottomButton()
    {
        if(ItemManager.Instance.IsItemLevelUp(SelectItemId))
            ItemUpgrade.gameObject.SetActive(true);
        else
            ItemUpgrade.gameObject.SetActive(false);
    }

    public void OnClickItemSlot(int itemId)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].mItemData.id == itemId)
                ItemList[i].SetSelect(true);
            else
                ItemList[i].SetSelect(false);
        }

        SelectItemId = itemId;
        var skillName = ItemManager.Instance.GetItemSkill(SelectItemId);
        ItemDesc.text = SkillManager.Instance.GetSkillDesc(skillName);
        RefreshBottomButton();
    }
    public void OnClickMyItemSlot(int index)
    {
        if (PlayerData.Instance.mNormalitemArr[index] != 0)
        {
            PlayerData.Instance.SetItemSlotId((CommonData.ITEM_SLOT_INDEX)index, 0);
            MyItemSlotUI();
        }
    }
    public void OnClickMyShieldItemSlot()
    {
        PlayerData.Instance.mShielditem = 0;
        MyItemSlotUI();
    }
    public void OnClickItemBuy()
    {
        var itemData = ItemManager.Instance.GetItemData(SelectItemId);
        UnityAction yesAction = () =>
        {
            PopupManager.Instance.DismissPopup();
            if (CommonFunc.UseCoin(itemData.cost))
            {
                PlayerData.Instance.AddItem(SelectItemId);
                RefreshItemSlot();
            }
        };
        UnityAction noAction = () =>
        {
            PopupManager.Instance.DismissPopup();
        };
        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("BUY_ITEM_TITLE"), yesAction, noAction);
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);

    }
    public void OnClickItemEquip()
    {
        bool equipEnable = false;
        var itemData = ItemManager.Instance.GetItemData(SelectItemId);
        if(PlayerData.Instance.GetHaveItem(SelectItemId) > 0)
        {
            if (itemData.slot_type == CommonData.ITEM_SLOT_TYPE.NORMAL)
            {
                var normalItemArr = PlayerData.Instance.mNormalitemArr;
                for (int i = 0; i < normalItemArr.Length; i++)
                {
                    if (normalItemArr[i] == 0)
                    {
                        equipEnable = true;
                        normalItemArr[i] = SelectItemId;
                        break;
                    }
                }
            }
            else
            {
                if (PlayerData.Instance.mShielditem == 0)
                {
                    equipEnable = true;
                    PlayerData.Instance.mShielditem = SelectItemId;
                }
            }
            if (equipEnable)
                MyItemSlotUI();
            else
            {
                var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("EQUIP_ITEM_FULL"));
                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
            }
        }
        else
        {
            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("EQUIP_ITEM_NONE"));
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
        }
    }
    public void OnClickItemUpgrade()
    {
        var itemData = ItemManager.Instance.GetItemData(SelectItemId);
        UnityAction yesAction = () =>
        {
            PopupManager.Instance.DismissPopup();
            if (CommonFunc.UseCoin(itemData.levelup_cost))
            {
                ItemManager.Instance.ItemLevelUp(SelectItemId);
                RefreshItemSlot();
                RefreshBottomButton();
            }
        };
        UnityAction noAction = () =>
        {
            PopupManager.Instance.DismissPopup();
        };
        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("LEVELUP_ITEM"), yesAction, noAction);
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
    }
}

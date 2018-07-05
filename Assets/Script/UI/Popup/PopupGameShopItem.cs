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
    }

    public void MyItemSlotUI()
    {
        var normalItemArr = GManager.Instance.mPlayerData.mNormalitemArr;
        for (int i = 0; i < normalItemArr.Length; i++)
        {
            if (normalItemArr[i] == 0 ||
                GManager.Instance.mPlayerData.GetHaveItem(normalItemArr[i]) <= 0)
            {
                GManager.Instance.mPlayerData.SetItemSlotId((CommonData.ITEM_SLOT_INDEX)i, 0);
                MyItemSlotBGLIst[i].sprite = (Sprite)Resources.Load("item_empty", typeof(Sprite));
                MyItemSlotIconLIst[i].gameObject.SetActive(false);
            }
            else
            {
                var itemData = DataManager.Instance.ItemDataDic[normalItemArr[i]];
                MyItemSlotIconLIst[i].gameObject.SetActive(true);
                MyItemSlotIconLIst[i].sprite = (Sprite)Resources.Load(itemData.icon, typeof(Sprite));
                MyItemSlotBGLIst[i].sprite = (Sprite)Resources.Load("item_slot", typeof(Sprite));
            }
        }

        var shieldItmeId = GManager.Instance.mPlayerData.mShielditem;
        if (shieldItmeId == 0)
        {
            MyShieldItemSlotIcon.gameObject.SetActive(false);
            MyShieldItemSlotBG.sprite = (Sprite)Resources.Load("shield_empty", typeof(Sprite));
        }
        else
        {
            MyShieldItemSlotBG.sprite = (Sprite)Resources.Load("shield_slot", typeof(Sprite));
            var itemData = DataManager.Instance.ItemDataDic[shieldItmeId];
            MyShieldItemSlotIcon.gameObject.SetActive(true);
            MyShieldItemSlotIcon.sprite = (Sprite)Resources.Load(itemData.icon, typeof(Sprite));
        }
    }

    public void RefreshItemSlot()
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].RefreshUI();
        }
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
        var itemData = DataManager.Instance.ItemDataDic[SelectItemId];
        ItemDesc.text = SkillManager.Instance.GetSkillDesc(itemData.skill);
    }
    public void OnClickMyItemSlot(int index)
    {
        if (GManager.Instance.mPlayerData.mNormalitemArr[index] != 0)
        {
            GManager.Instance.mPlayerData.SetItemSlotId((CommonData.ITEM_SLOT_INDEX)index, 0);
            MyItemSlotUI();
        }
    }
    public void OnClickMyShieldItemSlot()
    {
        GManager.Instance.mPlayerData.mShielditem = 0;
        MyItemSlotUI();
    }
    public void OnClickItemBuy()
    {
        var itemData = DataManager.Instance.ItemDataDic[SelectItemId];
        UnityAction yesAction = () =>
        {
            PopupManager.Instance.DismissPopup();
            if (CommonFunc.UseCoin(itemData.cost))
            {
                GManager.Instance.mPlayerData.AddItem(SelectItemId);
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
        var itemData = DataManager.Instance.ItemDataDic[SelectItemId];
        if(GManager.Instance.mPlayerData.GetHaveItem(SelectItemId) > 0)
        {
            if (itemData.slot_type == CommonData.ITEM_SLOT_TYPE.NORMAL)
            {
                var normalItemArr = GManager.Instance.mPlayerData.mNormalitemArr;
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
                if (GManager.Instance.mPlayerData.mShielditem == 0)
                {
                    equipEnable = true;
                    GManager.Instance.mPlayerData.mShielditem = SelectItemId;
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
        var msgPopupData = new PopupMsg.PopupData("임시 강화 버튼 입니다.");
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
    }
}

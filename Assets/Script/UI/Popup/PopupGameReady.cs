using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGameReady : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_READY;
    }

    public UITopBar Topbar;
    public Image DescIcon;
    public Text Desc;
    public Button ItemBuyButton;
    public UIPointValue ItemBuyCost;
    public Button UpgradeButton;
    public UIPointValue UpgradeCost;
    public Button SkinChangeButton;
    public Button ItemEquipButton;

    public List<UISkinSlot> SkinSlotList = new List<UISkinSlot>();
    public List<UIItemSlot> ItemSlotList = new List<UIItemSlot>();

    public Button StartButton;

    private bool SelectSkinSlot = false;
    private int SelectSlotIndex = 0;
    private int EquipItemSlotIndex = -1;

    public void Awake()
    {
        ItemBuyButton.onClick.AddListener(OnClickItemBuy);
        UpgradeButton.onClick.AddListener(OnClickUpgrade);
        SkinChangeButton.onClick.AddListener(OnClickSkinChange);
        ItemEquipButton.onClick.AddListener(OnClickItemEquip);
        StartButton.onClick.AddListener(OnClickGameStart);
    }

    public override void ShowPopup(PopupUIData data)
    {
        SelectSkinSlot = false;
        SelectSlotIndex = 0;
        EquipItemSlotIndex = -1;
        Topbar.Initialize(true);

        SkinSlotList[0].SetSkinSlot(CommonData.SKIN_TYPE.CHAR);
        SkinSlotList[0].SlotButton.onClick.AddListener(() => { OnClickSkinSlot(0); });
        SkinSlotList[1].SetSkinSlot(CommonData.SKIN_TYPE.DOOR);
        SkinSlotList[1].SlotButton.onClick.AddListener(() => { OnClickSkinSlot(1); });
        SkinSlotList[2].SetSkinSlot(CommonData.SKIN_TYPE.BACKGROUND);
        SkinSlotList[2].SlotButton.onClick.AddListener(() => { OnClickSkinSlot(2); });

        var itemEnumerator = DataManager.Instance.ItemDataDic.GetEnumerator();
        var itemIndex = 0;
        while(itemEnumerator.MoveNext())
        {
            if (ItemSlotList.Count <= itemIndex)
                break;
            int index = itemIndex;
            ItemSlotList[itemIndex].SetItemSlot(itemEnumerator.Current.Key);
            ItemSlotList[itemIndex].SlotButton.onClick.AddListener(() => { OnClickItem(index); });
            itemIndex++;
        }

        RefreshSlot();
        RefreshDesc();
    }

    public void RefreshSlot()
    {
        for (int i = 0; i < SkinSlotList.Count; i++)
        {
            SkinSlotList[i].SetSelect(false);
            SkinSlotList[i].RefreshUI();
        }

        for (int i = 0; i < ItemSlotList.Count; i++)
        {
            ItemSlotList[i].SetSelect(false);
            ItemSlotList[i].SetEquip(false);
            ItemSlotList[i].RefreshUI();
        }

        if(SelectSkinSlot)
        {
            SkinSlotList[SelectSlotIndex].SetSelect(true);
        }
        else
        {
            ItemSlotList[SelectSlotIndex].SetSelect(true);
        }

        if(EquipItemSlotIndex > -1)
            ItemSlotList[EquipItemSlotIndex].SetEquip(true);
    }

    public void OnClickSkinSlot(int index)
    {
        SelectSkinSlot = true;
        SelectSlotIndex = index;
        RefreshDesc();
        RefreshSlot();
    }
    public void OnClickItem(int index)
    {
        SelectSkinSlot = false;
        SelectSlotIndex = index;
        RefreshDesc();
        RefreshSlot();
    }

    public void RefreshDesc()
    {
        ItemBuyButton.gameObject.SetActive(false);
        UpgradeButton.gameObject.SetActive(false);
        SkinChangeButton.gameObject.SetActive(false);
        ItemEquipButton.gameObject.SetActive(false);

        if(SelectSkinSlot)
        {
            var skinType = SkinSlotList[SelectSlotIndex].SkinType;
            var skinData = PlayerData.Instance.GetUseSkinData(skinType);
            CommonFunc.SetImageFile(skinData.GetIcon(), ref DescIcon);
            Desc.text = LocalizeData.Instance.GetLocalizeString(skinData.desc);

            SkinChangeButton.gameObject.SetActive(true);

            var level = PlayerData.Instance.GetSkinSlotLevel(skinType);
            if (level < DataManager.Instance.SkinSlotLevelDataList[skinType].Count)
            {
                var data = DataManager.Instance.SkinSlotLevelDataList[skinType][level];
                UpgradeButton.gameObject.SetActive(true);
                UpgradeCost.SetValue(data.cost);
            }
            else
                UpgradeButton.gameObject.SetActive(false);
        }
        else
        {
            var itemId = ItemSlotList[SelectSlotIndex].ItemId;
            var itemData = DataManager.Instance.ItemDataDic[itemId];
            CommonFunc.SetImageFile(itemData.icon, ref DescIcon);
            Desc.text = LocalizeData.Instance.GetLocalizeString(itemData.desc);

            ItemBuyButton.gameObject.SetActive(true);
            ItemBuyCost.SetValue(itemData.cost);
            if (ItemManager.Instance.IsItemLevelUp(itemId))
            {
                UpgradeButton.gameObject.SetActive(true);
                UpgradeCost.SetValue(itemData.levelup_cost);
            }
            else
                UpgradeButton.gameObject.SetActive(false);

            ItemEquipButton.gameObject.SetActive(true);
        }
    }

    public void OnClickItemBuy()
    {
        UnityAction yesAction = () =>
        {
            var id = ItemSlotList[SelectSlotIndex].ItemId;
            var itemData = DataManager.Instance.ItemDataDic[id];
            if (CommonFunc.UseCoin(itemData.cost))
                PlayerData.Instance.PlusItem_Count(id);

            RefreshDesc();
            RefreshSlot();
            PopupManager.Instance.DismissPopup();
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
        EquipItemSlotIndex = SelectSlotIndex;
        RefreshDesc();
        RefreshSlot();
    }

    public void OnClickUpgrade()
    {
        if (SelectSkinSlot)
        {
            UnityAction yesAction = () =>
            {
                var skinType = SkinSlotList[SelectSlotIndex].SkinType;
                var level = PlayerData.Instance.GetSkinSlotLevel(skinType);
                var data = DataManager.Instance.SkinSlotLevelDataList[skinType][level];
                if (CommonFunc.UseCoin(data.cost))
                {
                    PlayerData.Instance.SetSkinSlotLevel(skinType, level + 1);
                }

                RefreshDesc();
                RefreshSlot();
                PopupManager.Instance.DismissPopup();
            };
            UnityAction noAction = () =>
            {
                PopupManager.Instance.DismissPopup();
            };
            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("UPGRADE_SKIN_TITLE"), yesAction, noAction);
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
        }
        else
        {
            UnityAction yesAction = () =>
            {
                var itemId = ItemSlotList[SelectSlotIndex].ItemId;
                var itemData = DataManager.Instance.ItemDataDic[itemId];
                if (CommonFunc.UseCoin(itemData.levelup_cost))
                {
                    ItemManager.Instance.ItemLevelUp(itemId);
                }

                RefreshDesc();
                RefreshSlot();
                PopupManager.Instance.DismissPopup();
            };
            UnityAction noAction = () =>
            {
                PopupManager.Instance.DismissPopup();
            };
            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("UPGRADE_ITEM_TITLE"), yesAction, noAction);
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
        }
    }

    public void OnClickSkinChange()
    {
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_SHOP, new PopupGameShop.PopupData(false));
    }

    public void OnClickGameStart()
    {
        if (EquipItemSlotIndex < 0)
        {
            UnityAction yesAction = () =>
            {
                PopupManager.Instance.DismissPopup();
                GameStart();
            };
            UnityAction noAction = () =>
            {
                PopupManager.Instance.DismissPopup();
            };
            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("GAME_PLAY_ENABLE_ITEM_EQUIP"), yesAction, noAction);
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
        }
        else
        {
            var itemId = ItemSlotList[EquipItemSlotIndex].ItemId;
            if (PlayerData.Instance.GetItemCount(itemId) <= 0)
            {
                UnityAction yesAction = () =>
                {
                    PopupManager.Instance.DismissPopup();
                    GameStart();
                };
                UnityAction noAction = () =>
                {
                    PopupManager.Instance.DismissPopup();
                };
                var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("GAME_PLAY_ENABLE_ITEM_COUNT"), yesAction, noAction);
                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
            }
            else
                GameStart();
        }
    }

    public void GameStart()
    {
        if (PlayerData.Instance.IsPlayEnable())
        {
            PopupManager.Instance.DismissPopup();
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }
}

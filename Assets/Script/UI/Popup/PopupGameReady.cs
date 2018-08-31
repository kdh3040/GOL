using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    public Image DescCharIcon;
    public Animator DescCharIconAnim;
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
        PlayerData.Instance.SetUseItemId(0);

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

            if(PlayerData.Instance.LastEquipItemId == itemEnumerator.Current.Key)
            {
                OnClickItem(index);
                OnClickItemEquip();
            }
        }

        RefreshUI();
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
        RefreshUI();
    }
    public void OnClickItem(int index)
    {
        SelectSkinSlot = false;
        SelectSlotIndex = index;
        RefreshUI();
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

            if (skinType != CommonData.SKIN_TYPE.CHAR)
            {
                DescIcon.gameObject.SetActive(true);
                DescCharIcon.gameObject.SetActive(false);
                CommonFunc.SetImageFile(skinData.GetIcon(), ref DescIcon, false);
            }
            else
            {
                var charData = skinData as CharData;
                DescIcon.gameObject.SetActive(false);
                DescCharIcon.gameObject.SetActive(true);
                DescCharIconAnim.Rebind();
                DescCharIconAnim.SetTrigger(charData.shopani_trigger);
            }

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


            var slotSkillName = PlayerData.Instance.GetSkinSlotSkill(skinType);
            var slotSkillData = SkillManager.Instance.GetSkillData(slotSkillName);
            var skinSkillName = skinData.GetSkillName();

            StringBuilder desc = new StringBuilder();
            desc.AppendFormat("{0}{1} +{2}", LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_NAME"), skinData.GetSkinSlotTypeName(), level);
            desc.AppendLine();
            desc.AppendLine();
            desc.AppendFormat(slotSkillData.GetDesc());
            if(skinSkillName != "")
            {
                desc.AppendLine();
                var skinSkillData = SkillManager.Instance.GetSkillData(skinSkillName);
                desc.AppendFormat(skinSkillData.GetDesc());
            }
            Desc.text = desc.ToString();
        }
        else
        {
            var itemId = ItemSlotList[SelectSlotIndex].ItemId;
            var itemData = DataManager.Instance.ItemDataDic[itemId];
            CommonFunc.SetImageFile(itemData.icon, ref DescIcon, false);
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

            ItemEquipButton.gameObject.SetActive(EquipItemSlotIndex != SelectSlotIndex);

            var skillName = ItemManager.Instance.GetItemSkill(itemId);
            var skillData = SkillManager.Instance.GetSkillData(skillName);
            StringBuilder desc = new StringBuilder();
            desc.AppendFormat("{0}{1}", LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_NAME"), itemData.GetLocalizeName());
            desc.AppendLine();
            desc.AppendLine();
            desc.AppendFormat(itemData.GetLocalizeDesc());
            desc.AppendLine();
            desc.AppendLine();
            desc.AppendFormat(skillData.GetDesc());
            Desc.text = desc.ToString();
        }
    }

    public void RefreshUI()
    {
        RefreshDesc();
        RefreshSlot();
    }

    public void OnClickItemBuy()
    {
        var id = ItemSlotList[SelectSlotIndex].ItemId;
        var itemData = DataManager.Instance.ItemDataDic[id];
        if (CommonFunc.UseCoin(itemData.cost))
            PlayerData.Instance.PlusItem_Count(id);

        RefreshUI();
    }

    public void OnClickItemEquip()
    {
        EquipItemSlotIndex = SelectSlotIndex;
        RefreshUI();
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

                RefreshUI();
            };
            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("UPGRADE_SKIN_TITLE"), yesAction);
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

                RefreshUI();
            };
            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("UPGRADE_ITEM_TITLE"), yesAction);
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
        }
    }

    public void OnClickSkinChange()
    {
        if (SelectSkinSlot)
        {
            var skinType = SkinSlotList[SelectSlotIndex].SkinType;
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_SHOP, new PopupGameShop.PopupData(RefreshUI, skinType));
        }
        else
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_SHOP, new PopupGameShop.PopupData(RefreshUI));    
    }

    public void OnClickGameStart()
    {
        if (EquipItemSlotIndex < 0)
        {
            UnityAction yesAction = () =>
            {
                GameStart();
            };

            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("GAME_PLAY_ENABLE_ITEM_EQUIP"), yesAction);
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
        }
        else
        {
            var itemId = ItemSlotList[EquipItemSlotIndex].ItemId;
            if (PlayerData.Instance.GetItemCount(itemId) <= 0)
            {
                UnityAction yesAction = () =>
                {
                    GameStart();
                };
                var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("GAME_PLAY_ENABLE_ITEM_COUNT"), yesAction);
                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
            }
            else
            {
                PlayerData.Instance.SetUseItemId(itemId);
                PlayerData.Instance.MinusItem_Count(itemId);
                GameStart();
            }
        }
    }

    public void GameStart()
    {
        if (PlayerData.Instance.IsPlayEnable())
        {
            PlayerData.Instance.SetLastEquipItemId(ItemSlotList[EquipItemSlotIndex].ItemId);
            PopupManager.Instance.DismissPopup();
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }
}

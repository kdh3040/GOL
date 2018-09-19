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

    public class PopupData : PopupUIData
    {
        public UnityAction EndAction;
        public PopupData(UnityAction endAction)
        {
            EndAction = endAction;
        }
    }

    public UITopBar Topbar;
    public Text DescTitle;
    public UIItemSlot DescItemSlot;
    public UISkinSlot DescSkinSlot;
    public Text Desc;
    public Text UpgradeTitle;
    public Button UpgradeButton;
    public UIPointValue UpgradeCost;
    public Button SkinChangeButton;

    public List<UISkinSlot> SkinSlotList = new List<UISkinSlot>();
    public List<UIItemSlot> ItemSlotList = new List<UIItemSlot>();

    public Button StartButton;

    public GameObject ToastPos;

    private bool SelectSkinSlot = false;
    private int SelectSlotIndex = 0;
    private UnityAction EndAction = null;
    private List<UIToastMsg> ToastMsgList = new List<UIToastMsg>();
    private bool ShopPopupEnable = false;

    public void Awake()
    {
        UpgradeButton.onClick.AddListener(OnClickUpgrade);
        SkinChangeButton.onClick.AddListener(OnClickSkinChange);
        StartButton.onClick.AddListener(OnClickGameStart);
    }

    public override void ShowPopup(PopupUIData data)
    {
        SetBackGroundImg();
        var popupData = data as PopupData;
        if (popupData != null)
            EndAction = popupData.EndAction;
        else
            EndAction = null;
        ShopPopupEnable = false;

        for (int i = 0; i < ToastMsgList.Count; i++)
        {
            DestroyImmediate(ToastMsgList[i].gameObject);
        }
        ToastMsgList.Clear();

        if (ToastMsgList.Count <= 0)
        {
            for (int i = 0; i < 5; i++)
            {
                var obj = Instantiate(Resources.Load("Prefab/UIToastMsg"), gameObject.transform) as GameObject;
                var slot = obj.GetComponent<UIToastMsg>();
                slot.gameObject.transform.localPosition = ToastPos.transform.localPosition;
                slot.gameObject.SetActive(false);
                ToastMsgList.Add(slot);
            }
        }


        SelectSkinSlot = false;
        SelectSlotIndex = 0;
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
        OnClickItem(0);
        RefreshUI();
    }

    public override void DismissPopup()
    {
        base.DismissPopup();
        if (ShopPopupEnable == false && EndAction != null)
            EndAction();
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
        UpgradeButton.gameObject.SetActive(false);
        SkinChangeButton.gameObject.SetActive(false);
        DescSkinSlot.gameObject.SetActive(false);
        DescItemSlot.gameObject.SetActive(false);

        if (SelectSkinSlot)
        {
            var skinType = SkinSlotList[SelectSlotIndex].SkinType;
            var skinData = PlayerData.Instance.GetUseSkinData(skinType);

            DescSkinSlot.gameObject.SetActive(true);
            DescSkinSlot.SetSkinSlot(skinType);

            SkinChangeButton.gameObject.SetActive(true);

            DescTitle.text = skinData.GetSkinSlotTypeName();

            var level = PlayerData.Instance.GetSkinSlotLevel(skinType);
            if (level < DataManager.Instance.SkinSlotLevelDataList[skinType].Count)
            {
                var data = DataManager.Instance.SkinSlotLevelDataList[skinType][level];
                UpgradeButton.gameObject.SetActive(true);
                UpgradeCost.SetValue(data.cost);
                UpgradeTitle.text = LocalizeData.Instance.GetLocalizeString("SKIN_SLOT_UPGRADE_TITLE");
            }
            else
                UpgradeButton.gameObject.SetActive(false);


            var slotSkillName = PlayerData.Instance.GetSkinSlotSkill(skinType);
            var slotSkillData = SkillManager.Instance.GetSkillData(slotSkillName);
            var skinSkillData = SkillManager.Instance.GetSkillData(skinData.GetSkillName());

            StringBuilder desc = new StringBuilder();
            if(skinType == CommonData.SKIN_TYPE.BACKGROUND)
            {
                var bgData = skinData as BackgroundData;
                desc.AppendFormat(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_SLOT_CURR_SKIN", bgData.GetLocalizeNameReady()));
            }
            else
                desc.AppendFormat(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_SLOT_CURR_SKIN", skinData.GetLocalizeName()));

            desc.AppendLine();
            desc.AppendLine();
            desc.AppendFormat(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_SLOT_SKILL", slotSkillData.GetDesc()));
            desc.AppendLine();
            desc.AppendFormat(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_SKIN_SKILL", skinSkillData.GetDesc()));
            Desc.text = desc.ToString();
        }
        else
        {
            var itemId = ItemSlotList[SelectSlotIndex].ItemId;
            var itemData = DataManager.Instance.ItemDataDic[itemId];
            DescItemSlot.gameObject.SetActive(true);
            DescItemSlot.SetItemSlot(itemId);
            DescTitle.text = itemData.GetLocalizeName();

            if (ItemManager.Instance.IsItemLevelUp(itemId))
            {
                UpgradeButton.gameObject.SetActive(true);
                UpgradeCost.SetValue(ItemManager.Instance.GetNextItemLevelUpCost(itemId));
                UpgradeTitle.text = LocalizeData.Instance.GetLocalizeString("NORMAL_UPGRADE_TITLE");
            }
            else
                UpgradeButton.gameObject.SetActive(false);

            var skillName = ItemManager.Instance.GetItemSkill(itemId);
            var skillData = SkillManager.Instance.GetSkillData(skillName);
            StringBuilder desc = new StringBuilder();
            desc.AppendFormat(itemData.GetLocalizeDesc());
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

    public void OnClickUpgrade()
    {
        if (SelectSkinSlot)
        {
            var skinType = SkinSlotList[SelectSlotIndex].SkinType;
            var level = PlayerData.Instance.GetSkinSlotLevel(skinType);
            var data = DataManager.Instance.SkinSlotLevelDataList[skinType][level];
            SkinSlotLevelData levelUpdata = null;
            if (DataManager.Instance.SkinSlotLevelDataList[skinType].Count > level)
                levelUpdata = DataManager.Instance.SkinSlotLevelDataList[skinType][level];

            UnityAction yesAction = () =>
            {
                if (CommonFunc.UseCoin(data.cost))
                {
                    SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.LEVEL);
                    PlayerData.Instance.SetSkinSlotLevel(skinType, level + 1);
                    ShowToastMsg(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_UPGRADE_SKIN_SLOT"));
                }

                RefreshUI();
            };

            var skillData = SkillManager.Instance.GetSkillData(levelUpdata.skill);
            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("UPGRADE_SKIN_TITLE", skillData.GetDesc()), yesAction);
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
        }
        else
        {
            var itemId = ItemSlotList[SelectSlotIndex].ItemId;
            var itemData = DataManager.Instance.ItemDataDic[itemId];

            UnityAction yesAction = () =>
            {
                var cost = ItemManager.Instance.GetNextItemLevelUpCost(itemId);
                if (CommonFunc.UseCoin(cost))
                {
                    SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.LEVEL);
                    ItemManager.Instance.ItemLevelUp(itemId);
                    ShowToastMsg(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_UPGRADE_ITEM", itemData.GetLocalizeName()));
                }

                RefreshUI();
            };

            var nextSkill = ItemManager.Instance.GetNextItemLevelSkill(itemId);
            if (nextSkill == "")
                return;

            var skillData = SkillManager.Instance.GetSkillData(nextSkill);

            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("UPGRADE_ITEM_TITLE", skillData.GetDesc()), yesAction);
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
        }
    }

    public void OnClickSkinChange()
    {
        ShopPopupEnable = true;
        if (SelectSkinSlot)
        {
            var skinType = SkinSlotList[SelectSlotIndex].SkinType;
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_SHOP, new PopupGameShop.PopupData(ShopPopupEnd, skinType));
        }
        else
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_SHOP, new PopupGameShop.PopupData(ShopPopupEnd));    
    }

    public void ShopPopupEnd()
    {
        SetBackGroundImg();
        RefreshUI();
        ShopPopupEnable = false;
    }

    public void OnClickGameStart()
    {
        GameStart();
    }

    public void GameStart()
    {
        if (PlayerData.Instance.IsPlayEnable())
        {
            EndAction = null;

            SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.GAME_PLAY);
           
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }

    public void ShowToastMsg(string msg)
    {
        for (int i = 0; i < ToastMsgList.Count; i++)
        {
            if (ToastMsgList[i].Empty)
            {
                ToastMsgList[i].gameObject.SetActive(true);
                ToastMsgList[i].gameObject.transform.localPosition = ToastPos.transform.localPosition;
                ToastMsgList[i].SetMsg(msg);
                break;
            }
        }
    }
}

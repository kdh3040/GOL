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
    public Image DescIcon;
    public Image DescCharIcon;
    public Animator DescCharIconAnim;
    public Text Desc;
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

    public void Awake()
    {
        UpgradeButton.onClick.AddListener(OnClickUpgrade);
        SkinChangeButton.onClick.AddListener(OnClickSkinChange);
        StartButton.onClick.AddListener(OnClickGameStart);
    }

    public override void ShowPopup(PopupUIData data)
    {
        var popupData = data as PopupData;
        if (popupData != null)
            EndAction = popupData.EndAction;
        else
            EndAction = null;

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
        if (EndAction != null)
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
        //SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUTTON);

        SelectSkinSlot = true;
        SelectSlotIndex = index;
        RefreshUI();
    }
    public void OnClickItem(int index)
    {
        //SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUTTON);
        SelectSkinSlot = false;
        SelectSlotIndex = index;
        RefreshUI();
    }

    public void RefreshDesc()
    {
        UpgradeButton.gameObject.SetActive(false);
        SkinChangeButton.gameObject.SetActive(false);
        DescCharIcon.gameObject.SetActive(false);
        DescIcon.gameObject.SetActive(false);

        if (SelectSkinSlot)
        {
            var skinType = SkinSlotList[SelectSlotIndex].SkinType;
            var skinData = PlayerData.Instance.GetUseSkinData(skinType);

            if (skinType != CommonData.SKIN_TYPE.CHAR)
            {
                DescIcon.gameObject.SetActive(true);
                CommonFunc.SetImageFile(skinData.GetIcon(), ref DescIcon, false);
            }
            else
            {
                var charData = skinData as CharData;
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
            var skinSkillData = SkillManager.Instance.GetSkillData(skinData.GetSkillName());

            StringBuilder desc = new StringBuilder();
            if (level == 1)
                desc.AppendFormat("{0}{1}", LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_NAME"), skinData.GetLocalizeName());
            else
                desc.AppendFormat("{0}{1} +{2}", LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_NAME"), skinData.GetLocalizeName(), level - 1);
            desc.AppendLine();
            desc.AppendLine();
            if(slotSkillData.skilltype == skinSkillData.skilltype)
            {
                if(slotSkillData.GetPlusSkillDesc(skinSkillData) != "")
                    desc.AppendFormat(slotSkillData.GetPlusSkillDesc(skinSkillData));
            }
            else
            {
                if(slotSkillData.GetDesc() != "")
                    desc.AppendFormat(slotSkillData.GetDesc());
                if(skinSkillData.GetDesc() != "")
                    desc.AppendFormat(skinSkillData.GetDesc());
            }
            Desc.text = desc.ToString();
        }
        else
        {
            var itemId = ItemSlotList[SelectSlotIndex].ItemId;
            var itemData = DataManager.Instance.ItemDataDic[itemId];
            DescIcon.gameObject.SetActive(true);
            CommonFunc.SetImageFile(itemData.icon, ref DescIcon, false);
            Desc.text = LocalizeData.Instance.GetLocalizeString(itemData.desc);

            if (ItemManager.Instance.IsItemLevelUp(itemId))
            {
                UpgradeButton.gameObject.SetActive(true);
                UpgradeCost.SetValue(itemData.levelup_cost);
            }
            else
                UpgradeButton.gameObject.SetActive(false);


            var level = PlayerData.Instance.GetItemLevel(itemId);
            var skillName = ItemManager.Instance.GetItemSkill(itemId);
            var skillData = SkillManager.Instance.GetSkillData(skillName);
            StringBuilder desc = new StringBuilder();
            if (level == 1)
                desc.AppendFormat("{0}{1}", LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_NAME"), itemData.GetLocalizeName());
            else
                desc.AppendFormat("{0}{1} +{2}", LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_NAME"), itemData.GetLocalizeName(), level - 1);
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

    public void OnClickUpgrade()
    {
        if (SelectSkinSlot)
        {
            UnityAction yesAction = () =>
            {
                //SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUTTON);
                var skinType = SkinSlotList[SelectSlotIndex].SkinType;
                var level = PlayerData.Instance.GetSkinSlotLevel(skinType);
                var data = DataManager.Instance.SkinSlotLevelDataList[skinType][level];
                if (CommonFunc.UseCoin(data.cost))
                {
                    SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.LEVEL);
                    PlayerData.Instance.SetSkinSlotLevel(skinType, level + 1);
                    ShowToastMsg(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_UPGRADE_SKIN_SLOT"));
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
                //SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUTTON);
                var itemId = ItemSlotList[SelectSlotIndex].ItemId;
                var itemData = DataManager.Instance.ItemDataDic[itemId];
                if (CommonFunc.UseCoin(itemData.levelup_cost))
                {
                    SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.LEVEL);
                    ItemManager.Instance.ItemLevelUp(itemId);
                    ShowToastMsg(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_UPGRADE_ITEM", itemData.GetLocalizeName()));
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
        //SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUTTON);
        GameStart();
    }

    public void GameStart()
    {
        if (PlayerData.Instance.IsPlayEnable())
        {
            SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.GAME_START);
            PopupManager.Instance.AllDismissPopup();
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }

    public void ShowToastMsg(string msg)
    {
        var obj = Instantiate(Resources.Load("Prefab/UIToastMsg"), gameObject.transform) as GameObject;
        var slot = obj.GetComponent<UIToastMsg>();
        slot.gameObject.transform.localPosition = ToastPos.transform.localPosition;
        slot.SetMsg(msg);
    }
}

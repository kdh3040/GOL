using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGameShop : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_SHOP;
    }

    public class PopupData : PopupUIData
    {
        public UnityAction EndAction;
        public CommonData.SKIN_TYPE SelectSkinType;
        public PopupData(UnityAction endAction, CommonData.SKIN_TYPE selectSkinType = CommonData.SKIN_TYPE.CHAR)
        {
            EndAction = endAction;
            SelectSkinType = selectSkinType;
        }
    }

    public UITopBar Topbar;
    public Text DescTitle;
    public Image DescIcon;
    public Image DescCharIcon;
    public Animator DescCharIconAnim;
    public UISkinSlot DescSkinSlot;
    public Text Desc;
    public Button SkinBuyButton;
    public UIPointValue SkinBuyCost;
    public Button UpgradeSlotButton;
    public UIPointValue UpgradeSlotCost;
    public Button SkinEquipButton;
    public Button StartButton;

    public GridLayoutGroup SkinSlotGrid;
    public ScrollRect ScrollRect;

    public List<UISkinSlot> SkinSlotList = new List<UISkinSlot>();
    public List<UIShopSkinSlot> ShopSkinList = new List<UIShopSkinSlot>();

    public GameObject ToastPos;

    private CommonData.SKIN_TYPE SelectSkinType = CommonData.SKIN_TYPE.NONE;
    private int SelectSlotIndex = 0;
    private bool SelectLIst = false;
    private UnityAction EndAction = null;
    private List<UIToastMsg> ToastMsgList = new List<UIToastMsg>();

    public void Awake()
    {
        SkinBuyButton.onClick.AddListener(OnClickSkinBuy);
        UpgradeSlotButton.onClick.AddListener(OnClickSkinUpgrade);
        SkinEquipButton.onClick.AddListener(OnClickSkinEquip);
        StartButton.onClick.AddListener(OnClickGameStart);
    }

    public override void ShowPopup(PopupUIData data)
    {
        this.SetBackGroundImg();
        var popupData = data as PopupData;
        if (popupData != null)
        {
            EndAction = popupData.EndAction;
            SelectSkinType = popupData.SelectSkinType;
        }
        else
        {
            EndAction = null;
            SelectSkinType = CommonData.SKIN_TYPE.CHAR;
        }

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


        SelectSlotIndex = 0;
        SelectLIst = false;

        Topbar.Initialize(true);

        SkinSlotList[0].SetSkinSlot(CommonData.SKIN_TYPE.CHAR);
        SkinSlotList[0].SlotButton.onClick.AddListener(() => { OnClickSkinSlot(0); });
        SkinSlotList[1].SetSkinSlot(CommonData.SKIN_TYPE.DOOR);
        SkinSlotList[1].SlotButton.onClick.AddListener(() => { OnClickSkinSlot(1); });
        SkinSlotList[2].SetSkinSlot(CommonData.SKIN_TYPE.BACKGROUND);
        SkinSlotList[2].SlotButton.onClick.AddListener(() => { OnClickSkinSlot(2); });

        switch (SelectSkinType)
        {
            case CommonData.SKIN_TYPE.CHAR:
                OnClickSkinSlot(0);
                break;
            case CommonData.SKIN_TYPE.DOOR:
                OnClickSkinSlot(1);
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                OnClickSkinSlot(2);
                break;
            default:
                break;
        }
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

        for (int i = 0; i < ShopSkinList.Count; i++)
        {
            ShopSkinList[i].SetSelect(false);
            ShopSkinList[i].RefreshUI();
        }

        if (SelectLIst == false)
        {
            SkinSlotList[SelectSlotIndex].SetSelect(true);
        }
        else
        {
            ShopSkinList[SelectSlotIndex].SetSelect(true);
        }
    }

    public void RefreshDesc()
    {
        SkinBuyButton.gameObject.SetActive(false);
        UpgradeSlotButton.gameObject.SetActive(false);
        SkinEquipButton.gameObject.SetActive(false);

        DescIcon.gameObject.SetActive(false);
        DescCharIcon.gameObject.SetActive(false);
        DescSkinSlot.gameObject.SetActive(false);

        SkinData data = null;
        if (SelectLIst)
        {
            var skinId = ShopSkinList[SelectSlotIndex].SkinId;
            data = DataManager.Instance.GetSkinData(SelectSkinType, skinId);
            if (PlayerData.Instance.GetUseSkin(SelectSkinType) != skinId)
            {
                if(PlayerData.Instance.HasSkin(SelectSkinType, skinId))
                    SkinEquipButton.gameObject.SetActive(true);
                else
                {
                    SkinBuyButton.gameObject.SetActive(true);
                    SkinBuyCost.SetValue(data.cost);
                }
            }

            DescTitle.text = data.GetLocalizeName();

            var skinSkillName = data.GetSkillName();
            var skinSkillData = SkillManager.Instance.GetSkillData(skinSkillName);
            StringBuilder desc = new StringBuilder();
            desc.AppendFormat(data.GetLocalizeDesc());
            desc.AppendLine();
            desc.AppendLine();
            desc.AppendFormat(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_SKIN_SKILL", skinSkillData.GetDesc()));
            Desc.text = desc.ToString();

            if (SelectSkinType != CommonData.SKIN_TYPE.CHAR)
            {
                DescIcon.gameObject.SetActive(true);
                DescCharIcon.gameObject.SetActive(false);
                CommonFunc.SetImageFile(data.GetIcon(), ref DescIcon, false);
            }
            else
            {
                var charData = data as CharData;
                DescIcon.gameObject.SetActive(false);
                DescCharIcon.gameObject.SetActive(true);
                DescCharIconAnim.Rebind();
                DescCharIconAnim.SetTrigger(charData.shopani_trigger);
            }
        }
        else
        {
            DescSkinSlot.gameObject.SetActive(true);
            data = PlayerData.Instance.GetUseSkinData(SelectSkinType);
            var level = PlayerData.Instance.GetSkinSlotLevel(SelectSkinType);
            if (level < DataManager.Instance.SkinSlotLevelDataList[SelectSkinType].Count)
            {
                var levelData = DataManager.Instance.SkinSlotLevelDataList[SelectSkinType][level];
                UpgradeSlotButton.gameObject.SetActive(true);
                UpgradeSlotCost.SetValue(levelData.cost);
            }

            DescSkinSlot.SetSkinSlot(SelectSkinType);

            DescTitle.text = data.GetSkinSlotTypeName();

            var slotSkillName = PlayerData.Instance.GetSkinSlotSkill(SelectSkinType);
            var slotSkillData = SkillManager.Instance.GetSkillData(slotSkillName);
            var skinSkillData = SkillManager.Instance.GetSkillData(data.GetSkillName());

            StringBuilder desc = new StringBuilder();
            desc.AppendFormat(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_SLOT_CURR_SKIN", data.GetLocalizeName()));
            desc.AppendLine();
            desc.AppendLine();
            desc.AppendFormat(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_SLOT_SKILL", slotSkillData.GetDesc()));
            desc.AppendLine();
            desc.AppendFormat(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_DESC_SKIN_SKILL", skinSkillData.GetDesc()));
            Desc.text = desc.ToString();
        }

        
    }

    public void RefreshList()
    {
        for (int i = 0; i < ShopSkinList.Count; i++)
        {
            DestroyImmediate(ShopSkinList[i].gameObject);
        }
        ShopSkinList.Clear();

        List<int> skinIdList = new List<int>();

        switch (SelectSkinType)
        {
            case CommonData.SKIN_TYPE.CHAR:
                var charEnumerator = DataManager.Instance.CharDataDic.GetEnumerator();
                while (charEnumerator.MoveNext())
                {
                    skinIdList.Add(charEnumerator.Current.Key);
                }
                break;
            case CommonData.SKIN_TYPE.DOOR:
                var doorEnumerator = DataManager.Instance.DoorDataDic.GetEnumerator();
                while (doorEnumerator.MoveNext())
                {
                    skinIdList.Add(doorEnumerator.Current.Key);
                }
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                var bgEnumerator = DataManager.Instance.BackGroundDataDic.GetEnumerator();
                while (bgEnumerator.MoveNext())
                {
                    skinIdList.Add(bgEnumerator.Current.Key);
                }
                break;
            default:
                break;
        }

        for (int i = 0; i < skinIdList.Count; i++)
        {
            var obj = Instantiate(Resources.Load("Prefab/UIShopSkinSlot"), SkinSlotGrid.gameObject.transform) as GameObject;
            var slot = obj.GetComponent<UIShopSkinSlot>();
            ShopSkinList.Add(slot);
            int index = i;
            slot.SlotButton.onClick.AddListener(() => { OnClickSkin(index); });
            slot.SetData(SelectSkinType, skinIdList[i]);
        }

        ScrollRect.content.sizeDelta = new Vector2(ShopSkinList.Count * 260, ScrollRect.content.sizeDelta.y);
    }

    public void OnClickSkinSlot(int index)
    {
        SelectLIst = false;
        SelectSlotIndex = index;
        SelectSkinType = SkinSlotList[index].SkinType;
        RefreshUI();
    }

    public void OnClickSkin(int index)
    {
        SelectLIst = true;
        SelectSlotIndex = index;
        var skinId = ShopSkinList[SelectSlotIndex].SkinId;
        if(GamePlayManager.Instance.SkinShopImmEquipMode)
        {
            if (PlayerData.Instance.HasSkin(SelectSkinType, skinId))
                OnClickSkinEquip();
            else
                RefreshUI();
        }
        else
            RefreshUI();

    }

    public void RefreshUI()
    {
        RefreshList();
        RefreshDesc();
        RefreshSlot();
    }


    public void OnClickSkinBuy()
    {
        UnityAction yesAction = () =>
        {
            var skinId = ShopSkinList[SelectSlotIndex].SkinId;
            var skinData = DataManager.Instance.GetSkinData(SelectSkinType, skinId);
            if(CommonFunc.UseCoin(skinData.cost))
            {
                SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUY);
                PlayerData.Instance.AddSkin(SelectSkinType, skinId);
                ShowToastMsg(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_EQUIP_ITEM", skinData.GetLocalizeName()));
                PlayerData.Instance.SetUseSkin(SelectSkinType, skinId);
                RefreshUI();
                SetBackGroundImg();
            }
        };
        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("BUY_SKIN_TITLE"), yesAction);
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
    }

    public void OnClickSkinEquip()
    {
        SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.EQUIP);        
        var skinId = ShopSkinList[SelectSlotIndex].SkinId;
        var skinData = DataManager.Instance.GetSkinData(SelectSkinType, skinId);
        ShowToastMsg(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_READY_EQUIP_ITEM", skinData.GetLocalizeName()));
        PlayerData.Instance.SetUseSkin(SelectSkinType, skinId);
        RefreshUI();
        SetBackGroundImg();
    }
    public void OnClickSkinUpgrade()
    {
        var skinType = SkinSlotList[SelectSlotIndex].SkinType;
        var level = PlayerData.Instance.GetSkinSlotLevel(skinType);
        var data = DataManager.Instance.SkinSlotLevelDataList[skinType][level];
        SkinSlotLevelData levelUpdata = null;
        if (DataManager.Instance.SkinSlotLevelDataList[skinType].Count > level + 1)
            levelUpdata = DataManager.Instance.SkinSlotLevelDataList[skinType][level + 1];

        UnityAction yesAction = () =>
        {
            if (CommonFunc.UseCoin(data.cost))
            {
                SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.LEVEL);
                PlayerData.Instance.SetSkinSlotLevel(skinType, level + 1);
                ShowToastMsg(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_SHOP_UPGRADE_SKIN_SLOT"));
            }

            RefreshUI();
        };

        var skillData = SkillManager.Instance.GetSkillData(levelUpdata.skill);
        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("UPGRADE_SKIN_TITLE", skillData.GetDesc()), yesAction);
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
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
            if(ToastMsgList[i].Empty)
            {
                ToastMsgList[i].gameObject.SetActive(true);
                ToastMsgList[i].gameObject.transform.localPosition = ToastPos.transform.localPosition;
                ToastMsgList[i].SetMsg(msg);
                break;
            }
        }
    }
}
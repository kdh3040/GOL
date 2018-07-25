﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupGameShopSkin : MonoBehaviour {

    public Image SelectIcon;
    public Text SelectDesc;
    public GridLayoutGroup SkinShopGrid;

    public Button SkinBuyButton;
    public Button SkinEquipButton;

    private CommonData.SKIN_TYPE mSkinType = CommonData.SKIN_TYPE.NONE;
    private int mSelectSkinId = 0;
    private List<UIShopSkinSlot> mSkinSlotList = new List<UIShopSkinSlot>();
    private Dictionary<CommonData.SKIN_TYPE, List<int>> mSkinIndexDic = new Dictionary<CommonData.SKIN_TYPE, List<int>>();

    private int mDefaultSlotViewCount = 8;

    void Awake()
    {
        SkinBuyButton.onClick.AddListener(OnClickBuy);
        SkinEquipButton.onClick.AddListener(OnClickEquip);
    }

    public CommonData.SKIN_TYPE ConvertShopTab(PopupGameShop.TAB_TYPE type)
    {
        switch (type)
        {
            case PopupGameShop.TAB_TYPE.CHAR:
                return CommonData.SKIN_TYPE.CHAR;
            case PopupGameShop.TAB_TYPE.DOOR:
                return CommonData.SKIN_TYPE.DOOR;
            case PopupGameShop.TAB_TYPE.BG:
                return CommonData.SKIN_TYPE.BACKGROUND;
            default:
                break;
        }

        return CommonData.SKIN_TYPE.NONE;
    }

    public void ShowUI(PopupGameShop.TAB_TYPE type)
    {
        if (mSkinType == ConvertShopTab(type))
            return;

        mSkinType = ConvertShopTab(type);
        if (mSkinSlotList.Count <= 0)
        {
            for (int i = 0; i < mDefaultSlotViewCount; i++)
            {
                var obj = Instantiate(Resources.Load("Prefab/UIShopSkinSlot"), SkinShopGrid.gameObject.transform) as GameObject;
                var slot = obj.GetComponent<UIShopSkinSlot>();
                mSkinSlotList.Add(slot);
                slot.SlotButton.onClick.AddListener(() => {
                    if(slot.mSkinData != null)
                        OnClickSlot(slot.mSkinData.id);
                });
            }

            var tempCharIndexList = new List<int>();
            var charDataDicEnumerator = DataManager.Instance.CharDataDic.GetEnumerator();
            while (charDataDicEnumerator.MoveNext())
            {
                tempCharIndexList.Add(charDataDicEnumerator.Current.Key);
            }

            tempCharIndexList.Sort(delegate (int A, int B)
            {
                if (A > B)
                    return 1;
                else
                    return -1;
            });

            mSkinIndexDic.Add(CommonData.SKIN_TYPE.CHAR, tempCharIndexList);

            var tempDoorIndexList = new List<int>();
            var doorDataDicEnumerator = DataManager.Instance.DoorDataDic.GetEnumerator();
            while (doorDataDicEnumerator.MoveNext())
            {
                tempDoorIndexList.Add(doorDataDicEnumerator.Current.Key);
            }

            tempDoorIndexList.Sort(delegate (int A, int B)
            {
                if (A > B)
                    return 1;
                else
                    return -1;
            });

            mSkinIndexDic.Add(CommonData.SKIN_TYPE.DOOR, tempDoorIndexList);

            var tempBGIndexList = new List<int>();
            var bgDataDicEnumerator = DataManager.Instance.BackGroundDataDic.GetEnumerator();
            while (bgDataDicEnumerator.MoveNext())
            {
                tempBGIndexList.Add(bgDataDicEnumerator.Current.Key);
            }

            tempBGIndexList.Sort(delegate (int A, int B)
            {
                if (A > B)
                    return 1;
                else
                    return -1;
            });

            mSkinIndexDic.Add(CommonData.SKIN_TYPE.BACKGROUND, tempBGIndexList);
        }

        Initialize();

        OnClickSlot(mSkinSlotList[0].mSkinData.id);
    }

    public void Initialize()
    {
        var tempList = new List<int>();

        tempList = mSkinIndexDic[mSkinType];

        for (int i = 0; i < mSkinSlotList.Count; i++)
        {
            if (i < tempList.Count)
                mSkinSlotList[i].SetData(mSkinType, tempList[i]);
            else
                mSkinSlotList[i].SetData(mSkinType, 0);
        }
    }

    
    
    public void OnClickSlot(int id)
    {
        for (int i = 0; i < mSkinSlotList.Count; i++)
        {
            if (mSkinSlotList[i].mSkinData != null && 
                mSkinSlotList[i].mSkinData.id == id)
                mSkinSlotList[i].SetSelect(true);
            else
                mSkinSlotList[i].SetSelect(false);
        }

        mSelectSkinId = id;
        RefreshTopUI();
    }

    public void RefreshTopUI()
    {
        SkinData skinData = null;
        switch (mSkinType)
        {
            case CommonData.SKIN_TYPE.CHAR:
                skinData = DataManager.Instance.CharDataDic[mSelectSkinId];
                break;
            case CommonData.SKIN_TYPE.DOOR:
                skinData = DataManager.Instance.DoorDataDic[mSelectSkinId];
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                skinData = DataManager.Instance.BackGroundDataDic[mSelectSkinId];
                break;
            default:
                break;
        }
        if(skinData != null)
        {
            SelectIcon.sprite = (Sprite)Resources.Load(skinData.GetIcon(), typeof(Sprite));
            SelectDesc.text = skinData.desc;
        }
    }
    public void RefreshMidUI()
    {
        for (int i = 0; i < mSkinSlotList.Count; i++)
        {
            mSkinSlotList[i].RefreshUI();
        }
    }
    public void OnClickBuy()
    {
        UnityAction yesAction = () =>
        {
            PopupManager.Instance.DismissPopup();
            SkinData skinData = null;
            switch (mSkinType)
            {
                case CommonData.SKIN_TYPE.CHAR:
                    skinData = DataManager.Instance.CharDataDic[mSelectSkinId];
                    break;
                case CommonData.SKIN_TYPE.DOOR:
                    skinData = DataManager.Instance.DoorDataDic[mSelectSkinId];
                    break;
                case CommonData.SKIN_TYPE.BACKGROUND:
                    skinData = DataManager.Instance.BackGroundDataDic[mSelectSkinId];
                    break;
                default:
                    break;
            }

            if (CommonFunc.UseCoin(skinData.cost))
            {
                switch (mSkinType)
                {
                    case CommonData.SKIN_TYPE.CHAR:
                        PlayerData.Instance.AddChar(mSelectSkinId);
                        break;
                    case CommonData.SKIN_TYPE.DOOR:
                        PlayerData.Instance.AddDoor(mSelectSkinId);
                        break;
                    case CommonData.SKIN_TYPE.BACKGROUND:
                        PlayerData.Instance.AddBG(mSelectSkinId);
                        break;
                    default:
                        break;
                }
                
                RefreshMidUI();
            }
        };
        UnityAction noAction = () =>
        {
            PopupManager.Instance.DismissPopup();
        };
        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("BUY_SKIN_TITLE"), yesAction, noAction);
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);

    }
    public void OnClickEquip()
    {
        switch (mSkinType)
        {
            case CommonData.SKIN_TYPE.CHAR:
                PlayerData.Instance.SetUseCharId(mSelectSkinId);
                break;
            case CommonData.SKIN_TYPE.DOOR:
                PlayerData.Instance.SetUseDoorId(mSelectSkinId);
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                PlayerData.Instance.SetUseBGId(mSelectSkinId);
                break;
            default:
                break;
        }
        RefreshMidUI();
    }
}

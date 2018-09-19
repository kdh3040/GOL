﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupGamePurchase : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_PURCHASE;
    }

    public class PopupData : PopupUIData
    {
        public CommonData.POINT_TYPE BuyType;
        public PopupData(CommonData.POINT_TYPE buyType = CommonData.POINT_TYPE.DDONG)
        {
            BuyType = buyType;
        }
    }

    public UITopBar TopBar;
    public List<UIPurchaseSlot> SlotList = new List<UIPurchaseSlot>();

    public override void ShowPopup(PopupUIData data)
    {
        this.SetBackGroundImg();
        PopupData popupData = data as PopupData;

        TopBar.Initialize(true);

        if(popupData.BuyType == CommonData.POINT_TYPE.COIN)
        {
            SlotList[0].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 1100, CommonData.POINT_TYPE.COIN, 1500, CommonData.PURCHASE_ID_ARRAY[0]);
            SlotList[0].SlotButton.onClick.AddListener(() => { OnClickPurchase(0); });

            SlotList[1].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 3300, CommonData.POINT_TYPE.COIN, 4500, CommonData.PURCHASE_ID_ARRAY[1]);
            SlotList[1].SlotButton.onClick.AddListener(() => { OnClickPurchase(1); });

            SlotList[2].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 5500, CommonData.POINT_TYPE.COIN, 7500, CommonData.PURCHASE_ID_ARRAY[2]);
            SlotList[2].SlotButton.onClick.AddListener(() => { OnClickPurchase(2); });

            SlotList[3].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 11000, CommonData.POINT_TYPE.COIN, 16000, CommonData.PURCHASE_ID_ARRAY[3]);
            SlotList[3].SlotButton.onClick.AddListener(() => { OnClickPurchase(3); });

            SlotList[4].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 33000, CommonData.POINT_TYPE.COIN, 51000, CommonData.PURCHASE_ID_ARRAY[4]);
            SlotList[4].SlotButton.onClick.AddListener(() => { OnClickPurchase(4); });

            SlotList[5].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 55000, CommonData.POINT_TYPE.COIN, 90000, CommonData.PURCHASE_ID_ARRAY[5]);
            SlotList[5].SlotButton.onClick.AddListener(() => { OnClickPurchase(5); });

            SlotList[6].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 110000, CommonData.POINT_TYPE.COIN, 200000, CommonData.PURCHASE_ID_ARRAY[6]);
            SlotList[6].SlotButton.onClick.AddListener(() => { OnClickPurchase(6); });

            SlotList[7].gameObject.SetActive(false);
        }
        else
        {
            SlotList[0].SetPurchaseFreeSlot();
            SlotList[0].SlotButton.onClick.AddListener(() => {  });

            SlotList[1].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 1100, CommonData.POINT_TYPE.DDONG, 20, CommonData.PURCHASE_DDONG_ARRAY[0]);
            SlotList[1].SlotButton.onClick.AddListener(() => { OnClickPurchase(0); });

            SlotList[2].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 3300, CommonData.POINT_TYPE.DDONG, 60, CommonData.PURCHASE_DDONG_ARRAY[1]);
            SlotList[2].SlotButton.onClick.AddListener(() => { OnClickPurchase(1); });

            SlotList[3].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 5500, CommonData.POINT_TYPE.DDONG, 105, CommonData.PURCHASE_DDONG_ARRAY[2]);
            SlotList[3].SlotButton.onClick.AddListener(() => { OnClickPurchase(2); });

            SlotList[4].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 11000, CommonData.POINT_TYPE.DDONG, 220, CommonData.PURCHASE_DDONG_ARRAY[3]);
            SlotList[4].SlotButton.onClick.AddListener(() => { OnClickPurchase(3); });

            SlotList[5].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 33000, CommonData.POINT_TYPE.DDONG, 650, CommonData.PURCHASE_DDONG_ARRAY[4]);
            SlotList[5].SlotButton.onClick.AddListener(() => { OnClickPurchase(4); });

            SlotList[6].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 55000, CommonData.POINT_TYPE.DDONG, 1100, CommonData.PURCHASE_DDONG_ARRAY[5]);
            SlotList[6].SlotButton.onClick.AddListener(() => { OnClickPurchase(5); });

            SlotList[7].gameObject.SetActive(true);
            SlotList[7].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 110000, CommonData.POINT_TYPE.DDONG, 2300, CommonData.PURCHASE_DDONG_ARRAY[6]);
            SlotList[7].SlotButton.onClick.AddListener(() => { OnClickPurchase(6); });
        }
    }

    public void OnClickPurchase(int index)
    {
        if(SlotList[index].AdsSlot)
        {
            UnityAction yesAction = () =>
            {
                //
            };
            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("PURCHASE_ADS_BUY_DDONG"), yesAction);
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
        }
        else
        {
            UnityAction yesAction = () =>
            {
                PurchaseManager.Instance.BuyProductID(SlotList[index]);
            };

            if (SlotList[index].RewardType == CommonData.POINT_TYPE.COIN)
            {
                var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("PURCHASE_COIN_BUY_MSG", CommonFunc.ConvertNumber(SlotList[index].Reward)), yesAction);
                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
            }
            else if (SlotList[index].RewardType == CommonData.POINT_TYPE.DDONG)
            {
                var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("PURCHASE_DDONG_BUY_MSG", CommonFunc.ConvertNumber(SlotList[index].Reward)), yesAction);
                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
            }
        }
        
    }

}

using System.Collections;
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
        public PopupData(CommonData.POINT_TYPE type = CommonData.POINT_TYPE.DDONG)
        {
            BuyType = type;
        }
    }

    public UITopBar TopBar;
    public List<UIPurchaseSlot> SlotList = new List<UIPurchaseSlot>();

    public override void ShowPopup(PopupUIData data)
    {
        PopupData popupData = data as PopupData;

        TopBar.Initialize(true);

        if(popupData.BuyType == CommonData.POINT_TYPE.COIN)
        {
            SlotList[0].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 1000, CommonData.POINT_TYPE.COIN, 15000, CommonData.PURCHASE_ID_ARRAY[0]);
            SlotList[0].SlotButton.onClick.AddListener(() => { OnClickPurchase(0); });

            SlotList[1].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 5000, CommonData.POINT_TYPE.COIN, 75000, CommonData.PURCHASE_ID_ARRAY[1]);
            SlotList[1].SlotButton.onClick.AddListener(() => { OnClickPurchase(1); });

            SlotList[2].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 10000, CommonData.POINT_TYPE.COIN, 150000, CommonData.PURCHASE_ID_ARRAY[2]);
            SlotList[2].SlotButton.onClick.AddListener(() => { OnClickPurchase(2); });

            SlotList[3].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 50000, CommonData.POINT_TYPE.COIN, 750000, CommonData.PURCHASE_ID_ARRAY[3]);
            SlotList[3].SlotButton.onClick.AddListener(() => { OnClickPurchase(3); });

            SlotList[4].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 100000, CommonData.POINT_TYPE.COIN, 1500000, CommonData.PURCHASE_ID_ARRAY[4]);
            SlotList[4].SlotButton.onClick.AddListener(() => { OnClickPurchase(4); });
        }
        else
        {
            SlotList[0].SetPurchaseSlot(CommonData.POINT_TYPE.COIN, 3000, CommonData.POINT_TYPE.DDONG, 1);
            SlotList[0].SlotButton.onClick.AddListener(() => { OnClickPurchase(0); });

            SlotList[1].SetPurchaseSlot(CommonData.POINT_TYPE.COIN, 14000, CommonData.POINT_TYPE.DDONG, 5);
            SlotList[1].SlotButton.onClick.AddListener(() => { OnClickPurchase(1); });

            SlotList[2].SetPurchaseSlot(CommonData.POINT_TYPE.COIN, 28000, CommonData.POINT_TYPE.DDONG, 10);
            SlotList[2].SlotButton.onClick.AddListener(() => { OnClickPurchase(2); });

            SlotList[3].SetPurchaseSlot(CommonData.POINT_TYPE.COIN, 52000, CommonData.POINT_TYPE.DDONG, 20);
            SlotList[3].SlotButton.onClick.AddListener(() => { OnClickPurchase(3); });

            SlotList[4].SetPurchaseSlot(CommonData.POINT_TYPE.COIN, 78000, CommonData.POINT_TYPE.DDONG, 30);
            SlotList[4].SlotButton.onClick.AddListener(() => { OnClickPurchase(4); });
        }
    }

    public void OnClickPurchase(int index)
    {
        UnityAction yesAction = () =>
        {
            //SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUTTON);
            if (SlotList[index].RewardType == CommonData.POINT_TYPE.COIN)
            {
             
                PurchaseManager.Instance.BuyProductID(SlotList[index]);
                // 캐쉬로 구입
                
            }
            else if (SlotList[index].RewardType == CommonData.POINT_TYPE.DDONG)
            {
                if(CommonFunc.UseCoin(SlotList[index].Cost))
                {
                    PlayerData.Instance.PlusDDong(SlotList[index].Reward);
                }
            }
        };

        if (SlotList[index].RewardType == CommonData.POINT_TYPE.COIN)
        {
            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("PURCHASE_COIN_BUY_MSG"), yesAction);
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
        }
        else if (SlotList[index].RewardType == CommonData.POINT_TYPE.DDONG)
        {
            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("PURCHASE_DDONG_BUY_MSG"), yesAction);
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
        }
    }

}

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

    public UITopBar TopBar;
    public List<UIPurchaseSlot> SlotList = new List<UIPurchaseSlot>();

    public override void ShowPopup(PopupUIData data)
    {
        TopBar.Initialize(true);

        SlotList[0].SetPurchaseSlot(CommonData.POINT_TYPE.COIN, 1000, CommonData.POINT_TYPE.DDONG, 5);
        SlotList[0].SlotButton.onClick.AddListener(() => { OnClickPurchase(0); });

        SlotList[1].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 1, CommonData.POINT_TYPE.COIN, 1000, CommonData.PURCHASE_ID_ARRAY[0]);
        SlotList[1].SlotButton.onClick.AddListener(() => { OnClickPurchase(1); });

        SlotList[2].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 9, CommonData.POINT_TYPE.COIN, 9000, CommonData.PURCHASE_ID_ARRAY[1]);
        SlotList[2].SlotButton.onClick.AddListener(() => { OnClickPurchase(2); });

        SlotList[3].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 19, CommonData.POINT_TYPE.COIN, 19000, CommonData.PURCHASE_ID_ARRAY[2]);
        SlotList[3].SlotButton.onClick.AddListener(() => { OnClickPurchase(3); });

        SlotList[4].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 99, CommonData.POINT_TYPE.COIN, 99000, CommonData.PURCHASE_ID_ARRAY[3]);
        SlotList[4].SlotButton.onClick.AddListener(() => { OnClickPurchase(4); });

        //PurchaseManager.Instance.InitializePurchasing();

    }

    public void OnClickPurchase(int index)
    {
        UnityAction yesAction = () =>
        {
            if(SlotList[index].RewardType == CommonData.POINT_TYPE.COIN)
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

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
        public PopupData(CommonData.POINT_TYPE buyType = CommonData.POINT_TYPE.DDONG)
        {
            BuyType = buyType;
        }
    }

    public UITopBar TopBar;
    public List<UIPurchaseSlot> SlotList = new List<UIPurchaseSlot>();
    public GameObject ToastPos;

    private List<UIToastMsg> ToastMsgList = new List<UIToastMsg>();

    public override void ShowPopup(PopupUIData data)
    {
        this.SetBackGroundImg();
        PopupData popupData = data as PopupData;

        TopBar.Initialize(true);

        if(popupData.BuyType == CommonData.POINT_TYPE.COIN)
        {
            SlotList[0].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 1100, CommonData.POINT_TYPE.COIN, 1500, 50, CommonData.PURCHASE_ID_ARRAY[0]);
            SlotList[0].SlotButton.onClick.RemoveAllListeners();
            SlotList[0].SlotButton.onClick.AddListener(() => { OnClickPurchase(0); });

            SlotList[1].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 3300, CommonData.POINT_TYPE.COIN, 4500, 50, CommonData.PURCHASE_ID_ARRAY[1]);
            SlotList[1].SlotButton.onClick.RemoveAllListeners();
            SlotList[1].SlotButton.onClick.AddListener(() => { OnClickPurchase(1); });

            SlotList[2].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 5500, CommonData.POINT_TYPE.COIN, 7500, 50, CommonData.PURCHASE_ID_ARRAY[2]);
            SlotList[2].SlotButton.onClick.RemoveAllListeners();
            SlotList[2].SlotButton.onClick.AddListener(() => { OnClickPurchase(2); });

            SlotList[3].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 11000, CommonData.POINT_TYPE.COIN, 16000, 60, CommonData.PURCHASE_ID_ARRAY[3]);
            SlotList[3].SlotButton.onClick.RemoveAllListeners();
            SlotList[3].SlotButton.onClick.AddListener(() => { OnClickPurchase(3); });

            SlotList[4].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 33000, CommonData.POINT_TYPE.COIN, 51000, 70, CommonData.PURCHASE_ID_ARRAY[4]);
            SlotList[4].SlotButton.onClick.RemoveAllListeners();
            SlotList[4].SlotButton.onClick.AddListener(() => { OnClickPurchase(4); });

            SlotList[5].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 55000, CommonData.POINT_TYPE.COIN, 90000, 80, CommonData.PURCHASE_ID_ARRAY[5]);
            SlotList[5].SlotButton.onClick.RemoveAllListeners();
            SlotList[5].SlotButton.onClick.AddListener(() => { OnClickPurchase(5); });

            SlotList[6].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 110000, CommonData.POINT_TYPE.COIN, 200000, 100, CommonData.PURCHASE_ID_ARRAY[6]);
            SlotList[6].SlotButton.onClick.RemoveAllListeners();
            SlotList[6].SlotButton.onClick.AddListener(() => { OnClickPurchase(6); });

            SlotList[7].gameObject.SetActive(false);
        }
        else
        {
            SlotList[0].SetPurchaseFreeSlot();
            SlotList[0].SlotButton.onClick.RemoveAllListeners();
            SlotList[0].SlotButton.onClick.AddListener(() => { OnClickPurchase(0); });

            SlotList[1].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 1100, CommonData.POINT_TYPE.DDONG, 20, 33,CommonData.PURCHASE_DDONG_ARRAY[0]);
            SlotList[1].SlotButton.onClick.RemoveAllListeners();
            SlotList[1].SlotButton.onClick.AddListener(() => { OnClickPurchase(1); });

            SlotList[2].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 3300, CommonData.POINT_TYPE.DDONG, 60, 33, CommonData.PURCHASE_DDONG_ARRAY[1]);
            SlotList[2].SlotButton.onClick.RemoveAllListeners();
            SlotList[2].SlotButton.onClick.AddListener(() => { OnClickPurchase(2); });

            SlotList[3].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 5500, CommonData.POINT_TYPE.DDONG, 105, 40, CommonData.PURCHASE_DDONG_ARRAY[2]);
            SlotList[3].SlotButton.onClick.RemoveAllListeners();
            SlotList[3].SlotButton.onClick.AddListener(() => { OnClickPurchase(3); });

            SlotList[4].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 11000, CommonData.POINT_TYPE.DDONG, 220, 47, CommonData.PURCHASE_DDONG_ARRAY[3]);
            SlotList[4].SlotButton.onClick.RemoveAllListeners();
            SlotList[4].SlotButton.onClick.AddListener(() => { OnClickPurchase(4); });

            SlotList[5].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 33000, CommonData.POINT_TYPE.DDONG, 675, 50, CommonData.PURCHASE_DDONG_ARRAY[4]);
            SlotList[5].SlotButton.onClick.RemoveAllListeners();
            SlotList[5].SlotButton.onClick.AddListener(() => { OnClickPurchase(5); });

            SlotList[6].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 55000, CommonData.POINT_TYPE.DDONG, 1200, 60, CommonData.PURCHASE_DDONG_ARRAY[5]);
            SlotList[6].SlotButton.onClick.RemoveAllListeners();
            SlotList[6].SlotButton.onClick.AddListener(() => { OnClickPurchase(6); });

            SlotList[7].gameObject.SetActive(true);
            SlotList[7].SetPurchaseSlot(CommonData.POINT_TYPE.CASH, 110000, CommonData.POINT_TYPE.DDONG, 2500, 67, CommonData.PURCHASE_DDONG_ARRAY[6]);
            SlotList[7].SlotButton.onClick.RemoveAllListeners();
            SlotList[7].SlotButton.onClick.AddListener(() => { OnClickPurchase(7); });
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
    }

    public void OnClickPurchase(int index)
    {
        if(SlotList[index].AdsSlot)
        {
            if (PlayerData.Instance.MyDDong >= 10)
            {
                var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("PURCHASE_ADS_MY_DDONG"));
                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
            }
            else
            {
                UnityAction yesAction = () =>
                {
                    //
                    //AdManager.Instance.ShowFreeDDongVideo();
                    UnityAdsHelper.Instance.ShowRewardedAd();
                };
                var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("PURCHASE_ADS_BUY_DDONG"), yesAction);
                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
            }
       
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupMsg : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.MSG_POPUP;
    }

    public class PopupData : PopupUIData
    {
        public string Title;
        public string Desc;
        public MSG_POPUP_TYPE MsgPopupType = MSG_POPUP_TYPE.OK;
        public UnityAction YesAction = null;
        public UnityAction NoAction = null;
        public UnityAction ChargeAction = null;
        public string BtnTitle = "";
        public CommonData.POINT_TYPE PointType = CommonData.POINT_TYPE.COIN;
        public int PointValue = 0;

        public PopupData(string desc)
        {
            Title = string.Empty;
            Desc = desc;
            MsgPopupType = MSG_POPUP_TYPE.OK;
        }

        public PopupData(string desc, UnityAction yesAction, UnityAction noAction = null, UnityAction chargeAction = null)
        {
            Title = string.Empty;
            Desc = desc;
            if(chargeAction != null)
                MsgPopupType = MSG_POPUP_TYPE.YES_CHARGE;
            else
                MsgPopupType = MSG_POPUP_TYPE.YES_NO;
            YesAction = yesAction;
            NoAction = noAction;
            ChargeAction = chargeAction;
        }

        public PopupData(string desc, string btnTitle, MSG_POPUP_TYPE type, CommonData.POINT_TYPE pointType, int value, UnityAction yesAction)
        {
            Title = string.Empty;
            Desc = desc;
            BtnTitle = btnTitle;
            MsgPopupType = type;
            YesAction = yesAction;
            PointValue = value;
            PointType = pointType;
        }
    }

    public enum MSG_POPUP_TYPE
    {
        OK,
        YES_NO,
        YES_CHARGE,
        BUY_NO,
        UPGRADE_NO,
    }


    public Button YesButton;
    public Button NoButton;
    public Button ChargeButton;
    public Button CostButton;
    public Image CostButtonImg;
    public Text CostTitle;
    public Image CostIcon;
    public UIPointValue CostValue;
    public Text Title;
    public Text Desc;
    private PopupData mPopupData;

    void Awake()
    {
        YesButton.onClick.AddListener(OnClickYes);
        NoButton.onClick.AddListener(OnClickNo);
        ChargeButton.onClick.AddListener(OnClickCharge);
    }

    public override void ShowPopup(PopupUIData data)
    {
        mPopupData = data as PopupData;
        if(mPopupData.Title == string.Empty)
            Title.text = LocalizeData.Instance.GetLocalizeString("MSG_POPUP_TITLE_NORMAL");
        else
            Title.text = LocalizeData.Instance.GetLocalizeString(mPopupData.Title);

        Desc.text = mPopupData.Desc;

        YesButton.gameObject.SetActive(false);
        NoButton.gameObject.SetActive(false);
        ChargeButton.gameObject.SetActive(false);
        CostButton.gameObject.SetActive(false);

        switch (mPopupData.MsgPopupType)
        {
            case MSG_POPUP_TYPE.OK:
                YesButton.gameObject.SetActive(true);
                break;
            case MSG_POPUP_TYPE.YES_NO:
                YesButton.gameObject.SetActive(true);
                NoButton.gameObject.SetActive(true);
                break;
            case MSG_POPUP_TYPE.YES_CHARGE:
                YesButton.gameObject.SetActive(true);
                ChargeButton.gameObject.SetActive(true);
                break;
            case MSG_POPUP_TYPE.BUY_NO:
            case MSG_POPUP_TYPE.UPGRADE_NO:
                {
                    CostButton.gameObject.SetActive(true);
                    NoButton.gameObject.SetActive(true);

                    switch (mPopupData.PointType)
                    {
                        case CommonData.POINT_TYPE.DDONG:
                            CommonFunc.SetImageFile("Renewal/UI/icon_ddong", ref CostIcon, false);
                            break;
                        case CommonData.POINT_TYPE.COIN:
                            CommonFunc.SetImageFile("Renewal/UI/icon_gold", ref CostIcon, false);
                            break;
                    }

                    CostValue.SetValue(mPopupData.PointValue);

                    if(mPopupData.MsgPopupType == MSG_POPUP_TYPE.UPGRADE_NO)
                        CommonFunc.SetImageFile("Renewal/UI/btn_bg_4", ref CostButtonImg, false);
                    else
                        CommonFunc.SetImageFile("Renewal/UI/btn_bg_1", ref CostButtonImg, false);

                    CostTitle.text = mPopupData.BtnTitle;

                    break;
                }
            default:
                break;
        }
    }

    private void OnClickYes()
    {
        PopupManager.Instance.DismissPopup();
        if (mPopupData.YesAction != null)
            mPopupData.YesAction();
    }
    private void OnClickNo()
    {
        PopupManager.Instance.DismissPopup();
        if (mPopupData.NoAction != null)
            mPopupData.NoAction();
    }

    private void OnClickCharge()
    {
        PopupManager.Instance.DismissPopup();
        if (mPopupData.ChargeAction != null)
            mPopupData.ChargeAction();
    }

}

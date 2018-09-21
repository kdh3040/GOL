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

        public PopupData(string desc, UnityAction yesAction, CommonData.POINT_TYPE pointType, int value)
        {
            Title = string.Empty;
            Desc = desc;
            MsgPopupType = MSG_POPUP_TYPE.YES_NO;
            PointType = pointType;
            PointValue = value;
            YesAction = yesAction;
        }
    }

    public enum MSG_POPUP_TYPE
    {
        OK,
        YES_NO,
        YES_CHARGE
    }


    public Button YesButton;
    public Button NoButton;
    public Button ChargeButton;
    public Text Title;
    public Text Desc;
    public GameObject PointObj;
    public Text PointDesc;
    public UIPointValue PointValue;
    public Image PointIcon;
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

        if(mPopupData.PointValue == 0)
        {
            Desc.gameObject.SetActive(true);
            PointObj.gameObject.SetActive(false);
            Desc.text = mPopupData.Desc;
        }
        else
        {
            Desc.gameObject.SetActive(false);
            PointObj.gameObject.SetActive(true);
            PointValue.SetValue(mPopupData.PointValue);

            switch (mPopupData.PointType)
            {
                case CommonData.POINT_TYPE.DDONG:
                    CommonFunc.SetImageFile("Renewal/UI/icon_ddong", ref PointIcon);
                    break;
                case CommonData.POINT_TYPE.COIN:
                    CommonFunc.SetImageFile("Renewal/UI/icon_gold", ref PointIcon);
                    break;
            }
            PointDesc.text = mPopupData.Desc;
        }

        YesButton.gameObject.SetActive(true);
        NoButton.gameObject.SetActive(mPopupData.MsgPopupType == MSG_POPUP_TYPE.YES_NO);
        ChargeButton.gameObject.SetActive(mPopupData.MsgPopupType == MSG_POPUP_TYPE.YES_CHARGE);
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

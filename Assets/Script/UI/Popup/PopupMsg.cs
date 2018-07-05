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
        public string Msg;
        public MSG_POPUP_TYPE MsgPopupType = MSG_POPUP_TYPE.OK;
        public UnityAction YesAction = null;
        public UnityAction NoAction = null;
        public PopupData(string msg)
        {
            Msg = msg;
            MsgPopupType = MSG_POPUP_TYPE.OK;
        }

        public PopupData(string msg, UnityAction yesAction, UnityAction noAction)
        {
            Msg = msg;
            MsgPopupType = MSG_POPUP_TYPE.YES_NO;
            YesAction = yesAction;
            NoAction = noAction;
        }
    }

    public enum MSG_POPUP_TYPE
    {
        OK,
        YES_NO,
    }


    public Button YesButton;
    public Button NoButton;

    public Text Msg;
    public Button OkButton;
    public Button BackgroundButton;

    private PopupData mPopupData;

    void Awake()
    {
        OkButton.onClick.AddListener(OnClickOk);
        BackgroundButton.onClick.AddListener(()=> { PopupManager.Instance.DismissPopup(); });
        YesButton.onClick.AddListener(OnClickYes);
        NoButton.onClick.AddListener(OnClickNo);
    }

    public override void ShowPopup(PopupUIData data)
    {
        mPopupData = data as PopupData;
        Msg.text = mPopupData.Msg;

        OkButton.gameObject.SetActive(mPopupData.MsgPopupType == MSG_POPUP_TYPE.OK);
        YesButton.gameObject.SetActive(mPopupData.MsgPopupType == MSG_POPUP_TYPE.YES_NO);
        NoButton.gameObject.SetActive(mPopupData.MsgPopupType == MSG_POPUP_TYPE.YES_NO);
    }

    private void OnClickOk()
    {
        PopupManager.Instance.DismissPopup();
    }

    private void OnClickYes()
    {
        if (mPopupData.YesAction != null)
            mPopupData.YesAction();
    }
    private void OnClickNo()
    {
        if (mPopupData.NoAction != null)
            mPopupData.NoAction();
    }
}

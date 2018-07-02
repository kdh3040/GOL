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
        }

        public PopupData(string msg, UnityAction yesAction, UnityAction noAction)
        {
            Msg = msg;
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

    void Awake()
    {
        OkButton.onClick.AddListener(OnClickOk);
    }

    public override void ShowPopup(PopupUIData data)
    {
        var popupData = data as PopupData;
        Msg.text = popupData.Msg;

        OkButton.gameObject.SetActive(popupData.MsgPopupType == MSG_POPUP_TYPE.OK);
        YesButton.gameObject.SetActive(popupData.MsgPopupType == MSG_POPUP_TYPE.YES_NO);
        NoButton.gameObject.SetActive(popupData.MsgPopupType == MSG_POPUP_TYPE.YES_NO);

        if(popupData.MsgPopupType == MSG_POPUP_TYPE.YES_NO)
        {
            YesButton.onClick.AddListener(popupData.YesAction);
            NoButton.onClick.AddListener(popupData.NoAction);
        }
    }

    private void OnClickOk()
    {
        PopupManager.Instance.DismissPopup();
    }
}

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
        public PopupData(string desc)
        {
            Title = string.Empty;
            Desc = desc;
            MsgPopupType = MSG_POPUP_TYPE.OK;
        }

        public PopupData(string desc, UnityAction yesAction, UnityAction noAction = null)
        {
            Title = string.Empty;
            Desc = desc;
            MsgPopupType = MSG_POPUP_TYPE.YES_NO;
            YesAction = yesAction;
            NoAction = noAction;
        }
        public PopupData(string title, string desc)
        {
            Title = title;
            Desc = desc;
            MsgPopupType = MSG_POPUP_TYPE.OK;
        }

        public PopupData(string title, string desc, UnityAction yesAction, UnityAction noAction = null)
        {
            Title = title;
            Desc = desc;
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
    public Text Title;
    public Text Desc;
    private PopupData mPopupData;

    void Awake()
    {
        YesButton.onClick.AddListener(OnClickYes);
        NoButton.onClick.AddListener(OnClickNo);
    }

    public override void ShowPopup(PopupUIData data)
    {
        mPopupData = data as PopupData;
        if(mPopupData.Title == string.Empty)
            Title.text = LocalizeData.Instance.GetLocalizeString("MSG_POPUP_TITLE_NORMAL");
        else
            Title.text = LocalizeData.Instance.GetLocalizeString(mPopupData.Title);
        Desc.text = mPopupData.Desc;

        YesButton.gameObject.SetActive(true);
        NoButton.gameObject.SetActive(mPopupData.MsgPopupType == MSG_POPUP_TYPE.YES_NO);
    }

    private void OnClickYes()
    {
        PlayClickSound();
        PopupManager.Instance.DismissPopup();
        if (mPopupData.YesAction != null)
            mPopupData.YesAction();
    }
    private void OnClickNo()
    {
        PlayClickSound();
        PopupManager.Instance.DismissPopup();
        if (mPopupData.NoAction != null)
            mPopupData.NoAction();
    }

    public void PlayClickSound()
    {
        if (PlayerData.Instance.GetSoundSetting() == true)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupManager : MonoBehaviour {

    public static PopupManager _instance = null;
    public static PopupManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PopupManager>() as PopupManager;
            }
            return _instance;
        }
    }

    public enum POPUP_TYPE
    {
        NONE,
        GAME_END,
        GAME_PAUSE,
        GAME_SHOP,
        GAME_BOOK,
        GAME_ENDING_SCENE,
        GAME_READY,
        GAME_SETTING,
        GAME_PURCHASE,
        MSG_POPUP,
    }

    private Dictionary<POPUP_TYPE, PopupUI> mPopupUIList = new Dictionary<POPUP_TYPE, PopupUI>();
    private PopupUI mShowPopup = null;
    private List<PopupUI> mShowPopupList = new List<PopupUI>();
    public GameObject PopupRoot;
    
    private void Start()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(PopupRoot);
    }

    public void AddPopup(PopupUI page)
    {
        mPopupUIList.Add(page.GetPopupType(), page);
        page.gameObject.SetActive(false);
    }

    public bool IsLoadPopup(POPUP_TYPE type)
    {
        if (mPopupUIList.ContainsKey(type))
            return true;

        return false;
    }

    public void ShowPopup(POPUP_TYPE type, PopupUIData data = null)
    {
//        SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUTTON);
        var popup = mPopupUIList[type];
        if (mShowPopup != null && mShowPopup.GetPopupType() == popup.GetPopupType())
            return;

        popup.gameObject.SetActive(true);
        popup.ShowPopup(data);
        mShowPopup = popup;
        mShowPopupList.Add(popup);
    }

    public void DismissPopup()
    {
        if(mShowPopup != null)
        {
            mShowPopupList.Remove(mShowPopup);

            mShowPopup.gameObject.SetActive(false);
            mShowPopup.DismissPopup();
            if (mShowPopupList.Count > 0)
                mShowPopup = mShowPopupList[mShowPopupList.Count - 1];
            else
                mShowPopup = null;
        }
    }

    public void AllDismissPopup()
    {
        for (int i = 0; i < mShowPopupList.Count; i++)
        {
            mShowPopupList[i].DismissPopup();
            mShowPopupList[i].gameObject.SetActive(false);
        }
        mShowPopupList.Clear();
        mShowPopup = null;
    }

    public POPUP_TYPE CurrentPopupType()
    {
        if (mShowPopup == null)
            return POPUP_TYPE.NONE;

        return mShowPopup.GetPopupType();
    }

    public PopupUI GetCurrentPopup()
    {
        return mShowPopup;
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUTTON);
                if (CurrentPopupType() == PopupManager.POPUP_TYPE.GAME_END)
                {
                    var endPopup = GetCurrentPopup() as PopupGameEnd;
                    if (endPopup == null)
                        return;

                    endPopup.OnClickGameExit();
                    return;
                }

                if (GamePlayManager.Instance.InGame)
                    return;

                if (GManager.Instance.GetVideoStatus() == false )
                {
                    if (mShowPopup == null)
                    {
                        UnityAction yesAction = () =>
                        {
                            Application.Quit();
                        };
                        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("POPUP_GAME_END"), yesAction);
                        ShowPopup(POPUP_TYPE.MSG_POPUP, msgPopupData);
                        return;
                    }
                }
                else if (GManager.Instance.GetVideoStatus() == true)
                {
                    Destroy(GManager.Instance.webViewObject);
                    PlayerData.Instance.SetSoundSetting(PlayerData.Instance.SoundSetting);
                    GManager.Instance.SetVideoStatus(false);
                }
              
                

                DismissPopup();
            }
        }
    }

}

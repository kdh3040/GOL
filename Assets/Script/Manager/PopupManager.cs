﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GAME_END,
        GAME_PAUSE,
        GAME_SHOP,
        MSG_POPUP,
    }

    private Dictionary<POPUP_TYPE, PopupUI> mPopupUIList = new Dictionary<POPUP_TYPE, PopupUI>();
    private PopupUI mShowPopup = null;
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

    public void ShowPopup(POPUP_TYPE type, PopupUIData data = null)
    {
        var popup = mPopupUIList[type];
        popup.gameObject.SetActive(true);
        popup.ShowPopup(data);
        mShowPopup = popup;
    }

    public void DismissPopup()
    {
        if(mShowPopup != null)
        {
            mShowPopup.gameObject.SetActive(false);
            mShowPopup.DismissPopup();
        }
    }
}

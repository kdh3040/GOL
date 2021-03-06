﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPoint : MonoBehaviour {

    public Button ChargeButton;
    public Text PointText;
    public Image PointIcon;
    [System.NonSerialized]
    public CommonData.POINT_TYPE mPointType;
    [System.NonSerialized]
    public int mTempSaveValue = 0;

    void Awake()
    {
        ChargeButton.onClick.AddListener(OnClickCharge);
    }

    public void Initialize(CommonData.POINT_TYPE type)
    {
        mTempSaveValue = -1;
        mPointType = type;

        switch(mPointType)
        {
            case CommonData.POINT_TYPE.DDONG:
                CommonFunc.SetImageFile("Renewal/UI/icon_ddong", ref PointIcon);
                break;
            case CommonData.POINT_TYPE.COIN:
                CommonFunc.SetImageFile("Renewal/UI/icon_gold", ref PointIcon);
                break;
        }

        UpdatePoint();
    }

    public void UpdatePoint()
    {
        switch (mPointType)
        {
            case CommonData.POINT_TYPE.DDONG:
                var time = PlayerData.Instance.GetNextDDongRefileTime();
                if (time == TimeSpan.Zero)
                {
                    if (PlayerData.Instance.MyDDong < CommonData.MAX_DDONG_COUNT)
                        PointText.text = string.Format("{0}/{1} ({2}:{3})", CommonFunc.ConvertNumber(PlayerData.Instance.MyDDong), CommonFunc.ConvertNumber(CommonData.MAX_DDONG_COUNT), time.Minutes, time.Seconds);
                    else
                        PointText.text = string.Format("{0}/{1}", CommonFunc.ConvertNumber(PlayerData.Instance.MyDDong), CommonFunc.ConvertNumber(CommonData.MAX_DDONG_COUNT));
                }    
                else
                    PointText.text = string.Format("{0}/{1} ({2}:{3})", CommonFunc.ConvertNumber(PlayerData.Instance.MyDDong), CommonFunc.ConvertNumber(CommonData.MAX_DDONG_COUNT), time.Minutes, time.Seconds);
                break;
            case CommonData.POINT_TYPE.COIN:
                if (mTempSaveValue == PlayerData.Instance.MyCoin)
                    break;

                mTempSaveValue = PlayerData.Instance.MyCoin;
                PointText.text = CommonFunc.ConvertNumber(PlayerData.Instance.MyCoin);
                break;
        }
    }

    public void OnClickCharge()
    {
        if(PopupManager.Instance.CurrentPopupType() == PopupManager.POPUP_TYPE.GAME_PURCHASE)
            PopupManager.Instance.DismissPopup();

        switch (mPointType)
        {
            case CommonData.POINT_TYPE.DDONG:
                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_PURCHASE, new PopupGamePurchase.PopupData(CommonData.POINT_TYPE.DDONG));
                break;
            case CommonData.POINT_TYPE.COIN:
                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_PURCHASE, new PopupGamePurchase.PopupData(CommonData.POINT_TYPE.COIN));
                break;
        }
    }
}

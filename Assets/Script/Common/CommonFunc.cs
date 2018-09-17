using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommonFunc
{
    static public string ConvertNumber(int count)
    {
        return string.Format("{0:n0}", count);
    }

    static public void SetImageFile(string fileName, ref Image img, bool sizeAuto = true)
    {
        var imgSprite = (Sprite)Resources.Load(fileName, typeof(Sprite));
        if (imgSprite == null)
            return;

        if(sizeAuto)
        {
            RectTransform rt = img.GetComponent<RectTransform>();
            rt.sizeDelta = imgSprite.rect.size;
        }
        img.sprite = imgSprite;
    }

    static public bool IsEnoughCoin(int coin, bool showPopup = false)
    {
        var myCoin = PlayerData.Instance.MyCoin;

        if (myCoin < coin)
        {
            if (showPopup)
            {
                UnityAction chargeAction = () =>
                {
                    PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_PURCHASE, new PopupGamePurchase.PopupData(CommonData.POINT_TYPE.COIN));
                };

                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("GAME_PLAY_LACK_COIN"), null, null, chargeAction));
            }
            return false;
        }
        else
            return true;
    }

    static public bool UseCoin(int coin)
    {
        if (IsEnoughCoin(coin, true))
        {
            PlayerData.Instance.MinusCoin(coin);
            return true;
        }

        return false;
    }

    static public bool IsEnoughDDong(bool showPopup = false)
    {
        var myDDong = PlayerData.Instance.MyDDong;

        if (myDDong <= 0)
        {
            if (showPopup)
            {
                UnityAction chargeAction = () =>
                {
                    PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_PURCHASE, new PopupGamePurchase.PopupData(CommonData.POINT_TYPE.DDONG));
                };

                var time = PlayerData.Instance.GetNextDDongRefileTime();

                string remainTime = "";
                if (time.Minutes > 0)
                    remainTime = string.Format("{0} {1}", LocalizeData.Instance.GetLocalizeString("TIME_VIEW_MIN", time.Minutes), LocalizeData.Instance.GetLocalizeString("TIME_VIEW_SEC", time.Seconds));
                else
                    remainTime = string.Format("{0}", LocalizeData.Instance.GetLocalizeString("TIME_VIEW_SEC", time.Seconds));
                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("GAME_PLAY_LACK_DDONG", remainTime), null, null, chargeAction));
            }
            return false;
        }
        else
            return true;
    }

    static public bool UseDDong()
    {
        if (IsEnoughDDong(true))
        {
            PlayerData.Instance.MinusDDong();
            return true;
        }

        return false;
    }

    static public DateTime GetCurrentTime()
    {
        return DateTime.Now;
    }

    static public int ConvertCoin(int score)
    {
        return score / 100;
    }
}

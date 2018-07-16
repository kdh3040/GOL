using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonFunc
{
    static public string ConvertNumber(int count)
    {
        return string.Format("{0:n0}", count);
    }

    static public void SetImageFile(string fileName, ref Image img)
    {
        var imgSprite = (Sprite)Resources.Load(fileName, typeof(Sprite));
        if (imgSprite == null)
            return;

        RectTransform rt = img.GetComponent<RectTransform>();
        rt.sizeDelta = imgSprite.rect.size;
        img.sprite = imgSprite;
    }

    static public bool IsEnoughCoin(int coin, bool showPopup = false)
    {
        var myCoin = PlayerData.Instance.Coin;

        if (myCoin < coin)
        {
            if(showPopup)
            {
                PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("GAME_PLAY_LACK_COIN")));
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
            PlayerData.Instance.SubCoin(coin);
            return true;
        }

        return false;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkinSlot : MonoBehaviour
{
    public Button SlotButton;
    public UICountImgFont Level;
    public Image SkinIcon;
    public Image SelectImg;

    [System.NonSerialized]
    public CommonData.SKIN_TYPE SkinType;
    [System.NonSerialized]
    public bool Select;

    public void SetSkinSlot(CommonData.SKIN_TYPE type)
    {
        SkinType = type;
        RefreshUI();
        SetSelect(false);
    }
    public void SetSelect(bool enable)
    {
        Select = enable;
        SelectImg.gameObject.SetActive(enable);
    }

    public void RefreshUI()
    {
        var level = PlayerData.Instance.GetSkinSlotLevel(SkinType);
        Level.SetValue(string.Format("+{0}", level), UICountImgFont.IMG_RANGE.RIGHT, UICountImgFont.IMG_TYPE.GREEN);
        SkinData skinData = PlayerData.Instance.GetUseSkinData(SkinType);
        CommonFunc.SetImageFile(skinData.GetIcon(), ref SkinIcon, false);
    }
}
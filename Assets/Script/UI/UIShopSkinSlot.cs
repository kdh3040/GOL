﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopSkinSlot : MonoBehaviour
{
    public Button SlotButton;
    public Image Background;
    public GameObject SelectImg;
    public Image Icon;
    public UIPointValue Cost;
    public Text Desc;

    [System.NonSerialized]
    public CommonData.SKIN_TYPE SkinType;
    [System.NonSerialized]
    public int SkinId;

    public void SetData(CommonData.SKIN_TYPE type, int id)
    {
        SkinType = type;
        SkinId = id;
        SetSelect(false);
        RefreshUI();
    }

    public void SetSelect(bool enable)
    {
        SelectImg.gameObject.SetActive(enable);
    }

    private void SetEquip(bool enable)
    {
        if (enable)
            CommonFunc.SetImageFile("Renewal/UI/slot_bg_4", ref Background, false);
        else
            CommonFunc.SetImageFile("Renewal/UI/slot_bg_1", ref Background, false);
    }

    public void RefreshUI()
    {
        var data = DataManager.Instance.GetSkinData(SkinType, SkinId);
        CommonFunc.SetImageFile(data.GetIcon(), ref Icon);
        Cost.gameObject.SetActive(false);
        Desc.gameObject.SetActive(false);
        SetEquip(false);

        if (PlayerData.Instance.HasSkin(SkinType, SkinId))
        {
            Desc.gameObject.SetActive(true);
            if (PlayerData.Instance.GetUseSkin(SkinType) == SkinId)
            {
                SetEquip(true);
                Desc.text = LocalizeData.Instance.GetLocalizeString("SKIN_SLOT_EQUIP");
            }
            else
                Desc.text = LocalizeData.Instance.GetLocalizeString("SKIN_SLOT_HAS");
        }
        else
        {
            Cost.gameObject.SetActive(true);
            Cost.SetValue(data.cost);
        }
    }
}

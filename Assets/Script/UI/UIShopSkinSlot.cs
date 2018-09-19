using System.Collections;
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
    public Image Lock;

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
        CommonFunc.SetImageFile(data.GetIcon(), ref Icon , false);
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


        if (SkinType == CommonData.SKIN_TYPE.DOOR)
        {
            var doorData = DataManager.Instance.GetSkinData(SkinType, SkinId) as DoorData;
            if (PlayerData.Instance.HasSkinName(CommonData.SKIN_TYPE.BACKGROUND, doorData.buy_bg) == false)
                Lock.gameObject.SetActive(true);
            else
                Lock.gameObject.SetActive(false);
        }
        else if(SkinType == CommonData.SKIN_TYPE.BACKGROUND)
        {
            var bgData = DataManager.Instance.GetSkinData(SkinType, SkinId) as BackgroundData;
            if (PlayerData.Instance.HasSkinName(CommonData.SKIN_TYPE.BACKGROUND, bgData.buy_bg) == false)
                Lock.gameObject.SetActive(true);
            else
                Lock.gameObject.SetActive(false);
        }
        else
            Lock.gameObject.SetActive(false);
    }
}

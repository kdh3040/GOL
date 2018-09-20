using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkinSlot : MonoBehaviour
{
    public Button SlotButton;
    public GameObject LevelBg;
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
        if (level == 1)
        {
            LevelBg.gameObject.SetActive(false);
            Level.gameObject.SetActive(false);
        }
        else
        {
            LevelBg.gameObject.SetActive(true);
            Level.gameObject.SetActive(true);
            Level.SetValue(string.Format("+{0}", level - 1), UICountImgFont.IMG_RANGE.CENTER, UICountImgFont.IMG_TYPE.GREEN);
        }
        SkinData skinData = PlayerData.Instance.GetUseSkinData(SkinType);
        CommonFunc.SetImageFile(skinData.GetIcon(), ref SkinIcon, false);
    }
}

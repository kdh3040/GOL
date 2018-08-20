using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkinSlot : MonoBehaviour
{
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
        var level = PlayerData.Instance.GetSkinSlotLevel(type);
        Level.SetValue(string.Format("+{0}", level), UICountImgFont.IMG_RANGE.RIGHT, UICountImgFont.IMG_TYPE.GREEN);
        SkinData skinData = PlayerData.Instance.GetUseSkinData(type);
        CommonFunc.SetImageFile(skinData.GetIcon(), ref SelectImg);
        SetSelect(false);
    }
    public void SetSelect(bool enable)
    {
        Select = enable;
        SelectImg.gameObject.SetActive(enable);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBookSkinSlot : MonoBehaviour {
    public Button SlotButton;
    public Image Icon;
    public Image LockIcon;
    public GameObject SelectImg;

    [System.NonSerialized]
    public int SkinId;
    [System.NonSerialized]
    CommonData.SKIN_TYPE SkinType = CommonData.SKIN_TYPE.NONE;

    public void SetSkinSlot(int id, CommonData.SKIN_TYPE type)
    {
        SkinId = id;
        SkinType = type;

        var data = DataManager.Instance.GetSkinData(SkinType, SkinId);
        CommonFunc.SetImageFile(data.GetIcon(), ref Icon, false);
        RefreshUI();
    }

    public void SetSelect(bool enable)
    {
        SelectImg.SetActive(enable);
    }

    public void RefreshUI()
    {
        if (PlayerData.Instance.HasSkin(SkinType, SkinId))
            LockIcon.gameObject.SetActive(false);
        else
            LockIcon.gameObject.SetActive(true);
    }
    
}

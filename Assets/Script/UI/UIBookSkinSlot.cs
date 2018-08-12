using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBookSkinSlot : MonoBehaviour
{
    public Button SlotButton;
    public GameObject SelectImg;
    public Image Icon;
    public GameObject NotHaveIcon;

    [System.NonSerialized]
    public CommonData.SKIN_TYPE mSkinType;

    [System.NonSerialized]
    public SkinData mSkinData;

    public void SetData(CommonData.SKIN_TYPE type, int id)
    {
        mSkinType = type;
        mSkinData = null;

        if (id != 0)
        {
            switch (mSkinType)
            {
                case CommonData.SKIN_TYPE.CHAR:
                    mSkinData = DataManager.Instance.CharDataDic[id];
                    break;
                case CommonData.SKIN_TYPE.DOOR:
                    mSkinData = DataManager.Instance.DoorDataDic[id];
                    break;
                case CommonData.SKIN_TYPE.ENDING:
                    mSkinData = DataManager.Instance.EndingDataList[id];
                    break;
                case CommonData.SKIN_TYPE.BACKGROUND:
                    mSkinData = DataManager.Instance.BackGroundDataDic[id];
                    break;
                default:
                    break;
            } 
        }

        Initialize();
    }

    public void Initialize()
    {
        CommonFunc.SetImageFile(mSkinData.GetIcon(), ref Icon);
        SetSelect(false);
        RefreshUI();
    }

    public void RefreshUI()
    {
        NotHaveIcon.SetActive(!IsSkinHave());
    }

    private bool IsSkinHave()
    {
        return PlayerData.Instance.HasSkin(mSkinType, mSkinData.id);
    }

    public void SetSelect(bool enable)
    {
        SelectImg.SetActive(enable);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopSkinSlot : MonoBehaviour
{
    public Button SlotButton;
    public GameObject SelectImg;
    public Image Icon;
    public UICountImgFont Cost;
    public GameObject UseIcon;
    public GameObject HaveIcon;
    public GameObject QuestionMark;

    [System.NonSerialized]
    public PopupGameShop.TAB_TYPE mSkinType;

    [System.NonSerialized]
    public SkinData mSkinData;

    public void SetData(PopupGameShop.TAB_TYPE type, int id)
    {
        mSkinType = type;
        mSkinData = null;

        if(id != 0)
        {
            switch (mSkinType)
            {
                case PopupGameShop.TAB_TYPE.CHAR:
                    mSkinData = DataManager.Instance.CharDataDic[id];
                    break;
                case PopupGameShop.TAB_TYPE.DOOR:
                    mSkinData = DataManager.Instance.DoorDataDic[id];
                    break;
                case PopupGameShop.TAB_TYPE.BG:
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
        if (mSkinData == null)
        {
            Icon.gameObject.SetActive(false);
            SetSelect(false);
            RefreshUI();
        }
        else
        {
            Icon.gameObject.SetActive(true);
            Icon.sprite = (Sprite)Resources.Load(mSkinData.icon, typeof(Sprite));
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        UseIcon.SetActive(false);
        HaveIcon.SetActive(false);
        Cost.gameObject.SetActive(false);
        QuestionMark.gameObject.SetActive(false);

        if(mSkinData == null)
        {
            QuestionMark.gameObject.SetActive(true);
        }
        else if (IsSkinUse())
        {
            UseIcon.SetActive(true);
        }
        else if (IsSkinHave())
        {
            HaveIcon.SetActive(true);
        }
        else
        {
            Cost.gameObject.SetActive(true);
            Cost.SetValue(mSkinData.cost, UICountImgFont.IMG_RANGE.CENTER);
        }
    }

    private bool IsSkinUse()
    {
        switch (mSkinType)
        {
            case PopupGameShop.TAB_TYPE.CHAR:
                return mSkinData.id == GManager.Instance.mPlayerData.UseCharId;
            case PopupGameShop.TAB_TYPE.DOOR:
                return mSkinData.id == GManager.Instance.mPlayerData.UseDoorId;
            case PopupGameShop.TAB_TYPE.BG:
                return mSkinData.id == GManager.Instance.mPlayerData.UseBGId;
            default:
                break;
        }

        return false;
    }

    private bool IsSkinHave()
    {
        switch (mSkinType)
        {
            case PopupGameShop.TAB_TYPE.CHAR:
                return GManager.Instance.mPlayerData.IsHasChar(mSkinData.id);
            case PopupGameShop.TAB_TYPE.DOOR:
                return GManager.Instance.mPlayerData.IsHasDoor(mSkinData.id);
            case PopupGameShop.TAB_TYPE.BG:
                return GManager.Instance.mPlayerData.IsHasBG(mSkinData.id);
                break;
            default:
                break;
        }

        return false;
    }

    public void SetSelect(bool enable)
    {
        SelectImg.SetActive(enable);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopSkinSlot : MonoBehaviour
{
    public Button SlotButton;
    public GameObject SelectImg;
    public Image Icon;
    public GameObject CostObj;
    public Text CostValue;
    public GameObject UseObj;

    [System.NonSerialized]
    public CommonData.SKIN_TYPE mSkinType;

    [System.NonSerialized]
    public SkinData mSkinData;

    public void SetData(CommonData.SKIN_TYPE type, int id)
    {
        mSkinType = type;
        mSkinData = null;

        if(id != 0)
        {
            switch (mSkinType)
            {
                case CommonData.SKIN_TYPE.CHAR:
                    mSkinData = DataManager.Instance.CharDataDic[id];
                    break;
                case CommonData.SKIN_TYPE.DOOR:
                    mSkinData = DataManager.Instance.DoorDataDic[id];
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
        if (mSkinData == null)
        {
            Icon.gameObject.SetActive(false);
            SetSelect(false);
            RefreshUI();
        }
        else
        {
            Icon.gameObject.SetActive(true);
            Icon.sprite = (Sprite)Resources.Load(mSkinData.GetIcon(), typeof(Sprite));
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        CostObj.SetActive(false);
        UseObj.SetActive(false);

        if (IsSkinUse())
        {
            UseObj.SetActive(true);
        }
        else if(IsSkinHave() == false)
        {
            CostObj.SetActive(true);
            CostValue.text = CommonFunc.ConvertNumber(mSkinData.cost);
        }
    }

    private bool IsSkinUse()
    {
        switch (mSkinType)
        {
            case CommonData.SKIN_TYPE.CHAR:
                return mSkinData.id == PlayerData.Instance.UseCharId;
            case CommonData.SKIN_TYPE.DOOR:
                return mSkinData.id == PlayerData.Instance.UseDoorId;
            case CommonData.SKIN_TYPE.BACKGROUND:
                return mSkinData.id == PlayerData.Instance.UseBGId;
            default:
                break;
        }

        return false;
    }

    private bool IsSkinHave()
    {
        switch (mSkinType)
        {
            case CommonData.SKIN_TYPE.CHAR:
                return PlayerData.Instance.IsHasChar(mSkinData.id);
            case CommonData.SKIN_TYPE.DOOR:
                return PlayerData.Instance.IsHasDoor(mSkinData.id);
            case CommonData.SKIN_TYPE.BACKGROUND:
                return PlayerData.Instance.IsHasBG(mSkinData.id);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopCharSlot : MonoBehaviour
{
    public Button SlotButton;
    public GameObject SelectImg;
    public Image Icon;
    public UICountImgFont Cost;
    public GameObject UseIcon;
    public GameObject HaveIcon;
    public GameObject QuestionMark;

    [System.NonSerialized]
    public CharData mCharData;

    public void SetChar(int charId)
    {
        if(charId == 0)
        {
            mCharData = null;
            Icon.gameObject.SetActive(false);
            SetSelect(false);
            RefreshUI();
        }
        else
        {
            mCharData = DataManager.Instance.CharDataDic[charId];
            Icon.gameObject.SetActive(true);
            Icon.sprite = (Sprite)Resources.Load(mCharData.icon, typeof(Sprite));
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        UseIcon.SetActive(false);
        HaveIcon.SetActive(false);
        Cost.gameObject.SetActive(false);
        QuestionMark.gameObject.SetActive(false);

        if(mCharData == null)
        {
            QuestionMark.gameObject.SetActive(true);
        }
        else if (mCharData.id == GManager.Instance.mPlayerData.UseCharId)
        {
            UseIcon.SetActive(true);
        }
        else if (GManager.Instance.mPlayerData.IsHasChar(mCharData.id))
        {
            HaveIcon.SetActive(true);
        }
        else
        {
            Cost.gameObject.SetActive(true);
            Cost.SetValue(mCharData.cost, UICountImgFont.IMG_RANGE.CENTER);
        }
    }

    public void SetSelect(bool enable)
    {
        SelectImg.SetActive(enable);
    }
}

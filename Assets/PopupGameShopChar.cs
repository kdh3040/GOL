using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupGameShopChar : MonoBehaviour {

    public Image SelectCharThumbnail;
    public Text SelectCharDesc;
    public GridLayoutGroup CharGrid;

    public Button BuyButton;
    public Button EquipButton;

    private int mSelectId = 0;
    private List<UIShopCharSlot> mCharSlotList = new List<UIShopCharSlot>();

    private int mDefaultSlotViewCount = 8;

    void Awake()
    {
        BuyButton.onClick.AddListener(OnClickBuy);
        EquipButton.onClick.AddListener(OnClickEquip);
    }

    public void ShowUI()
    {
        if (mCharSlotList.Count <= 0)
        {
            var indexList = new List<int>();
            var charDataDicEnumerator = DataManager.Instance.CharDataDic.GetEnumerator();

            while (charDataDicEnumerator.MoveNext())
            {
                indexList.Add(charDataDicEnumerator.Current.Key);
            }

            indexList.Sort(delegate (int A, int B)
            {
                if (A < B)
                    return 1;
                else
                    return -1;
            });

            for (int i = 0; i < mDefaultSlotViewCount; i++)
            {
                var obj = Instantiate(Resources.Load("Prefab/UIShopCharSlot"), CharGrid.gameObject.transform) as GameObject;
                var slot = obj.GetComponent<UIShopCharSlot>();
                if (i < indexList.Count)
                {
                    slot.SetChar(indexList[i]);
                    slot.SlotButton.onClick.AddListener(() => { OnClickSlot(slot.mCharData.id); });
                    mCharSlotList.Add(slot);
                }
                else
                {
                    slot.SetChar(0);
                }
            }
        }

        OnClickSlot(mCharSlotList[0].mCharData.id);
    }

    public void RefreshUI()
    {
        for (int i = 0; i < mCharSlotList.Count; i++)
        {
            mCharSlotList[i].RefreshUI();
        }
    }
    
    public void OnClickSlot(int charId)
    {
        for (int i = 0; i < mCharSlotList.Count; i++)
        {
            if (mCharSlotList[i].mCharData.id == charId)
                mCharSlotList[i].SetSelect(true);
            else
                mCharSlotList[i].SetSelect(false);
        }

        mSelectId = charId;
        var charData = DataManager.Instance.CharDataDic[mSelectId];
        SelectCharThumbnail.sprite = (Sprite)Resources.Load(charData.icon, typeof(Sprite));
        SelectCharDesc.text = charData.desc;
    }
   
    public void OnClickBuy()
    {
        var charData = DataManager.Instance.CharDataDic[mSelectId];
        UnityAction yesAction = () =>
        {
            PopupManager.Instance.DismissPopup();
            if (CommonFunc.UseCoin(charData.cost))
            {
                GManager.Instance.mPlayerData.AddChar(mSelectId);
                RefreshUI();
            }
        };
        UnityAction noAction = () =>
        {
            PopupManager.Instance.DismissPopup();
        };
        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("BUY_CHAR_TITLE"), yesAction, noAction);
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);

    }
    public void OnClickEquip()
    {
        GManager.Instance.mPlayerData.SetUseCharId(mSelectId);
        RefreshUI();
    }
}

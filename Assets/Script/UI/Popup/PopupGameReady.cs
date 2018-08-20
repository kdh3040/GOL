using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameReady : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_READY;
    }

    public Image DescIcon;
    public Text Desc;
    public Button BuyButton;
    public Button UpgradeButton;

    public List<UISkinSlot> SkinSlotList = new List<UISkinSlot>();
    public List<UIItemSlot> ItemSlotList = new List<UIItemSlot>();

    public Button StartButton;

    public override void ShowPopup(PopupUIData data)
    {
        SkinSlotList[0].SetSkinSlot(CommonData.SKIN_TYPE.CHAR);
        SkinSlotList[1].SetSkinSlot(CommonData.SKIN_TYPE.DOOR);
        SkinSlotList[2].SetSkinSlot(CommonData.SKIN_TYPE.BACKGROUND);

        //for (int i = 0; i < DataManager.Instance.ItemDataDic.; i++)
        //{

        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMsg : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.MSG_POPUP;
    }

    public class PopupData : PopupUIData
    {
        public string Msg;
        public PopupData(string msg)
        {
            Msg = msg;
        }
    }

    public Text Msg;
    public Button OkButton;

    void Awake()
    {
        OkButton.onClick.AddListener(OnClickOk);
    }

    public override void ShowPopup(PopupUIData data)
    {
        var popupData = data as PopupData;
        Msg.text = popupData.Msg;
    }

    private void OnClickOk()
    {
        PopupManager.Instance.DismissPopup();
    }
}

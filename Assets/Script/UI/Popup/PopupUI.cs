using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupUI : MonoBehaviour {

    public Button BackgroundButton;
    public abstract PopupManager.POPUP_TYPE GetPopupType();
    public virtual void ShowPopup(PopupUIData data) { }
    public virtual void DismissPopup() { }
    void Awake()
    {
        if(BackgroundButton != null)
            BackgroundButton.onClick.AddListener(OnClickBackground);
    }
    void Start()
    {
        PopupManager.Instance.AddPopup(this);
    }

    public void OnClickBackground()
    {
        PopupManager.Instance.DismissPopup();
    }
}

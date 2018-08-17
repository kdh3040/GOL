using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupUI : MonoBehaviour {
    public abstract PopupManager.POPUP_TYPE GetPopupType();
    public virtual void ShowPopup(PopupUIData data) { }
    public virtual void DismissPopup() { }
    public virtual void RefreshPopup() { }
    void Start()
    {
        PopupManager.Instance.AddPopup(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupUI : MonoBehaviour {

    public abstract PopupManager.POPUP_TYPE GetPopupType();
    public virtual void ShowPopup() { }
    public virtual void DismissPopup() { }

    void Start()
    {
        PopupManager.Instance.AddPopup(this);
        //DontDestroyOnLoad(this);
    }
}

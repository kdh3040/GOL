using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupUI : MonoBehaviour {

    public abstract PopupManager.POPUP_TYPE GetPageType();

    void Start()
    {
        PopupManager.Instance.AddPopup(this);
        DontDestroyOnLoad(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour {

    public static PopupManager _instance = null;
    public static PopupManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PopupManager>() as PopupManager;
            }
            return _instance;
        }
    }

    public enum POPUP_TYPE
    {
        GAME_END,
    }

    private Dictionary<POPUP_TYPE, PopupUI> PopupUIList = new Dictionary<POPUP_TYPE, PopupUI>();

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void AddPopup(PopupUI page)
    {
        PopupUIList.Add(page.GetPageType(), page);
        page.gameObject.SetActive(false);
    }
}

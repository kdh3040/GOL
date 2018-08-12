using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGameShop : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_SHOP;
    }

    public class PopupData : PopupUIData
    {
        public bool GameStartReady;
        public PopupData(bool ready)
        {
            GameStartReady = ready;
        }
    }

    public enum TAB_TYPE
    {
        NONE,
        ITEM,
        CHAR,
        DOOR,
        BG
    }

    public UITopBar TopBar;
    public PopupGameShopItem ItemUI;
    public PopupGameShopSkin SkinUI;
    public UITabButton ItamTab;
    public UITabButton CharTab;
    public UITabButton DoorTab;
    public UITabButton BGTab;
    public TAB_TYPE TabType = TAB_TYPE.NONE;

    public Button GameStart;

    void Awake()
    {
        GameStart.onClick.AddListener(OnClickGameStart);
        ItamTab.TabButton.onClick.AddListener(() => { OnClickTab(TAB_TYPE.ITEM); });
        CharTab.TabButton.onClick.AddListener(() => { OnClickTab(TAB_TYPE.CHAR); });
        DoorTab.TabButton.onClick.AddListener(() => { OnClickTab(TAB_TYPE.DOOR); });
        BGTab.TabButton.onClick.AddListener(() => { OnClickTab(TAB_TYPE.BG); });
    }

    public override void ShowPopup(PopupUIData data)
    {
        var popupData = data as PopupData;


        ItamTab.SetTabTitle("POPUP_SHOP_TAB_ITEM");
        CharTab.SetTabTitle("POPUP_SHOP_TAB_CHAR");
        DoorTab.SetTabTitle("POPUP_SHOP_TAB_DOOR");
        BGTab.SetTabTitle("POPUP_SHOP_TAB_BACKGROUND");

        GameStart.gameObject.SetActive(popupData.GameStartReady);
        TopBar.Initialize(true);
        OnClickTab(TAB_TYPE.ITEM);
    }

    public void OnClickTab(TAB_TYPE type)
    {
        if (TabType == type)
            return;

        TabType = type;

        ItamTab.SetSelect(TabType == TAB_TYPE.ITEM);
        CharTab.SetSelect(TabType == TAB_TYPE.CHAR);
        DoorTab.SetSelect(TabType == TAB_TYPE.DOOR);
        BGTab.SetSelect(TabType == TAB_TYPE.BG);

        ItemUI.gameObject.SetActive(false);
        SkinUI.gameObject.SetActive(false);
        switch (TabType)
        {
            case TAB_TYPE.ITEM:
                ItemUI.gameObject.SetActive(true);
                ItemUI.ShowUI();
                break;
            case TAB_TYPE.CHAR:
            case TAB_TYPE.DOOR:
            case TAB_TYPE.BG:
                SkinUI.gameObject.SetActive(true);
                SkinUI.ShowUI(TabType);
                break;
        }
    }
    
    public void OnClickGameStart()
    {
        PlayerData.Instance.MinusDDong();
        PopupManager.Instance.DismissPopup();
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PageMainUI : MonoBehaviour
{
    public UITopBar TopBar;

    public Button GamePlay;
    public Button GameShop;
    public Button GameBook;
    public Button GameRank;
    public Button GameSetting;

    void Awake()
    {
        GamePlay.onClick.AddListener(OnClickGamePlay);
        GameShop.onClick.AddListener(OnClickGameShop);
        GameBook.onClick.AddListener(OnClickGameBook);
        GameRank.onClick.AddListener(OnClickGameRank);
        GameSetting.onClick.AddListener(OnClickGameSetting);
    }

    void Start()
    {
        TopBar.Initialize(false);
    }

    public void OnClickGamePlay()
    {
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_READY);
    }

    public void OnClickGameShop()
    {
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_SHOP, new PopupGameShop.PopupData(false));
    }

    public void OnClickGameBook()
    {
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_BOOK);
    }
    public void OnClickGameRank()
    {
        
    }
    public void OnClickGameSetting()
    {
        
    }

}

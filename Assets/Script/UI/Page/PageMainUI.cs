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
    public Button GameRank;
    public Button GameSetting;
    public Button GameBook;

    void Awake()
    {
        GamePlay.onClick.AddListener(OnClickGamePlay);
        GameShop.onClick.AddListener(OnClickGameShop);
        GameRank.onClick.AddListener(OnClickGameRank);
        GameSetting.onClick.AddListener(OnClickGameSetting);
        GameBook.onClick.AddListener(OnClickGameBook);
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
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_SHOP);
    }
    public void OnClickGameRank()
    {
        GameCenterManager.Instance.ShowLeaderboardUI();
    }
    public void OnClickGameSetting()
    {
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_SETTING);
    }
    public void OnClickGameBook()
    {
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_BOOK);
    }
}

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
        if (GManager.Instance.mPlayerData.Ddong <= 0)
        {
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("GAME_PLAY_LACK_DDONG")));
            return;
        }
        else
        {
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_SHOP, new PopupGameShop.PopupData(true));
        }
    }

    public void OnClickGameShop()
    {
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_SHOP, new PopupGameShop.PopupData(false));
    }

    public void OnClickGameBook()
    {
        
    }
    public void OnClickGameRank()
    {
        
    }
    public void OnClickGameSetting()
    {
        
    }

}

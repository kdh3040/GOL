using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    public Button VideoLInk_1;
    public Button VideoLInk_2;

    public Image Img_BackGround;
    public Animator Anim_Char;

    public AudioSource mBGM;
    public AudioClip mClip;
    
    void Awake()
    {
        GamePlay.onClick.AddListener(OnClickGamePlay);
        GameShop.onClick.AddListener(OnClickGameShop);
        GameRank.onClick.AddListener(OnClickGameRank);
        GameSetting.onClick.AddListener(OnClickGameSetting);
        GameBook.onClick.AddListener(OnClickGameBook);

        VideoLInk_1.onClick.AddListener(OnClickVideo_1);
        VideoLInk_2.onClick.AddListener(OnClickVideo_2);
    }

    void Start()
    {
        PopupManager.Instance.AllDismissPopup();
        TopBar.Initialize(false);
        PlayBGM();
        Refresh(); 
    }

    public void OnClickGamePlay()
    {
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_READY, new PopupGameReady.PopupData(Refresh));
    }

    public void OnClickGameShop()
    {
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_SHOP, new PopupGameShop.PopupData(Refresh));
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
    public void OnClickVideo_1()
    {
        AudioListener.pause = true;
        GManager.Instance.SetVideoStatus(true);

        string strUrl = "https://www.youtube.com/watch?v=4mLGWjx6CS0";

        GManager.Instance.webViewObject =
            (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        GManager.Instance.webViewObject.Init((msg) => {
            Debug.Log(string.Format("CallFromJS[{0}]", msg));
        });

        GManager.Instance.webViewObject.LoadURL(strUrl);
        GManager.Instance.webViewObject.SetVisibility(true);
        GManager.Instance.webViewObject.SetMargins(0, 0, 0, 0);
    }

    public void OnClickVideo_2()
    {
        AudioListener.pause = true;
        GManager.Instance.SetVideoStatus(true);

        string strUrl = "https://www.youtube.com/watch?v=c1TwgBJCMR8";

        GManager.Instance.webViewObject =
            (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        GManager.Instance.webViewObject.Init((msg) => {
            Debug.Log(string.Format("CallFromJS[{0}]", msg));
        });

        GManager.Instance.webViewObject.LoadURL(strUrl);
        GManager.Instance.webViewObject.SetVisibility(true);
        GManager.Instance.webViewObject.SetMargins(0, 0, 0, 0);
    }

    private void Refresh()
    {
        SetBackGroundImg();
        SetCharAnim();       
    }


    private void SetBackGroundImg()
    {
        SkinData mSkin = PlayerData.Instance.GetUseSkinData(CommonData.SKIN_TYPE.BACKGROUND);
        var backgoundData = mSkin as BackgroundData;
        Img_BackGround.sprite = (Sprite)Resources.Load(backgoundData.img_main, typeof(Sprite));        
    }

    private void SetCharAnim()
    {
        SkinData mSkin =  PlayerData.Instance.GetUseSkinData(CommonData.SKIN_TYPE.CHAR);
        var charData = mSkin as CharData;
        Anim_Char.Rebind();
        Anim_Char.SetTrigger(charData.shopani_trigger);
    }
    
    public void PlayBGM()
    {
            mBGM.Play();
    }
}

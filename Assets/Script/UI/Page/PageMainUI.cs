﻿using System.Collections;
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

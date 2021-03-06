﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGamePause : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_PAUSE;
    }

    public Button RestartButton;
    public Button ResumeButton;
    public Button ExitButton;

    public void Awake()
    {
        RestartButton.onClick.AddListener(OnClickRestart);
        ResumeButton.onClick.AddListener(OnClickResume);
        ExitButton.onClick.AddListener(OnClickExit);
    }

    public void OnClickRestart()
    {
        if (PlayerData.Instance.IsPlayEnable())
        {
            GamePlayManager.Instance.GameStart(GamePlayManager.START_TYPE.RESTART);
            PopupManager.Instance.DismissPopup();
        }
    }

    public void OnClickResume()
    {
        GamePlayManager.Instance.GameResumeCountStart();
        PopupManager.Instance.DismissPopup();
    }

    public void OnClickExit()
    {
        GamePlayManager.Instance.GameExit();
        PopupManager.Instance.AllDismissPopup();
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
    
}

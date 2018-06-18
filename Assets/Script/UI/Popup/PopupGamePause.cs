using System.Collections;
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
    public Button ExitButton;
    public Button CancelButton;

    void Awake()
    {
        RestartButton.onClick.AddListener(OnClickRestart);
        ExitButton.onClick.AddListener(OnClickExit);
        CancelButton.onClick.AddListener(OnClickCancel);
    }

    public void OnClickRestart()
    {
        GamePlayManager.Instance.GameStart();
        PopupManager.Instance.DismissPopup();
    }

    public void OnClickExit()
    {
        GamePlayManager.Instance.GameExit();
        PopupManager.Instance.DismissPopup();
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void OnClickCancel()
    {
        PopupManager.Instance.DismissPopup();
        GamePlayManager.Instance.GamePause = false;
    }
}
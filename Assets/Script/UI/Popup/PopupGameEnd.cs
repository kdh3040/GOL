using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGameEnd : PopupUI {

    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_END;
    }

    public Button GameExitButton;
    public Button GameRestartButton;
    public Button GameResurrectionButton;

    void Awake()
    {
        GameExitButton.onClick.AddListener(OnClickGameExit);
        GameRestartButton.onClick.AddListener(OnClickGameRestart);
        GameResurrectionButton.onClick.AddListener(OnClickGameResurrection);
    }

    void OnClickGameExit()
    {
        GamePlayManager.Instance.GameExit();
        PopupManager.Instance.DismissPopup();
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
    void OnClickGameRestart()
    {
        GamePlayManager.Instance.GameRestart();
        PopupManager.Instance.DismissPopup();
    }
    void OnClickGameResurrection()
    {
        GamePlayManager.Instance.GameRestart();
        PopupManager.Instance.DismissPopup();
    }
}

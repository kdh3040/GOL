using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGameShop : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_SHOP;
    }

    public Button RestartButton;
    public Button ExitButton;

    public Button MyDongButton;
    public Button MyCoinButton;


    public Transform ItemPos_1;
    public Transform ItemPos_2;
    public Transform ItemPos_3;


    void Awake()
    {
        RestartButton.onClick.AddListener(OnClickRestart);
        ExitButton.onClick.AddListener(OnClickExit);

        MyDongButton.onClick.AddListener(OnClickMyDong);
        MyCoinButton.onClick.AddListener(OnClickMyCoin);

    }
    

    public void OnClickRestart()
    {
        GamePlayManager.Instance.GameRestart();
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
        GamePlayManager.Instance.GameContinue();
    }

    public void OnClickMyDong()
    {
        // 똥 사는 팝업
    }

    public void OnClickMyCoin()
    {
        // 코인 사는 팝업
    }
}
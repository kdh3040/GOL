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

    public class PopupData : PopupUIData
    {
        public int EndingNoteId = 0;
        public int ScoreValue = 0;
        public int CoinValue = 0;
        public PopupData(int endingNoteId, int score, int coin)
        {
            EndingNoteId = endingNoteId;
            ScoreValue = score;
            CoinValue = coin;
        }
    }

    public Image EndingScene;
    public Text Score;
    public Text Coin;
    public Button GameRestartButton;
    public Button GameRevivalButton;
    public Button GameExitButton;

    void Awake()
    {
        GameExitButton.onClick.AddListener(OnClickGameExit);
        GameRestartButton.onClick.AddListener(OnClickGameRestart);
        GameRevivalButton.onClick.AddListener(OnClickGameRevival);
    }

    public override void ShowPopup(PopupUIData data)
    {
        var popupData = data as PopupData;
        Score.text = CommonFunc.ConvertNumber(popupData.ScoreValue);
        Coin.text = CommonFunc.ConvertNumber(popupData.CoinValue);
        var noteData = DataManager.Instance.NoteDataDic[popupData.EndingNoteId];
        var endingData = DataManager.Instance.EndingDataList_NAME[noteData.endingName];
        CommonFunc.SetImageFile(endingData.img, ref EndingScene);
    }

    void OnClickGameExit()
    {
        GamePlayManager.Instance.GameExit();
        PopupManager.Instance.DismissPopup();
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
    void OnClickGameRestart()
    {
        if (PlayerData.Instance.IsPlayEnable(true))
        {
            GamePlayManager.Instance.GameRestart();
            PopupManager.Instance.DismissPopup();
        }
    }
    void OnClickGameRevival()
    {
        GamePlayManager.Instance.GameRestart();
        PopupManager.Instance.DismissPopup();
    }
}

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

    public UITopBar TopBar;
    public Image EndingScene;
    public Text EndingDesc;
    public Text Score;
    public Text Coin;
    public Button GameRestartButton;
    public Button GameRevivalButton;
    public UIPointValue GameRevivalCost;
    public Button GameExitButton;

    private int ScoreValue = 0;
    private int CoinValue = 0;
    private int PlusScoreValue = 0;
    private int PlusCoinValue = 0;
    private int EndingSceneId = 0;

    void Awake()
    {
        GameExitButton.onClick.AddListener(OnClickGameExit);
        GameRestartButton.onClick.AddListener(OnClickGameRestart);
        GameRevivalButton.onClick.AddListener(OnClickGameRevival);
    }

    public override void ShowPopup(PopupUIData data)
    {
        ScoreValue = GamePlayManager.Instance.Score;
        CoinValue = 0;
        PlusScoreValue = 0;
        PlusCoinValue = 0;
        TopBar.Initialize(false);
        GameRevivalCost.SetValue(ConfigData.Instance.REVIVAL_COST);

        // 스킬로 추가 획득
        if (SkillManager.Instance.IsSkillEnable(SkillManager.SKILL_TYPE.GAME_OVER_SCORE_BONUS))
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.GAME_OVER_SCORE_BONUS) as GameSkill_GameOverScoreBonus;
            PlusScoreValue = skill.BonusScore(ScoreValue);
        }

        GameCenterManager.Instance.UnlockAchievement(PlusScoreValue + ScoreValue);
        GameCenterManager.Instance.ReportScore(PlusScoreValue + ScoreValue);

        CoinValue = CommonFunc.ConvertCoin(ScoreValue + PlusScoreValue);

        if (SkillManager.Instance.IsSkillEnable(SkillManager.SKILL_TYPE.GAME_OVER_COIN_BONUS))
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.GAME_OVER_COIN_BONUS) as GameSkill_GameOverCoinBonus;
            PlusCoinValue = skill.BonusCoin(CoinValue);
        }

        Score.text = CommonFunc.ConvertNumber(0);
        Coin.text = CommonFunc.ConvertNumber(0);

        var bgData = DataManager.Instance.BackGroundDataDic[PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.BACKGROUND)];
        var endingGroupList = bgData.endingGroupList;
        for (int index_1 = 0; index_1 < endingGroupList.Count; index_1++)
        {
            var endingList = DataManager.Instance.EndingGroupDataList[endingGroupList[index_1]].ending_list;
            for (int index_2 = 0; index_2 < endingList.Count; index_2++)
            {
                var endingData = DataManager.Instance.EndingDataList[endingList[index_2]];
                if (endingData.IsOpenEnding(CoinValue, ScoreValue) || PlayerData.Instance.HasEnding(endingData.id))
                    EndingSceneId = endingData.id;
            }
        }

        var endingViewData = DataManager.Instance.EndingDataList[EndingSceneId];
        CommonFunc.SetImageFile(endingViewData.img, ref EndingScene, false);
        EndingDesc.text = endingViewData.GetLocalizeDesc();

        StartCoroutine(Co_ScoreCoinEffect());
    }

    IEnumerator Co_ScoreCoinEffect()
    {
        yield return null;
        PlayerData.Instance.PlusCoin(CoinValue + PlusCoinValue);
        PlayerData.Instance.AddEnding(EndingSceneId);

        yield return new WaitForSeconds(0.3f);
        float saveTime = 0;
        while(saveTime < 1f)
        {
            yield return null;
            saveTime += Time.deltaTime;
            Score.text = CommonFunc.ConvertNumber((int)Mathf.Lerp(0, ScoreValue, saveTime / 1f));
        }

        Score.text = CommonFunc.ConvertNumber(ScoreValue);

        if(PlusScoreValue > 0)
        {
            saveTime = 0;

            while (saveTime < 0.5f)
            {
                yield return null;
                saveTime += Time.deltaTime;
                Score.text = string.Format("{0} +{1}", CommonFunc.ConvertNumber(ScoreValue), CommonFunc.ConvertNumber((int)Mathf.Lerp(0, PlusScoreValue, saveTime / 0.5f)));
            }

            Score.text = string.Format("{0} +{1}", CommonFunc.ConvertNumber(ScoreValue), CommonFunc.ConvertNumber(PlusScoreValue));
        }

        yield return null;

        saveTime = 0;
        while (saveTime < 1f)
        {
            yield return null;
            saveTime += Time.deltaTime;
            Coin.text = CommonFunc.ConvertNumber((int)Mathf.Lerp(0, CoinValue, saveTime / 1f));
        }

        Coin.text = CommonFunc.ConvertNumber(CoinValue);

        if (PlusCoinValue > 0)
        {
            saveTime = 0;

            while (saveTime < 0.5f)
            {
                yield return null;
                saveTime += Time.deltaTime;
                Coin.text = string.Format("{0} +{1}", CommonFunc.ConvertNumber(CoinValue), CommonFunc.ConvertNumber((int)Mathf.Lerp(0, PlusCoinValue, saveTime / 0.5f)));
            }

            Coin.text = string.Format("{0} +{1}", CommonFunc.ConvertNumber(CoinValue), CommonFunc.ConvertNumber(PlusCoinValue));
        }

        yield return null;
    }

    void OnClickGameExit()
    {
        GamePlayManager.Instance.GameExit();
        PopupManager.Instance.DismissPopup();
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
    void OnClickGameRestart()
    {
        if (PlayerData.Instance.IsPlayEnable())
        {
            GamePlayManager.Instance.GameRestart();
            PopupManager.Instance.DismissPopup();
        }
    }
    void OnClickGameRevival()
    {
        if(CommonFunc.UseCoin(ConfigData.Instance.REVIVAL_COST))
        {
            GamePlayManager.Instance.GameRevival();
            PopupManager.Instance.DismissPopup();
        }
    }
}

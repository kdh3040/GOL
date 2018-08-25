﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGameEnd : PopupUI {

    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_END;
    }

    public Image EndingScene;
    public Text Score;
    public Text Coin;
    public Button GameRestartButton;
    public Button GameRevivalButton;
    public Button GameExitButton;

    private int ScoreValue = 0;
    private int CoinValue = 0;
    private int PlusScoreValue = 0;
    private int PlusCoinValue = 0;

    void Awake()
    {
        GameExitButton.onClick.AddListener(OnClickGameExit);
        GameRestartButton.onClick.AddListener(OnClickGameRestart);
        GameRevivalButton.onClick.AddListener(OnClickGameRevival);
    }

    public override void ShowPopup(PopupUIData data)
    {
        ScoreValue = GamePlayManager.Instance.Score;
        CoinValue = GamePlayManager.Instance.ConvertScoreToCoin();
        PlusScoreValue = 0;
        PlusCoinValue = 0;

        // 스킬로 추가 획득
        if (SkillManager.Instance.IsSkillEnable(SkillManager.SKILL_TYPE.GAME_OVER_SCORE_BONUS))
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.GAME_OVER_SCORE_BONUS) as GameSkill_GameOverScoreBonus;
            PlusScoreValue = skill.ConvertScore(ScoreValue);
        }

        if (SkillManager.Instance.IsSkillEnable(SkillManager.SKILL_TYPE.GAME_OVER_COIN_BONUS))
        {
            var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.GAME_OVER_COIN_BONUS) as GameSkill_GameOverCoinBonus;
            PlusCoinValue = skill.ConvertCoin(CoinValue);
        }

        Score.text = CommonFunc.ConvertNumber(0);
        Coin.text = CommonFunc.ConvertNumber(0);

        var bgData = DataManager.Instance.BackGroundDataDic[PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.BACKGROUND)];
        var endingGroupList = bgData.endingGroupList;
        int endingId = 1;
        for (int index_1 = 0; index_1 < endingGroupList.Count; index_1++)
        {
            var endingList = DataManager.Instance.EndingGroupDataList[endingGroupList[index_1]].ending_list;
            for (int index_2 = 0; index_2 < endingList.Count; index_2++)
            {
                var endingData = DataManager.Instance.EndingDataList[endingList[index_2]];
                if (endingData.IsOpenEnding(CoinValue, ScoreValue))
                {
                    PlayerData.Instance.AddEnding(endingData.id);
                    endingId = endingData.id;
                }
            }
        }

        var endingViewData = DataManager.Instance.EndingDataList[endingId];
        CommonFunc.SetImageFile(endingViewData.img, ref EndingScene);

        StartCoroutine(Co_ScoreCoinEffect());
    }

    IEnumerator Co_ScoreCoinEffect()
    {
        yield return new WaitForSeconds(0.3f);
        float saveTime = 0;
        while(saveTime < 3f)
        {
            yield return null;
            saveTime += Time.deltaTime;
            Score.text = CommonFunc.ConvertNumber((int)Mathf.Lerp(0, ScoreValue, saveTime / 3f));
        }

        Score.text = CommonFunc.ConvertNumber(ScoreValue);

        yield return new WaitForSeconds(0.1f);
        if(PlusScoreValue > 0)
        {
            saveTime = 0;

            while (saveTime < 2f)
            {
                yield return null;
                saveTime += Time.deltaTime;
                Score.text = string.Format("{0} +{1}", CommonFunc.ConvertNumber(ScoreValue), CommonFunc.ConvertNumber((int)Mathf.Lerp(0, PlusScoreValue, saveTime / 2f)));
            }

            Score.text = string.Format("{0} +{1}", CommonFunc.ConvertNumber(ScoreValue), CommonFunc.ConvertNumber(PlusScoreValue));
        }

        yield return null;

        saveTime = 0;
        while (saveTime < 3f)
        {
            yield return null;
            saveTime += Time.deltaTime;
            Coin.text = CommonFunc.ConvertNumber((int)Mathf.Lerp(0, CoinValue, saveTime / 3f));
        }

        Coin.text = CommonFunc.ConvertNumber(CoinValue);

        yield return new WaitForSeconds(0.1f);
        if (PlusCoinValue > 0)
        {
            saveTime = 0;

            while (saveTime < 2f)
            {
                yield return null;
                saveTime += Time.deltaTime;
                Coin.text = string.Format("{0} +{1}", CommonFunc.ConvertNumber(CoinValue), CommonFunc.ConvertNumber((int)Mathf.Lerp(0, PlusCoinValue, saveTime / 2f)));
            }

            Coin.text = string.Format("{0} +{1}", CommonFunc.ConvertNumber(CoinValue), CommonFunc.ConvertNumber(PlusCoinValue));
        }

        yield return null;

        PlayerData.Instance.PlusCoin(CoinValue + PlusCoinValue);
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

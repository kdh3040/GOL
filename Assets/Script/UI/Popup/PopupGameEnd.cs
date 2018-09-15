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
        public int EndNoteId;
        public PopupData(int endNoteId)
        {
            EndNoteId = endNoteId;
        }
    }

    public UITopBar TopBar;
    public Image EndingScene;
    public Text EndingDesc;
    public Text Score;
    public Image NewRecord;
    public Text Coin;
    public Button GameRestartButton;
    public UIPointValue GameRestartCost;
    public Button GameRevivalButton;
    public Text GameRevivalText;
    public Button GameExitButton;

    private int ScoreValue = 0;
    private int CoinValue = 0;
    private int PlusScoreValue = 0;
    private int PlusCoinValue = 0;
    private int EndNoteId = 0;

    private bool EffectStart = false;

    void Awake()
    {
        GameExitButton.onClick.AddListener(OnClickGameExit);
        GameRestartButton.onClick.AddListener(OnClickGameRestart);
        GameRevivalButton.onClick.AddListener(OnClickGameRevival);
    }

    public override void ShowPopup(PopupUIData data)
    {
        var popupData = data as PopupData;
        EndNoteId = popupData.EndNoteId;
        ScoreValue = GamePlayManager.Instance.Score;
        CoinValue = 0;
        PlusScoreValue = 0;
        PlusCoinValue = 0;
        TopBar.Initialize(false);
        NewRecord.gameObject.SetActive(false);
        GameRestartCost.SetValue(1);
        GameRevivalText.text = LocalizeData.Instance.GetLocalizeString("GAME_END_POPUP_CONTINUE_COUNT", GamePlayManager.Instance.ContinueCount, CommonData.GAME_CONTINUE_MAX_COUNT);

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

        var noteData = DataManager.Instance.NoteDataDic[EndNoteId];
        CommonFunc.SetImageFile(noteData.img, ref EndingScene, false);
        EndingDesc.text = noteData.GetEndDesc();// + string.Format("\n플레이 시간 {0:f2}초", GamePlayManager.Instance.PlayTime);

        StartCoroutine(Co_ScoreCoinEffect());
    }

    IEnumerator Co_ScoreCoinEffect()
    {
        EffectStart = true;
        yield return null;
        PlayerData.Instance.PlusCoin(CoinValue + PlusCoinValue);

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

        EndResultScoreCoinEffect();
    }

    private bool EndResultScoreCoinEffect()
    {
        EffectStart = false;
        StopAllCoroutines();

        Score.text = CommonFunc.ConvertNumber(ScoreValue);

        if (PlusScoreValue > 0)
        {
            Score.text = string.Format("{0} +{1}", CommonFunc.ConvertNumber(ScoreValue), CommonFunc.ConvertNumber(PlusScoreValue));
        }

        NewRecord.gameObject.SetActive(PlayerData.Instance.BestScore < ScoreValue + PlusScoreValue);
        PlayerData.Instance.SetBestScore(ScoreValue + PlusScoreValue);

        Coin.text = CommonFunc.ConvertNumber(CoinValue);

        if (PlusCoinValue > 0)
        {
            Coin.text = string.Format("{0} +{1}", CommonFunc.ConvertNumber(CoinValue), CommonFunc.ConvertNumber(PlusCoinValue));
        }


        var EndingSceneGroupId = 0;
        var EndingSceneId = 0;
        var bgData = DataManager.Instance.BackGroundDataDic[PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.BACKGROUND)];
        var endingGroupList = bgData.endingGroupList;
        for (int index_1 = 0; index_1 < endingGroupList.Count; index_1++)
        {
            if (endingGroupList[index_1] == 0)
                continue;

            var endingList = DataManager.Instance.EndingGroupDataList[endingGroupList[index_1]].ending_list;
            EndingSceneGroupId = DataManager.Instance.EndingGroupDataList[endingGroupList[index_1]].id;
            for (int index_2 = 0; index_2 < endingList.Count; index_2++)
            {
                var endingData = DataManager.Instance.EndingDataList[endingList[index_2]];
                if (endingData.IsOpenEnding(CoinValue, ScoreValue) && PlayerData.Instance.HasEnding(endingData.id) == false)
                    EndingSceneId = endingData.id;
            }
        }

        if(EndingSceneId != 0)
        {
            PlayerData.Instance.AddEnding(EndingSceneId);
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_ENDING_SCENE, new PopupGameEndingScene.PopupData(EndingSceneGroupId, EndingSceneId, null, true));
            return true;
        }

        return false;
    }

    public void OnClickGameExit()
    {

        if (EffectStart)
        {
            bool endEnable = EndResultScoreCoinEffect();
            if(endEnable)
                return;
        }
        GamePlayManager.Instance.GameExit();
        PopupManager.Instance.AllDismissPopup();
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
    void OnClickGameRestart()
    {

        if (EffectStart)
        {
            bool endEnable = EndResultScoreCoinEffect();
            if (endEnable)
                return;
        }

        if (PlayerData.Instance.IsPlayEnable())
        {
            SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.GAME_PLAY);
            GamePlayManager.Instance.GameStart();
            PopupManager.Instance.DismissPopup();
        }
    }
    void OnClickGameRevival()
    {

        if (EffectStart)
        {
            bool endEnable = EndResultScoreCoinEffect();
            if (endEnable)
                return;
        }

        if (GamePlayManager.Instance.ContinueCount > 0)
        {
#if UNITY_EDITOR
            GamePlayManager.Instance.ContinueCount--;
            GamePlayManager.Instance.GameRevival();
            PopupManager.Instance.DismissPopup();
#elif UNITY_ANDROID
            AdManager.Instance.ShowRewardVideo();
#elif UNITY_IPHONE
            AdManager.Instance.ShowRewardVideo();
#endif
        }
        else
        {
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("GAME_END_POPUP_NOT_CONTINUE")));
        }
    }
}

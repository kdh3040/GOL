using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PageGameUI : MonoBehaviour
{
    public UICountImgFont Score;
    public Text Info;
    public List<Image> ShieldIconList = new List<Image>();
    public Transform SkillProgressStartPos;
    public Button GamePauseButton;
    public UICountImgFont GameResumeCount;
    

    public GameObject CharMsgObj;
    public Text CharMsg;
    private float CharMsgViewInterval;

    private List<UISkillProgressBar> mSkillProgressBarList = new List<UISkillProgressBar>();

    public GameObject DoorButtons;
    public List<Button> DoorButtonsList = new List<Button>();


    void Awake()
    {
        GamePauseButton.onClick.AddListener(OnClickPause);

        for (int i = 0; i < DoorButtonsList.Count; i++)
        {
            int index = i;
            DoorButtonsList[i].onClick.AddListener(() => { OnClickDoorButton(index); });
        }
    }

    public void ResetUI()
    {
        CharMsgViewInterval = ConfigData.Instance.CHAR_MSG_VIEW_INTERVAL;
        RefreshShieldItemUI();
        Score.SetValue("0", UICountImgFont.IMG_RANGE.CENTER, UICountImgFont.IMG_TYPE.YELLOW);
    }

    public void RefreshUI()
    {
        Score.SetValue(GamePlayManager.Instance.Score.ToString(), UICountImgFont.IMG_RANGE.CENTER, UICountImgFont.IMG_TYPE.YELLOW);
    }

    public void RefreshShieldItemUI()
    {
        var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_COUNT);
        for (int i = 0; i < ShieldIconList.Count; i++)
        {
            if (skill != null && skill.mCount >= i + 1)
            {
                ShieldIconList[i].gameObject.SetActive(true);
            }
            else
            {
                ShieldIconList[i].gameObject.SetActive(false);
            }
        }
    }

    public void RefreshItemSkillUI()
    {
        for (int i = mSkillProgressBarList.Count - 1; i >= 0 ; --i)
        {
            if(mSkillProgressBarList[i].GetUISkillData() == null)
            {
                DestroyImmediate(mSkillProgressBarList[i].gameObject);
                mSkillProgressBarList.RemoveAt(i);
                UpdateSkillProgressBar();
            }
            else
                mSkillProgressBarList[i].UpdateSkillProgress();
        }
    }

    public void UseItemSkill(int itemId, GameSkill skill)
    {
        for (int i = 0; i < mSkillProgressBarList.Count; i++)
        {
            if (mSkillProgressBarList[i].mItemId == itemId)
                return;
        }

        var obj = Instantiate(Resources.Load("Prefab/UISkillProgressBar"), SkillProgressStartPos) as GameObject;
        var progressBar = obj.GetComponent<UISkillProgressBar>();
        progressBar.SetItemSkill(itemId, skill.mSkillType);
        //progressBar.gameObject.transform.localPosition = new Vector3(0, mSkillProgressBarList.Count * 80);
        mSkillProgressBarList.Add(progressBar);
        UpdateSkillProgressBar();
    }

    public void UpdateSkillProgressBar()
    {
        for (int i = 0; i < mSkillProgressBarList.Count; i++)
        {
            mSkillProgressBarList[i].gameObject.transform.localPosition = new Vector3(0, i * 80);
        }
    }

    public void GameResume()
    {
        CharMsgViewInterval = ConfigData.Instance.CHAR_MSG_VIEW_INTERVAL;
        GameResumeCount.gameObject.SetActive(true);
        StartCoroutine(Co_GameResume());
    }

    public IEnumerator Co_GameResume()
    {
        GamePauseButton.gameObject.SetActive(false);
        int waitTime = 3;

        while(waitTime > 0)
        {
            GameResumeCount.SetValue(waitTime.ToString(), UICountImgFont.IMG_RANGE.CENTER, UICountImgFont.IMG_TYPE.YELLOW);
            yield return new WaitForSecondsRealtime(1f);
            waitTime--;
        }

        GamePauseButton.gameObject.SetActive(true);
        GameResumeCount.gameObject.SetActive(false);
        GamePlayManager.Instance.GameResume();
    }

    public void OnClickPause()
    {
        GamePlayManager.Instance.GamePause();
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_PAUSE);
    }

    public void GameOver()
    {
    }

    

    public void RefreshCharMsgUI(float time)
    {
        if (GamePlayManager.Instance.IsGamePause)
            return;

        CharMsgViewInterval -= time;
        if (CharMsgViewInterval < 0)
        {
            CharMsgViewInterval = ConfigData.Instance.CHAR_MSG_VIEW_INTERVAL;
            bool charMsgViewEnable = ConfigData.Instance.CHAR_MSG_VIEW_PERCENT > Random.Range(1, 100);
            if(charMsgViewEnable)
            {
                var randId = Random.Range(1, DataManager.Instance.CharMsgDataList.Count);
                ShowCharMsg(randId);
            }
        }
    }

    public void ShowCharMsg(int id)
    {
        CharMsgObj.SetActive(true);
        var data = DataManager.Instance.CharMsgDataList[id];
        CharMsg.text = LocalizeData.Instance.GetLocalizeString(data.Msg);
        StartCoroutine(Co_CharMsgDisable());
    }

    public IEnumerator Co_CharMsgDisable()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        CharMsgObj.gameObject.SetActive(false);
    }

    public void OnClickDoorButton(int index)
    {
        GamePlayManager.Instance.ClickDoorButton(index);
    }
}

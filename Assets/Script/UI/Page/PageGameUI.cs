using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PageGameUI : MonoBehaviour
{
    public UICountImgFont Score;
    public Text Info;
    public Button ItemButton;
    public Image ItemImg;
    public List<Image> ShieldIconList = new List<Image>();
    public Transform SkillProgressStartPos;
    public Button GamePauseButton;
    public UICountImgFont GameResumeCount;

    private List<UISkillProgressBar> mSkillProgressBarList = new List<UISkillProgressBar>();

    void Awake()
    {
        ItemButton.onClick.AddListener(OnClickItem);
        GamePauseButton.onClick.AddListener(OnClickPause);
    }

    public void ResetUI()
    {
        RefreshItemUI();
        RefreshShieldItemUI();
        Score.SetValue("0", UICountImgFont.IMG_RANGE.CENTER, UICountImgFont.IMG_TYPE.YELLOW);
    }

    public void RefreshUI()
    {
        Score.SetValue(GamePlayManager.Instance.Score.ToString(), UICountImgFont.IMG_RANGE.CENTER, UICountImgFont.IMG_TYPE.YELLOW);
    }

    public void RefreshItemUI()
    {
        int ItemId = GamePlayManager.Instance.UseItemId;
        if (ItemId != 0)
        {
            ItemImg.sprite = ItemManager.Instance.GetItemIcon(ItemId);
            ItemImg.color = new Color(1, 1, 1, 1);
        }
        else
        {
            ItemImg.color = new Color(1, 1, 1, 0);
            ItemImg.sprite = null;
        }
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
            }
            else
                mSkillProgressBarList[i].UpdateSkillProgress();
        }
    }

    public void OnClickItem()
    {
        GamePlayManager.Instance.UseGameNormalItem();
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
        progressBar.gameObject.transform.localPosition = new Vector3(0, mSkillProgressBarList.Count * 80);
        mSkillProgressBarList.Add(progressBar);
    }

    public void GameResume()
    {
        GameResumeCount.gameObject.SetActive(true);
        StartCoroutine(Co_GameResume());
    }

    public IEnumerator Co_GameResume()
    {
        int waitTime = 3;

        while(waitTime > 0)
        {
            GameResumeCount.SetValue(waitTime.ToString(), UICountImgFont.IMG_RANGE.CENTER, UICountImgFont.IMG_TYPE.YELLOW);
            yield return new WaitForSecondsRealtime(1f);
            waitTime--;
        }

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
        StartCoroutine(Co_GameOver());
    }

    public IEnumerator Co_GameOver()
    {
        yield return new WaitForSecondsRealtime(1f);

        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_END);
    }

    void Update()
    {
        StringBuilder text = new StringBuilder();
        var skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.SPEED_DOWN);
        if (skillData != null)
        {
            text.AppendFormat("노트속도 : {0:f2}", GamePlayManager.Instance.NoteSpeed * skillData.mPercent);
            text.AppendLine();
        }
        else
        {
            text.AppendFormat("노트속도 : {0:f2}", GamePlayManager.Instance.NoteSpeed);
            text.AppendLine();
        }

        // 스킬
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_TIME);
        if(skillData != null)
        {
            text.AppendFormat("무적스킬 남은시간 {0:f2}", skillData.mTime);
            text.AppendLine();
        }
        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_COUNT);
        if (skillData != null)
        { 
            text.AppendFormat("쉴드스킬 남은갯수 {0}", skillData.mCount);
            text.AppendLine();
        }
        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.SCORE_UP);
        if (skillData != null)
        { 
            text.AppendFormat("점수두배스킬 남은시간 {0:f2} {1:f2}배", skillData.mTime, skillData.mPercent);
            text.AppendLine();
        }

        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.RESURRECTION);
        if (skillData != null)
        {
            text.AppendFormat("부활스킬 발동중");
            text.AppendLine();
        }
        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.GAME_OVER_SCORE_BONUS);
        if (skillData != null)
        {
            text.AppendFormat("게임오버시 점수 상승 스킬 {0:f2}배", skillData.mPercent);
            text.AppendLine();
        }
        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.ITEM_CREATE);
        if (skillData != null)
        {
            text.AppendFormat("아이템 노트 생성 스킬 {0:f2}퍼센트", skillData.mPercent * 100);
            text.AppendLine();
        }
        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.GAME_OVER_COIN_BONUS);
        if (skillData != null)
        {
            text.AppendFormat("게임오버시 코인 상승 스킬 {0:f2}배", skillData.mPercent);
            text.AppendLine();
        }

        Info.text = text.ToString();
    }
}

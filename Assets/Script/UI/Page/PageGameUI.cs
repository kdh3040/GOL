using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PageGameUI : MonoBehaviour
{
    public UICountImgFont Score;
    public Text Info;
    public Button mItemRightButton;
    public Image mItemRightImg;
    public Button mItemLeftButton;
    public Image mItemLeftImg;
    public List<Image> mShieldIconList = new List<Image>();
    public Transform mSkillProgressStartPos;

    private List<UISkillProgressBar> mSkillProgressBarList = new List<UISkillProgressBar>();

    void Awake()
    {
        mItemRightButton.onClick.AddListener(OnClickItemRight);
        mItemLeftButton.onClick.AddListener(OnClickItemLeft);
    }

    public void ResetUI()
    {
        RefreshItemUI();
        RefreshShieldItemUI();
        Score.SetValue(0, UICountImgFont.IMG_RANGE.CENTER);
    }

    public void RefreshUI()
    {
        Score.SetValue(GamePlayManager.Instance.Score, UICountImgFont.IMG_RANGE.CENTER);
    }

    public void RefreshItemUI()
    {
        if (GamePlayManager.Instance.mNormalitemArr[(int)CommonData.ITEM_SLOT_INDEX.LEFT] != 0)
        {
            int itemId = GamePlayManager.Instance.mNormalitemArr[(int)CommonData.ITEM_SLOT_INDEX.LEFT];
            mItemLeftImg.sprite = ItemManager.Instance.GetItemIcon(itemId);
            mItemLeftImg.color = new Color(1, 1, 1, 1);
        }
        else
        {
            mItemLeftImg.color = new Color(1, 1, 1, 0);
            mItemLeftImg.sprite = null;
        }

        if (GamePlayManager.Instance.mNormalitemArr[(int)CommonData.ITEM_SLOT_INDEX.RIGHT] != 0)
        {
            int itemId = GamePlayManager.Instance.mNormalitemArr[(int)CommonData.ITEM_SLOT_INDEX.RIGHT];
            mItemRightImg.sprite = ItemManager.Instance.GetItemIcon(itemId);
            mItemRightImg.color = new Color(1, 1, 1, 1);
        }
        else
        {
            mItemRightImg.color = new Color(1, 1, 1, 0);
            mItemRightImg.sprite = null;
        }
    }

    public void RefreshShieldItemUI()
    {
        var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_COUNT);
        for (int i = 0; i < mShieldIconList.Count; i++)
        {
            if (skill != null && skill.mCount >= i + 1)
            {
                mShieldIconList[i].gameObject.SetActive(true);
            }
            else
            {
                mShieldIconList[i].gameObject.SetActive(false);
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

    public void OnClickItemLeft()
    {
        UseItem(CommonData.ITEM_SLOT_INDEX.LEFT);
    }
    public void OnClickItemRight()
    {
        UseItem(CommonData.ITEM_SLOT_INDEX.RIGHT);
    }
    private void UseItem(CommonData.ITEM_SLOT_INDEX index)
    {
        GamePlayManager.Instance.UseGameNormalItem(index);
        RefreshItemUI();
    }
    public void UseItemSkill(int itemId, GameSkill skill)
    {
        for (int i = 0; i < mSkillProgressBarList.Count; i++)
        {
            if (mSkillProgressBarList[i].mItemId == itemId)
                return;
        }

        var obj = Instantiate(Resources.Load("Prefab/UISkillProgressBar"), mSkillProgressStartPos) as GameObject;
        var progressBar = obj.GetComponent<UISkillProgressBar>();
        progressBar.SetItemSkill(itemId, skill.mSkillType);
        progressBar.gameObject.transform.localPosition = new Vector3(0, mSkillProgressBarList.Count * 80);
        mSkillProgressBarList.Add(progressBar);
    }
    public void GameOver(int gameOverNoteId)
    {
        StartCoroutine(Co_GameOver(gameOverNoteId));
    }

    public IEnumerator Co_GameOver(int gameOverNoteId)
    {
        yield return new WaitForSecondsRealtime(1f);

        var popupData = new PopupGameEnd.PopupData(gameOverNoteId, GamePlayManager.Instance.Score, GamePlayManager.Instance.ConvertScoreToCoin());
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_END, popupData);
    }

    void Update()
    {
        StringBuilder text = new StringBuilder();
        text.AppendFormat("노트속도 : {0}", GamePlayManager.Instance.NoteSpeed);
        text.AppendLine();
        //text.AppendFormat("누적노트 : {0}", GamePlayManager.Instance.AccumulateCreateNoteCount);
        //text.AppendLine();

        // 스킬
        var skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_TIME);
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

        Info.text = text.ToString();
    }
}

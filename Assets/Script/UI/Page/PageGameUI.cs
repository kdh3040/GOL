﻿using System.Collections;
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

    void Awake()
    {
        mItemRightButton.onClick.AddListener(OnClickItemLeft);
        mItemLeftButton.onClick.AddListener(OnClickItemRight);
    }

    public void ResetUI()
    {
        RefreshItemUI();
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
            var itemData = DataManager.Instance.ItemDataDic[itemId];
            mItemRightImg.sprite = (Sprite)Resources.Load(itemData.icon, typeof(Sprite));
            mItemRightImg.color = new Color(1, 1, 1, 1);
        }
        else
        {
            mItemRightImg.color = new Color(1, 1, 1, 0);
            mItemRightImg.sprite = null;
        }

        if (GamePlayManager.Instance.mNormalitemArr[(int)CommonData.ITEM_SLOT_INDEX.RIGHT] != 0)
        {
            int itemId = GamePlayManager.Instance.mNormalitemArr[(int)CommonData.ITEM_SLOT_INDEX.RIGHT];
            var itemData = DataManager.Instance.ItemDataDic[itemId];
            mItemLeftImg.sprite = (Sprite)Resources.Load(itemData.icon, typeof(Sprite));
            mItemLeftImg.color = new Color(1, 1, 1, 1);
        }
        else
        {
            mItemLeftImg.color = new Color(1, 1, 1, 0);
            mItemLeftImg.sprite = null;
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
    public void GameOver()
    {
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_END);
    }

    void Update()
    {
        StringBuilder text = new StringBuilder();
        text.AppendFormat("노트속도 : {0}", GamePlayManager.Instance.NoteSpeed);
        text.AppendLine();
        text.AppendFormat("누적노트 : {0}", GamePlayManager.Instance.AccumulateCreateNoteCount);
        text.AppendLine();

        // 스킬
        var skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD, SkillManager.SKILL_CHECK_TYPE.TIME);
        if(skillData != null)
        {
            text.AppendFormat("무적스킬 남은시간 {0:f2}", skillData.mValue1);
            text.AppendLine();
        }
        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD, SkillManager.SKILL_CHECK_TYPE.COUNT);
        if (skillData != null)
        { 
            text.AppendFormat("쉴드스킬 남은갯수 {0}", skillData.mValue1);
            text.AppendLine();
        }
        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.SCORE_UP, SkillManager.SKILL_CHECK_TYPE.TIME);
        if (skillData != null)
        { 
            text.AppendFormat("점수두배스킬 남은시간 {0:f2} {1}배", skillData.mValue1, skillData.mValue2);
            text.AppendLine();
        }

        Info.text = text.ToString();
    }
}

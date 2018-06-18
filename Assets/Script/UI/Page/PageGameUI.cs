using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PageGameUI : MonoBehaviour
{
    public Text Score;
    public Text Combo;
    public Button PauseButton;
    public Text Info;
    public Button Item_1;
    public Image ItemImg_1;
    public Button Item_2;
    public Image ItemImg_2;

    void Awake()
    {
        PauseButton.onClick.AddListener(OnClickPause);
        Item_1.onClick.AddListener(OnClickItem_1);
        Item_2.onClick.AddListener(OnClickItem_2);
    }

    public void ResetUI()
    {
        RefreshItemUI();
        Score.text = string.Format(LocalizeData.Instance.GetLocalizeString("SCORE_COUNT"), 0);
        Combo.text = "";
    }

    public void RefreshUI()
    {
        Score.text = string.Format(LocalizeData.Instance.GetLocalizeString("SCORE_COUNT"), GamePlayManager.Instance.Score);
    }

    public void RefreshItemUI()
    {
        if (GamePlayManager.Instance.mPlayItemArr[0] != 0)
        {
            int itemId = GamePlayManager.Instance.mPlayItemArr[0];
            var itemData = DataManager.Instance.ItemDataList[itemId];
            ItemImg_1.sprite = (Sprite)Resources.Load(itemData.icon, typeof(Sprite));
        }
        else
            ItemImg_1.sprite = null;

        if (GamePlayManager.Instance.mPlayItemArr[1] != 0)
        {
            int itemId = GamePlayManager.Instance.mPlayItemArr[1];
            var itemData = DataManager.Instance.ItemDataList[itemId];
            ItemImg_2.sprite = (Sprite)Resources.Load(itemData.icon, typeof(Sprite));
        }
        else
            ItemImg_2.sprite = null;
    }

    public void RefreshCombo(bool plus)
    {
        // TODO 환웅 : 콤보가 추가 되면 효과?

        if (GamePlayManager.Instance.Combo <= 0)
            Combo.text = "";
        else
            Combo.text = string.Format(LocalizeData.Instance.GetLocalizeString("COMBO_COUNT"), GamePlayManager.Instance.Combo);
    }

    public void OnClickPause()
    {
        GamePlayManager.Instance.GamePause();
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_PAUSE);
    }

    public void OnClickItem_1()
    {
        UseItem(0);
    }
    public void OnClickItem_2()
    {
        UseItem(1);
    }
    private void UseItem(int index)
    {
        int itemId = GamePlayManager.Instance.mPlayItemArr[index];
        if (itemId == 0)
            return;

        GamePlayManager.Instance.UseGameItem(index);
        var itemData = DataManager.Instance.ItemDataList[itemId];
        SkillManager.Instance.AddUseSkill(itemData.skill);
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
            text.AppendFormat("점수두배스킬 남은시간 {0:f2} {1}배", skillData.mSkillData.name, skillData.mValue1, skillData.mValue2);
            text.AppendLine();
        }

        Info.text = text.ToString();
    }
}

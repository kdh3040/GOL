using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillProgressBar : MonoBehaviour {

    public Image Icon;
    public Slider ProgressBar;
    public Text Time;

    [System.NonSerialized]
    public int mItemId = 0;
    private SkillManager.SKILL_TYPE mSkillType = SkillManager.SKILL_TYPE.NONE;

    public void SetItemSkill(int itemId, SkillManager.SKILL_TYPE skillType)
    {
        mItemId = itemId;
        mSkillType = skillType;
        Icon.sprite = ItemManager.Instance.GetItemIcon(itemId);
    }

    public void UpdateSkillProgress()
    {
        var skill = GetUISkillData();
        if(skill != null && skill.mTime > 0)
        {
            // TODO 환웅 시간 관련 스킬인지 알아야함
            ProgressBar.value = skill.mTime / skill.mMaxTime;
            Time.text = string.Format(LocalizeData.Instance.GetLocalizeString("SKILL_CHECK_TYPE_TIME_REMAIN"), skill.mTime);
        }
    }

    public GameSkill GetUISkillData()
    {
        return SkillManager.Instance.GetGameSkill(mSkillType);
    }
}

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
        var itemData = DataManager.Instance.ItemDataDic[itemId];
        Icon.sprite = (Sprite)Resources.Load(itemData.icon, typeof(Sprite));
    }

    public void UpdateSkillProgress()
    {
        var skill = GetUISkillData();
        if(skill != null)
        {
            ProgressBar.value = skill.mValue1 / skill.mMaxValue1;

            if (skill.mSkillCheckType == SkillManager.SKILL_CHECK_TYPE.TIME)
            {
                Time.text = string.Format(LocalizeData.Instance.GetLocalizeString("SKILL_CHECK_TYPE_TIME_REMAIN"), skill.mValue1);
            }
        }
    }

    public GameSkill GetUISkillData()
    {
        return SkillManager.Instance.GetGameSkill(mSkillType, SkillManager.SKILL_CHECK_TYPE.TIME);
    }
}

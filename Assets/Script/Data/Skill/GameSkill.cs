using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSkill 
{
    public SkillData mSkillData;
    public bool mEnable = false;
    public bool mSkillRemoveReady = false;
    public float mValue1 { get; protected set; }
    public float mValue2 { get; protected set; }
    public float mValue3 { get; protected set; }
    public SkillManager.SKILL_TYPE mSkillType = SkillManager.SKILL_TYPE.NONE;
    public SkillManager.SKILL_CHECK_TYPE mSkillCheckType = SkillManager.SKILL_CHECK_TYPE.NONE;
    public float mMaxValue1 { get; protected set; }

    public GameSkill(string name)
    {
        mSkillData = SkillManager.Instance.SkillDataList[name];
        mValue1 = mSkillData.value1;
        mValue2 = mSkillData.value2;
        mValue3 = mSkillData.value3;
        mMaxValue1 = mSkillData.value1;
        mEnable = false;
        mSkillRemoveReady = false;
        mSkillType = SkillManager.Instance.ConvertSkillType(mSkillData.skilltype);
        mSkillCheckType = SkillManager.Instance.ConvertSkillCheckType(mSkillData.checktype);
    }

    public virtual void StartSkill()
    {
        mEnable = true;
    }

    public virtual void EndSkill()
    {
        mEnable = false;
        mSkillRemoveReady = true;
    }

    public virtual void PlusSameSkill()
    {
        mValue1 += mSkillData.value1;
        mValue2 += mSkillData.value2;
        mValue3 += mSkillData.value3;
        mMaxValue1 += mSkillData.value1;
    }
    public abstract void SkillUpdate(float time);
}

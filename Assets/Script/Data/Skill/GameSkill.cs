using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSkill 
{
    public SkillData mSkillData;
    public bool mEnable = false;
    public bool mSkillRemoveReady = false;
    public float mTime { get; protected set; }
    public float mCount { get; protected set; }
    public float mPercent { get; protected set; }
    public float mMaxTime { get; protected set; }
    public SkillManager.SKILL_TYPE mSkillType = SkillManager.SKILL_TYPE.NONE;

    public GameSkill(string name)
    {
        mSkillData = SkillManager.Instance.GetSkillData(name);
        mEnable = false;
        mSkillRemoveReady = false;
        mSkillType = SkillManager.Instance.ConvertSkillType(mSkillData.skilltype);
        mTime = mSkillData.time;
        mCount = mSkillData.count;
        mPercent = mSkillData.percent * 0.01f;
        mMaxTime = mSkillData.time;
    }

    public virtual void StartSkill()
    {
        mEnable = true;
    }

    public virtual void EndSkill()
    {
        GamePlayManager.Instance.EndSkill(this);
        mEnable = false;
        mSkillRemoveReady = true;
    }

    public virtual void PlusSameSkill(GameSkill data)
    {
        mTime += data.mTime;
        if (mMaxTime < mTime)
            mTime = mMaxTime;
        mCount += data.mCount;
        mPercent += data.mPercent;
    }
    public abstract void SkillUpdate(float time);
}

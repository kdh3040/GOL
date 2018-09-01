using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSkill_SpeedDown : GameSkill
{
    public GameSkill_SpeedDown(string name)
        : base(name)
    {

    }

    public override void SkillUpdate(float time)
    {
        mTime -= time;

        if (mTime <= 0)
            EndSkill();
    }

    public override void PlusSameSkill(GameSkill data)
    {
        mTime += data.mTime;

        if (mMaxTime < mTime)
            mTime = mMaxTime;
    }

    public float ConvertSpeed(float time)
    {
        if (mEnable == false && mTime <= 0)
            return time;

        return time * mPercent;
    }
}

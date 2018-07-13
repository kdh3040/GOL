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
        mValue1 -= time;

        if (mValue1 <= 0)
            EndSkill();
    }

    public float ConvertSpeed(float time)
    {
        if (mEnable == false && mValue1 <= 0)
            return time;

        return time / mValue2;
    }
}

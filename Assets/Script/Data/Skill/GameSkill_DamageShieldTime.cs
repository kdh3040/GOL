using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSkill_DamageShieldTime : GameSkill
{
    public GameSkill_DamageShieldTime(string name)
        : base(name)
    {

    }

    public override void SkillUpdate(float time)
    {
        mTime -= time;

        if (mTime <= 0)
            EndSkill();
    }
}

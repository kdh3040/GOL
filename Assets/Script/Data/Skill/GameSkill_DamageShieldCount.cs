using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSkill_DamageShieldCount : GameSkill
{
    public GameSkill_DamageShieldCount(string name)
        : base(name)
    {

    }

    public override void SkillUpdate(float time)
    {
    }

    public bool CharShield()
    {
        if(mCount > 0)
        {
            mCount -= 1;
            return true;
        }

        if (mCount <= 0)
            EndSkill();

        return false;
    }
}

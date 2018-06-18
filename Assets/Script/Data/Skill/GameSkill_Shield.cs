using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSkill_Shield : GameSkill
{
    public GameSkill_Shield(string name)
        : base(name)
    {

    }

    public override void SkillUpdate(float time)
    {
    }

    public bool CharShield()
    {
        if(mValue1 > 0)
        {
            mValue1 -= 1;
            return true;
        }

        if (mValue1 <= 0)
            EndSkill();

        return false;
    }
}

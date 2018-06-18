using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSkill_Invincibility : GameSkill
{
    public GameSkill_Invincibility(string name)
        : base(name)
    {

    }

    public override void SkillUpdate(float time)
    {
        mValue1 -= time;

        if (mValue1 <= 0)
            EndSkill();
    }
}

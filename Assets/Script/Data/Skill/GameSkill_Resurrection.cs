using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSkill_Resurrection: GameSkill
{
    public GameSkill_Resurrection(string name)
        : base(name)
    {

    }

    public override void SkillUpdate(float time)
    {
    }

    public bool IsResurrection()
    {
        var value = Random.Range(0.01f, 1.00f);

        if(mPercent >= value)
        {
            EndSkill();
            return true;
        }

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSkill_ScoreUP : GameSkill
{
    public GameSkill_ScoreUP(string name)
        : base(name)
    {

    }

    public override void SkillUpdate(float time)
    {
        mValue1 -= time;

        if (mValue1 <= 0)
            EndSkill();
    }

    public override void PlusSameSkill()
    {
        mValue1 += mSkillData.value1;
        mValue2 *= mSkillData.value2;
    }

    public int ConvertNoteScore(int score)
    {
        if (mEnable == false && mValue1 <= 0)
            return score;

        return score * (int)mValue2;
    }
}

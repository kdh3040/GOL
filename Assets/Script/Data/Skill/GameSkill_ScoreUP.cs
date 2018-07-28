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
        mTime -= time;

        if (mTime <= 0)
            EndSkill();
    }


    public int ConvertNoteScore(int score)
    {
        if (mEnable == false && mTime <= 0)
            return score;

        return (int)(score * mPercent);
    }
}

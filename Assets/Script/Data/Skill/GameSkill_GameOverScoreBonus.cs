using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSkill_GameOverScoreBonus : GameSkill
{
    public GameSkill_GameOverScoreBonus(string name)
        : base(name)
    {

    }

    public override void SkillUpdate(float time)
    {
    }


    public int ConvertScore(int score)
    {
        return score + (int)(score * mPercent);
    }
}

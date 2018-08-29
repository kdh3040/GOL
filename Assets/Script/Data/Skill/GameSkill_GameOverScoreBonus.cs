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


    public int BonusScore(int score)
    {
        return (int)(score * mPercent);
    }
}

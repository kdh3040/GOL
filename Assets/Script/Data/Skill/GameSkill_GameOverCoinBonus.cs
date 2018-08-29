﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSkill_GameOverCoinBonus : GameSkill
{
    public GameSkill_GameOverCoinBonus(string name)
        : base(name)
    {

    }

    public override void SkillUpdate(float time)
    {
    }


    public int BonusCoin(int coin)
    {
        return (int)(coin * mPercent);
    }
}
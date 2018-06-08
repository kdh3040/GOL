using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIPage : MonoBehaviour {
    public Text Score;
    public int ScoreCount = 0;
    public Text Combo;
    public int ComboCount = 0;
    public float ComboKeepTime = 0f;
    public bool ComboKeepEnable = false;

    public void PageReset()
    {
        ScoreCount = 0;
        ComboCount = 0;
        ComboKeepTime = 0f;
        ComboKeepEnable = false;

        Score.text = string.Format(CommonData.STRING_SCORE_COUNT, 0);
        Combo.text = "";
    }
    public void PageUpdate(float time)
    {
        ComboKeepTime += time;

        if (ComboKeepTime <= CommonData.COMBO_KEEP_TIME)
        {
            ComboKeepEnable = true;
        }
        else
        {
            ComboCount = 0;
            ComboKeepTime = 0;
            ComboKeepEnable = false;
            Combo.text = "";
        }
    }

    public void PlusScore(int score)
    {
        ScoreCount += score;
        Score.text = string.Format(CommonData.STRING_SCORE_COUNT, ScoreCount);
    }

    public void PlusCombo()
    {
        if (ComboKeepEnable)
        {
            ComboCount++;
            Combo.text = string.Format(CommonData.STRING_COMBO_COUNT, ComboCount);
        }
    }
}

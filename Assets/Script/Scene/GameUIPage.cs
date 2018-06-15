using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIPage : MonoBehaviour {
    public Text Score;
    public Text Combo;

    public void PageReset()
    {
        Score.text = string.Format(LocalizeData.Instance.GetLocalizeString("SCORE_COUNT"), 0);
        Combo.text = "";
    }

    public void RefreshUI()
    {
        Score.text = string.Format(LocalizeData.Instance.GetLocalizeString("SCORE_COUNT"), GManager.Instance.Score);
    }

    public void RefreshCombo(bool plus)
    {
        // TODO 환웅 : 콤보가 추가 되면 효과?

        if (GManager.Instance.Combo <= 0)
            Combo.text = "";
        else
            Combo.text = string.Format(LocalizeData.Instance.GetLocalizeString("COMBO_COUNT"), GManager.Instance.Combo);
    }
}

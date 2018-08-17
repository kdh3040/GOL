using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPointValue : MonoBehaviour {
    public Text Title;
    public UICountImgFont Cost;

    public int CostValue { get; private set; }

    public void SetValue(int cost, string title)
    {
        Title.text = title;
        CostValue = cost;
        Cost.SetValue(CostValue, UICountImgFont.IMG_RANGE.RIGHT);
    }
}

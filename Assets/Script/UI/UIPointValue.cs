using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPointValue : MonoBehaviour {
    public UICountImgFont Cost;

    public int CostValue { get; private set; }

    public void SetValue(int cost)
    {
        CostValue = cost;
        Cost.SetValue(CostValue.ToString(), UICountImgFont.IMG_RANGE.RIGHT, UICountImgFont.IMG_TYPE.YELLOW);
    }
}

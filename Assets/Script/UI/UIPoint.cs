using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPoint : MonoBehaviour {

    public enum POINT_TYPE
    {
        DDONG,
        COIN,
    }

    public Button ChargeButton;
    public Text PointText;
    public Image PointIcon;
    [System.NonSerialized]
    public POINT_TYPE mPointType;

    public void Initialize(POINT_TYPE type)
    {
        mPointType = type;

        switch(mPointType)
        {
            case POINT_TYPE.DDONG:
                CommonFunc.SetImageFile("ddong", ref PointIcon);
                break;
            case POINT_TYPE.COIN:
                CommonFunc.SetImageFile("market_gold", ref PointIcon);
                break;
        }
    }
    public void SetPoint(int count)
    {
        PointText.text = CommonFunc.ConvertNumber(count);
    }


}

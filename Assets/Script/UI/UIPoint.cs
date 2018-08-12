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
    [System.NonSerialized]
    public int mTempSaveValue = 0;

    void Awake()
    {
        ChargeButton.onClick.AddListener(OnClickCharge);
    }

    public void Initialize(POINT_TYPE type)
    {
        mTempSaveValue = -1;
        mPointType = type;

        switch(mPointType)
        {
            case POINT_TYPE.DDONG:
                CommonFunc.SetImageFile("Renewal/UI/icon_ddong", ref PointIcon);
                break;
            case POINT_TYPE.COIN:
                CommonFunc.SetImageFile("Renewal/UI/icon_gold", ref PointIcon);
                break;
        }

        UpdatePoint();
    }

    public void UpdatePoint()
    {
        switch (mPointType)
        {
            case POINT_TYPE.DDONG:
                if (mTempSaveValue == PlayerData.Instance.MyDDong)
                    break;

                if (PlayerData.Instance.MyDDong > 0)
                {
                    mTempSaveValue = PlayerData.Instance.MyDDong;
                    PointText.text = string.Format("{0}/{1}", CommonFunc.ConvertNumber(PlayerData.Instance.MyDDong), CommonFunc.ConvertNumber(CommonData.MAX_DDONG_COUNT));
                }
                else
                {
                    mTempSaveValue = -1;
                    var time = PlayerData.Instance.GetNextDDongRefileTime();
                    PointText.text = string.Format("{0}:{1}", time.Minutes, time.Seconds);
                }
                break;
            case POINT_TYPE.COIN:
                if (mTempSaveValue == PlayerData.Instance.MyCoin)
                    break;

                mTempSaveValue = PlayerData.Instance.MyCoin;
                PointText.text = CommonFunc.ConvertNumber(PlayerData.Instance.MyCoin);
                break;
        }
    }

    public void OnClickCharge()
    {
        switch (mPointType)
        {
            case POINT_TYPE.DDONG:
                PlayerData.Instance.PlusDDong(1);
                break;
            case POINT_TYPE.COIN:
                PlayerData.Instance.PlusCoin(100);
                break;
        }
    }


}

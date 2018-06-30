using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITopBar : MonoBehaviour
{
    public Button BackButton;
    public UIPoint DdongPoint;
    public UIPoint CoinPoint;

    void Awake()
    {
        BackButton.onClick.AddListener(OnClickBack);
    }
    public void Initialize(bool backButtonEnable)
    {
        BackButton.gameObject.SetActive(backButtonEnable);

        if (backButtonEnable == false)
        {
            DdongPoint.gameObject.transform.localPosition = new Vector3(-364, 0);
            CoinPoint.gameObject.transform.localPosition = new Vector3(20, 0);
        }
        else
        {
            DdongPoint.gameObject.transform.localPosition = new Vector3(-171, 0);
            CoinPoint.gameObject.transform.localPosition = new Vector3(213, 0);
        }

        DdongPoint.Initialize(UIPoint.POINT_TYPE.DDONG);
        CoinPoint.Initialize(UIPoint.POINT_TYPE.COIN);
        DdongPoint.SetPoint(GManager.Instance.mPlayerData.Ddong);
        CoinPoint.SetPoint(GManager.Instance.mPlayerData.Coin);

        StartCoroutine(UpdateTopBar());
    }

    public void OnClickBack()
    {
        PopupManager.Instance.DismissPopup();
    }

    IEnumerator UpdateTopBar()
    {
        while (true)
        {
            CoinPoint.SetPoint(GManager.Instance.mPlayerData.Coin);
            DdongPoint.SetPoint(GManager.Instance.mPlayerData.Ddong);
            yield return null;
        }
    }
}

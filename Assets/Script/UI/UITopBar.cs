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

        DdongPoint.Initialize(CommonData.POINT_TYPE.DDONG);
        CoinPoint.Initialize(CommonData.POINT_TYPE.COIN);
        DdongPoint.UpdatePoint();
        CoinPoint.UpdatePoint();

        StartCoroutine(UpdateTopBar());
    }

    public void OnClickBack()
    {
        SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUTTON);
        PopupManager.Instance.DismissPopup();
    }

    IEnumerator UpdateTopBar()
    {
        while (true)
        {
            CoinPoint.UpdatePoint();
            DdongPoint.UpdatePoint();
            yield return null;
        }
    }
}

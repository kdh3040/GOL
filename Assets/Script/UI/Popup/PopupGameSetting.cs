using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameSetting : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_SETTING;
    }

    public UITopBar Topbar;
    public Button SoundSettingButton;
    public Button VibrationSettingButton;
    public Button AlarmSettingButton;
    public GameObject SoundSettingCheck;
    public GameObject VibrationSettingCheck;
    public GameObject AlarmSettingCheck;

    public List<InputField> Test = new List<InputField>();

    public static float speed_1 = 0f; // 기본
    public static float speed_2 = 0f; // 증가 시간
    public static float speed_3 = 0f; // 증가폭

    public void Awake()
    {
        SoundSettingButton.onClick.AddListener(OnClickSoundSetting);
        VibrationSettingButton.onClick.AddListener(OnClickVibrationSetting);
        AlarmSettingButton.onClick.AddListener(OnClickAlarmSetting);
    }

    public override void ShowPopup(PopupUIData data)
    {
        this.SetBackGroundImg();
        Topbar.Initialize(true);
        SoundSettingCheck.SetActive(PlayerData.Instance.SoundSetting);
        VibrationSettingCheck.SetActive(PlayerData.Instance.VibrationSetting);
        AlarmSettingCheck.SetActive(PlayerData.Instance.AlarmSetting);

        if(speed_1 == 0)
            Test[0].text = string.Format("{0:F2}", ConfigData.Instance.DEFAULT_NOTE_SPEED + speed_1);
        else
            Test[0].text = string.Format("{0:F2}", speed_1);
        if(speed_2 == 0)
            Test[1].text = string.Format("{0:F2}", ConfigData.Instance.NOTE_SPEED_UP_INTERVAL + speed_2);
        else
            Test[1].text = string.Format("{0:F2}", speed_2);
        if(speed_3 == 0)
            Test[2].text = string.Format("{0:F2}", ConfigData.Instance.NOTE_SPEED_UP + speed_3);
        else
            Test[2].text = string.Format("{0:F2}", speed_3);
    }

    public override void DismissPopup()
    {
        base.DismissPopup();
        speed_1 = float.Parse(Test[0].text);
        speed_2 = float.Parse(Test[1].text);
        speed_3 = float.Parse(Test[2].text);
    }

    public void OnClickSoundSetting()
    {
        PlayerData.Instance.SetSoundSetting(!PlayerData.Instance.SoundSetting);
        SoundSettingCheck.SetActive(PlayerData.Instance.SoundSetting);
    }

    public void OnClickVibrationSetting()
    {
        PlayerData.Instance.SetVibrationSetting(!PlayerData.Instance.VibrationSetting);
        VibrationSettingCheck.SetActive(PlayerData.Instance.VibrationSetting);
    }

    public void OnClickAlarmSetting()
    {
        PlayerData.Instance.SetAlarmSetting(!PlayerData.Instance.AlarmSetting);
        AlarmSettingCheck.SetActive(PlayerData.Instance.AlarmSetting);
    }
}

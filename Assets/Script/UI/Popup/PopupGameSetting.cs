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

        Test[0].text = string.Format("{0:F2}", ConfigData.Instance.DEFAULT_NOTE_SPEED);
        Test[1].text = string.Format("{0:F2}", ConfigData.Instance.NOTE_SPEED_UP_INTERVAL);
        Test[2].text = string.Format("{0:F2}", ConfigData.Instance.NOTE_SPEED_UP);
    }

    public override void DismissPopup()
    {
        base.DismissPopup();
        ConfigData.Instance.DEFAULT_NOTE_SPEED = float.Parse(Test[0].text);
        ConfigData.Instance.NOTE_SPEED_UP_INTERVAL = float.Parse(Test[1].text);
        ConfigData.Instance.NOTE_SPEED_UP = float.Parse(Test[2].text);
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

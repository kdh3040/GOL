﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameSetting : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_SETTING;
    }

    public Button SoundSettingButton;
    public Button VibrationSettingButton;
    public Button AlarmSettingButton;
    public GameObject SoundSettingCheck;
    public GameObject VibrationSettingCheck;
    public GameObject AlarmSettingCheck;

    public void Awake()
    {
        SoundSettingButton.onClick.AddListener(OnClickSoundSetting);
        VibrationSettingButton.onClick.AddListener(OnClickVibrationSetting);
        AlarmSettingButton.onClick.AddListener(OnClickAlarmSetting);
    }

    public override void ShowPopup(PopupUIData data)
    {
        SoundSettingCheck.SetActive(PlayerData.Instance.SoundSetting);
        VibrationSettingCheck.SetActive(PlayerData.Instance.VibrationSetting);
        AlarmSettingCheck.SetActive(PlayerData.Instance.AlarmSetting);
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
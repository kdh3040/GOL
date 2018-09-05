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

    public Button SoundSettingButton;
    public Button VibrationSettingButton;
    public Button AlarmSettingButton;
    public GameObject SoundSettingCheck;
    public GameObject VibrationSettingCheck;
    public GameObject AlarmSettingCheck;

    public GameObject DebugSetting;
    public Text DefaultSpeed;
    public Button DefaultSpeedUp;
    public Button DefaultSpeedDown;
    public Text SpeedUpTime;
    public Button SpeedUpTimeUp;
    public Button SpeedUpTimeDown;
    public Text SpeedUpValue;
    public Button SpeedUp;
    public Button SpeedDown;

    public void Awake()
    {
        SoundSettingButton.onClick.AddListener(OnClickSoundSetting);
        VibrationSettingButton.onClick.AddListener(OnClickVibrationSetting);
        AlarmSettingButton.onClick.AddListener(OnClickAlarmSetting);

        DefaultSpeedUp.onClick.AddListener(() => { ChangeValue(1, true); });
        DefaultSpeedDown.onClick.AddListener(() => { ChangeValue(1, false); });
        SpeedUpTimeUp.onClick.AddListener(() => { ChangeValue(2, true); });
        SpeedUpTimeDown.onClick.AddListener(() => { ChangeValue(2, false); });
        SpeedUp.onClick.AddListener(() => { ChangeValue(3, true); });
        SpeedDown.onClick.AddListener(() => { ChangeValue(3, false); });
    }

    public override void ShowPopup(PopupUIData data)
    {
        SoundSettingCheck.SetActive(PlayerData.Instance.SoundSetting);
        VibrationSettingCheck.SetActive(PlayerData.Instance.VibrationSetting);
        AlarmSettingCheck.SetActive(PlayerData.Instance.AlarmSetting);

        RefreshDebugData();
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

    public void ChangeValue(int type, bool up)
    {
        if(type == 1)
        {
            // 기본 속도
            if (up)
                ConfigData.Instance.DEBUG_DEFAULT_SPEED += 0.1f;
            else
                ConfigData.Instance.DEBUG_DEFAULT_SPEED -= 0.1f;
        }
        else if (type == 2)
        {
            // 속도 증가 시간
            if (up)
                ConfigData.Instance.DEBUG_SPEED_UP_TIME += 0.1f;
            else
                ConfigData.Instance.DEBUG_SPEED_UP_TIME -= 0.1f;
        }
        else if (type == 3)
        {
            // 속도 증가 폭
            if (up)
                ConfigData.Instance.DEBUG_SPEED_UP += 0.1f;
            else
                ConfigData.Instance.DEBUG_SPEED_UP -= 0.1f;
        }

        RefreshDebugData();
    }

    public void RefreshDebugData()
    {
        DefaultSpeed.text = string.Format("{0:f2}", ConfigData.Instance.DEFAULT_NOTE_SPEED + ConfigData.Instance.DEBUG_DEFAULT_SPEED);
        SpeedUpTime.text = string.Format("{0:f2}", ConfigData.Instance.NOTE_SPEED_UP_INTERVAL + ConfigData.Instance.DEBUG_SPEED_UP_TIME);
        SpeedUpValue.text = string.Format("{0:f2}", ConfigData.Instance.NOTE_SPEED_UP + ConfigData.Instance.DEBUG_SPEED_UP);
    }
}

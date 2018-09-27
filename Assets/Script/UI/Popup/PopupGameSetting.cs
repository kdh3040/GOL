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
    public Button EffectSoundSettingButton;
    public Button VibrationSettingButton;
    public Button AlarmSettingButton;
    public GameObject SoundSettingCheck;
    public GameObject EffectSoundSettingCheck;
    public GameObject VibrationSettingCheck;
    public GameObject AlarmSettingCheck;

    public List<InputField> Test_1_1 = new List<InputField>();
    public List<InputField> Test_1_2 = new List<InputField>();
    public List<InputField> Test_2 = new List<InputField>();
    public InputField Test_3;

    public void Awake()
    {
        SoundSettingButton.onClick.AddListener(OnClickSoundSetting);
        EffectSoundSettingButton.onClick.AddListener(OnClickEffectSoundSetting);
        VibrationSettingButton.onClick.AddListener(OnClickVibrationSetting);
        AlarmSettingButton.onClick.AddListener(OnClickAlarmSetting);
        
    }

    public override void ShowPopup(PopupUIData data)
    {
        this.SetBackGroundImg();
        Topbar.Initialize(true);
        SoundSettingCheck.SetActive(PlayerData.Instance.SoundSetting);
        EffectSoundSettingCheck.SetActive(PlayerData.Instance.EffectSoundSetting);
        VibrationSettingCheck.SetActive(PlayerData.Instance.VibrationSetting);
        AlarmSettingCheck.SetActive(PlayerData.Instance.AlarmSetting);

        Test_2[0].text = string.Format("{0}", DataManager.Instance.ItemDataDic[1].create_probability);
        Test_2[1].text = string.Format("{0}", DataManager.Instance.ItemDataDic[2].create_probability);
        Test_2[2].text = string.Format("{0}", DataManager.Instance.ItemDataDic[3].create_probability);
        Test_2[3].text = string.Format("{0}", DataManager.Instance.ItemDataDic[4].create_probability);

        Test_1_1[0].text = string.Format("{0}", PlayerData.Instance.GetSkinSlotLevel(CommonData.SKIN_TYPE.CHAR));
        Test_1_1[1].text = string.Format("{0}", PlayerData.Instance.GetSkinSlotLevel(CommonData.SKIN_TYPE.DOOR));
        Test_1_1[2].text = string.Format("{0}", PlayerData.Instance.GetSkinSlotLevel(CommonData.SKIN_TYPE.BACKGROUND));

        Test_1_2[0].text = string.Format("{0}", PlayerData.Instance.GetItemLevel(1));
        Test_1_2[1].text = string.Format("{0}", PlayerData.Instance.GetItemLevel(2));
        Test_1_2[2].text = string.Format("{0}", PlayerData.Instance.GetItemLevel(3));

        Test_3.text = string.Format("{0}", ConfigData.Instance.NOTE_ITEM_CREATE_PERCENT);
    }

    public override void DismissPopup()
    {
        base.DismissPopup();

        DataManager.Instance.ItemDataDic[1].create_probability = int.Parse(Test_2[0].text);
        DataManager.Instance.ItemDataDic[2].create_probability = int.Parse(Test_2[1].text);
        DataManager.Instance.ItemDataDic[3].create_probability = int.Parse(Test_2[2].text);
        DataManager.Instance.ItemDataDic[4].create_probability = int.Parse(Test_2[3].text);

        ConfigData.Instance.NOTE_ITEM_CREATE_PERCENT = float.Parse(Test_3.text);

        PlayerData.Instance.SetSkinSlotLevel(CommonData.SKIN_TYPE.CHAR, int.Parse(Test_1_1[0].text));
        PlayerData.Instance.SetSkinSlotLevel(CommonData.SKIN_TYPE.DOOR, int.Parse(Test_1_1[1].text));
        PlayerData.Instance.SetSkinSlotLevel(CommonData.SKIN_TYPE.BACKGROUND, int.Parse(Test_1_1[2].text));

        PlayerData.Instance.SetItemLevel(1, int.Parse(Test_1_2[0].text));
        PlayerData.Instance.SetItemLevel(2, int.Parse(Test_1_2[1].text));
        PlayerData.Instance.SetItemLevel(3, int.Parse(Test_1_2[2].text));
    }

    public void OnClickSoundSetting()
    {
        PlayerData.Instance.SetSoundSetting(!PlayerData.Instance.SoundSetting);
        SoundSettingCheck.SetActive(PlayerData.Instance.SoundSetting);
      
    }

    public void OnClickEffectSoundSetting()
    {
        PlayerData.Instance.SetEffectSoundSetting(!PlayerData.Instance.EffectSoundSetting);
        EffectSoundSettingCheck.SetActive(PlayerData.Instance.EffectSoundSetting);
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

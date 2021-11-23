using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SettingType
{
    MouseSensitive,
    Volume_Main,
    Volume_Effect,
    Volume_Background
}

public class UI_Setting : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Slider slider;
    [SerializeField] private SettingType settingType;
    [SerializeField] private Image muteImage;
    [SerializeField] private Sprite mute_on;
    [SerializeField] private Sprite mute_off;

    private Settings settings;
    float value;
    float originValue;
    private bool isMute;
    private float tempValue;

    public void Init()
    {
        
    }

    private void Start()
    {
        //settings.SetData(settingType, value);
    }

    private void OnEnable()
    {
        if (settings == null)
        {
            settings = GameManager.Instance.GetSettings();
        }

        UpdateInfo();
    }

    public void UpdateInfo()
    {
        switch (settingType)
        {
            case SettingType.MouseSensitive:
                value = settings.data.mouseMoveSpeed;

                break;
            case SettingType.Volume_Main:
                value = settings.data.mainVolume;
                break;
            case SettingType.Volume_Effect:
                value = settings.data.effectVolume;
                break;
            case SettingType.Volume_Background:
                value = settings.data.backgroundVolume;
                break;
        }

        if (settingType == SettingType.MouseSensitive)
        {
            slider.value = value / 300;
            text.text = (slider.value).ToString("N2");
        }
        else
        {
            slider.value = value;
            text.text = (slider.value).ToString("N2");
        }

        originValue = value;
    }

    public void SetInfo()
    {
        if (settings == null) return;

        text.text = (slider.value).ToString("N2");

        if (settingType == SettingType.MouseSensitive)
        {
            value = float.Parse(text.text) * 300;
        }
        else
        {
            value = float.Parse(text.text);

            if (value == 0)
                muteImage.sprite = mute_on;
            else
                muteImage.sprite = mute_off;
        }
    }

    public void ApplyInfo()
    {
        settings.SetData(settingType, value);
    }

    public void CancelInfo()
    {
        settings.SetData(settingType, originValue);
    }

    public void MuteToggle()
    {
        isMute = !isMute;

        if (isMute)
        {
            muteImage.sprite = mute_on;
            tempValue = slider.value;
            slider.value = 0;
        }
        else
        {
            muteImage.sprite = mute_off;
            slider.value = tempValue;
        }

        SetInfo();
    }
}

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
    [SerializeField] SettingType settingType;
    private Settings settings;
    float value;
    float originValue;

    public void Init()
    {

    }

    private void OnEnable()
    {
        if (settings == null)
            settings = GameManager.Instance.GetSettings();

        UpdateInfo();
        settings.SetData(settingType, value);
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
            slider.value = value / 500;
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
            value = float.Parse(text.text) * 500;
            if (originValue != value)
                settings.SetData(settingType, value);
        }
        else
        {
            value = float.Parse(text.text);

            if (originValue != value)
                settings.SetData(settingType, value);
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
}

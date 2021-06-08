using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Data
{
    public float mouseMoveSpeed;
    public float mainVolume;
    public float effectVolume;
    public float backgroundVolume;

    //public void UpdateData(SettingType settingType, float value)
    //{
    //    switch (settingType)
    //    {
    //        case SettingType.MouseSensitive:
    //            mouseMoveSpeeds = value;
    //            break;
    //        case SettingType.Volume_Main:
    //            mainVolume = value;
    //            break;
    //        case SettingType.Volume_Effect:
    //            effectVolume = value;
    //            break;
    //        case SettingType.Volume_Background:
    //            backgroundVolume = value;
    //            break;
    //    }
    //}
}

public class Settings : MonoBehaviour
{
    public Data data;
    private FPPCamController cam;

    [SerializeField] private string path;

    public void Init()
    {
        path = Application.dataPath + "/SettingData.json";
        cam = Camera.main.GetComponent<FPPCamController>();

        if (File.Exists(path))
            LoadData();
        else
        {
            data = new Data();
            data.mouseMoveSpeed = 250;
            data.mainVolume = 0.5f;
            data.effectVolume = 0.5f;
            data.backgroundVolume = 0.5f;
        }
    }

    public void SetData(SettingType settingType, float value)
    {
        switch (settingType)
        {
            case SettingType.MouseSensitive:
                data.mouseMoveSpeed = value;
                cam.SetCameraMoveSpeed(value);
                break;
            case SettingType.Volume_Main:
                data.mainVolume = value;
                break;
            case SettingType.Volume_Effect:
                data.effectVolume = value;
                break;
            case SettingType.Volume_Background:
                data.backgroundVolume = value;
                break;
        }

        SaveData();
        GameManager.Instance.GetSoundManager().UpdateSetting();
    }

    private void SetData()
    {
        if(cam != null)
            cam.SetCameraMoveSpeed(data.mouseMoveSpeed);
    }

    private void LoadData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            data = JsonUtility.FromJson<Data>(json);
            SetData();
        }
    }

    private void SaveData()
    {
        File.WriteAllText(path, JsonUtility.ToJson(data));
    }
}

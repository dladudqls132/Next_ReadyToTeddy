using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SettingController : MonoBehaviour
{
    [SerializeField] private UI_Setting[] ui_settings;

    // Start is called before the first frame update
    public void Init()
    {
        ui_settings = this.GetComponentsInChildren<UI_Setting>();

        this.gameObject.SetActive(false);
    }

    public void ApplyInfo()
    {
        for (int i = 0; i < ui_settings.Length; i++)
        {
            ui_settings[i].ApplyInfo();
        }
    }
}

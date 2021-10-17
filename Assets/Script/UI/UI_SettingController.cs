using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SettingController : MonoBehaviour
{
    [SerializeField] private UI_Setting[] ui_settings;
    [SerializeField] private GameObject menu;

    // Start is called before the first frame update
    public void Init()
    {
        ui_settings = this.GetComponentsInChildren<UI_Setting>();

    }

    public void ApplyInfo()
    {
        for (int i = 0; i < ui_settings.Length; i++)
        {
            ui_settings[i].ApplyInfo();
        }

        ToggleMenu();
    }

    public void CancelInfo()
    {
        for (int i = 0; i < ui_settings.Length; i++)
        {
            ui_settings[i].CancelInfo();
        }

        ToggleMenu();
    }

    public void ToggleMenu()
    {
        if (menu != null)
            menu.GetComponent<Animator>().SetBool("isOn", !menu.GetComponent<Animator>().GetBool("isOn"));

        this.GetComponent<Animator>().SetBool("isOn", !this.GetComponent<Animator>().GetBool("isOn"));

        for (int i = 0; i < ui_settings.Length; i++)
        {
            ui_settings[i].UpdateInfo();
        }
    }
}

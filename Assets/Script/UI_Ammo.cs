using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ammo : MonoBehaviour
{
    [SerializeField] private Gun gun;
    [SerializeField] private Text text_ammoCount;
    [SerializeField] private Image image_lowAmmoCount;
    [SerializeField] private Image image_reload;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetPlayer().GetWeaponGameObject() != null)
        {
            if (GameManager.Instance.GetPlayer().GetWeaponGameObject().GetComponent<Gun>() != gun)
            {
                gun = GameManager.Instance.GetPlayer().GetWeaponGameObject().GetComponent<Gun>();
            }
        }

        if (gun != null)
        {
            text_ammoCount.text = gun.GetCurrentAmmoCount().ToString();

            if (gun.GetCurrentAmmoCount() <= gun.GetMaxAmmoCount() / 3)
            {
                image_lowAmmoCount.enabled = true;

                if(!gun.GetIsReload())
                    image_reload.enabled = true;
                else
                    image_reload.enabled = false;
            }
            else
            {
                image_lowAmmoCount.enabled = false;
                image_reload.enabled = false;
            }
        }
    }
}

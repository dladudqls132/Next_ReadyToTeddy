using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ammo : MonoBehaviour
{
    [SerializeField] private Gun gun = null;
    [SerializeField] private Projectile projectile = null;
    [SerializeField] private Text text_ammoCount_current = null;
    [SerializeField] private Text text_ammoCount_current_background = null;
    [SerializeField] private Text text_ammoCount_max = null;
    [SerializeField] private Text text_ammoCount_max_background = null;
    [SerializeField] private Text text_projectileCount = null;
    [SerializeField] private Text text_projectileCount_background = null;
    [SerializeField] private Image image_lowAmmoCount = null;
    [SerializeField] private Image image_reload = null;
    [SerializeField] private Image image_gun = null;
    [SerializeField] private Image image_panel = null;
    [SerializeField] private Image image_q_on = null;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetPlayer().GetWeaponGameObject() != null)
        {
            if (GameManager.Instance.GetPlayer().GetWeaponGameObject().GetComponent<Gun>() != null)
            {
                if (GameManager.Instance.GetPlayer().GetWeaponGameObject().GetComponent<Gun>() != gun)
                {
                    gun = GameManager.Instance.GetPlayer().GetWeaponGameObject().GetComponent<Gun>();
                }
            }
            else
            {
                if (GameManager.Instance.GetPlayer().GetWeaponGameObject().GetComponent<Projectile>() != projectile)
                {
                    projectile = GameManager.Instance.GetPlayer().GetWeaponGameObject().GetComponent<Projectile>();
                }
            }
        }

        if (gun != null)
        {
            if(image_gun.sprite != gun.GetSprite())
                image_gun.sprite = gun.GetSprite();

            text_ammoCount_current.text = gun.GetCurrentAmmoCount().ToString();
            text_ammoCount_current_background.text = text_ammoCount_current.text;
            text_ammoCount_max.text = gun.GetHaveAmmoCount().ToString();
            text_ammoCount_max_background.text = text_ammoCount_max.text;

            if (gun.GetHaveAmmoCount() == 0 && gun.GetCurrentAmmoCount() == 0)
            {
                image_lowAmmoCount.enabled = true;

                //if (!gun.GetIsReload() && gun.GetCurrentAmmoCount() <= 1)
                //{
                //    image_reload.enabled = true;
                //}
                //else
                //    image_reload.enabled = false;
            }
            else
            {
                image_lowAmmoCount.enabled = false;
                //image_reload.enabled = false;
            }
        }
        if(projectile != null)
        {
            text_projectileCount.text = projectile.GetHaveNum().ToString();
            text_projectileCount_background.text = text_projectileCount.text;

            image_lowAmmoCount.enabled = false;
            image_reload.enabled = false;
        }

        if (GameManager.Instance.GetPlayer().GetInventory().isOpen)
        {
            image_panel.enabled = true;
            image_q_on.enabled = true;
        }
        else
        {
            image_panel.enabled = false;
            image_q_on.enabled = false;
        }
    }
}

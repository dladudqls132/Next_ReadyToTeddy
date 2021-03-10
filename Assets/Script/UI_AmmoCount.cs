using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AmmoCount : MonoBehaviour
{
    [SerializeField] private GameObject gun;
    private Text text;

    private void Start()
    {
        text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.GetPlayer().GetWeaponGameObject() != null)
        {
            if(GameManager.Instance.GetPlayer().GetWeaponGameObject() != gun)
            {
                gun = GameManager.Instance.GetPlayer().GetWeaponGameObject();
            }
        }

        if(gun != null)
        {
            text.text = gun.GetComponent<Gun>().GetAmmoCount().ToString();
        }
    }
}

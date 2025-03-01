﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Magazine : Item
{
    [SerializeField] private GunType gunType;

    public GunType GetMagType()
    {
        return gunType;
    }

    private void FixedUpdate()
    {
        if (player.GetInventory().GetWeapon(gunType) != null)
        {
            if (player.GetInventory().GetWeapon(gunType).GetHaveAmmoCount() < player.GetInventory().GetWeapon(gunType).GetMaxAmmoCount())
            {
                UpdateMoveSpeed();
                this.GetComponent<Collider>().isTrigger = true;
                //rigid.position = Vector3.Lerp(rigid.position, player.GetAimPos().position, Time.deltaTime * 12);
                rigid.velocity = (player.GetAimPos().position - rigid.position).normalized * moveSpeed;
            }
            else
            {
                moveSpeed = 0;
                rigid.velocity = Vector3.Lerp(rigid.velocity, Vector3.zero, Time.deltaTime * 10);
            }

        }

    }

    private void Update()
    {


        if (Vector3.Distance(this.transform.position, player.GetAimPos().position) < 0.2f)
        {
                
            if(player.GetInventory().GetWeapon(gunType) != null)
            {
                if (player.GetInventory().GetWeapon(gunType).GetHaveAmmoCount() < player.GetInventory().GetWeapon(gunType).GetMaxAmmoCount())
                {
                    
                    Gun temp = player.GetInventory().GetWeapon(gunType);
                    //temp.SetHaveAmmoCount(temp.GetMaxAmmoCount() + temp.GetMaxAmmo_aMagCount());
                    temp.AddAmmo(temp.GetMaxAmmo_aMagCount());
                    Destroy(this.gameObject);
                }
            }
        }

    }
}

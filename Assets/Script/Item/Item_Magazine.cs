using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Magazine : Item
{
    [SerializeField] private GunType gunType;

    private void FixedUpdate()
    {
        if (player.GetInventory().GetWeapon(gunType) != null)
        {
            Debug.Log("asd");
            //rigid.position = Vector3.Lerp(rigid.position, player.GetAimPos().position, Time.deltaTime * 12);
            rigid.velocity = (player.GetAimPos().position - rigid.position).normalized * moveSpeed;
        }
    }

    private void Update()
    {
        UpdateMoveSpeed();

        if(Vector3.Distance(this.transform.position, player.GetAimPos().position) < 0.2f)
        {
            if(player.GetInventory().GetWeapon(gunType) != null)
            {
                Gun temp = player.GetInventory().GetWeapon(gunType);
                temp.SetMaxAmmoCount(temp.GetMaxAmmoCount() + temp.GetMaxAmmo_aMagCount());
                Destroy(this.gameObject);
            }
        }
        else
        {
            //if (player.GetInventory().GetWeapon(gunType) != null)
            //{
            //    this.transform.position = Vector3.Lerp(this.transform.position, player.GetAimPos().position, Time.deltaTime * 8);
            //}
        }
    }
}

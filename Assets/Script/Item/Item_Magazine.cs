using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Magazine : Item
{
    [SerializeField] private GunType gunType;
    [SerializeField] private int addAmmoNum;

    public GunType GetMagType()
    {
        return gunType;
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        if (player.GetInventory().GetWeapon(gunType) != null)
        {
            if (Vector3.Distance(this.transform.position, player.transform.position) <= 7.0f)
            {
                if (player.GetInventory().GetWeapon(gunType).GetCurrentAmmoCount() < player.GetInventory().GetWeapon(gunType).GetMaxAmmo_aMagCount() || player.GetInventory().GetWeapon(gunType).GetHaveAmmoCount() < player.GetInventory().GetWeapon(gunType).GetMaxAmmoCount())
                {
                    UpdateMoveSpeed();
                    this.GetComponent<Collider>().isTrigger = true;
                    //rigid.position = Vector3.Lerp(rigid.position, player.GetAimPos().position, Time.deltaTime * 12);
                    rigid.velocity = (player.GetAimPos().position - rigid.position).normalized * moveSpeed;
                }
                else
                {
                   //moveSpeed = 0;

                    if(this.GetComponent<Collider>().isTrigger)
                    rigid.velocity = Vector3.Lerp(rigid.velocity, Vector3.zero, Time.deltaTime * 15);
                }
            }
            else
            {
                //moveSpeed = 0;
                //rigid.velocity = Vector3.Lerp(rigid.velocity, Vector3.zero, Time.deltaTime * 15);
            }
        }

    }

    private void Update()
    {
        if (Vector3.Distance(this.transform.position, player.GetAimPos().position) < 0.2f)
        {
            if (player.GetInventory().GetWeapon(gunType) != null)
            {
                    Gun temp = player.GetInventory().GetWeapon(gunType);
                if (temp.GetCurrentAmmoCount() < temp.GetMaxAmmo_aMagCount() || temp.GetHaveAmmoCount() < temp.GetMaxAmmoCount())
                {
                    GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.GetMag);
                    
                    //temp.SetHaveAmmoCount(temp.GetMaxAmmoCount() + temp.GetMaxAmmo_aMagCount());

                    int addNum = (temp.GetMaxAmmo_aMagCount() - temp.GetHaveAmmoCount()) + (temp.GetMaxAmmo_aMagCount() - temp.GetCurrentAmmoCount());
                    addNum = Mathf.Clamp(addNum, 0, temp.GetMaxAmmo_aMagCount());
                    temp.AddAmmo(addNum);

                    ResetInfo();
                    this.gameObject.SetActive(false);
                }
            }
        }

    }
}

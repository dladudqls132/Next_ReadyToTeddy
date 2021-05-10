using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimController : MonoBehaviour
{
    [SerializeField] private Gun currentWeapon;
    [SerializeField] private PlayerController player;

    [SerializeField] private UnityEngine.Animations.Rigging.Rig handIK;
    [SerializeField] public Transform weaponLeftGrip;
    [SerializeField] public Transform weaponRightGrip;

    Animator anim;

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();

        anim = this.GetComponent<Animator>();
    }


    public void SetIsReloadFinish()
    {
        GameManager.Instance.GetPlayer().GetGun().SetIsReloadFinish();
    }

    public void SetSwapGun()
    {
        if(player.SwapWeapon())
        {
            //overrides["weapon_anim_empty"] = GameManager.Instance.GetPlayer().GetWeaponGameObject().GetComponent<Gun>().weaponAnimation;
            if (player.GetWeaponGameObject().GetComponent<Gun>() != null)
            {
                if (player.GetWeaponGameObject().GetComponent<Gun>().GetGunType() == GunType.AR)
                {
                
                    anim.SetBool("Hand_AR", true);
                    anim.SetBool("Hand_Shotgun", false);
                    anim.SetBool("Hand_Empty", false);
                }
                else if (player.GetWeaponGameObject().GetComponent<Gun>().GetGunType() == GunType.ShotGun)
                {
           
                    anim.SetBool("Hand_AR", false);
                    anim.SetBool("Hand_Shotgun", true);
                    anim.SetBool("Hand_Empty", false);
                }
            }
            else
            {
                anim.SetBool("Hand_AR", false);
                anim.SetBool("Hand_Shotgun", false);
                anim.SetBool("Hand_Empty", true);
            }

            handIK.weight = 1;
            anim.SetLayerWeight(1, 1.0f);
        }
        else
        {
            Debug.Log("asd");
            anim.SetBool("Hand_AR", false);
            anim.SetBool("Hand_Shotgun", false);
            anim.SetBool("Hand_Empty", true);
            //handIK.weight = 0;
            // anim.SetLayerWeight(1, 0.0f);
        }
        this.GetComponent<Animator>().SetBool("isSwap", false);
    }

    public void SetIsSwapFinish()
    {

        this.GetComponent<Animator>().SetBool("isSwap", false);
        player.SetIsSwap(false);
    }
}

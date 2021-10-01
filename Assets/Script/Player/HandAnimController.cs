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
    private float horizontal;


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
                    anim.SetBool("Hand_SG", false);
                    anim.SetBool("Hand_CL", false);
                    anim.SetBool("Hand_FT", false);
                    anim.SetBool("Hand_Empty", false);
                }
                else if (player.GetWeaponGameObject().GetComponent<Gun>().GetGunType() == GunType.ShotGun)
                {
           
                    anim.SetBool("Hand_AR", false);
                    anim.SetBool("Hand_SG", true);
                    anim.SetBool("Hand_CL", false);
                    anim.SetBool("Hand_FT", false);
                    anim.SetBool("Hand_Empty", false);
                }
                else if (player.GetWeaponGameObject().GetComponent<Gun>().GetGunType() == GunType.ChainLightning)
                {
                    anim.SetBool("Hand_AR", false);
                    anim.SetBool("Hand_SG", false);
                    anim.SetBool("Hand_CL", true);
                    anim.SetBool("Hand_FT", false);
                    anim.SetBool("Hand_Empty", false);
                }
                else if (player.GetWeaponGameObject().GetComponent<Gun>().GetGunType() == GunType.Flamethrower)
                {
                    anim.SetBool("Hand_AR", false);
                    anim.SetBool("Hand_SG", false);
                    anim.SetBool("Hand_CL", false);
                    anim.SetBool("Hand_FT", true);
                    anim.SetBool("Hand_Empty", false);
                }
            }
            else
            {
                anim.SetBool("Hand_AR", false);
                anim.SetBool("Hand_SG", false);
                anim.SetBool("Hand_CL", false);
                anim.SetBool("Hand_FT", false);
                anim.SetBool("Hand_Empty", true);
            }

            handIK.weight = 1;
            anim.SetLayerWeight(1, 1.0f);
        }
        else
        {
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

    Transform parent_mag;
    Vector3 originalPos_mag;
    Quaternion originalRot_mag;

    public void ResetMag()
    {
        player.GetGun().mag.GetComponent<Weapon_Magazine>().ResetInfo();
    }

    void Reload_AR_1()
    {
        GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.AutoRifle_Reload_1);
        originalPos_mag = player.GetGun().mag.localPosition;
        originalRot_mag = player.GetGun().mag.localRotation;
        parent_mag = player.GetGun().mag.parent;
        player.GetGun().mag.parent = weaponLeftGrip;
    }

    void Reload_AR_2()
    {
        GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.AutoRifle_Reload_2);
        ResetMag();
    }

    void LoadMag()
    {
        GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Gun_load);
    }

    public void Pakour_End()
    {
        anim.SetBool("isPakour", false);
        //Debug.Log("asd");
    }

    void FootStep()
    {
        GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Walk);
    }

    bool temp;
    private void Update()
    {
        if (player.GetIsClimbUp() && !player.GetGun().GetIsReload() && !player.GetIsSwap())
        {
            if(!temp)
            anim.SetBool("isPakour", true);
            temp = true;
        }
        else
        {
            temp = false;
        }

        horizontal = Mathf.Lerp(horizontal, GameManager.Instance.GetPlayer().moveInput.x, Time.deltaTime * 20);
        anim.SetFloat("horizontal", horizontal);
    }
}

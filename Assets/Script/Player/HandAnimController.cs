using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;


public class HandAnimController : MonoBehaviour
{
    [SerializeField] private Gun currentWeapon;
    [SerializeField] private PlayerController player;

    [SerializeField] private UnityEngine.Animations.Rigging.Rig handIK;
    [SerializeField] private Transform weaponLeftGrip;
    [SerializeField] private Transform weaponRightGrip;

    Animator anim;
    AnimatorOverrideController overrides;

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();

        anim = this.GetComponent<Animator>();
        overrides = anim.runtimeAnimatorController as AnimatorOverrideController;
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

    [ContextMenu("Save weapon pose")]
    void SaveWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);

        //recorder.BindComponentsOfType<Transform>(this.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);

        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(player.GetWeaponGameObject().GetComponent<Gun>().weaponAnimation);
        //recorder.SaveToClip(temp);
    }
}

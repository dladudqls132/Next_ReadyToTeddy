using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerController player;
    private Animator anim;
    private Vector3 originPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.GetPlayer();
        anim = this.GetComponent<Animator>();
        originPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetGun() != null)
        {
            if(player.GetGun().GetGunType() == GunType.AR)
            {
                this.transform.localPosition = originPos;
                anim.SetBool("AR", true);
                anim.SetBool("SG", false);
                anim.SetBool("CL", false);
                anim.SetBool("FT", false);
                anim.SetBool("SN", false);
            }
            else if(player.GetGun().GetGunType() == GunType.ShotGun)
            {
                this.transform.localPosition = new Vector3(0.017f, -1.474f, -0.144f);
                anim.SetBool("AR", false);
                anim.SetBool("SG", true);
                anim.SetBool("CL", false);
                anim.SetBool("FT", false);
                anim.SetBool("SN", false);
            }
            else if(player.GetGun().GetGunType() == GunType.ChainLightning)
            {
                this.transform.localPosition = originPos;
                anim.SetBool("AR", false);
                anim.SetBool("SG", false);
                anim.SetBool("CL", true);
                anim.SetBool("FT", false);
                anim.SetBool("SN", false);
            }
            else if(player.GetGun().GetGunType() == GunType.Flamethrower)
            {
                this.transform.localPosition = new Vector3(0.017f, -1.474f, -0.194f);
                anim.SetBool("AR", false);
                anim.SetBool("SG", false);
                anim.SetBool("CL", false);
                anim.SetBool("FT", true);
                anim.SetBool("SN", false);
            }
            else if (player.GetGun().GetGunType() == GunType.Sniper)
            {
                this.transform.localPosition = originPos;
                anim.SetBool("AR", false);
                anim.SetBool("SG", false);
                anim.SetBool("CL", false);
                anim.SetBool("FT", false);
                anim.SetBool("SN", true);
            }

            anim.SetBool("isReload", player.GetGun().GetIsReload());
        }
        else
        {
            anim.SetBool("isReload", false);
        }


    }
}

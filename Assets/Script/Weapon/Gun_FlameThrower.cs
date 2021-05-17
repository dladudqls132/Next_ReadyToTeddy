using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_FlameThrower : Gun
{
    [SerializeField] private ParticleSystem fire;
    private bool isFire;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckingParent();

        if (owner != null)
        {
            if (isShot)
            {
                currentShotDelay -= Time.deltaTime;

                if (currentShotDelay <= 0)
                {
                    currentAmmo--;
                    currentShotDelay = shotDelay;
                }
            }

            //if (isShot)
            //{
            //    currentShotDelay -= Time.deltaTime;

            //    if (currentShotDelay <= 0)
            //    {
            //        isShot = false;
            //        isRecoil = false;
            //        //Debug.Log("asd");
            //        currentShotDelay = shotDelay;
            //    }

            //    if (currentShotDelay <= shotDelay / 1.6f)
            //    {
            //        isRecoil = true;
            //    }
            //}
            //else
            //{
            //    //fire.Stop();
            //}

            //if (isReload)
            //{
            //    currentReloadTime -= Time.deltaTime;

            //    if (currentReloadTime <= 0)
            //    {
            //        currentReloadTime = reloadTime;
            //        currentAmmo = maxAmmo;

            //        if (currentAmmo >= maxAmmo)
            //        {
            //            isReload = false;
            //        }
            //    }
            //}
            //else
            //{
            //    currentReloadTime = reloadTime;
            //}

            if (CanReload() && currentAmmo <= 0 && !GameManager.Instance.GetPlayer().GetIsSwap())
            {
                SetIsReload(true);
            }

            if (currentAmmo > 0)
                canShot = true;
            else
            {
                canShot = false;
            }
        }
        else
        {
            ResetInfo();
        }
    }

    public override void SetIsReload(bool value)
    {
        base.SetIsReload(value);

        Off();
        hand.GetComponent<Animator>().SetBool("isReload_FT", value);
    }

    public override void SetIsReloadFinish()
    {
        base.SetIsReloadFinish();

        hand.GetComponent<Animator>().SetBool("isReload_FT", false);
    }

    public void Off()
    {
        isShot = false;
        fire.Stop();
    }

    private void OnDisable()
    {
        Off();
    }

    override public bool Fire()
    {
        if (isReload)
            return false;

        if (canShot)
        {
            if (!owner.GetComponent<PlayerController>().GetIsAiming())
            {
                currentSpreadAngle = spreadAngle_normal;
                mainCam.Shake(0.02f, 0.015f);
                //handFireRot = mainCam.SetFireRecoilRot(new Vector3(2.0f, 1.5f, 0), 15.0f, 3.0f);
                //handFireRot = mainCam.SetFireRecoilRot(recoil, 15.0f, 3.0f);
            }
            else
            {
                currentSpreadAngle = spreadAngle_aiming;
                mainCam.Shake(0.02f, 0.015f);
                //handFireRot = mainCam.SetFireRecoilRot(new Vector3(1.0f, 1.0f, 0), 10.0f, 3.0f);
                //handFireRot = mainCam.SetFireRecoilRot(recoil / 4, 15.0f, 3.0f);
            }

            hand.GetComponent<Animator>().SetTrigger("Fire_FT");

            isReload = false;
            isRecoil = false;
            
            fire.transform.SetParent(null);
            fire.transform.position = shotPos.position;
            fire.transform.rotation = Camera.main.transform.rotation;

            //muzzleFlash.Play();

            if (!isShot)
            {
                fire.Play();
            }
            isShot = true;

            return true;
        }


        return false;
    }

    override public void ResetInfo()
    {
        base.ResetInfo();
    }
}

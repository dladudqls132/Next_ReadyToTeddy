using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_ChainLightning : Gun
{
    Vector3 temp;
    bool isCharge;
    [SerializeField] float intensify = 0;
    [SerializeField] MeshRenderer mr;
    [SerializeField] private ParticleSystem spark;
    [SerializeField] private ParticleSystem shotEffect;

    private float chargeTick = 0.1f;
    private float currentChargeTick;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        //audioSource.volume = GameManager.Instance.GetSettings().data.mainVolume;
        UI_gunsound = GameManager.Instance.GetGunSoundManager().GetGunSound(GunType.ChainLightning);
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
                    isShot = false;
                    isRecoil = false;
                    //Debug.Log("asd");
                    currentShotDelay = shotDelay;
                }

                if (currentShotDelay <= shotDelay / 1.6f)
                {
                    isRecoil = true;
                }
            }

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

            if (isCharge)
            {
                intensify = Mathf.Lerp(intensify, 2, Time.deltaTime / 1.5f);
                mr.material.SetFloat("_Intensity", intensify);

                if (!this.GetComponent<AudioSource>().isPlaying)
                    this.GetComponent<AudioSource>().Play();
            }
            else
            {
                intensify = Mathf.Lerp(intensify, 0, Time.deltaTime * 4);
                mr.material.SetFloat("_Intensity", intensify);
                if (this.GetComponent<AudioSource>().isPlaying)
                    this.GetComponent<AudioSource>().Stop();
            }

            shotEffect.transform.position = mainCam.transform.position + mainCam.transform.forward * 2.5f;
            shotEffect.transform.forward = mainCam.transform.forward;
            GameManager.Instance.GetCrosshairController().GetCrosshair(gunType).GetComponent<UI_Crosshair_CL>().SetCharging(Mathf.Clamp((int)(intensify / 0.3f), 0, 4));

            if (CanReload() && currentAmmo <= 0 && !GameManager.Instance.GetPlayer().GetIsSwap())
            {
                SetIsReload(true);
            }

            if (currentAmmo > 0 && !isShot)
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

        hand.GetComponent<Animator>().SetBool("isReload_CL", value);

        this.GetComponent<Animator>().SetBool("isReload", value);

        UI_gunsound.DisplayImage_Reload();
    }

    public override void SetIsReloadFinish()
    {
        base.SetIsReloadFinish();

        this.GetComponent<Animator>().SetBool("isReload", false);
        hand.GetComponent<Animator>().SetBool("isReload_CL", false);
    }

    public bool Charging()
    {
        if (isReload)
            return false;

        if (canShot)
        {

            currentSpreadAngle = spreadAngle_normal;
            mainCam.Shake(0.01f, 0.03f, false);
            handFireRot = Vector3.zero;
            //handFireRot = mainCam.SetFireRecoilRot(new Vector3(2.0f, 1.5f, 0), 15.0f, 3.0f);
            //handFireRot = mainCam.SetFireRecoilRot(recoil, 5.0f, 5.0f);

            if (currentChargeTick >= chargeTick)
            {
                if (Mathf.Clamp((int)(intensify / 0.3f), 0, 4) != 4)
                    UI_gunsound.DisplayImage_Charge1();
                else
                    UI_gunsound.DisplayImage_Charge2();

                currentChargeTick = 0;
            }
            else
            {
                currentChargeTick += Time.deltaTime;
            }

            if (spark.isStopped)
                spark.Play();

            isCharge = true;
            isReload = false;
            isRecoil = false;

            return true;
        }

        return false;
    }

    public void ResetValue()
    {
        spark.Stop();
        intensify = 0;
        isCharge = false;
    }

    private void LateUpdate()
    {
        temp = shotPos.position;
    }

    override public bool Fire()
    {
        if (isReload)
            return false;

        if (canShot)
        {
            owner.GetComponent<Rigidbody>().AddForce(-owner.transform.forward * 50, ForceMode.Impulse);
            currentSpreadAngle = spreadAngle_normal;
            mainCam.FovMove(78, 0.05f, 0.23f, 0.4f);
            mainCam.Shake(0.6f, 0.07f, false);
            //handFireRot = mainCam.SetFireRecoilRot(new Vector3(2.0f, 1.5f, 0), 15.0f, 3.0f);
            handFireRot = mainCam.SetFireRecoilRot(recoil, 3.0f, 3.0f);

            UI_gunsound.DisplayImage_Attack();

            hand.GetComponent<Animator>().SetTrigger("Fire_CL");

            GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.EnergyGun_Fire);

            if (spark.isPlaying)
                spark.Stop();

            shotEffect.Play();

            isCharge = false;
            isReload = false;
            isRecoil = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Bullet tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.CL).GetComponent<Bullet>();

            float tempIntensify = (intensify / 1.3f);
            tempIntensify = Mathf.Clamp(tempIntensify, 0, 1.0f);
            tempBullet.SetFire(temp, ray.direction, 70, damagePerBullet, 0.5f + stunTime * (Mathf.Clamp(intensify / 0.3f, 0, 4) / 4));
            //tempBullet.gameObject.SetActive(true);

            isShot = true;

            currentAmmo--;

            return true;
        }

        return false;
    }

    override public void ResetInfo()
    {
        base.ResetInfo();

        isCharge = false;

    }
}

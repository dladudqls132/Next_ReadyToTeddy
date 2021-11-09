using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_FlameThrower : Gun
{
    [SerializeField] private ParticleSystem fire;
    [SerializeField] private List<float> originSpeed_min = new List<float>();
    [SerializeField] private List<float> originSpeed_max = new List<float>();
    //[SerializeField] private float refillTime;
    private float currentRefillTime;
    [SerializeField] private float fireSoundRate;
    private float currentFireSoundRate;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        fire.GetComponentInChildren<Fire>().damage = damagePerBullet;

        for (int i = 0; i < fire.transform.childCount; i++)
        {
            originSpeed_min.Add(fire.transform.GetChild(i).GetComponent<ParticleSystem>().main.startSpeed.constantMin);
            originSpeed_max.Add(fire.transform.GetChild(i).GetComponent<ParticleSystem>().main.startSpeed.constantMax);
        }

        UI_gunsound = GameManager.Instance.GetGunSoundManager().GetGunSound(GunType.Flamethrower);
    }

    // Update is called once per frame
    void Update()
    {
        currentFireSoundRate += Time.deltaTime;
        //CheckingParent();

        if (owner != null)
        {
            if (isShot)
            {
                currentRefillTime = 0;
                currentShotDelay -= Time.deltaTime;

                if (currentShotDelay <= 0)
                {
                    currentAmmo--;


                    GameManager.Instance.GetCrosshairController().GetCrosshair(gunType).GetComponent<UI_Crosshair_FT>().SetGauge((float)currentAmmo / (float)maxAmmo_aMag);
                    currentShotDelay = shotDelay;
                }
            }

            if (currentAmmo > 0)
                canShot = true;
            else
            {
                Off();
                canShot = false;
            }
        }
        else
        {
            ResetInfo();
        }
    }

    public void RefillAmmo()
    {
        if (!isShot)
        {
            currentRefillTime += Time.deltaTime;

            if (currentRefillTime >= refillTime)
            {
                currentRefillTime = 0;

                if (currentAmmo < maxAmmo_aMag)
                {
                    currentAmmo++;

                    if (this.gameObject.activeSelf)
                        UI_gunsound.DisplayImage_Reload();
                }

                GameManager.Instance.GetCrosshairController().GetCrosshair(gunType).GetComponent<UI_Crosshair_FT>().SetGauge((float)currentAmmo / (float)maxAmmo_aMag);
            }
        }
    }

    public override void SetIsReload(bool value)
    {
        //base.SetIsReload(value);

        Off();
        //hand.GetComponent<Animator>().SetBool("isReload_FT", value);
    }

    public override void SetIsReloadFinish()
    {
        base.SetIsReloadFinish();

        hand.GetComponent<Animator>().SetBool("isReload_FT", false);
    }

    public void Off()
    {
        isShot = false;
        if (fire != null)
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

            currentSpreadAngle = spreadAngle_normal;
            mainCam.Shake(0.1f, 0.03f, false);
            //handFireRot = mainCam.SetFireRecoilRot(new Vector3(2.0f, 1.5f, 0), 15.0f, 3.0f);
            handFireRot = mainCam.SetFireRecoilRot(recoil, 3.0f, 3.0f);

            if (currentFireSoundRate >= fireSoundRate)
            {
                currentFireSoundRate = 0;

                GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.FlameThrower_Fire);

                UI_gunsound.DisplayImage_Attack();
            }

            isReload = false;
            isRecoil = false;

            if (fire.transform.parent != null)
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

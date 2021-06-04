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
                    currentShotDelay = shotDelay;
                }
            }
            //else
            //{
            //    currentRefillTime += Time.deltaTime;

            //    if(currentRefillTime >= refillTime)
            //    {
            //        currentRefillTime = 0;

            //        if(currentAmmo < maxAmmo_aMag)
            //            currentAmmo++;
            //    }
            //}

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

            //if (CanReload() && currentAmmo <= 0 && !GameManager.Instance.GetPlayer().GetIsSwap())
            //{
            //    SetIsReload(true);
            //}

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
                    currentAmmo++;
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
        if(fire != null)
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
                mainCam.Shake(0.1f, 0.015f, false);
                //handFireRot = mainCam.SetFireRecoilRot(new Vector3(2.0f, 1.5f, 0), 15.0f, 3.0f);
                handFireRot = mainCam.SetFireRecoilRot(recoil, 3.0f, 3.0f);
            }
            else
            {
                currentSpreadAngle = spreadAngle_aiming;
                mainCam.Shake(0.05f, 0.015f, false);
                //handFireRot = mainCam.SetFireRecoilRot(new Vector3(1.0f, 1.0f, 0), 10.0f, 3.0f);
                //handFireRot = mainCam.SetFireRecoilRot(recoil / 4, 15.0f, 3.0f);
            }

            //hand.GetComponent<Animator>().SetTrigger("Fire_FT");

            if(currentFireSoundRate >= fireSoundRate)
            {
                currentFireSoundRate = 0;
                
                GameManager.Instance.GetSoundManager().AudioPlayOneShot(AudioSourceType.SFX, SoundType.FlameThrower_Fire);
            }



            isReload = false;
            isRecoil = false;
            
            if(fire.transform.parent != null)
                fire.transform.SetParent(null);

            //if (GameManager.Instance.GetPlayer().moveInput.y > 0)
            //{
            //    for (int i = 0; i < fire.transform.childCount; i++)
            //    {
            //        float rnd = Random.Range(originSpeed_min[i], originSpeed_max[i]);
            //        var main = fire.transform.GetChild(i).GetComponent<ParticleSystem>().main;
            //        main.startSpeed = rnd + GameManager.Instance.GetPlayer().GetWalkSpeed();
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < fire.transform.childCount; i++)
            //    {
            //        float rnd = Random.Range(originSpeed_min[i], originSpeed_max[i]);
            //        var main = fire.transform.GetChild(i).GetComponent<ParticleSystem>().main;
            //        main.startSpeed = rnd;
            //    }
            //}

            //var main = fire.main;
            //main.startSpeed = 3;

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

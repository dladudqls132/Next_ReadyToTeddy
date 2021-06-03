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

            if(isCharge)
            {
                intensify = Mathf.Lerp(intensify, 3, Time.deltaTime * 4);
                mr.material.SetFloat("_Intensity", intensify);
            }
            else
            {
                intensify = Mathf.Lerp(intensify, 0, Time.deltaTime * 4);
                mr.material.SetFloat("_Intensity", intensify);
            }

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
            if (!owner.GetComponent<PlayerController>().GetIsAiming())
            {
                currentSpreadAngle = spreadAngle_normal;
                mainCam.Shake(0.01f, 0.03f, false);

                //handFireRot = mainCam.SetFireRecoilRot(new Vector3(2.0f, 1.5f, 0), 15.0f, 3.0f);
                //handFireRot = mainCam.SetFireRecoilRot(recoil, 5.0f, 5.0f);
            }
            else
            {
                currentSpreadAngle = spreadAngle_aiming;
                mainCam.Shake(0.02f, 0.015f, false);
                //handFireRot = mainCam.SetFireRecoilRot(new Vector3(1.0f, 1.0f, 0), 10.0f, 3.0f);
                //handFireRot = mainCam.SetFireRecoilRot(recoil / 4, 15.0f, 3.0f);
            }

            if(spark.isStopped)
                spark.Play();

            isCharge = true;
            isReload = false;
            isRecoil = false;

            return true;
        }

        return false;
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
            if (!owner.GetComponent<PlayerController>().GetIsAiming())
            {
                currentSpreadAngle = spreadAngle_normal;
                mainCam.FovMove(78, 0.05f, 0.23f, 0.4f);
                mainCam.Shake(0.6f, 0.07f, false);
                //handFireRot = mainCam.SetFireRecoilRot(new Vector3(2.0f, 1.5f, 0), 15.0f, 3.0f);
                //handFireRot = mainCam.SetFireRecoilRot(recoil, 3.0f, 3.0f);
            }
            else
            {
                currentSpreadAngle = spreadAngle_aiming;
                mainCam.Shake(0.02f, 0.015f, false);
                //handFireRot = mainCam.SetFireRecoilRot(new Vector3(1.0f, 1.0f, 0), 10.0f, 3.0f);
                //handFireRot = mainCam.SetFireRecoilRot(recoil / 4, 15.0f, 3.0f);
            }

            hand.GetComponent<Animator>().SetTrigger("Fire_CL");

            if (spark.isPlaying)
                spark.Stop();

            isCharge = false;
            isReload = false;
            isRecoil = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;

            //if (Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Enemy"))))
            //{
            //    direction = (hit.point - Camera.main.transform.position).normalized;
            //}
            //else
            //    direction = Camera.main.transform.forward;

            Bullet tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.CL).GetComponent<Bullet>();
            tempBullet.gameObject.SetActive(true);
            tempBullet.SetFire(temp, ray.direction, 60, damagePerBullet, stunTime);

            //float temp = Random.Range(-Mathf.PI, Mathf.PI);

            //Vector3 shotDir = direction + (Camera.main.transform.up * Mathf.Sin(temp) + Camera.main.transform.right * Mathf.Cos(temp)) * Random.Range(0.0f, currentSpreadAngle / 180);

            ////Debug.DrawRay(shotPos.position, shotDir * 1000);

            //RaycastHit hit2;
            //if (Physics.Raycast(Camera.main.transform.position, shotDir, out hit2, Mathf.Infinity, (1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Enemy")), QueryTriggerInteraction.Ignore))
            //{
            //    if (LayerMask.LayerToName(hit2.transform.gameObject.layer).Equals("Enemy"))
            //    {
            //        Enemy enemy = hit2.transform.root.GetComponent<Enemy>();

            //        if (!hit2.transform.CompareTag("Head"))
            //        {
            //            enemy.DecreaseHp(owner, damagePerBullet, hit2.point, hit2.transform, Vector3.ClampMagnitude(ray.direction * 20, 20));
            //        }
            //        else
            //        {
            //            enemy.DecreaseHp(owner, damagePerBullet * 2, hit2.point, hit2.transform, Vector3.ClampMagnitude(ray.direction * 5, 5));
            //        }

            //        GameManager.Instance.GetCrosshair().ResetAttack();
            //        if (enemy.GetIsDead())
            //            GameManager.Instance.GetCrosshair().SetAttack_Kill(true);
            //        else if (!GameManager.Instance.GetCrosshair().GetIsKill())
            //            GameManager.Instance.GetCrosshair().SetAttack_Normal(true);
            //    }
            //    else if (hit.transform.CompareTag("InteractiveObject"))
            //    {
            //        hit.transform.GetComponent<InteractiveObject>().DecreaseHp(damagePerBullet);
            //    }
            //    else
            //    {
            //        GameObject tempObect = GameManager.Instance.GetPoolBulletHit().GetBulletHit(BulletHitType.Normal);
            //        tempObect.transform.SetParent(null);
            //        tempObect.transform.localScale = new Vector3(0.1f, 0.1f, 0.0018857f);
            //        tempObect.transform.position = hit2.point;
            //        tempObect.transform.rotation = Quaternion.LookRotation(hit2.normal);
            //        tempObect.transform.SetParent(hit2.transform, true);
            //        tempObect.SetActive(true);
            //    }
            //}

            //muzzleFlash.Play();
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

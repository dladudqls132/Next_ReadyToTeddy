using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_AR : Gun
{

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

        hand.GetComponent<Animator>().SetBool("isReload_AR", value);
    }

    public override void SetIsReloadFinish()
    {
        base.SetIsReloadFinish();

        hand.GetComponent<Animator>().SetBool("isReload_AR", false);
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
                mainCam.Shake(0.02f, 0.03f, false);
                //handFireRot = mainCam.SetFireRecoilRot(new Vector3(2.0f, 1.5f, 0), 15.0f, 3.0f);
                handFireRot = mainCam.SetFireRecoilRot(recoil, 60.0f, 3.0f);
            }
            else
            {
                currentSpreadAngle = spreadAngle_aiming;
                mainCam.Shake(0.02f, 0.015f, false);
                //handFireRot = mainCam.SetFireRecoilRot(new Vector3(1.0f, 1.0f, 0), 10.0f, 3.0f);
                handFireRot = mainCam.SetFireRecoilRot(recoil / 2, 30.0f, 3.0f);
            }

            GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.AutoRifle_Fire);
            hand.GetComponent<Animator>().SetTrigger("Fire_AR");

            isReload = false;
            isRecoil = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Enemy"))))
            {
                direction = (hit.point - Camera.main.transform.position).normalized;
            }
            else
                direction = Camera.main.transform.forward;

            float temp = Random.Range(-Mathf.PI, Mathf.PI);

            Vector3 shotDir = direction + (Camera.main.transform.up * Mathf.Sin(temp) + Camera.main.transform.right * Mathf.Cos(temp)) * Random.Range(0.0f, currentSpreadAngle / 180);
            //Debug.DrawRay(Camera.main.transform.position, shotDir * 100, Color.red);
            //Debug.DrawRay(shotPos.position, shotDir * 1000);
    
            RaycastHit hit2;
            if (Physics.Raycast(Camera.main.transform.position, shotDir, out hit2, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Weapon")),QueryTriggerInteraction.Ignore))
            {
                if (LayerMask.LayerToName(hit2.transform.gameObject.layer).Equals("Enemy"))
                {
                    Enemy enemy = hit2.transform.GetComponent<Enemy_Bone>().root.GetComponent<Enemy>();

                    //audioSource.PlayOneShot(GameManager.Instance.GetSoundInfo().GetInfo(SoundType.Hit).clip, GameManager.Instance.GetSoundInfo().GetInfo(SoundType.Hit).volume * GameManager.Instance.GetSettings().data.effectVolume);
                    GameManager.Instance.GetCrosshair().ResetAttack();
                    if (!hit2.transform.CompareTag("Head"))
                    {
                        GameManager.Instance.GetCrosshair().SetAttack_Normal(true);
                        enemy.DecreaseHp(/*owner, */damagePerBullet, hit2.point, hit2.transform, Vector3.ClampMagnitude(ray.direction * 20, 20), EffectType.Damaged_normal);
                        GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Hit);
                    }
                    else
                    {
                        GameManager.Instance.GetCrosshair().SetAttack_Kill(true);
                        enemy.DecreaseHp(/*owner, */damagePerBullet * 2, hit2.point, hit2.transform, Vector3.ClampMagnitude(ray.direction * 5, 5), EffectType.Damaged_normal);
                        GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.WeaknessHit);
                    }

                    if (enemy.GetIsDead())
                    {
                        GameManager.Instance.GetCrosshair().ResetAttack();
                        GameManager.Instance.GetCrosshair().SetAttack_Kill(true);
                    }
                    //else if (!GameManager.Instance.GetCrosshair().GetIsKill())
                    //    GameManager.Instance.GetCrosshair().SetAttack_Normal(true);
                }
                else if (hit2.transform.CompareTag("InteractiveObject"))
                {
                    hit2.transform.GetComponent<InteractiveObject>().DecreaseHp(damagePerBullet);
                }
                else
                {
                    GameObject tempObect = GameManager.Instance.GetPoolBulletHit().GetBulletHit(BulletHitType.Normal);
                    tempObect.transform.SetParent(null);
                    tempObect.transform.localScale = new Vector3(0.1f, 0.1f, 0.0018857f);
                    tempObect.transform.position = hit2.point;
                    tempObect.transform.rotation = Quaternion.LookRotation(hit2.normal);
                    tempObect.transform.SetParent(hit2.transform, true);
                    tempObect.SetActive(true);
                }

            }
            

            muzzleFlash.Play();
            isShot = true;

            currentAmmo--;

            return true;
        }

        return false;
    }

    override public void ResetInfo()
    {
        base.ResetInfo();
    }
}

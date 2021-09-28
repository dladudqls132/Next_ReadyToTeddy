using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Test : Gun
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

            if (CanReload() && currentAmmo <= 0 && !GameManager.Instance.GetPlayer().GetIsSwap() && hand.GetComponent<Animator>().GetBool("isIdle"))
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
            canShot = false;
            isReload = false;
            currentReloadTime = reloadTime;
            currentShotDelay = shotDelay;
            isShot = false;
        }
    }

    public override void SetIsReload(bool value)
    {
        base.SetIsReload(value);

        hand.GetComponent<Animator>().SetBool("isReload_SG", value);
    }

    public override void SetIsReloadFinish()
    {
        base.SetIsReloadFinish();

        hand.GetComponent<Animator>().SetBool("isReload_SG", false);
    }

    override public bool Fire()
    {
        if (isReload)
            return false;

        if (canShot)
        {
            
                currentSpreadAngle = spreadAngle_normal;
                mainCam.Shake(0.05f, 0.06f, false);
                //handFireRot = mainCam.SetFireRecoilRot(new Vector3(8.0f, 2.5f, 0.0f), 30.0f, 20.0f);
                handFireRot = mainCam.SetFireRecoilRot(recoil, 30.0f, 20.0f);
            
          

            GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.ShotGun_Fire);

            hand.GetComponent<Animator>().SetTrigger("Fire_SG");

            isReload = false;
            isRecoil = false;

            bool isHit = false;
            bool headShot = false;
            bool isDead = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Enemy"))))
            {
                direction = (hit.point - Camera.main.transform.position).normalized;
            }
            else
                direction = Camera.main.transform.forward;

                bool checkingDead = false;
            for (int i = 0; i < fireNum; i++)
            {
                float temp = Random.Range(-Mathf.PI, Mathf.PI);

                Vector3 shotDir = direction + (Camera.main.transform.up * Mathf.Sin(temp) + Camera.main.transform.right * Mathf.Cos(temp)) * Random.Range(0.0f, currentSpreadAngle / 180);

                //Debug.DrawRay(shotPos.position, shotDir * 1000);

                RaycastHit hit2;
                if (Physics.Raycast(Camera.main.transform.position, shotDir, out hit2, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Weapon")), QueryTriggerInteraction.Ignore))
                {
                    if (LayerMask.LayerToName(hit2.transform.gameObject.layer).Equals("Enemy"))
                    {
                        Enemy enemy = hit2.transform.GetComponent<Enemy_Bone>().root.GetComponent<Enemy>();

                        //audioSource.PlayOneShot(GameManager.Instance.GetSoundInfo().GetInfo(SoundType.Hit).clip, GameManager.Instance.GetSoundInfo().GetInfo(SoundType.Hit).volume * GameManager.Instance.GetSettings().data.effectVolume);

                        if (!hit2.transform.CompareTag("Head"))
                        {
                            if(enemy.GetEnemyType() != EnemyType.D)
                                enemy.DecreaseHp(damagePerBullet, hit2.point, hit2.transform, Vector3.ClampMagnitude(ray.direction * 70, 70), EffectType.Damaged_normal);
                            else
                                enemy.DecreaseHp(1, hit2.point, hit2.transform, Vector3.ClampMagnitude(ray.direction * 70, 70), EffectType.Damaged_normal);
                        }
                        else
                        {
                            headShot = true;
                            enemy.DecreaseHp(damagePerBullet * 2, hit2.point, hit2.transform, Vector3.ClampMagnitude(ray.direction * 70, 70), EffectType.Damaged_normal);
                        }

                        isHit = true;

                        if (enemy.GetIsDead())
                        {
                            if (!checkingDead)
                            {
                                owner.GetComponent<PlayerController>().IncreaseSpeed();
                                owner.GetComponent<PlayerController>().IncreaseHp(10);
                            }

                            checkingDead = true;
                        }
                    }
                    else if(hit2.transform.CompareTag("InteractiveObject"))
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
            }

            muzzleFlash.time = 0;
            muzzleFlash.Play();
            isShot = true;

            currentAmmo--;

            if(isHit)
            {
                GameManager.Instance.GetCrosshair().ResetAttack();

                if (headShot)
                {
                    GameManager.Instance.GetCrosshair().SetAttack_Kill(true);
                    GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.WeaknessHit);
                }
                else if(isDead)
                {
                    GameManager.Instance.GetCrosshair().SetAttack_Kill(true);

                    if(headShot)
                        GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.WeaknessHit);
                    else
                        GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Hit);
                }
                else
                {
                    GameManager.Instance.GetCrosshair().SetAttack_Normal(true);
                    GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Hit);
                }
            }

            return true;
        }

        return false;
    }

    override public void ResetInfo()
    {
        base.ResetInfo();
    }
}

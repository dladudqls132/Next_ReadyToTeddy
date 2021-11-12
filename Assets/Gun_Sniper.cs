using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Sniper : Gun
{
    [SerializeField] private Trail_Bullet trail;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        UI_gunsound = GameManager.Instance.GetGunSoundManager().GetGunSound(GunType.AR);
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
                    hand.GetComponent<Animator>().SetBool("isFire_SN", false);
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

            if(GameManager.Instance.GetPlayer().GetIsSwap() || hand.GetComponent<Animator>().GetBool("isPakour"))
            {
                hand.GetComponent<Animator>().SetBool("isFire_SN", false);
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

            if (!isAiming)
                currentSpreadAngle = spreadAngle_normal;
            else
                currentSpreadAngle = spreadAngle_aiming;
        }
        else
        {
            ResetInfo();
        }
    }

    public override void SetIsReload(bool value)
    {
        base.SetIsReload(value);

        hand.GetComponent<Animator>().SetBool("isReload_SN", value);
    }

    public override void SetIsReloadFinish()
    {
        base.SetIsReloadFinish();

        hand.GetComponent<Animator>().SetBool("isReload_SN", false);
    }

    override public bool Fire()
    {
        if (isReload)
            return false;

        if (canShot)
        {
            mainCam.Shake(0.02f, 0.02f, false);
            //handFireRot = mainCam.SetFireRecoilRot(recoil, 60.0f, 3.0f);
            mainCam.GetComponent<Animator>().SetTrigger("Fire_SN");

            GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.EnergyGun_Fire2);
                hand.GetComponent<Animator>().SetBool("isFire_SN", true);

            if (!isAiming)
            {
                hand.GetComponent<Animator>().SetTrigger("Fire_SN");
            }

            UI_gunsound.DisplayImage_Attack();

            isReload = false;
            isRecoil = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Weapon") | 1 << LayerMask.NameToLayer("Root"))))
            {
                direction = (hit.point - Camera.main.transform.position).normalized;
            }
            else
                direction = Camera.main.transform.forward;

            float temp = Random.Range(-Mathf.PI, Mathf.PI);

            Vector3 shotDir = direction + (Camera.main.transform.up * Mathf.Sin(temp) + Camera.main.transform.right * Mathf.Cos(temp)) * Random.Range(0.0f, currentSpreadAngle / 180);

            GameObject tempTrail = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Trail_Bullet);
            tempTrail.GetComponent<Trail_Bullet>().SetFire(shotPos.position, shotDir);

            RaycastHit hit2;
            if (Physics.Raycast(Camera.main.transform.position, shotDir, out hit2, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Weapon") | 1 << LayerMask.NameToLayer("Root")), QueryTriggerInteraction.Ignore))
            {
                if (LayerMask.LayerToName(hit2.transform.gameObject.layer).Equals("Enemy"))
                {
                    Enemy enemy = hit2.transform.GetComponent<Enemy_Bone>().root.GetComponent<Enemy>();

                    //audioSource.PlayOneShot(GameManager.Instance.GetSoundInfo().GetInfo(SoundType.Hit).clip, GameManager.Instance.GetSoundInfo().GetInfo(SoundType.Hit).volume * GameManager.Instance.GetSettings().data.effectVolume);
                    GameManager.Instance.GetCrosshairController().ResetAttack();
                    if (!hit2.transform.CompareTag("Head"))
                    {
                        GameManager.Instance.GetCrosshairController().SetAttack_Normal(true);
                 
                            enemy.DecreaseHp(damagePerBullet, hit2.point, hit2.transform, Vector3.ClampMagnitude(ray.direction * 20, 20), EffectType.Damaged_normal);

                        GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Hit);
                    }
                    else
                    {
                        GameManager.Instance.GetCrosshairController().SetAttack_Kill(true);
                        enemy.DecreaseHp(damagePerBullet * 2, hit2.point, hit2.transform, Vector3.ClampMagnitude(ray.direction * 5, 5), EffectType.Damaged_normal);
                        GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.WeaknessHit);
                    }

                    if (enemy.GetIsDead())
                    {
                        GameManager.Instance.GetCrosshairController().ResetAttack();
                        GameManager.Instance.GetCrosshairController().SetAttack_Kill(true);
                    }
                    //else if (!GameManager.Instance.GetCrosshair().GetIsKill())
                    //    GameManager.Instance.GetCrosshair().SetAttack_Normal(true);
                }
                else if (hit2.transform.CompareTag("InteractiveObject"))
                {
                    hit2.transform.GetComponentInChildren<InteractiveObject>().DecreaseHp(damagePerBullet, hit2.point, hit2.transform, Vector3.ClampMagnitude(ray.direction * 20, 20), EffectType.Damaged_normal);
                    GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Hit);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Type_C : Enemy
{
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform sandScatterPos;
    [SerializeField] private float coolTime_dodge;
    private float currentCoolTime_dodge;
    [SerializeField] private Transform mesh;
    [SerializeField] private float attackTime;
    private float currentAttackTime;
    [SerializeField] private float coolTime_attack;
    private float currentCoolTime_attack;
    [SerializeField] private float fireRate;
    private float currentFireRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform wings;
    private bool isAttack;

    private Vector3 dir;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        currentCoolTime_attack = Random.Range(0, coolTime_attack);
    }

    // Update is called once per frame
    void Update()
    {
        CheckingPlayer();

        if (isRigidity)
        {
            currentRigidityTime += Time.deltaTime;
            if (currentRigidityTime >= rigidityTime)
            {
                isRigidity = false;
                currentRigidityTime = 0;
                agent.isStopped = false;
            }
        }

        if (!isDetect || isDead || isRigidity)
        {
            return;
        }

        if (currentCoolTime_attack <= coolTime_attack / 1.75f)
        {
            foreach (Renderer r in renderers)
            {
                r.material.SetColor("_EmissionColor", Color.Lerp(r.material.GetColor("_EmissionColor"), (emissionColor_normal * 35f), Time.deltaTime * 6));
            }
        }
        else
        {
            foreach (Renderer r in renderers)
            {
                r.material.SetColor("_EmissionColor", Color.Lerp(r.material.GetColor("_EmissionColor"), (emissionColor_angry * 35f) * (currentCoolTime_attack / coolTime_attack), Time.deltaTime * 4));
            }
        }

        if (isAttack)
        {
            currentFireRate += Time.deltaTime;
            currentAttackTime += Time.deltaTime;

            if (currentAttackTime >= attackTime)
            {
                currentAttackTime = 0;
                isAttack = false;
            }
        }
        else
        {
            currentCoolTime_attack += Time.deltaTime;

            if (currentCoolTime_attack >= coolTime_attack)
            {
                currentCoolTime_attack = 0;
                isAttack = true;
            }
        }

        currentCoolTime_dodge += Time.deltaTime;

        if (anim.GetBool("isIdle"))
        {
            if (currentFireRate >= fireRate)
            {
                Bullet tempBullet1 = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Normal_small).GetComponent<Bullet>();

                //tempBullet1.SetFire(firePos.position, ((target.position + Random.insideUnitSphere * 2) - firePos.position).normalized, bulletSpeed, damage);
                float rndX = Random.Range(-15.0f, 15.0f);
                float rndY = Random.Range(-20.0f, 20.0f);
                if (Mathf.Abs(rndX) < 10)
                {
                    rndX = rndX < 0 ? Random.Range(-15, -10) : Random.Range(10, 15);
                }
                if (Mathf.Abs(rndY) < 15)
                {
                    rndY = rndY < 0 ? Random.Range(-20, -15) : Random.Range(15, 20);
                }
                tempBullet1.SetFire(firePos.position, Quaternion.Euler(rndX, rndY, 0) * (target.position - firePos.position).normalized, target, bulletSpeed, damage);

                GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.FutureGun_Fire, firePos.position, false);
                GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.AttackSpark_normal);
                temp.transform.position = firePos.position;
                temp.transform.rotation = Quaternion.Euler(firePos.eulerAngles.x + 90, firePos.eulerAngles.y, firePos.eulerAngles.z);

                temp.SetActive(true);
                anim.SetTrigger("Fire");

                currentFireRate = 0;
            }

            if (currentCoolTime_dodge >= coolTime_dodge && !isAttack)
            {
                int rndNum = Random.Range(0, 2);

                if (rndNum == 0)
                {
                    anim.SetTrigger("Dodge_Left");
                }
                else
                {
                    anim.SetTrigger("Dodge_Right");
                }

                currentCoolTime_dodge = 0;
            }
        }

        dir = (target.position - mesh.position).normalized;

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(target.position - new Vector3(this.transform.position.x, target.position.y, this.transform.position.z)), Time.deltaTime * 10);
        //mesh.localRotation = Quaternion.Euler(mesh.localRotation.eulerAngles + Quaternion.Euler(Quaternion.LookRotation(temp).eulerAngles.x, 0, 0).eulerAngles);
    }

    private void LateUpdate()
    {
        if (dir != Vector3.zero)
            mesh.localRotation = Quaternion.Euler(mesh.localRotation.eulerAngles + Quaternion.Euler(Quaternion.LookRotation(dir).eulerAngles.x, 0, 0).eulerAngles);

        if (anim.GetBool("isIdle"))
            wings.rotation = Quaternion.LookRotation(this.transform.forward);
    }

    void SandScatter()
    {
        if (!isDetect) return;

        GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Sand_Scatter_Large);
        temp.transform.position = sandScatterPos.position;
        temp.SetActive(true);
    }

    public override void SetDead(bool value)
    {
        isDead = value;

        if (isDead)
        {
            GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Explosion_destroy);
            temp.transform.position = this.transform.position;
            temp.SetActive(true);

            GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion_1, this.transform.position, false);

            this.gameObject.SetActive(false);
        }
    }

    //void Destroy()
    //{
    //    this.gameObject.SetActive(false);
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Type_A : Enemy
{
    [SerializeField] private Transform[] firePos;
    [SerializeField] private Transform sandScatterPos;

    [SerializeField] private float fireRate;
    private float currentFireRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform arms;
    private bool move_left;
    private bool isAttack;
    [SerializeField] private bool isShield;
    [SerializeField] protected float speed_sideStep;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        currentFireRate = Random.Range(0, fireRate);
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
            agent.SetDestination(this.transform.position);
            return;
        }

        if(!isAttack)
            currentFireRate += Time.deltaTime;

        if(currentFireRate >= fireRate)
        {
            agent.isStopped = true;
            isAttack = true;
            currentFireRate = 0;
            anim.SetTrigger("Attack");
        }

        if (!isAttack)
        {
            if (Vector3.Distance(this.transform.position, target.position) > attackRange)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
                anim.SetFloat("horizontal", Mathf.Lerp(anim.GetFloat("horizontal"), 0, Time.deltaTime * 4));
                anim.SetFloat("vertical", Mathf.Lerp(anim.GetFloat("vertical"), 1, Time.deltaTime * 4));
            }
            else
            {
                agent.isStopped = true;
                anim.SetFloat("vertical", Mathf.Lerp(anim.GetFloat("vertical"), 0, Time.deltaTime * 4));

                if (move_left)
                {
                    anim.SetFloat("horizontal", Mathf.Lerp(anim.GetFloat("horizontal"), -1, Time.deltaTime * 4));
                    this.transform.position = this.transform.position + -this.transform.right * Time.deltaTime * speed_sideStep;
                }
                else
                {
                    anim.SetFloat("horizontal", Mathf.Lerp(anim.GetFloat("horizontal"), 1, Time.deltaTime * 4));
                    this.transform.position = this.transform.position + this.transform.right * Time.deltaTime * speed_sideStep;
                }
            }
        }

        //anim.SetBool("isMove", !agent.isStopped);

        if (currentFireRate < fireRate / 2)
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
                r.material.SetColor("_EmissionColor", Color.Lerp(r.material.GetColor("_EmissionColor"), (emissionColor_angry * 35f) * (currentFireRate / fireRate), Time.deltaTime * 4));
            }
        }


        if(isShield)
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(new Vector3(target.position.x, this.transform.position.y, target.position.z) - this.transform.position), Time.deltaTime * (currentFireRate / fireRate) * 8);
        else
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(new Vector3(target.position.x, this.transform.position.y, target.position.z) - this.transform.position), Time.deltaTime * 10);
    }

    private void LateUpdate()
    {
        Vector3 dir = (target.position - this.transform.position).normalized;
        arms.rotation = Quaternion.Euler(arms.rotation.eulerAngles.x, arms.rotation.eulerAngles.y, -Quaternion.LookRotation(dir).eulerAngles.x - 90);
     
    }


    void SandScatter()
    {
        if (!isDetect) return;

        GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Sand_Scatter_Small);
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


    void Attack1()
    {
        Bullet tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Normal).GetComponent<Bullet>();

        if (isShield)
        {
            if(Vector3.Dot(this.transform.forward, (target.position - this.transform.position).normalized) > 0.7f)
            {
                tempBullet.SetFire(firePos[0].position, (target.position - firePos[0].position).normalized, bulletSpeed, damage);
            }
            else
                tempBullet.SetFire(firePos[0].position, firePos[0].forward, bulletSpeed, damage);
            
        }
        else
            tempBullet.SetFire(firePos[0].position, (target.position - firePos[0].position).normalized, bulletSpeed, damage);

        GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.FutureGun_Fire, firePos[0].position, false);
        GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.AttackSpark_normal);
        temp.transform.position = firePos[0].position;
        temp.transform.rotation = Quaternion.Euler(firePos[0].eulerAngles.x + 90, firePos[0].eulerAngles.y, firePos[0].eulerAngles.z);
        temp.SetActive(true);
    }

    void Attack2()
    {
        Bullet tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Normal).GetComponent<Bullet>();

        if (isShield)
        {
            if (Vector3.Dot(this.transform.forward, (target.position - this.transform.position).normalized) > 0.7f)
            {
                tempBullet.SetFire(firePos[1].position, (target.position - firePos[1].position).normalized, bulletSpeed, damage);
            }
            else
                tempBullet.SetFire(firePos[1].position, firePos[1].forward, bulletSpeed, damage);
        }
        else
            tempBullet.SetFire(firePos[1].position, (target.position - firePos[1].position).normalized, bulletSpeed, damage);

        GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.FutureGun_Fire, firePos[1].position, false);
        GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.AttackSpark_normal);
        temp.transform.position = firePos[1].position;
        temp.transform.rotation = Quaternion.Euler(firePos[1].eulerAngles.x + 90, firePos[1].eulerAngles.y, firePos[1].eulerAngles.z);
        temp.SetActive(true);
        agent.isStopped = false;
        isAttack = false;

        int rndNum = Random.Range(0, 2);

        switch(rndNum)
        {
            case 0:
                move_left = true;
                break;
            case 1:
                move_left = false;
                break;
        }
    }
}

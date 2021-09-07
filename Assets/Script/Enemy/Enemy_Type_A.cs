using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Type_A : Enemy
{
    [SerializeField] private Transform[] firePos;
    [SerializeField] private float fireRate;
    private float currentFireRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private Transform arms;
    [SerializeField] private Vector3 temp;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Vector3.Distance(this.transform.position, target.position) > attackRange)
        //{
        //    agent.isStopped = false;
        //    agent.SetDestination(target.position);
        //}
        //else
        //{
        //    if(Vector3.Distance(this.transform.position, target.position) <= attackRange - 5)
        //        agent.isStopped = true;
        //}

        currentFireRate += Time.deltaTime;

        if(currentFireRate >= fireRate)
        {
            currentFireRate = 0;

            anim.SetTrigger("Attack");
        }

      
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(new Vector3(target.position.x, this.transform.position.y, target.position.z) - this.transform.position), Time.deltaTime *10);
        
    }

    private void LateUpdate()
    {
        Vector3 dir = (target.position - this.transform.position).normalized;
        arms.rotation = Quaternion.Euler(arms.rotation.eulerAngles.x, arms.rotation.eulerAngles.y, -Quaternion.LookRotation(dir).eulerAngles.x - 90);
     
    }

    public override void SetDead(bool value)
    {
        isDead = value;

        if (isDead)
        {
            //if (effect_explosion != null)
            //{
            //    effect_explosion.SetActive(true);
            //    effect_explosion.transform.position = this.transform.position;
            //    effect_explosion.GetComponent<ParticleSystem>().Play();

            //    if (Vector3.Distance(this.transform.position, target.position) <= attackRange)
            //        GameManager.Instance.GetPlayer().DecreaseHp(damage);
            //}
            GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Explosion_destroy);
            temp.transform.position = this.transform.position;
            temp.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    void Attack1()
    {
        Bullet tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Normal).GetComponent<Bullet>();
        tempBullet.gameObject.SetActive(true);
        tempBullet.SetFire(firePos[0].position, (target.position - firePos[0].position).normalized, bulletSpeed, damage);
        GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.AttackSpark_normal);
        temp.transform.position = firePos[0].position;
        temp.transform.rotation = Quaternion.Euler(firePos[0].eulerAngles.x + 90, firePos[0].eulerAngles.y, firePos[0].eulerAngles.z);
        temp.SetActive(true);
    }

    void Attack2()
    {
        Bullet tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Normal).GetComponent<Bullet>();
        tempBullet.gameObject.SetActive(true);
        tempBullet.SetFire(firePos[1].position, (target.position - firePos[1].position).normalized, bulletSpeed, damage);
        GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.AttackSpark_normal);
        temp.transform.position = firePos[1].position;
        temp.transform.rotation = Quaternion.Euler(firePos[1].eulerAngles.x + 90, firePos[1].eulerAngles.y, firePos[1].eulerAngles.z);
        temp.SetActive(true);
    }
}

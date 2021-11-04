using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_LightningBall : Boss_Skill
{
    [SerializeField] private Transform center;
    [SerializeField] private float speed;
    [SerializeField] private int num;

    protected override void Start()
    {
        base.Start();
    }

    public override void Use()
    {
        base.Use();

        for (int i = 0; i < num; i++)
        {
            GameObject tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Plasma);
            tempBullet.GetComponent<Bullet>().SetFire(center.position + Random.insideUnitSphere * 4, (target.position - this.transform.position).normalized, target, speed, damage, 999);
        }

        base.ResetInfo();
    }

    protected override void ResetInfo()
    {
        base.ResetInfo();
    }

    protected override void Update()
    {
        //currentAttackTime -= Time.deltaTime;

        //anim.SetBool("isAttack_EnergyBall_LeftHand", true);

        //if (currentAttackTime <= 0)
        //{
        //    ResetInfo();
        //}
    }
}

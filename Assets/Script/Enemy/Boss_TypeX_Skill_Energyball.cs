using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_Energyball : Boss_Skill
{
    enum Hand
    {
        Left,
        Right
    }

    enum Type
    {
        Single,
        Multi
    }

    [SerializeField] private Transform firePos_leftHand;
    [SerializeField] private Transform firePos_rightHand;
    [SerializeField] private float speed;

    private Hand hand;
    private Type type;

    protected override void Start()
    {
        base.Start();
    }

    public override void Use()
    {
        base.Use();

        if (this.GetComponent<Boss_TypeX>().GetCurrentPhase() < 3)
        {
            int rndNum = Random.Range(0, 4);

            switch (rndNum)
            {
                case 0:
                    anim.SetBool("isAttack_EnergyBall_LeftHand", true);
                    hand = Hand.Left;
                    type = Type.Single;
                    break;
                case 1:
                    anim.SetBool("isAttack_EnergyBall_RightHand", true);
                    hand = Hand.Right;
                    type = Type.Single;
                    break;
                case 2:
                    anim.SetBool("isAttack_MultiShot_LeftHand", true);
                    hand = Hand.Left;
                    type = Type.Multi;
                    break;
                case 3:
                    anim.SetBool("isAttack_MultiShot_RightHand", true);
                    hand = Hand.Right;
                    type = Type.Multi;
                    break;
            }
        }
        else
        {
            int rndNum = Random.Range(0, 2);

            switch (rndNum)
            {
                case 0:
                    anim.SetBool("isAttack_EnergyBall_LeftHand", true);
                    hand = Hand.Left;
                    type = Type.Single;
                    break;
                case 1:
                    anim.SetBool("isAttack_MultiShot_LeftHand", true);
                    hand = Hand.Left;
                    type = Type.Multi;
                    break;
            }
        }
    }

    public override void ResetInfo()
    {
        anim.SetBool("isAttack_EnergyBall_LeftHand", false);
        anim.SetBool("isAttack_EnergyBall_RightHand", false);
        anim.SetBool("isAttack_MultiShot_LeftHand", false);
        anim.SetBool("isAttack_MultiShot_RightHand", false);

        base.ResetInfo();
    }

    void Fire()
    {
        if(hand == Hand.Left)
        {
            if(type == Type.Single)
            {
                Bullet_Boss tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                tempBullet.Fire(firePos_leftHand.position, Quaternion.Euler(0, Random.Range(-5.0f, 5.0f), 0) * (target.position - firePos_leftHand.position).normalized, speed, damage);

                GameObject tempAttackSpark = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.AttackSpark_EnergyBall);
                tempAttackSpark.transform.position = firePos_leftHand.position;
                tempAttackSpark.transform.rotation = firePos_leftHand.rotation;
                tempAttackSpark.SetActive(true);
            }
            else
            {
                Bullet_Boss tempBullet = null;
                for (int i = 0; i < 3; i++)
                {
                    tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                    tempBullet.Fire(firePos_leftHand.position, Quaternion.Euler(0, 15 * i, 0) * (target.position - firePos_leftHand.position).normalized, speed, damage);

                    if (i != 0)
                    {
                        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                        tempBullet.Fire(firePos_leftHand.position, Quaternion.Euler(0, 15 * -i, 0) * (target.position - firePos_leftHand.position).normalized, speed, damage);
                    }
                }

                GameObject tempAttackSpark = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.AttackSpark_EnergyBall);
                tempAttackSpark.transform.position = firePos_leftHand.position;
                tempAttackSpark.transform.rotation = firePos_leftHand.rotation;
                tempAttackSpark.SetActive(true);
            }
        }
        else
        {
            if (type == Type.Single)
            {
                Bullet_Boss tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                tempBullet.Fire(firePos_rightHand.position, Quaternion.Euler(0, Random.Range(-5.0f, 5.0f), 0) * (target.position - firePos_rightHand.position).normalized, speed, damage);

                GameObject tempAttackSpark = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.AttackSpark_EnergyBall);
                tempAttackSpark.transform.position = firePos_rightHand.position;
                tempAttackSpark.transform.rotation = firePos_rightHand.rotation;
                tempAttackSpark.SetActive(true);
            }
            else
            {
                Bullet_Boss tempBullet = null;
                for (int i =0; i < 3; i++)
                {
                    tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                    tempBullet.Fire(firePos_rightHand.position, Quaternion.Euler(0, 15 * i, 0) * (target.position - firePos_rightHand.position).normalized, speed, damage);

                    if(i != 0)
                    {
                        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                        tempBullet.Fire(firePos_rightHand.position, Quaternion.Euler(0, 15 * -i, 0) * (target.position - firePos_rightHand.position).normalized, speed, damage);
                    }
                }
             

                GameObject tempAttackSpark = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.AttackSpark_EnergyBall);
                tempAttackSpark.transform.position = firePos_rightHand.position;
                tempAttackSpark.transform.rotation = firePos_rightHand.rotation;
                tempAttackSpark.SetActive(true);
            }
        }
        //tempBullet.GetComponent<Bullet_Boss>().Fire(firePos_leftHand)
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

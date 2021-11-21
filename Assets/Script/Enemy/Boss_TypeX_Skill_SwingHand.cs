using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_SwingHand : Boss_Skill
{
    [SerializeField] private Boss_TypeX_Skill_SwingHand_AttackRange attackRange;
    [SerializeField] private Targeting targetingObj;
    [SerializeField] private ParticleSystem sandWind;

    protected override void Start()
    {
        base.Start();
    }

    public override void Use()
    {
        base.Use();

        int rndNum = Random.Range(0, 2);

        if (rndNum == 0)
            anim.SetTrigger("SwingHand_Left");
        else
            anim.SetTrigger("SwingHand_Right");

        targetingObj.SetCanRot(false);
    }

    void SwingHand_VisibleAttackRange()
    {
        attackRange.ActiveTrue();
    }

    void SwingHand_InvisibleAttackRange()
    {
        attackRange.ActiveFalse();
    }

    void SwingHand_Attack()
    {
        GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Swing, this.transform.position, false);
        attackRange.Attack(damage);
        sandWind.Play();
    }

    public override void ResetInfo()
    {
        targetingObj.SetCanRot(true);
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

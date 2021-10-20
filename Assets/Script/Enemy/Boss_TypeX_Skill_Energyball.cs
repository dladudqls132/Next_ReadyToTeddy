using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_Energyball : Boss_Skill
{
    protected override void Start()
    {
        base.Start();
    }

    public override void Use()
    {
        base.Use();

        int rndNum = Random.Range(0, 2);

        if(rndNum == 0)
            anim.SetBool("isAttack_EnergyBall_LeftHand", true);
        else
            anim.SetBool("isAttack_EnergyBall_RightHand", true);
    }

    protected override void ResetInfo()
    {
        anim.SetBool("isAttack_EnergyBall_LeftHand", false);
        anim.SetBool("isAttack_EnergyBall_RightHand", false);

        base.ResetInfo();
    }

    void SetIdle()
    {
        anim.SetBool("isIdle", true);
    }

    public override void Update()
    {
        //currentAttackTime -= Time.deltaTime;

        //anim.SetBool("isAttack_EnergyBall_LeftHand", true);

        //if (currentAttackTime <= 0)
        //{
        //    ResetInfo();
        //}
    }
}

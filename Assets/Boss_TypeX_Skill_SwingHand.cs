using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_SwingHand : Boss_Skill
{
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
    }

    protected override void ResetInfo()
    {
        base.ResetInfo();
    }

    void SetIdle()
    {
        anim.SetBool("isIdle", true);
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

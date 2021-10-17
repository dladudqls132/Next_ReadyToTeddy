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
    }

    protected override void ResetInfo()
    {
        anim.SetBool("isAttack_EnergyBall", false);

        base.ResetInfo();
    }

    public override void Update()
    {
        currentAttackTime -= Time.deltaTime;

        anim.SetBool("isAttack_EnergyBall", true);

        if (currentAttackTime <= 0)
        {
            ResetInfo();
        }
    }
}

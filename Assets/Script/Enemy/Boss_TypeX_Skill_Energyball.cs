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

        if (this.GetComponent<Boss_TypeX>().GetCurrentPhase() < 3)
        {
            int rndNum = Random.Range(0, 4);

            switch (rndNum)
            {
                case 0:
                    anim.SetBool("isAttack_EnergyBall_LeftHand", true);
                    break;
                case 1:
                    anim.SetBool("isAttack_EnergyBall_RightHand", true);
                    break;
                case 2:
                    anim.SetBool("isAttack_MultiShot_LeftHand", true);
                    break;
                case 3:
                    anim.SetBool("isAttack_MultiShot_RightHand", true);
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
                    break;
                case 1:
                    anim.SetBool("isAttack_MultiShot_LeftHand", true);
                    break;
            }
        }
    }

    protected override void ResetInfo()
    {
        anim.SetBool("isAttack_EnergyBall_LeftHand", false);
        anim.SetBool("isAttack_EnergyBall_RightHand", false);
        anim.SetBool("isAttack_MultiShot_LeftHand", false);
        anim.SetBool("isAttack_MultiShot_RightHand", false);

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

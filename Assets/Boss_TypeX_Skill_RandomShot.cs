using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_RandomShot : Boss_Skill
{
    [SerializeField] private float attackTime;
    [SerializeField] private float currentAttackTime;

    protected override void Start()
    {
        base.Start();
    }

    public override void Use()
    {
        base.Use();

        anim.SetBool("isRandomShot", true);
    }

    protected override void ResetInfo()
    {
        anim.SetBool("isRandomShot", false);

        currentAttackTime = attackTime;

        base.ResetInfo();
    }

    protected override void Update()
    {
        currentAttackTime -= Time.deltaTime;

        if (currentAttackTime <= 0)
        {
            ResetInfo();
        }
    }
}

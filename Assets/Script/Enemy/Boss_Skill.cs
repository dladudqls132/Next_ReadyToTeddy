using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss_Skill : MonoBehaviour
{
    protected bool isActive;
    [SerializeField] protected float coolTime;
    [SerializeField] protected float attackTime;
    [SerializeField] protected float damage;
    protected bool canUse;

    protected float currentCoolTime;
    protected float currentAttackTime;
    protected Animator anim;

    protected virtual  void Start()
    {
        currentCoolTime = coolTime;

        anim = this.GetComponent<Animator>();
    }

    public virtual void Update()
    {
       
    }

    protected virtual void ResetInfo()
    {
        this.enabled = false;
        isActive = false;
        currentCoolTime = coolTime;
    }

    public virtual void Use()
    {
        this.enabled = true;

        isActive = true;
        canUse = false;

        currentAttackTime = attackTime;

        //currentCoolTime = coolTime;
    }

    public bool CoolDown()
    {
        if (isActive) return false;

        if(currentCoolTime <= 0)
        {
            canUse = true;
        }
        else
        {
            currentCoolTime -= Time.deltaTime;
        }

        return canUse;
    }
}

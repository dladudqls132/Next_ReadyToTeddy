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

    [SerializeField] protected float currentCoolTime;
    protected float currentAttackTime;
    protected Animator anim;
    [SerializeField] private int activePhase;

    protected virtual void Awake()
    {
        currentCoolTime = coolTime;

        anim = this.GetComponent<Animator>();
    }

    protected virtual  void Start()
    {

    }

    protected virtual void Update()
    {
       
    }

    protected virtual void ResetInfo()
    {
        if (this.enabled)
        {
            anim.SetBool("isIdle", true);

            this.enabled = false;
            isActive = false;
            currentCoolTime = coolTime;
        }
    }

    public virtual void Use()
    {
        anim.SetBool("isIdle", false);
        this.enabled = true;

        isActive = true;
        canUse = false;

        currentAttackTime = attackTime;

        //currentCoolTime = coolTime;
    }

    public bool CoolDown()
    {
        if (this.GetComponent<Boss_TypeX>())
        {
            if (this.GetComponent<Boss_TypeX>().GetCurrentPhase() != activePhase) return false;
        }
        else if (this.GetComponent<Boss_TypeX_Shield>())
        {
            if (this.GetComponent<Boss_TypeX_Shield>().GetCurrentPhase() != activePhase) return false;
        }

        if(currentCoolTime <= 0)
        {
            canUse = true;
            return true;
        }
        else
        {
            currentCoolTime -= Time.deltaTime;
            return false;
        }
    }
}

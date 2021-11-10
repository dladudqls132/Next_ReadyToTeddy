using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss_Skill : MonoBehaviour
{
    [SerializeField] private int activePhase;
    protected bool isActive;
    [SerializeField] protected float coolTime;
    [SerializeField] protected float damage;
    protected bool canUse;

    [SerializeField] protected float currentCoolTime;
    protected Animator anim;
    protected Transform target;

    public float GetCurrentCoolTime() { return currentCoolTime; }
    public int GetActivePhase() { return activePhase; }

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
        if (this.GetComponent<Enemy>())
            target = this.GetComponent<Enemy>().GetTarget();
        else
            target = this.GetComponent<Boss_TypeX_Shield>().GetTarget();

        anim.SetBool("isIdle", false);
        this.enabled = true;

        isActive = true;
        canUse = false;

        //currentAttackTime = attackTime;

        //currentCoolTime = coolTime;
    }

    public bool CoolDown()
    {
        if (this.GetComponent<Boss_TypeX>())
        {
            if (this.GetComponent<Boss_TypeX>().GetCurrentPhase() < activePhase) return false;
        }
        else if (this.GetComponent<Boss_TypeX_Shield>())
        {
            if (this.GetComponent<Boss_TypeX_Shield>().GetCurrentPhase() < activePhase) return false;
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

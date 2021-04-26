using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MeleeTest : Enemy
{
    enum Enemy_Behavior
    {
        Idle,
        Walk,
        Run,
        Jump,
        Attack,
        RunningAttack
    }

    [SerializeField] private Enemy_Behavior behavior;
    [SerializeField] private float attackDelay;
    [SerializeField] private GameObject ragdoll;
    [SerializeField] private float attackAfterTime;
    [SerializeField] private float currentAttackAfterTime;
    private float currentAttackDelay;
    private bool canAttackTurn;
    private float jumpAngle;
    private bool canAttack;

    override protected void Start()
    {
        base.Start();

        //target = GameManager.Instance.GetPlayer().transform;
        state = Enemy_State.None;

        anim = this.GetComponent<Animator>();
    }

    protected override void SetDead(bool value)
    {
        isDead = value;
        behavior = Enemy_Behavior.Idle;
        state = Enemy_State.None;
        agent.isStopped = true;
        currentHp = maxHp;
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(isDead)
        {
            return;
        }

        if (this.GetComponent<RoomInfo>().GetRoom() == target.GetComponent<RoomInfo>().GetRoom())
            state = Enemy_State.Chase;


        if (state == Enemy_State.None || state == Enemy_State.Return)
            return;

        if (agent.enabled)
        {
            if (!agent.isStopped)
            {
                agent.SetDestination(target.position);
            }
        }

        //공격중이 아닐때
        if (behavior != Enemy_Behavior.Attack && behavior != Enemy_Behavior.RunningAttack)
        {
            if(currentAttackDelay > 0)
                currentAttackDelay -= Time.deltaTime;
            if(currentAttackAfterTime > 0)
                currentAttackAfterTime -= Time.deltaTime;

            if (currentAttackDelay <= 0)
            {
                if (behavior != Enemy_Behavior.Jump)
                {
                    if (Vector3.Distance(this.transform.position, target.position) <= attackRange)
                    {
                        currentAttackDelay = attackDelay;
                        currentAttackAfterTime = attackAfterTime;
                        agent.isStopped = true;
                        canAttackTurn = true;

                        behavior = Enemy_Behavior.Attack;
                    }
                    else
                    {

                        if (Vector3.Distance(this.transform.position, target.position) > attackRange)
                        {
                            if (currentAttackAfterTime <= 0)
                            {
                                agent.isStopped = false;
                                behavior = Enemy_Behavior.Run;
                            }
                        }

                    }
                }
            }
            else
            {
                if (Vector3.Distance(this.transform.position, target.position) > attackRange)
                {
                    if (currentAttackAfterTime <= 0)
                    {
                        agent.isStopped = false;
                        behavior = Enemy_Behavior.Run;
                    }
                }
                else
                {
                    if (Vector3.Distance(this.transform.position, target.position) <= attackRange - 1)
                    {
                        agent.isStopped = true;

                        behavior = Enemy_Behavior.Idle;
                    }

                    if (behavior == Enemy_Behavior.Idle)
                    {
                        Vector3 dir = (target.position - this.transform.position).normalized;
                        dir.y = 0;
                        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 20);
                    }
                }

            }
        }
        else // 공격중일때
        {
            if (canAttackTurn)
            {
                Vector3 dir = (target.position - this.transform.position).normalized;
                dir.y = 0;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 20);
            }
            else
                agent.isStopped = true;
        }

        //Jump
        if (agent.isOnOffMeshLink)
        {
            //currentShotDelay = shotDelay;
            agent.isStopped = false;
            behavior = Enemy_Behavior.Jump;

            agent.speed = 5.5f;
            jumpAngle += (2 * Mathf.PI / ((agent.currentOffMeshLinkData.endPos - agent.currentOffMeshLinkData.startPos).magnitude / 5.5f)) * Time.deltaTime;

            agent.baseOffset = Mathf.Sin(jumpAngle) * 2;
            agent.baseOffset = Mathf.Clamp(agent.baseOffset, 1, agent.baseOffset);
        }
        else
        {
            if (behavior == Enemy_Behavior.Jump)
                behavior = Enemy_Behavior.Idle;

            agent.speed = speed;
            jumpAngle = 0;
            agent.baseOffset = 0;
        }

        //Animation Controll
        //for (int i = 0; i < anim.parameterCount; i++)
        //{
        //    if (anim.parameters[i].type == AnimatorControllerParameterType.Bool)
        //    {
        //        anim.SetBool(anim.parameters[i].nameHash, false);
        //    }
        //}

        AnimationUpdate();
    }

    void AnimationUpdate()
    {
        switch (behavior)
        {
            case Enemy_Behavior.Idle:
                anim.SetBool("isAttack", false);
                anim.SetBool("isRunningAttack", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                break;
            case Enemy_Behavior.Walk:
                anim.SetBool("isAttack", false);
                anim.SetBool("isRunningAttack", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("isWalking", true);
                anim.SetBool("isRunning", false);
                break;
            case Enemy_Behavior.Run:
                anim.SetBool("isAttack", false);
                anim.SetBool("isRunningAttack", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", true);
                break;
            case Enemy_Behavior.Attack:
                anim.SetBool("isRunningAttack", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isAttack", true);
                break;
            case Enemy_Behavior.RunningAttack:
                anim.SetBool("isAttack", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isRunningAttack", true);
                break;
            case Enemy_Behavior.Jump:
                anim.SetBool("isAttack", false);
                anim.SetBool("isRunningAttack", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isJumping", true);
                break;
        }
    }

    void Attack()
    {
        if (Physics.CheckBox(this.GetComponent<Collider>().bounds.center + this.transform.forward * (attackRange / 2), new Vector3(1, 1, attackRange), this.transform.rotation, 1 << LayerMask.NameToLayer("Player")))
        {
            target.GetComponent<PlayerController>().DecreaseHp(damage);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(this.GetComponent<Collider>().bounds.center + this.transform.forward * (attackRange / 2), new Vector3(1, 1, attackRange));
    //}

    void CanAttackTurnFalse()
    {
        canAttackTurn = false;
    }

    void AttackFalse()
    {
        anim.SetBool("isAttack", false);
        anim.SetBool("isRunningAttack", false);

        //if (Vector3.Distance(this.transform.position, target.position) > attackRange)
        //{
        //    behavior = Enemy_Behavior.Run;
        //}
        //else
            behavior = Enemy_Behavior.Idle;
    }
}

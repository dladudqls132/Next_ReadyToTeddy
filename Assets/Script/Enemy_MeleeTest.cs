using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MeleeTest : Enemy
{
    enum Enemy_Behavior
    {
        Idle,
        Run,
        Jump,
        Attack,
        RunningAttack
    }

    [SerializeField] private Enemy_Behavior behavior;
    [SerializeField] private float attackDelay;
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

    private void Update()
    {
        if(isDead)
        {
            behavior = Enemy_Behavior.Idle;
            state = Enemy_State.None;
            agent.isStopped = true;
            this.gameObject.SetActive(false);

            return;
        }

        if (Vector3.Distance(this.transform.position, target.position) > detectRange && state == Enemy_State.None)
            return;

        if (!agent.isStopped)
        {
            agent.SetDestination(target.position);
        }

        RaycastHit hit;
        if (Physics.Raycast(eye.position, (target.position - eye.position).normalized, out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("Player"))
                state = Enemy_State.Targeting;
            else
                state = Enemy_State.Search;
        }
        else
        {
            state = Enemy_State.Search;
        }

        if (behavior != Enemy_Behavior.Attack && behavior != Enemy_Behavior.RunningAttack)
        {
            currentAttackDelay -= Time.deltaTime;

            if (currentAttackDelay <= 0)
            {
                if (behavior != Enemy_Behavior.Jump)
                {
                    if (Vector3.Distance(this.transform.position, target.position) <= attackRange)
                    {
                        currentAttackDelay = attackDelay;
                        agent.isStopped = true;
                        canAttackTurn = true;

                        behavior = Enemy_Behavior.Attack;
                    }
                    else if (Vector3.Distance(this.transform.position, target.position) <= attackRange + 2)
                    {
                        currentAttackDelay = attackDelay;
                        agent.isStopped = true;
                        canAttackTurn = true;

                        behavior = Enemy_Behavior.RunningAttack;
                    }
                    else
                    {
                        agent.isStopped = false;
                        canAttackTurn = false;

                        behavior = Enemy_Behavior.Run;
                    }
                }
            }
            else
            {
                if (Vector3.Distance(this.transform.position, target.position) > attackRange)
                {
                    agent.isStopped = false;
                    canAttackTurn = false;

                    behavior = Enemy_Behavior.Run;
                }
                else
                {
                    agent.isStopped = true;

                    behavior = Enemy_Behavior.Idle;

                    Vector3 dir = (target.position - this.transform.position).normalized;
                    dir.y = 0;
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 20);
                }
            }
        }
        else
        {
            if (canAttackTurn)
            {
                Vector3 dir = (target.position - this.transform.position).normalized;
                dir.y = 0;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 20);
            }
        }

        if ((behavior == Enemy_Behavior.Attack || behavior == Enemy_Behavior.RunningAttack))
        {
            agent.isStopped = true;

            if (canAttackTurn)
            {
                Vector3 dir = (target.position - this.transform.position).normalized;
                dir.y = 0;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 20);
            }
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

        switch (behavior)
        {
            case Enemy_Behavior.Idle:
                break;
            case Enemy_Behavior.Run:
                anim.SetBool("isAttack", false);
                anim.SetBool("isRunningAttack", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("isRunning", true);
                break;
            case Enemy_Behavior.Attack:
                anim.SetBool("isRunningAttack", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isAttack", true);
                break;
            case Enemy_Behavior.RunningAttack:
                anim.SetBool("isAttack", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isRunningAttack", true);
                break;
            case Enemy_Behavior.Jump:
                anim.SetBool("isAttack", false);
                anim.SetBool("isRunningAttack", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isJumping", true);
                break;
        }
    }

    void Attack()
    {
        if (Physics.CheckBox(this.GetComponent<Collider>().bounds.center + this.transform.forward * (attackRange / 2), new Vector3(1, 1, attackRange), this.transform.rotation, 1 << LayerMask.NameToLayer("Player")))
        {
            target.parent.GetComponent<PlayerController>().DecreaseHp(damage);
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

        if (Vector3.Distance(this.transform.position, target.position) > attackRange)
        {
            behavior = Enemy_Behavior.Run;
        }
        else
            behavior = Enemy_Behavior.Idle;
    }
}

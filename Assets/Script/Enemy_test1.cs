using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_test1 : Enemy
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
    private NavMeshAgent agent;
    [SerializeField] private float attackDelay;
    private float currentAttackDelay;
    private bool canAttackTurn;
    private float jumpAngle;
    private bool canAttack;
    private Vector3 originPos;

    override protected void Start()
    {
        base.Start();

        target = GameManager.Instance.GetPlayer().transform;

        anim = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        originPos = this.transform.position;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(eye.position, (target.GetComponent<Collider>().bounds.center - eye.position).normalized, out hit, detectRange, (1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Player")) ))
        {
   
            if(hit.transform.CompareTag("Player"))
            {
                if (Vector3.Dot(this.transform.forward, (target.position - this.transform.position).normalized) >= 0.5f)
                {
                    state = Enemy_State.Chase;
                    currentCombatTime = combatTime;
                }
                else
                {
                    if (target.GetComponent<Rigidbody>().velocity.magnitude > 7.0f)
                    {
                        state = Enemy_State.Chase;
                        currentCombatTime = combatTime;
                    }
                }
            }
            else
            {
                if(target.GetComponent<Rigidbody>().velocity.magnitude > 7.0f && Vector3.Distance(this.transform.position, target.position) <= detectRange)
                {
                    if (behavior != Enemy_Behavior.Attack && behavior != Enemy_Behavior.RunningAttack)
                    {
                        agent.SetDestination(target.position);
                        state = Enemy_State.Search;
                        behavior = Enemy_Behavior.Run;
                    }
                }
            }
        }
        else
        {
            if(state == Enemy_State.Chase)
                state = Enemy_State.Search;

            if(state == Enemy_State.Search)
            {
                currentCombatTime -= Time.deltaTime;

                if(currentCombatTime <= 0)
                {
                    currentCombatTime = combatTime;
                    state = Enemy_State.Patrol;
                    agent.SetDestination(originPos);
                }
            }
        }

        if (state == Enemy_State.Chase)
        {
            //agent.speed = 8.0f;
            agent.SetDestination(target.position);

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

        }
        else if(state == Enemy_State.Search)
        {
            //agent.speed = 2.0f;
            //agent.SetDestination(target.position);
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

            agent.speed = 3;
            jumpAngle += (2 * Mathf.PI / ((agent.currentOffMeshLinkData.endPos - agent.currentOffMeshLinkData.startPos).magnitude / 3)) * Time.deltaTime;

            agent.baseOffset = Mathf.Sin(jumpAngle) * 2;
            agent.baseOffset = Mathf.Clamp(agent.baseOffset, 1, agent.baseOffset);
        }
        else
        {
            if (behavior == Enemy_Behavior.Jump)
                behavior = Enemy_Behavior.Idle;

            if (state == Enemy_State.Chase)
                agent.speed = 8;
            else if (state == Enemy_State.Search)
                agent.speed = 3;
            jumpAngle = 0;
            agent.baseOffset = 0;
        }

        //Animation Controll
        for (int i = 0; i < anim.parameterCount; i++)
        {
            if (anim.parameters[i].type == AnimatorControllerParameterType.Bool)
            {
                anim.SetBool(anim.parameters[i].nameHash, false);
            }
        }

        switch (behavior)
        {
            case Enemy_Behavior.Idle:
                break;
            case Enemy_Behavior.Run:
                anim.SetBool("isRunning", true);
                break;
            case Enemy_Behavior.Attack:
                anim.SetBool("isAttack", true);
                break;
            case Enemy_Behavior.RunningAttack:
                anim.SetBool("isRunningAttack", true);
                break;
            case Enemy_Behavior.Jump:
                anim.SetBool("isJumping", true);
                break;
        }
    }

    void Attack()
    {
        if (Physics.CheckBox(this.GetComponent<Collider>().bounds.center + this.transform.forward * (attackRange / 2), new Vector3(1, 1, attackRange), this.transform.rotation, 1 << LayerMask.NameToLayer("Player")))
        {
            target.GetComponent<PlayerController>().DecreaseHp(10);
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

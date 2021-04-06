using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_MeleeTest : Enemy
{
    enum Enemy_Behavior
    {
        Idle,
        Run,
        Jump,
        Attack
    }

    [SerializeField] private Enemy_Behavior behavior;
    private NavMeshAgent agent;
    [SerializeField] private float attackDelay;
    private float currentAttackDelay;

    override protected void Start()
    {
        base.Start();

        target = GameManager.Instance.GetPlayer().transform;

        anim = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Vector3.Distance(this.transform.position, target.position) > detectRange && state == Enemy_State.None)
            return;

        if(agent.enabled)
        agent.SetDestination(target.position);

        RaycastHit hit;
        if (Physics.Raycast(eye.position, (target.GetComponent<Collider>().bounds.center - eye.position).normalized, out hit, Mathf.Infinity))
        {
            state = Enemy_State.Targeting;
        }
        else
        {
            state = Enemy_State.Search;
        }

        if(behavior != Enemy_Behavior.Attack)
            currentAttackDelay -= Time.deltaTime;

        if (Vector3.Distance(this.transform.position, target.position) <= attackRange)
        {
            if(currentAttackDelay <= 0)
            {
                agent.enabled = false;
                currentAttackDelay = attackDelay;
                behavior = Enemy_Behavior.Attack;
            }
        }
        else
        {
            if(behavior != Enemy_Behavior.Attack)
            {
                agent.enabled = true;
                behavior = Enemy_Behavior.Run;
            }
        }

        if(behavior == Enemy_Behavior.Attack)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(new Vector3(target.position.x, 0, target.position.z) - new Vector3(this.transform.position.x, 0, this.transform.position.z).normalized), Time.deltaTime * 12);
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
                anim.SetBool("isRunningAttack", true);
                break;
            case Enemy_Behavior.Jump:
                anim.SetBool("isJumping", true);
                break;
        }
    }

    void AttackFalse()
    {
        if (Vector3.Distance(this.transform.position, target.position) > attackRange)
        {
            behavior = Enemy_Behavior.Run;
        }
        else
            behavior = Enemy_Behavior.Idle;
    }
}

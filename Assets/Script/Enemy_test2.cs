using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class Enemy_test2 : Enemy
{
    enum Enemy_Behavior
    {
        Idle,
        Walk,
        Run,
        Jump,
        Aiming,
        Attack
    }

    [SerializeField] private Enemy_Behavior behavior;
    //[SerializeField] private bool isAiming;
    //[SerializeField] private bool isAttack;
    [SerializeField] private Rig aimRig;
    [SerializeField] private Rig bodyRig;
    [SerializeField] private Rig handRig;
    private LineRenderer laser;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform aimPos;
    [SerializeField] private float shotDelay;
    [SerializeField] private float currentShotDelay;

    private Quaternion tempRot;
    //private NavMeshAgent agent;
    [SerializeField] private float jumpAngle;
    Vector3 originPos;
    //[SerializeField] private Transform head;
    //[SerializeField] private Transform spine1;
    //[SerializeField] private Transform spine2;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        anim = this.GetComponent<Animator>();
        //agent = this.GetComponent<NavMeshAgent>();
        laser = this.GetComponent<LineRenderer>();

        target = GameManager.Instance.GetPlayer().GetCamPos();

        currentShotDelay = shotDelay;

        aimPos = GameObject.Find("Player_targetPos").transform;
        tempRot = this.transform.rotation;

        //state = Enemy_State.Patrol;

        foreach (MultiAimConstraint component in bodyRig.GetComponentsInChildren<MultiAimConstraint>())
        {
            var data = component.data.sourceObjects;
            data.SetTransform(0, GameManager.Instance.GetPlayer().transform.Find("CamPos"));
            component.data.sourceObjects = data;
        }

        foreach (MultiAimConstraint component in aimRig.GetComponentsInChildren<MultiAimConstraint>())
        {
            var data = component.data.sourceObjects;
            data.SetTransform(0, aimPos);
            component.data.sourceObjects = data;
        }

        RigBuilder rigs = GetComponent<RigBuilder>();
        rigs.Build();

        originPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != Enemy_State.RunAway)
        {
            RaycastHit hit;
            if (Physics.Raycast(eye.position, (target.position - eye.position).normalized, out hit, detectRange, (1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Player"))))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    canSee = true;

                    if (Vector3.Dot(this.transform.forward, (target.position - this.transform.position).normalized) >= 0.5f)
                    {
                        state = Enemy_State.Chase;
                        currentCombatTime = combatTime;
                        currentReturnToPatrolTime = returnToPatorlTime;
                    }
                    else
                    {
                        if (target.parent.GetComponent<Rigidbody>().velocity.magnitude > 7.0f)
                        {
                            state = Enemy_State.Chase;
                            currentCombatTime = combatTime;
                            currentReturnToPatrolTime = returnToPatorlTime;
                        }
                    }
                }
                else
                {
                    canSee = false;
                    if (target.parent.GetComponent<Rigidbody>().velocity.magnitude > 7.0f && Vector3.Distance(this.transform.position, target.position) <= detectRange)
                    {
                        if (behavior != Enemy_Behavior.Attack && behavior != Enemy_Behavior.Aiming)
                        {
                            agent.SetDestination(target.position);
                            currentReturnToPatrolTime = returnToPatorlTime;
                            if (state != Enemy_State.Chase)
                                state = Enemy_State.Search;

                            behavior = Enemy_Behavior.Walk;
                        }
                    }
                }
            }
            else
                canSee = false;
        }

        if (state == Enemy_State.Chase)
        {
            agent.SetDestination(target.position);

            if (behavior != Enemy_Behavior.Jump)
            {
                if (canSee)
                {
                    if (Vector3.Distance(this.transform.position, target.position) <= attackRange)
                    {
                        agent.isStopped = true;
                        behavior = Enemy_Behavior.Aiming;
                    }
                    else if (behavior != Enemy_Behavior.Aiming && behavior != Enemy_Behavior.Attack)
                    {
                        agent.isStopped = false;
                        agent.SetDestination(target.position);
                        behavior = Enemy_Behavior.Walk;
                    }
                }
                else
                {
                    if (behavior != Enemy_Behavior.Aiming && behavior != Enemy_Behavior.Attack)
                    {
                        agent.isStopped = false;
                        behavior = Enemy_Behavior.Walk;
                    }
                }
            }

            currentCombatTime -= Time.deltaTime;

            if (currentCombatTime <= 0)
            {
                currentCombatTime = combatTime;
                state = Enemy_State.Search;
            }
        }
        else if (state == Enemy_State.Search)
        {
            if (behavior != Enemy_Behavior.Aiming && behavior != Enemy_Behavior.Attack)
            {
                agent.isStopped = false;
                behavior = Enemy_Behavior.Walk;
            }
            currentReturnToPatrolTime -= Time.deltaTime;

            if(currentReturnToPatrolTime <= 0)
            {
                currentReturnToPatrolTime = returnToPatorlTime;
                state = Enemy_State.Patrol;
            }
        }
        else if (state == Enemy_State.Patrol)
        {
            behavior = Enemy_Behavior.Walk;
        }
        else if(state == Enemy_State.RunAway)
        {
            behavior = Enemy_Behavior.Run;
        }


        if (behavior == Enemy_Behavior.Aiming)
        {
            currentShotDelay -= Time.deltaTime;
            currentShotDelay = Mathf.Clamp(currentShotDelay, 0, currentShotDelay);

            if (currentShotDelay <= 0)
            {
                behavior = Enemy_Behavior.Attack;
                currentShotDelay = shotDelay;
                laser.startWidth = 0;
            }
            else
            {
                if (currentShotDelay <= shotDelay / 2)
                {
                    float rayScale = 0.05f * (currentShotDelay / shotDelay);

                    laser.startWidth = rayScale;
                }

                laser.SetPosition(0, firePos.position);

                RaycastHit laserHit;
                if (Physics.Raycast(firePos.position, (aimPos.position - firePos.position).normalized, out laserHit, 30.0f, ~(1 << LayerMask.NameToLayer("Enemy"))))
                {
                    laser.SetPosition(1, laserHit.point + (aimPos.position - firePos.position).normalized * 0.2f + Vector3.down * 0.01f);
                }
                else
                {
                    laser.SetPosition(1, firePos.position + (aimPos.position - firePos.position).normalized * 30 + Vector3.down * 0.01f);
                }
            }
        }
        else
        {
            currentShotDelay = shotDelay;
            laser.startWidth = 0f;
        }

        if (behavior == Enemy_Behavior.Attack)
        {
            RaycastHit fireHit;

            if (Physics.Raycast(firePos.position, firePos.forward, out fireHit, Mathf.Infinity))
            {
                if (fireHit.transform.CompareTag("Player"))
                {
                    fireHit.transform.GetComponent<PlayerController>().DecreaseHp(1);
                }
                else if (fireHit.transform.CompareTag("Enemy"))
                {
                    fireHit.transform.GetComponent<Enemy>().DecreaseHp(1, true);
                }
            }
            if (state == Enemy_State.Chase)
            {
                if (canSee)
                    behavior = Enemy_Behavior.Aiming;
                else
                    behavior = Enemy_Behavior.Walk;
            }
            else
                behavior = Enemy_Behavior.Idle;
        }

        //Jump
        if (agent.isOnOffMeshLink)
        {
            agent.isStopped = false;
            behavior = Enemy_Behavior.Jump;

            agent.speed = 3;
            jumpAngle += (2 * Mathf.PI / ((agent.currentOffMeshLinkData.endPos - agent.currentOffMeshLinkData.startPos).magnitude / 3)) * Time.deltaTime;

            agent.baseOffset = Mathf.Sin(jumpAngle) * 2;
            agent.baseOffset = Mathf.Clamp(agent.baseOffset, 0, agent.baseOffset);
        }
        else
        {
            if (behavior == Enemy_Behavior.Jump)
                behavior = Enemy_Behavior.Idle;

            if(state == Enemy_State.RunAway)
                agent.speed = 6.0f;
            else
                agent.speed = 2;

            jumpAngle = 0;
            agent.baseOffset = 0;

            //GoToPatrolNode();

            if (currentHp < maxHp / 2 && !isRunAway)
            {
                if (state != Enemy_State.RunAway)
                {
                    currentDestPatrolNode = patrolNode[0];
                    currentDestPatrolNodeIndex = 0;

                    for (int i = 0; i < patrolNode.Length; i++)
                    {
                        if (Vector3.Distance(target.position, patrolNode[i].position) > Vector3.Distance(target.position, currentDestPatrolNode.position))
                        {
                            currentDestPatrolNode = patrolNode[i];
                            currentDestPatrolNodeIndex = i;
                        }
                    }

                    state = Enemy_State.RunAway;
                    behavior = Enemy_Behavior.Run;
                    isRunAway = true;

                    agent.isStopped = false;
                    agent.SetDestination(currentDestPatrolNode.position);
                }
            }
        }

        if(state == Enemy_State.RunAway)
        {
            if (Vector3.Distance(this.transform.position, currentDestPatrolNode.position) < 1.0f)
            {
                state = Enemy_State.None;
                behavior = Enemy_Behavior.Idle;
            }
        }

        //IK, Rotation Controll
        if (behavior == Enemy_Behavior.Aiming || behavior == Enemy_Behavior.Attack)
        {
            if (Vector3.Dot(this.transform.forward, (target.position - this.transform.position).normalized) <= 0.5f)
            {
                tempRot = Quaternion.LookRotation(target.position - this.transform.position);
                tempRot = Quaternion.Euler(0, tempRot.eulerAngles.y, 0);
            }

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, tempRot, Time.deltaTime * 12.0f);

            bodyRig.weight = Mathf.Lerp(bodyRig.weight, 1, Time.deltaTime * 15);
            aimRig.weight = Mathf.Lerp(aimRig.weight, 1, Time.deltaTime * 15);
        }
        else
        {
            tempRot = this.transform.rotation;

            bodyRig.weight = Mathf.Lerp(bodyRig.weight, 0, Time.deltaTime * 15);
            aimRig.weight = Mathf.Lerp(aimRig.weight, 0, Time.deltaTime * 15);
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
            case Enemy_Behavior.Walk:
                anim.SetBool("isWalking", true);
                break;
            case Enemy_Behavior.Run:
                anim.SetBool("isRunning", true);
                break;
            case Enemy_Behavior.Aiming:
                anim.SetBool("isAiming", true);
                break;
            case Enemy_Behavior.Attack:
                break;
            case Enemy_Behavior.Jump:
                anim.SetBool("isJumping", true);
                break;
        }

    }

    //private void LateUpdate()
    //{

    //    if (behavior == Enemy_Behavior.Aiming)
    //    {
    //        Quaternion oringinRot_Spine1 = spine1.rotation;
    //        Quaternion oringinRot_Spine2 = spine2.rotation;

    //        Vector3 dir = (target.position - spine1.position).normalized;
    //        spine1.rotation = Quaternion.LookRotation(dir);
    //        spine1.rotation *= Quaternion.Inverse(this.transform.rotation);
    //        spine1.rotation *= oringinRot_Spine1;

    //        dir = (target.position - spine2.position).normalized;
    //        spine2.rotation = Quaternion.LookRotation(dir);
    //        spine2.rotation *= Quaternion.Inverse(this.transform.rotation);
    //        spine2.rotation *= oringinRot_Spine2;
    //    }
    //}
}

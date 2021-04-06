using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.AI;


public class Enemy_ShooterTest : Enemy
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
    [SerializeField] private LineRenderer laser;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform aimPos;
    [SerializeField] private float shotDelay;
    [SerializeField] private float currentShotDelay;

    private Quaternion tempRot;
    private NavMeshAgent agent;
    [SerializeField] private float jumpAngle;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        anim = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        laser = this.GetComponent<LineRenderer>();

        target = GameManager.Instance.GetPlayer().transform;

        currentShotDelay = shotDelay;

        aimPos = GameObject.Find("Player_targetPos").transform;
        tempRot = this.transform.rotation;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, target.position) > detectRange && state == Enemy_State.None)
            return;

        agent.SetDestination(target.position);

        if (Vector3.Distance(this.transform.position, target.position) > attackRange)
        {
            state = Enemy_State.Search;
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(eye.position, (target.GetComponent<Collider>().bounds.center - eye.position).normalized, out hit, Mathf.Infinity))
            {
                state = Enemy_State.Targeting;
            }
            else
            {
                state = Enemy_State.Search;
            }
        }

        if (state == Enemy_State.Targeting)
        {
            agent.isStopped = true;
            behavior = Enemy_Behavior.Aiming;

        }
        else if (state == Enemy_State.Search)
        {
            if (behavior != Enemy_Behavior.Aiming && behavior != Enemy_Behavior.Attack)
            {
                agent.isStopped = false;
                behavior = Enemy_Behavior.Walk;
            }
        }

        if (behavior == Enemy_Behavior.Aiming)
        {
            currentShotDelay -= Time.deltaTime;

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
                laser.SetPosition(1, firePos.position + (aimPos.position - firePos.position).normalized * 30);
            }
        }
        else
        {
            laser.startWidth = 0;
        }

        if (behavior == Enemy_Behavior.Attack)
        {
            if (state == Enemy_State.Targeting)
                behavior = Enemy_Behavior.Aiming;
            else
                behavior = Enemy_Behavior.Idle;
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
            agent.baseOffset = Mathf.Clamp(agent.baseOffset, 0, agent.baseOffset);
        }
        else
        {
            agent.speed = 2;
            jumpAngle = 0;
            agent.baseOffset = 0;
        }

        //IK, Rotation Controll
        if (behavior == Enemy_Behavior.Aiming || behavior == Enemy_Behavior.Attack)
        {
            if (Vector3.Dot(this.transform.forward, target.GetComponent<Collider>().bounds.center - this.transform.position) <= 0.5f)
            {
                tempRot = Quaternion.LookRotation(target.GetComponent<Collider>().bounds.center - this.transform.position);
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

            //if(behavior == Enemy_Behavior.Jump)
            //    handRig.weight = Mathf.Lerp(handRig.weight, 0, Time.deltaTime * 15);
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
            case Enemy_Behavior.Aiming:
                anim.SetBool("isAiming", true);
                break;
            case Enemy_Behavior.Attack:
                break;
            case Enemy_Behavior.Jump:
                anim.SetBool("isJumping", true);
                break;
        }

        //    RaycastHit hit;
        //    if (Physics.Raycast(eye.position, (target.GetComponent<Collider>().bounds.center - eye.position).normalized, out hit, detectRange * 3))
        //    {
        //        if (hit.transform.CompareTag("Player"))
        //        {
        //            canSee = true;

        //            if (Vector3.Dot(this.transform.forward, target.GetComponent<Collider>().bounds.center - this.transform.position) > 0.5f || Vector3.Distance(this.transform.position, target.GetComponent<Collider>().bounds.center) <= detectRange)
        //            {
        //                state = Enemy_State.Targeting;
        //            }
        //        }
        //        else
        //            canSee = false;
        //    }
        //    else
        //        canSee = false;

        //    if(state == Enemy_State.Targeting || state == Enemy_State.Search)
        //    {
        //        if (canSee)
        //        {
        //            currentCombatTime = combatTime;

        //            //isAiming = true;
        //        }
        //        else
        //        {
        //            currentCombatTime -= Time.deltaTime;

        //            state = Enemy_State.Search;

        //            if (behavior != Enemy_Behavior.Attack && behavior != Enemy_Behavior.Aiming)
        //            {
        //                //isAiming = false;
        //            }
        //        }

        //        if(currentCombatTime <= 0)
        //        {
        //            state = Enemy_State.None;
        //            currentCombatTime = combatTime;
        //        }
        //        else
        //        {
        //            if (Vector3.Dot(this.transform.forward, target.GetComponent<Collider>().bounds.center - this.transform.position) <= 0.5f)
        //            {
        //                tempRot = Quaternion.LookRotation(target.GetComponent<Collider>().bounds.center - this.transform.position);
        //                tempRot = Quaternion.Euler(0, tempRot.eulerAngles.y, 0);
        //            }
        //            if(state == Enemy_State.Search && !canSee)
        //                tempRot = this.transform.rotation;

        //            if (canSee)
        //                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, tempRot, Time.deltaTime * 12.0f);
        //        }
        //    }
        //    else
        //    {
        //        //isAiming = false;
        //    }

        //    if (!agent.isOnOffMeshLink)
        //    {
        //        if (behavior == Enemy_Behavior.Jump)
        //        {
        //            behavior = Enemy_Behavior.Idle;
        //        }

        //        agent.speed = 2;
        //        handRig.weight = Mathf.Lerp(handRig.weight, 1, Time.deltaTime * 15);
        //        if (behavior == Enemy_Behavior.Aiming || behavior == Enemy_Behavior.Attack)
        //        {
        //            bodyRig.weight = Mathf.Lerp(bodyRig.weight, 1, Time.deltaTime * 15);
        //            aimRig.weight = Mathf.Lerp(aimRig.weight, 1, Time.deltaTime * 15);

        //            agent.isStopped = true;
        //        }
        //        else
        //        {
        //            bodyRig.weight = Mathf.Lerp(bodyRig.weight, 0, Time.deltaTime * 15);
        //            aimRig.weight = Mathf.Lerp(aimRig.weight, 0, Time.deltaTime * 15);

        //            if (state == Enemy_State.Targeting || state == Enemy_State.Search)
        //            {
        //                if (!canSee)
        //                {
        //                    agent.isStopped = false;
        //                    agent.SetDestination(target.position);
        //                }
        //            }
        //            else
        //            {
        //                agent.isStopped = true;
        //            }
        //        }

        //        jumpAngle = 0;
        //        if (agent.baseOffset > 0)
        //            agent.baseOffset = Mathf.Lerp(agent.baseOffset, 0, Time.deltaTime * 6);
        //        else
        //            agent.baseOffset = 0;

        //        anim.SetBool("isJumping", false);
        //        //anim.SetBool("isAiming", isAiming);
        //        anim.SetBool("isWalking", !agent.isStopped);
        //    }
        //    else
        //    {
        //        currentShotDelay = shotDelay;
        //        agent.speed = Mathf.Lerp(agent.speed, 3, Time.deltaTime * 10);
        //        jumpAngle += (2 * Mathf.PI / ((agent.currentOffMeshLinkData.endPos - agent.currentOffMeshLinkData.startPos).magnitude / 3)) * Time.deltaTime;

        //        agent.baseOffset = Mathf.Sin(jumpAngle) * 2;
        //        agent.baseOffset = Mathf.Clamp(agent.baseOffset, 0, agent.baseOffset);

        //        behavior = Enemy_Behavior.Jump;
        //        //isAiming = false;
        //        agent.isStopped = false;

        //        bodyRig.weight = Mathf.Lerp(bodyRig.weight, 0, Time.deltaTime * 15);
        //        aimRig.weight = Mathf.Lerp(aimRig.weight, 0, Time.deltaTime * 15);
        //        handRig.weight = Mathf.Lerp(handRig.weight, 0, Time.deltaTime * 15);

        //        anim.SetBool("isJumping", true);
        //        //anim.SetBool("isAiming", false);
        //        anim.SetBool("isWalking", false);
        //    }

        //    if(state == Enemy_State.Targeting)
        //    {
        //        if(behavior != Enemy_Behavior.Attack && behavior != Enemy_Behavior.Jump)
        //            behavior = Enemy_Behavior.Aiming;
        //    }
        //    else if(state == Enemy_State.Search || state == Enemy_State.Patrol)
        //    {
        //        if (behavior != Enemy_Behavior.Aiming)
        //        {
        //            behavior = Enemy_Behavior.Walk;
        //        }
        //    }
        //    else
        //    {
        //        behavior = Enemy_Behavior.Idle;
        //    }

        //    if(behavior == Enemy_Behavior.Aiming)
        //    {
        //        currentShotDelay -= Time.deltaTime;

        //        if (currentShotDelay <= shotDelay / 2)
        //        {
        //            float rayScale = 0.05f * (currentShotDelay / shotDelay);

        //            laser.startWidth = rayScale;
        //        }
        //        laser.SetPosition(0, firePos.position);
        //        laser.SetPosition(1, firePos.position + (aimPos.position - firePos.position).normalized * 30);

        //        if (currentShotDelay <= 0)
        //        {
        //            currentShotDelay = shotDelay;

        //            behavior = Enemy_Behavior.Attack;
        //        }
        //    }
        //    else if(behavior == Enemy_Behavior.Attack)
        //    {
        //        RaycastHit fireHit;
        //        if (Physics.Raycast(firePos.position, firePos.forward, out fireHit, Mathf.Infinity))
        //        {
        //            if(fireHit.transform.CompareTag("Player"))
        //            {
        //                fireHit.transform.GetComponent<PlayerController>().DecreaseHp(1);
        //            }
        //            else if(fireHit.transform.CompareTag("Enemy"))
        //            {
        //                fireHit.transform.GetComponent<Enemy>().DecreaseHp(1);
        //            }
        //        }

        //        laser.startWidth = 0;

        //        if (state == Enemy_State.Targeting)
        //            behavior = Enemy_Behavior.Aiming;
        //        else
        //            behavior = Enemy_Behavior.Idle;
        //    }
        //    else
        //    {
        //        laser.startWidth = 0;
        //    }
        //}
    }
    }

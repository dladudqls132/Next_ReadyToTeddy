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
    [SerializeField] private bool isAiming;
    [SerializeField] private bool isAttack;
    [SerializeField] private Rig aimRig;
    [SerializeField] private Rig bodyRig;
    [SerializeField] private Rig handRig;
    [SerializeField] private LineRenderer laser;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform aimPos;
    [SerializeField] private float shotDelay;
    private float currentShotDelay;

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
        RaycastHit hit;
        if (Physics.Raycast(eye.position, (target.position - eye.position).normalized, out hit, 10))
        {
            if (hit.transform.CompareTag("Player"))
            {
                canSee = true;

                if (Vector3.Dot(this.transform.forward, target.position - this.transform.position) > 0.5f || Vector3.Distance(this.transform.position, target.position) <= detectRange)
                {
                    state = Enemy_State.Targeting;
                }
            }
            else
                canSee = false;
        }
        else
            canSee = false;

        if(state == Enemy_State.Targeting || state == Enemy_State.Search)
        {
            if (canSee)
            {
                currentCombatTime = combatTime;

                isAiming = true;
            }
            else
            {
                currentCombatTime -= Time.deltaTime;

                isAiming = false;
                state = Enemy_State.Search;
            }

            if(currentCombatTime <= 0)
            {
                state = Enemy_State.None;
                currentCombatTime = combatTime;
            }
            else
            {
                if (Vector3.Dot(this.transform.forward, target.position - this.transform.position) <= 0.5f)
                {
                    tempRot = Quaternion.LookRotation(target.transform.position - this.transform.position);
                    tempRot = Quaternion.Euler(0, tempRot.eulerAngles.y, 0);
                }
                if(state == Enemy_State.Search && !canSee)
                    tempRot = this.transform.rotation;

                if (canSee)
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, tempRot, Time.deltaTime * 12.0f);
            }
        }
        else
        {
            isAiming = false;
        }

        if (!agent.isOnOffMeshLink)
        {
            if(behavior != Enemy_Behavior.Attack)
                behavior = Enemy_Behavior.Idle;

            agent.speed = 2;
            handRig.weight = Mathf.Lerp(handRig.weight, 1, Time.deltaTime * 15);
            if (isAiming)
            {
                bodyRig.weight = Mathf.Lerp(bodyRig.weight, 1, Time.deltaTime * 15);
                aimRig.weight = Mathf.Lerp(aimRig.weight, 1, Time.deltaTime * 15);

                agent.isStopped = true;
            }
            else
            {
                bodyRig.weight = Mathf.Lerp(bodyRig.weight, 0, Time.deltaTime * 15);
                aimRig.weight = Mathf.Lerp(aimRig.weight, 0, Time.deltaTime * 15);

                if (state == Enemy_State.Targeting || state == Enemy_State.Search)
                {
                    if (!canSee)
                    {
                        agent.isStopped = false;
                        agent.SetDestination(target.position);
                    }
                }
                else
                {
                    agent.isStopped = true;
                }
            }

            jumpAngle = 0;
            if (agent.baseOffset > 0)
                agent.baseOffset = Mathf.Lerp(agent.baseOffset, 0, Time.deltaTime * 6);
            else
                agent.baseOffset = 0;

            anim.SetBool("isJumping", false);
            //anim.SetBool("isAiming", isAiming);
            anim.SetBool("isWalking", !agent.isStopped);
        }
        else
        {
            currentShotDelay = shotDelay;
            agent.speed = Mathf.Lerp(agent.speed, 3, Time.deltaTime * 10);
            jumpAngle += (2 * Mathf.PI / ((agent.currentOffMeshLinkData.endPos - agent.currentOffMeshLinkData.startPos).magnitude / 3)) * Time.deltaTime;

            agent.baseOffset = Mathf.Sin(jumpAngle) * 2;
            agent.baseOffset = Mathf.Clamp(agent.baseOffset, 0, agent.baseOffset);

            behavior = Enemy_Behavior.Jump;
            isAiming = false;
            agent.isStopped = false;

            bodyRig.weight = Mathf.Lerp(bodyRig.weight, 0, Time.deltaTime * 15);
            aimRig.weight = Mathf.Lerp(aimRig.weight, 0, Time.deltaTime * 15);
            handRig.weight = Mathf.Lerp(handRig.weight, 0, Time.deltaTime * 15);

            anim.SetBool("isJumping", true);
            //anim.SetBool("isAiming", false);
            anim.SetBool("isWalking", false);
        }

        if(state == Enemy_State.Targeting)
        {
            if(behavior != Enemy_Behavior.Attack && behavior != Enemy_Behavior.Jump)
                behavior = Enemy_Behavior.Aiming;
        }
        else if(state == Enemy_State.Search || state == Enemy_State.Patrol)
        {
            behavior = Enemy_Behavior.Walk;
        }
        else
        {
            behavior = Enemy_Behavior.Idle;
        }

        if(behavior == Enemy_Behavior.Aiming)
        {
            currentShotDelay -= Time.deltaTime;

            if (currentShotDelay <= shotDelay / 2)
            {
                float rayScale = 0.05f * (currentShotDelay / shotDelay);

                laser.startWidth = rayScale;
            }
            laser.SetPosition(0, firePos.position);
            laser.SetPosition(1, firePos.position + (aimPos.position - firePos.position).normalized * 30);
            
            if (currentShotDelay <= 0)
            {
                currentShotDelay = shotDelay;

                behavior = Enemy_Behavior.Attack;
            }
        }
        else if(behavior == Enemy_Behavior.Attack)
        {
            RaycastHit fireHit;
            if (Physics.Raycast(firePos.position, firePos.forward, out fireHit, Mathf.Infinity))
            {
                if(fireHit.transform.CompareTag("Player"))
                {
                    fireHit.transform.GetComponent<PlayerController>().DecreaseHp(1);
                }
                else if(fireHit.transform.CompareTag("Enemy"))
                {
                    fireHit.transform.GetComponent<Enemy>().DecreaseHp(1);
                }
            }

            laser.startWidth = 0;
            behavior = Enemy_Behavior.Aiming;
        }
        else
        {
            laser.startWidth = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.AI;

public class Enemy_ShooterTest : Enemy
{
    [SerializeField] private bool isAiming;
    [SerializeField] private Rig aimRig;
    [SerializeField] private Rig bodyRig;
    [SerializeField] private Rig handRig;

    private Quaternion tempRot;
    private NavMeshAgent agent;
    [SerializeField] private float jumpAngle;
    private Vector3 jumpStartPos;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        anim = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
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

                if (Vector3.Dot(this.transform.forward, target.position - this.transform.position) > 0.5f)
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
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, tempRot, Time.deltaTime * 12);
            }
        }
        else
        {
            isAiming = false;
        }

        if (!agent.isOnOffMeshLink)
        {
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

            jumpStartPos = this.transform.position;
            jumpAngle = 0;
            if (agent.baseOffset > 0)
                agent.baseOffset = Mathf.Lerp(agent.baseOffset, 0, Time.deltaTime * 6);
            else
                agent.baseOffset = 0;

            anim.SetBool("isJumping", false);
            anim.SetBool("isAiming", isAiming);
            anim.SetBool("isWalking", !agent.isStopped);
        }
        else
        {
            agent.speed = 3;
            jumpAngle += (2 * Mathf.PI / ((agent.currentOffMeshLinkData.endPos - agent.currentOffMeshLinkData.startPos).magnitude / 3)) * Time.deltaTime;

            agent.baseOffset = Mathf.Sin(jumpAngle) * 2;
            agent.baseOffset = Mathf.Clamp(agent.baseOffset, 0, agent.baseOffset);

            isAiming = false;
            agent.isStopped = false;

            bodyRig.weight = Mathf.Lerp(bodyRig.weight, 0, Time.deltaTime * 15);
            aimRig.weight = Mathf.Lerp(aimRig.weight, 0, Time.deltaTime * 15);
            handRig.weight = Mathf.Lerp(handRig.weight, 0, Time.deltaTime * 15);

            anim.SetBool("isJumping", true);
            anim.SetBool("isAiming", false);
            anim.SetBool("isWalking", false);
        }
    }

    float CalculateVelocityY(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;

        float Sy = distance.y;

        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        return Vy;
    }
}

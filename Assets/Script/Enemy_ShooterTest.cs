using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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
    private LineRenderer laser;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform aimPos;
    [SerializeField] private float shotDelay;
    private float currentShotDelay;
    [SerializeField] private Vector3 destPos;

    private Quaternion tempRot;
    [SerializeField] private float jumpAngle;
    //[SerializeField] private Transform head;
    //[SerializeField] private Transform spine1;
    //[SerializeField] private Transform spine2;

    public void SetDestPos(Vector3 pos) { destPos = pos; }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        anim = this.GetComponent<Animator>();
        laser = this.GetComponent<LineRenderer>();

        //target = GameManager.Instance.GetPlayer().transform;
        target = GameManager.Instance.GetPlayer().GetCamPos();
        currentShotDelay = shotDelay;

        aimPos = GameObject.Find("Player_targetPos").transform;
        tempRot = this.transform.rotation;

        state = Enemy_State.None;

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
        if (isDead)
        {
            behavior = Enemy_Behavior.Idle;
            state = Enemy_State.None;
            agent.isStopped = true;
            this.gameObject.SetActive(false);

            return;
        }

        if (Vector3.Distance(this.transform.position, target.position) > detectRange && state == Enemy_State.None)
            return;

        agent.SetDestination(destPos);

        if(Vector3.Distance(this.transform.position, destPos) <= 1.0f)
        {
            agent.isStopped = true;

            RaycastHit hit;
            if (Physics.Raycast(eye.position, (target.position - eye.position).normalized, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enviroment"))))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    state = Enemy_State.Targeting;
                    behavior = Enemy_Behavior.Aiming;
                }
                else
                    state = Enemy_State.Search;
            }
            else
            {
                state = Enemy_State.Search;
            }

            if (state == Enemy_State.Search)
                behavior = Enemy_Behavior.Idle;
        }
        else
        {
            behavior = Enemy_Behavior.Walk;
        }

      
        //if (Vector3.Distance(this.transform.position, target.position) > attackRange)
        //{
        //    state = Enemy_State.Search;
        //}
        //else
        //{
        //    RaycastHit hit;
        //    if (Physics.Raycast(eye.position, (target.position - eye.position).normalized, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enviroment"))))
        //    {
        //        if (hit.transform.CompareTag("Player"))
        //            state = Enemy_State.Targeting;
        //        else
        //            state = Enemy_State.Search;
        //    }
        //    else
        //    {
        //        state = Enemy_State.Search;
        //    }
        //}

        //if (state == Enemy_State.Targeting)
        //{
        //    if (behavior != Enemy_Behavior.Jump)
        //    {
        //        agent.isStopped = true;
        //        behavior = Enemy_Behavior.Aiming;
        //    }
        //}
        //else if (state == Enemy_State.Search)
        //{
        //    if (behavior != Enemy_Behavior.Aiming && behavior != Enemy_Behavior.Attack)
        //    {
        //        agent.isStopped = false;
        //        behavior = Enemy_Behavior.Walk;
        //    }
        //}


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
                if (Physics.Raycast(firePos.position, (aimPos.position - firePos.position).normalized, out laserHit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Ignore Raycast"))))
                {
                    laser.SetPosition(1, laserHit.point + (aimPos.position - firePos.position).normalized * 0.2f);
                }
                else
                {
                    laser.SetPosition(1, firePos.position + (aimPos.position - firePos.position).normalized * 100);
                }
            }
        }
        else
        {
            currentShotDelay = shotDelay;
            laser.startWidth = 0;
        }

        if (behavior == Enemy_Behavior.Attack)
        {
            RaycastHit fireHit;

            if (Physics.Raycast(firePos.position, firePos.forward, out fireHit, Mathf.Infinity))
            {
                if (fireHit.transform.CompareTag("Player"))
                {
                    fireHit.transform.GetComponent<PlayerController>().DecreaseHp(damage);
                }
                else if (fireHit.transform.CompareTag("Enemy"))
                {
                    fireHit.transform.GetComponent<Enemy>().DecreaseHp(this.gameObject, damage, fireHit.point, false);
                }
            }
            if (state == Enemy_State.Targeting)
                behavior = Enemy_Behavior.Aiming;
            else
                behavior = Enemy_Behavior.Idle;
        }

        //Jump
        if (agent.isOnOffMeshLink)
        {
            agent.isStopped = false;
            behavior = Enemy_Behavior.Jump;

            agent.speed = 5.5f;
            jumpAngle += (2 * Mathf.PI / ((agent.currentOffMeshLinkData.endPos - agent.currentOffMeshLinkData.startPos).magnitude / 3)) * Time.deltaTime;

            agent.baseOffset = Mathf.Sin(jumpAngle) * 2;
            agent.baseOffset = Mathf.Clamp(agent.baseOffset, 0, agent.baseOffset);
        }
        else
        {
            if (behavior == Enemy_Behavior.Jump)
                behavior = Enemy_Behavior.Idle;

            agent.speed = speed;
            jumpAngle = 0;
            agent.baseOffset = 0;
        }

        //IK, Rotation Controll
        if (behavior == Enemy_Behavior.Aiming || behavior == Enemy_Behavior.Attack)
        {
            if (Vector3.Dot(this.transform.forward, target.position - this.transform.position) <= 0.5f)
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

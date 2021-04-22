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
    [SerializeField] private Transform aimPos;
    [SerializeField] private Rig aimRig;
    [SerializeField] private Rig bodyRig;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform firePos;
    [SerializeField] private float shotDelay_min;
    [SerializeField] private float shotDelay_max;
    private float shotDelay;
    private float currentShotDelay;

    private Quaternion tempRot;
    [SerializeField] private float jumpAngle;
    private Vector3 shotDir;
    [SerializeField] private float rndAimingWalkTime;
    [SerializeField] private float currentRndAimingWalkTime;
    [SerializeField] private Vector2 aimingWalkDir;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        anim = this.GetComponent<Animator>();

        shotDelay = Random.Range(shotDelay_min, shotDelay_max);
        currentShotDelay = Random.Range(0.1f, shotDelay);

        aimPos = target.GetComponent<PlayerController>().GetAimPos();
        tempRot = this.transform.rotation;

        state = Enemy_State.None;
        currentRndAimingWalkTime = rndAimingWalkTime / 2;

        foreach (MultiAimConstraint component in bodyRig.GetComponentsInChildren<MultiAimConstraint>())
        {
            var data = component.data.sourceObjects;
            data.SetTransform(0, aimPos);
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
            currentHp = maxHp;
            this.gameObject.SetActive(false);

            return;
        }

        if (this.GetComponent<RoomInfo>().GetRoom() == target.GetComponent<RoomInfo>().GetRoom())
            state = Enemy_State.Chase;

        if (state == Enemy_State.None)
            return;


        currentShotDelay -= Time.deltaTime;

        RaycastHit hit;
        if(Physics.Raycast(firePos.position, (aimPos.position - firePos.position).normalized, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Player"))))
        {
            if(!hit.transform.CompareTag("Player"))
            {
                canSee = false;
            }
            else
            {
                canSee = true;
            }
        }

        if (canSee)
        {
            agent.isStopped = true;

            currentRndAimingWalkTime -= Time.deltaTime;

            if (currentRndAimingWalkTime <= 0)
            {
                currentRndAimingWalkTime = rndAimingWalkTime;
                int dir = Random.Range(-1, 2);
                aimingWalkDir = new Vector2(dir, 0);
            }

            if (currentShotDelay <= 0)
            {
                behavior = Enemy_Behavior.Attack;

                Bullet tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Normal).GetComponent<Bullet>();
                tempBullet.gameObject.SetActive(true);
                tempBullet.SetFire(firePos.position, shotDir, bulletSpeed, damage);

                currentShotDelay = shotDelay;
            }
            else
            {
                behavior = Enemy_Behavior.Aiming;
                shotDir = ((aimPos.position + Random.insideUnitSphere * 0.1f + target.GetComponent<Rigidbody>().velocity * 0.25f) - firePos.position).normalized;
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            behavior = Enemy_Behavior.Walk;
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
        //if (behavior == Enemy_Behavior.Aiming || behavior == Enemy_Behavior.Attack)
        //{
        //    if (Vector3.Dot(this.transform.forward, target.position - this.transform.position) <= 0.5f)
        //    {
        //        tempRot = Quaternion.LookRotation(target.position - this.transform.position);
        //        tempRot = Quaternion.Euler(0, tempRot.eulerAngles.y, 0);
        //    }

        //    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, tempRot, Time.deltaTime * 12.0f);

        //    bodyRig.weight = Mathf.Lerp(bodyRig.weight, 1, Time.deltaTime * 15);
        //    aimRig.weight = Mathf.Lerp(aimRig.weight, 1, Time.deltaTime * 15);
        //}
        //else
        //{
        //    tempRot = this.transform.rotation;

        //    bodyRig.weight = Mathf.Lerp(bodyRig.weight, 0, Time.deltaTime * 15);
        //    aimRig.weight = Mathf.Lerp(aimRig.weight, 0, Time.deltaTime * 15);
        //}

   
        //Move
        if (Vector3.Dot(this.transform.forward, target.position - this.transform.position) <= 0.5f)
        {
            tempRot = Quaternion.LookRotation(target.position - this.transform.position);
            tempRot = Quaternion.Euler(0, tempRot.eulerAngles.y, 0);
        }

        if (behavior == Enemy_Behavior.Idle)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, tempRot, Time.deltaTime * 12.0f);
        }
        else
        {
            if (canSee)
            {
                Quaternion rot = Quaternion.LookRotation(target.position - this.transform.position);
                rot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rot, Time.deltaTime * 12.0f);
            }
        }

        bodyRig.weight = Mathf.Lerp(bodyRig.weight, 1, Time.deltaTime * 15);
        aimRig.weight = Mathf.Lerp(aimRig.weight, 1, Time.deltaTime * 15);

        AnimationUpdate();
    }

    void AnimationUpdate()
    {
        switch (behavior)
        {
            case Enemy_Behavior.Idle:
                break;
            case Enemy_Behavior.Walk:
                anim.SetBool("isAiming", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("isWalking", true);
                break;
            case Enemy_Behavior.Aiming:
                anim.SetBool("isAiming", true);
                anim.SetBool("isJumping", false);
                anim.SetBool("isWalking", false);
                break;
            case Enemy_Behavior.Attack:
                break;
            case Enemy_Behavior.Jump:
                anim.SetBool("isAiming", false);
                anim.SetBool("isJumping", true);
                anim.SetBool("isWalking", false);
                break;
        }

        if(canSee)
        {
            anim.SetFloat("horizontal", Mathf.Lerp(anim.GetFloat("horizontal"), aimingWalkDir.x, Time.deltaTime * 12));
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

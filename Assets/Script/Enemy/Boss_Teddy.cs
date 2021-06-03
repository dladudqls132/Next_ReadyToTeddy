using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossBehavior
{
    Idle,
    Dodge,
    FireBullet,
    Meteor,
    Laser,
    Shield
}

public class Boss_Teddy : Enemy
{
    [Header("Boss")]
    [SerializeField] private BossBehavior behavior;
    [SerializeField] private float acceleration = 1;
    [SerializeField] private Transform shotPos;

    Vector3 move;
    Vector3 rootMotionPos;
    Vector3 rootMotionRot;
    SphereCollider sphereCollider;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float attackDelay;
    private float currentAttackDelay;
    [SerializeField] private GameObject bullet;
    private float originY;
    [SerializeField] private float dodgeCoolTime;
    private float currentDodgeCoolTime;
    private bool isDodge;
    [SerializeField] private float meteorTime;
    private float currentMeteorTime;
    [SerializeField] private float meteorCoolTime;
    private float currentMeteorCoolTime;
    [SerializeField] private float meteorDropCoolTime;
    private float currentMeteorDropCoolTime;
    [SerializeField] private float bulletSpeed;
    private bool isMeteor;
    [SerializeField] private List<GameObject> containers = new List<GameObject>();
    int dropNum;
    [SerializeField] private ParticleSystem fireStart;
    [SerializeField] private GameObject laser;
    [SerializeField] private float turnDirTime;
    private float currentTurnDirTime;
    private Vector3 moveDir;
    [SerializeField] private float laserCoolTime;
    private float currentLaserCoolTime;
    [SerializeField] private List<Spawner> spawners = new List<Spawner>();
    [SerializeField] private float spawnMobRate;
    [SerializeField] private float ShieldCoolTime;
    private float currentShieldCoolTime;

    public BossBehavior GetCurrentBehavior() { return behavior; }

    void InvokeLoadScene()
    {
        GameManager.Instance.LoadScene("Credit", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public override void SetDead(bool value)
    {
        isDead = value;

        if (isDead)
        {
            laser.SetActive(false);
            for(int i = 0; i < containers.Count; i++)
            {
                containers[i].GetComponent<Boss_Teddy_Container>().SetNone();
            }
            for (int i = 0; i < spawners.Count; i++)
            {
                spawners[i].gameObject.SetActive(false);
            }
            rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            rigid.useGravity = true;

            anim.SetBool("isDead", true);
            Invoke("InvokeLoadScene", 8.0f);
        }
        //this.gameObject.SetActive(false);
        //anim.enabled = false;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        sphereCollider = this.GetComponent<SphereCollider>();
        originY = this.transform.position.y;
        fireStart = Instantiate(fireStart);

        laser = Instantiate(laser);
        laser.GetComponent<Boss_Laser>().SetParent(shotPos);
        laser.SetActive(false);

        currentShieldCoolTime = ShieldCoolTime;
        for(int i = 0; i < spawners.Count; i++)
        {
            spawners[i].SetSpawnRate(spawnMobRate);
        }

        energyShield.GetComponent<Boss_EnergyShield>().SetShieldHp(shieldHp);
        //playerObject = GameManager.Instance.GetPlayer().gameObject;
    }

    private void Update()
    {
        if (isDead || target.GetComponent<RoomInfo>().GetRoom() != this.GetComponent<RoomInfo>().GetRoom())
        {
            return;
        }

        currentDodgeCoolTime += Time.deltaTime;
        currentAttackDelay += Time.deltaTime;
        currentTurnDirTime += Time.deltaTime;

        if (behavior == BossBehavior.Idle)
        {
            UpdateBehavior();
        }
        else
        {
            if (behavior == BossBehavior.Meteor)
            {
                currentMeteorTime += Time.deltaTime;

                if (currentMeteorTime >= meteorTime)
                {
                    behavior = BossBehavior.Idle;
                    anim.SetBool("isMeteor", false);
                    currentMeteorTime = 0;

                    for (int i = 0; i < containers.Count; i++)
                    {
                        containers[i].GetComponent<Boss_Teddy_Container>().ResetPos();
                        dropNum = 0;
                    }
                }

                currentMeteorDropCoolTime += Time.deltaTime;

                if (currentMeteorDropCoolTime >= meteorDropCoolTime)
                {
                    currentMeteorDropCoolTime = 0;

                    //Debug.Log(Random.Range(0, containers.Count));
                    int[] rndNum = new int[containers.Count];
                    for (int i = 0; i < containers.Count; i++)
                    {
                        rndNum[i] = i;
                    }

                    for (int i = 0; i < containers.Count; i++)
                    {
                        int rnd = Random.Range(0, containers.Count);

                        int temp = rndNum[i];
                        rndNum[i] = rndNum[rnd];
                        rndNum[rnd] = temp;
                    }

                    containers[dropNum].GetComponent<Boss_Teddy_Container>().SetDropReady(target.position, 1.0f);
                    dropNum++;
                    dropNum = Mathf.Clamp(dropNum, 0, containers.Count - 1);
                }
            }
            else if(behavior == BossBehavior.Shield)
            {
                IncreaseHp(3 * Time.deltaTime);

                if(energyShield.GetComponent<Boss_EnergyShield>().GetIsDestroy())
                {
                    isGod = false;
                    behavior = BossBehavior.Idle;
                }
            }
        }

        if (currentDodgeCoolTime >= dodgeCoolTime)
        {
            if (behavior == BossBehavior.Idle || behavior == BossBehavior.FireBullet)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Enemy")), QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.root == this.transform)
                    {
                        ResetTrigger();

                        int rnd = Random.Range(0, 2);
                        if (rnd == 0)
                            anim.SetTrigger("Dodge_Left");
                        else
                            anim.SetTrigger("Dodge_Right");

                        behavior = BossBehavior.Dodge;
                        isDodge = true;
                        currentDodgeCoolTime = 0;

                        return;
                    }
                }
            }
        }

        if (behavior != BossBehavior.Meteor)
        {
            currentMeteorCoolTime += Time.deltaTime;
        }
        if (behavior != BossBehavior.Laser)
        {
            currentLaserCoolTime += Time.deltaTime;
        }
        if(behavior != BossBehavior.Shield)
        {
            currentShieldCoolTime += Time.deltaTime;
        }
    }

    void UpdateBehavior()
    {
        if(currentHp <= maxHp / 3)
        {
            if(currentShieldCoolTime >= ShieldCoolTime)
            {
                ResetTrigger();
                currentShieldCoolTime = 0;
                behavior = BossBehavior.Shield;
                isGod = true;
                //for(int i = 0; i < spawners.Count; i++)
                //{
                //    spawners[i].gameObject.SetActive(true);
                //}

                energyShield = Instantiate(energyShield, this.transform.position, Quaternion.identity, this.transform);
                energyShield.SetActive(true);

                return;
            }
        }

        if (currentMeteorCoolTime >= meteorCoolTime)
        {
            ResetTrigger();
            behavior = BossBehavior.Meteor;
            anim.SetBool("isMeteor", true);
            currentMeteorCoolTime = 0;
            return;
        }

        if (currentLaserCoolTime >= laserCoolTime)
        {
            ResetTrigger();
            //laser.SetActive(true);
            laser.transform.position = shotPos.position;
            behavior = BossBehavior.Laser;
            anim.SetTrigger("Laser");
            currentLaserCoolTime = 0;
            return;
        }

        if (currentAttackDelay >= attackDelay)
        {
            //if (Vector3.Distance(this.transform.position, target.position) <= stoppingDistance )
            //{
            //    behavior = BossBehavior.FireBullet;
            //    ResetTrigger();
            //    anim.SetTrigger("FireBullet");
            //    currentAttackDelay = 0;

            //    return;
            //}
            behavior = BossBehavior.FireBullet;
            ResetTrigger();
            anim.SetTrigger("FireBullet");
            currentAttackDelay = 0;

            return;
        }
    }

    public void SetBehavior(BossBehavior behavior)
    {
        this.behavior = behavior;
    }

    void SetIsDodgeFalse()
    {
        isDodge = false;
    }

    void ResetTrigger()
    {
        anim.ResetTrigger("FireBullet");
        anim.ResetTrigger("Dodge_Left");
        anim.ResetTrigger("Dodge_Right");
        anim.ResetTrigger("Laser");
    }

    private void OnAnimatorMove()
    {
        rootMotionPos += anim.deltaPosition;
        rootMotionRot += anim.deltaRotation.eulerAngles;
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;

        if (behavior == BossBehavior.Laser)
        {
            laser.SetActive(true);
        }
        else
        {
            laser.SetActive(false);
        }

        if (Vector3.Distance(this.transform.position, target.position) >= stoppingDistance)
        {
            if (currentTurnDirTime >= turnDirTime)
            {
                int rnd = Random.Range(0, 2);
                if (rnd == 0)
                {
                    moveDir = (this.transform.right + this.transform.forward).normalized;
                }
                else
                {
                    moveDir = (-this.transform.right + this.transform.forward).normalized;
                }

                currentTurnDirTime = 0;
            }
        }
        else
        {
            if (currentTurnDirTime >= turnDirTime)
            {
                int rnd = Random.Range(0, 2);
                if (rnd == 0)
                {
                    moveDir = (this.transform.right).normalized;
                }
                else
                {
                    moveDir = (-this.transform.right).normalized;
                }

                currentTurnDirTime = 0;
            }

        }

        if (behavior != BossBehavior.Idle)
        {
            //this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, originY, this.transform.position.z), Time.deltaTime * 12);
            //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation((target.position - this.transform.position).normalized), Time.deltaTime * 12);
            move = Vector3.Lerp(move, Vector3.zero, Time.deltaTime * 10);
        }
        else
        {
            move = Vector3.Lerp(move, moveDir, Time.deltaTime * 10);
        }

        move.y = 0;

        rigid.velocity = move * acceleration * 1.5f;
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, originY, this.transform.position.z), Time.deltaTime * 12);

        if(behavior != BossBehavior.Shield)
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 12);

        this.transform.position = transform.position + rootMotionPos;
        this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles + rootMotionRot);

        rootMotionPos = Vector3.zero;
        rootMotionRot = Vector3.zero;
    }

    private void FireBullet()
    {
        //GameObject temp = Instantiate(bullet, shotPos.position, Quaternion.LookRotation((target.position - this.transform.position).normalized));
        //temp.GetComponent<Bullet_Boss>().Fire((target.position - shotPos.position).normalized, bulletSpeed);

        GameObject temp = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy);
        temp.transform.position = shotPos.position;
        temp.transform.rotation = shotPos.rotation;
        temp.gameObject.SetActive(true);
        temp.GetComponent<Bullet_Boss>().Fire(((target.position + Vector3.up * 0.5f) - shotPos.position).normalized, bulletSpeed);

        fireStart.transform.rotation = shotPos.rotation;
        fireStart.transform.position = shotPos.position + shotPos.forward;
        fireStart.Play();
    }

    private bool CanSeePlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, (target.position - this.transform.position).normalized, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Player"))))
        {
            return hit.transform.gameObject == target.root.gameObject;
        }
        return false;
    }

 
    //private void OnDrawGizmosSelected()
    //{
    //    if (rigid != null)
    //    {
    //        Gizmos.color = Color.blue;
    //        Vector3 predictedPosition = rigid.position + rigid.velocity * Time.deltaTime;
    //        Gizmos.DrawWireSphere(predictedPosition, sphereCollider.radius);
    //    }

    //    if (Path != null)
    //    {
    //        var path = Path;
    //        for (int i = 0; i < path.Path.Count - 1; i++)
    //        {
    //            Gizmos.color = Color.yellow;
    //            Gizmos.DrawWireSphere(path.Path[i], minReachDistance);
    //            Gizmos.color = Color.red;
    //            Gizmos.DrawRay(path.Path[i], Vector3.ClampMagnitude(rigid.position - path.Path[i], pathPointRadius));
    //            Gizmos.DrawWireSphere(path.Path[i], pathPointRadius);
    //            Gizmos.DrawLine(path.path[i], path.Path[i + 1]);
    //        }
    //    }
    //}
}

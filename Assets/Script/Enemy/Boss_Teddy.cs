using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossBehavior
{
    Idle,
    Dodge,
    FireBullet,
    Meteor
}

public class Boss_Teddy : Enemy
{
    [SerializeField] private BossBehavior behavior;
    [SerializeField] private float maxDistanceRebuildPath = 1;
    [SerializeField] private float acceleration = 1;
    [SerializeField] private float minReachDistance = 2f;
    [SerializeField] private float pathPointRadius = 0.2f;
    [SerializeField] private Octree octree;
    [SerializeField] private Transform shotPos;

    private Octree.PathRequest oldPath;
    private Octree.PathRequest newPath;
    private Vector3 currentDestination;
    private Vector3 lastDestination;
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

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        octree = GameObject.FindGameObjectWithTag("NodeManager").GetComponent<Octree>();
        sphereCollider = this.GetComponent<SphereCollider>();
        originY = this.transform.position.y;
        //playerObject = GameManager.Instance.GetPlayer().gameObject;
    }

    private void Update()
    {
        currentDodgeCoolTime += Time.deltaTime;
        currentAttackDelay += Time.deltaTime;

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

                if(currentMeteorDropCoolTime >= meteorDropCoolTime)
                {
                    currentMeteorDropCoolTime = 0;

                    //Debug.Log(Random.Range(0, containers.Count));
                    int[] rndNum = new int[containers.Count];
                    for(int i = 0; i < containers.Count; i++)
                    {
                        rndNum[i] = i;
                    }

                    for(int i = 0; i < containers.Count; i++)
                    {
                        int rnd = Random.Range(0, containers.Count);

                        int temp = rndNum[i];
                        rndNum[i] = rndNum[rnd];
                        rndNum[rnd] = temp;
                    }

                    containers[dropNum].GetComponent<Boss_Teddy_Container>().SetDropReady(target.position, 1.0f);
                    dropNum++;
                    dropNum = Mathf.Clamp(dropNum, 0, containers.Count - 1);

                    //int rnd = 0;

                    //while (true)
                    //{
                    //    if (containers[rnd].GetComponent<Boss_Teddy_Container>().SetDropReady(target.position, 1.0f))
                    //    {
                    //        break;
                    //    }

                    //    rnd = Random.Range(0, containers.Count);
                    //}

                    //for (int i = 0; i < containers.Count; i++)
                    //{

                    //container.GetComponent<Boss_Teddy_Container>().SetDropReady(target.position, 1.0f);
                    //}
                }
            }
        }

        if (behavior != BossBehavior.Meteor)
        {
            currentMeteorCoolTime += Time.deltaTime;

            if (currentDodgeCoolTime >= dodgeCoolTime)
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
    }

    void UpdateBehavior()
    {



        if (currentMeteorCoolTime >= meteorCoolTime)
        {
            ResetTrigger();
            behavior = BossBehavior.Meteor;
            anim.SetBool("isMeteor", true);
            currentMeteorCoolTime = 0;
            return;
        }

        if (currentAttackDelay >= attackDelay)
        {
            if (Vector3.Distance(this.transform.position, target.position) <= stoppingDistance)
            {
                behavior = BossBehavior.FireBullet;
                ResetTrigger();
                anim.SetTrigger("FireBullet");
                currentAttackDelay = 0;

                return;
            }
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
    }

    private void OnAnimatorMove()
    {
        rootMotionPos += anim.deltaPosition;
        rootMotionRot += anim.deltaRotation.eulerAngles;
    }

    private void FixedUpdate()
    {

         if (CanSeePlayer())
        {
            //if (Vector3.Distance(this.transform.position, target.position) <= stoppingDistance)
            //{
            //    if (currentAttackDelay >= attackDelay)
            //    {
            //        anim.SetTrigger("FireBullet");
            //        currentAttackDelay = 0;
            //    }

            //    this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, originY, this.transform.position.z), Time.deltaTime * 12);
            //    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation((target.position - this.transform.position).normalized), Time.deltaTime * 12);
            //    rigid.velocity = Vector3.Lerp(rigid.velocity, Vector3.zero, Time.deltaTime * 10);

            //    return;
            //}

            if (behavior == BossBehavior.Idle)
            {
                Vector3 dir = (target.position - transform.position).normalized;

                if (dir == Vector3.zero)
                    dir = this.transform.forward;

                move = Vector3.Lerp(move, dir, Time.deltaTime * 10);
                move.y = 0;

                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 12);
                rigid.velocity = move * acceleration * 1.5f;
            }
            //else
            //{
            //    if(behavior == BossBehavior.FireBullet || behavior == BossBehavior.Meteor)
            //    {
            //        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, originY, this.transform.position.z), Time.deltaTime * 12);
            //        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation((target.position - this.transform.position).normalized), Time.deltaTime * 12);
            //        rigid.velocity = Vector3.Lerp(rigid.velocity, Vector3.zero, Time.deltaTime * 10);
            //    }
            //}
        }
        else
        {
            //line.enabled = false;
            if ((newPath == null || !newPath.isCalculating) && Vector3.SqrMagnitude(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z) - lastDestination) > maxDistanceRebuildPath && !octree.IsBuilding)
            {
                lastDestination = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);

                oldPath = newPath;
                newPath = octree.GetPath(transform.position, lastDestination);
                newPath.path.Add(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));
            }

            var curPath = Path;

            if (!curPath.isCalculating && curPath != null && curPath.Path.Count > 0)
            {
                currentDestination = curPath.Path[0] + Vector3.ClampMagnitude(rigid.position - curPath.Path[0], pathPointRadius);

                Vector3 dir = (currentDestination - transform.position).normalized;

                if (dir == Vector3.zero)
                    dir = this.transform.forward;

                rigid.velocity = Vector3.MoveTowards(rigid.velocity, dir * acceleration, Time.deltaTime * 12);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation((target.position - this.transform.position).normalized), Time.deltaTime * 12);
                float sqrMinReachDistance = minReachDistance * minReachDistance;

                Vector3 predictedPosition = rigid.position + rigid.velocity * Time.deltaTime;
                float shortestPathDistance = Vector3.SqrMagnitude(predictedPosition - currentDestination);
                int shortestPathPoint = 0;

                for (int i = 0; i < curPath.Path.Count; i++)
                {
                    float sqrDistance = Vector3.SqrMagnitude(rigid.position - curPath.Path[i]);
                    if (sqrDistance <= sqrMinReachDistance)
                    {
                        if (i < curPath.Path.Count)
                        {
                            curPath.Path.RemoveRange(0, i + 1);
                        }
                        shortestPathPoint = 0;
                        break;
                    }

                    float sqrPredictedDistance = Vector3.SqrMagnitude(predictedPosition - curPath.Path[i]);
                    if (sqrPredictedDistance < shortestPathDistance)
                    {
                        shortestPathDistance = sqrPredictedDistance;
                        shortestPathPoint = i;
                    }
                }

                if (shortestPathPoint > 0)
                {
                    curPath.Path.RemoveRange(0, shortestPathPoint);
                }
            }
            else
            {
                rigid.velocity -= rigid.velocity * Time.deltaTime * acceleration;
            }
        }

        if (behavior == BossBehavior.FireBullet || behavior == BossBehavior.Meteor)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, originY, this.transform.position.z), Time.deltaTime * 12);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation((target.position - this.transform.position).normalized), Time.deltaTime * 12);
            rigid.velocity = Vector3.Lerp(rigid.velocity, Vector3.zero, Time.deltaTime * 10);
        }

        this.transform.position = transform.position + rootMotionPos;
        this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles + rootMotionRot);

        rootMotionPos = Vector3.zero;
        rootMotionRot = Vector3.zero;
    }

    private void FireBullet()
    {
        GameObject temp = Instantiate(bullet, shotPos.position, Quaternion.LookRotation((target.position - this.transform.position).normalized));
        temp.GetComponent<Bullet_Boss>().Fire((target.position - shotPos.position).normalized, bulletSpeed);
    }

    private bool CanSeePlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, (target.position - this.transform.position).normalized, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Player"))))
        {
            return hit.transform.gameObject == target.gameObject;
        }
        return false;
    }

    private Octree.PathRequest Path
    {
        get
        {
            if ((newPath == null || newPath.isCalculating) && oldPath != null)
            {
                return oldPath;
            }
            return newPath;
        }
    }

    public bool HasTarget
    {
        get
        {
            return Path != null && Path.Path.Count > 0;

        }
    }

    public Vector3 CurrentTargetPosition
    {
        get
        {
            if (Path != null && Path.Path.Count > 0)
            {
                return currentDestination;
            }
            else
            {
                return new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (rigid != null)
        {
            Gizmos.color = Color.blue;
            Vector3 predictedPosition = rigid.position + rigid.velocity * Time.deltaTime;
            Gizmos.DrawWireSphere(predictedPosition, sphereCollider.radius);
        }

        if (Path != null)
        {
            var path = Path;
            for (int i = 0; i < path.Path.Count - 1; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(path.Path[i], minReachDistance);
                Gizmos.color = Color.red;
                Gizmos.DrawRay(path.Path[i], Vector3.ClampMagnitude(rigid.position - path.Path[i], pathPointRadius));
                Gizmos.DrawWireSphere(path.Path[i], pathPointRadius);
                Gizmos.DrawLine(path.path[i], path.Path[i + 1]);
            }
        }
    }
}

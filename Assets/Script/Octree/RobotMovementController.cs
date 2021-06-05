using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotMovementController : Enemy
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
    [SerializeField] private float maxDistanceRebuildPath = 1;
	[SerializeField] private float acceleration = 1;
    private float currentSpeed;
	[SerializeField] private float minReachDistance = 2f;
	[SerializeField] private float pathPointRadius = 0.2f;
	[SerializeField] private Octree octree;
	//[SerializeField] private LayerMask playerSeeLayerMask = -1;
    [SerializeField] private float explosionRadius = 5.0f;
    [SerializeField] private ParticleSystem explosion;

	private Octree.PathRequest oldPath;
	private Octree.PathRequest newPath;
	private Vector3 currentDestination;
	private Vector3 lastDestination;
	[SerializeField] private SphereCollider sphereCollider;
    Vector3 move;

    //LineRenderer line;
    // Use this for initialization
    override protected void Start ()
	{
		base.Start();
        acceleration = Random.Range(speed_min, speed_max);
        target = GameManager.Instance.GetPlayer().GetCamPos();
		//sphereCollider = GetComponent<SphereCollider>();
		octree = GameObject.FindGameObjectWithTag("NodeManager").GetComponent<Octree>();
        //line = this.GetComponent<LineRenderer>();
        
        explosion = Instantiate(explosion);
        explosion.gameObject.SetActive(false);

        SetDead(false);
    }

    public override void SetDead(bool value)
    {
        isDead = value;
        if (isDead)
        {
            GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion, this.transform.position, false);
            behavior = Enemy_Behavior.Idle;
            state = Enemy_State.None;

            currentHp = maxHp;
            //this.GetComponent<Collider>().enabled = false;

            //rigid.useGravity = false;
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            explosion.transform.position = this.GetComponent<Enemy_RagdollController>().spineRigid.position;
            explosion.gameObject.SetActive(true);
            explosion.Play();

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius, (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Root")), QueryTriggerInteraction.Ignore);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null && rb.transform != this.transform)
                {
                    RaycastHit hit2;

                    if (rb.CompareTag("Player"))
                    {
                        if (Physics.Raycast(this.transform.position, (hit.ClosestPoint(explosionPos) - this.transform.position).normalized, out hit2, explosionRadius, 1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Player"), QueryTriggerInteraction.Ignore))
                        {
                            if (LayerMask.LayerToName(hit2.transform.gameObject.layer).Equals("Enviroment"))
                                continue;

                            PlayerController temp = rb.GetComponent<PlayerController>();

                            temp.DecreaseHp(damage);
                        }
                    }
                    else if (rb.CompareTag("Enemy"))
                    {
                        if (Physics.Raycast(this.transform.position, (hit.ClosestPoint(explosionPos) - this.transform.position).normalized, out hit2, explosionRadius, 1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Root")))
                        {
                            if (LayerMask.LayerToName(hit2.transform.gameObject.layer).Equals("Enviroment") || hit2.transform.CompareTag("Boss"))
                                continue;

                            if (GameManager.Instance.GetIsCombat())
                            {
                                Enemy temp = rb.transform.root.GetComponent<Enemy>();

                                temp.DecreaseHp(/*this.gameObject, */10000, hit.ClosestPoint(explosionPos), temp.GetComponent<Enemy_RagdollController>().spineRigid.transform, Vector3.ClampMagnitude((hit.ClosestPoint(explosionPos) - explosionPos).normalized * 100, 100), EffectType.Normal);

                                //rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 1.0F, ForceMode.VelocityChange);
                            }

                        }
                    }
                }
            }

            this.gameObject.SetActive(false);
        }
        else
        {
            if(anim != null)
            anim.enabled = true;
            this.gameObject.SetActive(true);

          
                GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Warning_TypeC, this.transform, true);
        }

        //this.gameObject.SetActive(false);
        //anim.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate ()
	{
        if (isDead)
        {
            //behavior = Enemy_Behavior.Idle;
            //state = Enemy_State.None;
            //currentHp = maxHp;
            //this.gameObject.SetActive(false);

            return;
        }
        else
        {
            if (Vector3.Distance(this.transform.position, target.position) <= 1.0f)
            {
                //playerObject.GetComponent<PlayerController>().DecreaseHp(damage);
                SetDead(true);
            }
        }

        if (isRigidity)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            currentRigidityTime += Time.deltaTime;
            behavior = Enemy_Behavior.Idle;

            AnimFalse();
            if (currentRigidityTime >= rigidityTime)
            {
                isRigidity = false;
                currentRigidityTime = 0;
            }
            else
                return;
        }

        if (this.GetComponent<RoomInfo>().GetRoom() != null && target.root.GetComponent<RoomInfo>().GetRoom() != null)
        {

            if (this.GetComponent<RoomInfo>().GetRoom() == target.root.GetComponent<RoomInfo>().GetRoom())
            {
                state = Enemy_State.Chase;
                anim.SetBool("isChase", true);
            }
        }

        if (state == Enemy_State.None || state == Enemy_State.Return)
            return;

        if (CanSeePlayer())
        {
            Vector3 dir = (target.position - transform.position).normalized;

            if (dir == Vector3.zero)
                dir = this.transform.forward;

            //rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, dir * acceleration, Time.deltaTime * 12);
        

            move = Vector3.Lerp(move, dir, Time.deltaTime * 10);

            rigid.velocity = move * acceleration * 1.5f;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(rigid.velocity), Time.deltaTime * 12);
            //this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + -90, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
            //line.enabled = true;
            //line.SetPosition(0, this.transform.position);
            //line.SetPosition(1, target.position + Vector3.down * 0.1f);
        }
        else
        {
            //line.enabled = false;
            if ((newPath == null || !newPath.isCalculating) && Vector3.SqrMagnitude(target.transform.position - lastDestination) > maxDistanceRebuildPath && !octree.IsBuilding)
            {
                lastDestination = target.transform.position;

                oldPath = newPath;
                newPath = octree.GetPath(transform.position, lastDestination);
                newPath.path.Add(target.position);
            }

            var curPath = Path;

            if (!curPath.isCalculating && curPath != null && curPath.Path.Count > 0)
            {
                currentDestination = curPath.Path[0] + Vector3.ClampMagnitude(rigid.position - curPath.Path[0], pathPointRadius);

                Vector3 dir = (currentDestination - transform.position).normalized;

                if (dir == Vector3.zero)
                    dir = this.transform.forward;

                rigid.velocity = Vector3.MoveTowards(rigid.velocity, dir * acceleration, Time.deltaTime * 12);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 12);
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


        //if (CanSeePlayer())
        //{
        //    var curPath = Path;
        //    curPath.Reset();
        //    //rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, (target.position - transform.position).normalized * acceleration, Time.deltaTime * 12);
        //    rigidbody.velocity += Vector3.ClampMagnitude(target.position - transform.position, 1) * Time.deltaTime * acceleration;
        //    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(target.position - this.transform.position), Time.deltaTime * 12);
        //}
        //else
        //{
        //    if ((newPath == null || !newPath.isCalculating) && Vector3.SqrMagnitude(target.transform.position - lastDestination) > maxDistanceRebuildPath && !octree.IsBuilding)
        //    {
        //        lastDestination = target.transform.position;

        //        oldPath = newPath;
        //        newPath = octree.GetPath(transform.position, lastDestination);
        //    }
        //    var curPath = Path;
        //    if (!curPath.isCalculating && curPath != null && curPath.Path.Count > 0)
        //    {
        //        currentDestination = curPath.Path[0] + Vector3.ClampMagnitude(rigidbody.position - curPath.Path[0], pathPointRadius);

        //        rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, (currentDestination - transform.position).normalized * acceleration, Time.deltaTime * 12);
        //        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(currentDestination - this.transform.position), Time.deltaTime * 12);
        //        float sqrMinReachDistance = minReachDistance * minReachDistance;

        //        Vector3 predictedPosition = rigidbody.position + rigidbody.velocity * Time.deltaTime;
        //        float shortestPathDistance = Vector3.SqrMagnitude(predictedPosition - currentDestination);
        //        int shortestPathPoint = 0;

        //        for (int i = 0; i < curPath.Path.Count; i++)
        //        {
        //            float sqrDistance = Vector3.SqrMagnitude(rigidbody.position - curPath.Path[i]);
        //            if (sqrDistance <= sqrMinReachDistance)
        //            {
        //                if (i < curPath.Path.Count)
        //                {
        //                    curPath.Path.RemoveRange(0, i + 1);
        //                }
        //                shortestPathPoint = 0;
        //                break;
        //            }

        //            float sqrPredictedDistance = Vector3.SqrMagnitude(predictedPosition - curPath.Path[i]);
        //            if (sqrPredictedDistance < shortestPathDistance)
        //            {
        //                shortestPathDistance = sqrPredictedDistance;
        //                shortestPathPoint = i;
        //            }
        //        }

        //        if (shortestPathPoint > 0)
        //        {
        //            curPath.Path.RemoveRange(0, shortestPathPoint);
        //        }
        //    }
        //}

    }

    void AnimFalse()
    {
        anim.SetBool("isChase", false);
    }

    private bool CanSeePlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, (target.position - this.transform.position).normalized, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Ignore))
        {
            return hit.transform.gameObject == target.root.gameObject;
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
                return target.position;
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

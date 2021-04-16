using System;
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
	[SerializeField] private LayerMask playerSeeLayerMask = -1;
	[SerializeField] private GameObject playerObject;
	private Octree.PathRequest oldPath;
	private Octree.PathRequest newPath;
	private Rigidbody rigidbody;
	private Vector3 currentDestination;
	private Vector3 lastDestination;
	private SphereCollider sphereCollider;
    Vector3 move;
    // Use this for initialization
    override protected void Start ()
	{
		base.Start();

		target = GameManager.Instance.GetPlayer().GetCamPos();
		sphereCollider = GetComponent<SphereCollider>();
		rigidbody = GetComponent<Rigidbody>();
		playerObject = GameManager.Instance.GetPlayer().gameObject;
		octree = GameObject.FindGameObjectWithTag("NodeManager").GetComponent<Octree>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        if (isDead)
        {
            behavior = Enemy_Behavior.Idle;
            state = Enemy_State.None;
            this.gameObject.SetActive(false);

            return;
        }
        else
        {
            if (Vector3.Distance(this.transform.position, target.position) <= 1.0f)
            {
                playerObject.GetComponent<PlayerController>().DecreaseHp(damage);
                DecreaseHp(10000, false);
            }
        }

        if (CanSeePlayer())
        {
            Vector3 dir = (target.position - transform.position).normalized;

            if (dir == Vector3.zero)
                dir = this.transform.forward;

            //rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, dir * acceleration, Time.deltaTime * 12);
        

            move = Vector3.Lerp(move, dir, Time.deltaTime * 10);

            rigidbody.velocity = move * acceleration * 1.5f;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(rigidbody.velocity), Time.deltaTime * 12);
        }
        else
        {

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
                currentDestination = curPath.Path[0] + Vector3.ClampMagnitude(rigidbody.position - curPath.Path[0], pathPointRadius);

                Vector3 dir = (currentDestination - transform.position).normalized;

                if (dir == Vector3.zero)
                    dir = this.transform.forward;

                rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, dir * acceleration, Time.deltaTime * 12);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 12);
                float sqrMinReachDistance = minReachDistance * minReachDistance;

                Vector3 predictedPosition = rigidbody.position + rigidbody.velocity * Time.deltaTime;
                float shortestPathDistance = Vector3.SqrMagnitude(predictedPosition - currentDestination);
                int shortestPathPoint = 0;

                for (int i = 0; i < curPath.Path.Count; i++)
                {
                    float sqrDistance = Vector3.SqrMagnitude(rigidbody.position - curPath.Path[i]);
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
                rigidbody.velocity -= rigidbody.velocity * Time.deltaTime * acceleration;
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

    private bool CanSeePlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, (target.position - this.transform.position).normalized, out hit, Mathf.Infinity))
        {
            return hit.transform.gameObject == playerObject;
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
        if (rigidbody != null)
        {
            Gizmos.color = Color.blue;
            Vector3 predictedPosition = rigidbody.position + rigidbody.velocity * Time.deltaTime;
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
                Gizmos.DrawRay(path.Path[i], Vector3.ClampMagnitude(rigidbody.position - path.Path[i], pathPointRadius));
                Gizmos.DrawWireSphere(path.Path[i], pathPointRadius);
                Gizmos.DrawLine(path.path[i], path.Path[i + 1]);
            }
        }
    }
}

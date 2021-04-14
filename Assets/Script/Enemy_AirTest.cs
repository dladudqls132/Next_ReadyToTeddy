using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AirTest : Enemy
{
    enum Enemy_Behavior
    {
        Idle,
        Move,
        Aiming,
        Attack
    }

    [SerializeField] private NodeManager nodeManager;
    private List<Node> path = new List<Node>();
    [SerializeField] private float pathFindingDelay;
    [SerializeField] private float shotDelay;
    [SerializeField] private float currentShotDelay;

    HashSet<Node> closeList = new HashSet<Node>();
    List<Node> openList = new List<Node>();

    [SerializeField] private Enemy_Behavior behavior;
    private Vector3 destPos;
    private Node[][][] node;
    private LineRenderer laser;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform aimPos;
    [SerializeField] private float speed;
    float currentSpeed;

    Vector3 move;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        nodeManager = GameObject.FindGameObjectWithTag("NodeManager").GetComponent<NodeManager>();
        node = nodeManager.node;

        currentShotDelay = shotDelay;
        laser = this.GetComponent<LineRenderer>();
        target = GameManager.Instance.GetPlayer().GetCamPos();
        aimPos = GameObject.Find("Player_targetPos").transform;

        StartCoroutine(PathFinding());
    }

    private void Update()
    {
        if (Vector3.Distance(this.transform.position, target.position) > detectRange && state == Enemy_State.None)
            return;


        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, (target.position - this.transform.position).normalized, out hit, attackRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                canSee = true;
                state = Enemy_State.Targeting;
            }
            else
            {
                canSee = false;
                state = Enemy_State.Search;
            }
        }
        else
        {
            state = Enemy_State.Search;
        }


        if (state == Enemy_State.Targeting)
        {
            if (Vector3.Distance(this.transform.position, target.position) <= attackRange)
                behavior = Enemy_Behavior.Aiming;
            else
            {
                if (behavior != Enemy_Behavior.Aiming && behavior != Enemy_Behavior.Attack)
                {
                    behavior = Enemy_Behavior.Move;
                }
            }
        }
        else if (state == Enemy_State.Search)
        {
            if (behavior != Enemy_Behavior.Aiming && behavior != Enemy_Behavior.Attack)
            {
                behavior = Enemy_Behavior.Move;
            }
        }

        currentShotDelay -= Time.deltaTime;

        if (behavior == Enemy_Behavior.Aiming)
        {
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
                    float rayScale = 0.1f * (currentShotDelay / shotDelay);

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
            laser.startWidth = 0;
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
            if (state == Enemy_State.Targeting)
                behavior = Enemy_Behavior.Aiming;
            else
                behavior = Enemy_Behavior.Idle;
        }

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(target.position - this.transform.position), Time.deltaTime * 12.0f);

        if (behavior == Enemy_Behavior.Move)
        {
            if (path.Count > 0)
            {
                if (canSee)
                {
                    Vector3 dir = this.transform.forward;

                    move = Vector3.Lerp(move, dir, Time.deltaTime * 10);

                    this.transform.position = this.transform.position + move * Time.deltaTime * speed;
                }
                else
                {
                    Vector3 dir = (path[path.Count - 1].position - this.transform.position).normalized;

                    move = Vector3.Lerp(move, dir, Time.deltaTime * 10);

                    this.transform.position = this.transform.position + move * Time.deltaTime * speed;
                }
            }
        }
        else
        {
            move = Vector3.Lerp(move, Vector3.zero, Time.deltaTime * 10);

            this.transform.position = this.transform.position + move * Time.deltaTime * speed;
        }

        //if (behavior == Enemy_Behavior.Move)
        //{
        //    if (path.Count > 1)
        //    {
        //        Vector3 dir = (path[path.Count - 1].position - this.transform.position).normalized;
        //        move = Vector3.Lerp(move, dir, Time.deltaTime);
        //        this.transform.position = this.transform.position + move * Time.deltaTime * speed;
        //    }
        //}
        //else
        //{
        //    Vector3 dir = Vector3.zero;
        //    move = Vector3.Lerp(move, dir, Time.deltaTime);
        //    this.transform.position = this.transform.position + move * Time.deltaTime * speed;
        //}
    }

    // Update is called once per frame
    IEnumerator PathFinding()
    {
        while (true)
        {
            yield return new WaitForSeconds(pathFindingDelay);

            if (behavior != Enemy_Behavior.Move)
                continue;

            Vector3 temp = this.transform.position - nodeManager.transform.position;
            Node startNode = node[Mathf.RoundToInt(Mathf.Abs(temp.x))][Mathf.RoundToInt(Mathf.Abs(temp.y))][Mathf.RoundToInt(Mathf.Abs(temp.z))];
            Node targetNode = GetNode(target.position);

            if (targetNode.nodeType == NodeType.Wall)
            {
                bool changed = false;
                foreach (Node neighborNode in GetNeighborNode(targetNode))
                {
                    if (neighborNode.nodeType == NodeType.Wall)
                    {
                        continue;
                    }
                    else
                    {
                        changed = true;
                        targetNode = neighborNode;
                    }
                }

                if (!changed)
                    continue;
            }

            closeList.Clear();
            openList.Clear();

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node currentNode = openList[0];

                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].f < currentNode.f || (openList[i].f == currentNode.f && openList[i].h < currentNode.h))
                        currentNode = openList[i];
                }

                openList.Remove(currentNode);
                closeList.Add(currentNode);

                if (currentNode == targetNode)
                {
                    GetFinalPath(startNode, targetNode);
                    break;
                }

                foreach (Node neighborNode in GetNeighborNode(currentNode))
                {
                    if (closeList.Contains(neighborNode) || neighborNode.nodeType == NodeType.Wall)
                    {
                        continue;
                    }

                    if (!openList.Contains(neighborNode))
                    {
                        neighborNode.parent = currentNode;
                        neighborNode.g = currentNode.g + GetManhattenDistance(currentNode, neighborNode);
                        neighborNode.h = GetManhattenDistance(neighborNode, targetNode);

                        openList.Add(neighborNode);
                    }
                    else
                    {
                        if (currentNode.g + GetManhattenDistance(currentNode, neighborNode) < neighborNode.g)
                        {
                            neighborNode.parent = currentNode;
                            neighborNode.g = currentNode.g + GetManhattenDistance(currentNode, neighborNode);
                        }
                    }
                }
            }
        }
    }

    Node GetNode(Vector3 pos)   //position에 맞는 node 반환
    {
        Vector3 temp = pos - nodeManager.transform.position;
        return node[Mathf.RoundToInt(Mathf.Abs(temp.x))][Mathf.RoundToInt(Mathf.Abs(temp.y))][Mathf.RoundToInt(Mathf.Abs(temp.z))];
    }

    List<Node> neighborNode = new List<Node>();

    List<Node> GetNeighborNode(Node node)   //근처 노드 반환
    {
        neighborNode.Clear();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                for (int k = -1; k < 2; k++)
                {
                    if (node.nodePosX > 0 && node.nodePosX < nodeManager.gridSizeX - 1 &&
                        node.nodePosY > 0 && node.nodePosY < nodeManager.gridSizeY - 1 &&
                        node.nodePosZ > 0 && node.nodePosZ < nodeManager.gridSizeZ - 1)
                    {
                        neighborNode.Add(this.node[node.nodePosX + i][node.nodePosY + j][node.nodePosZ + k]);
                    }
                }
            }
        }

        return neighborNode;
    }

    float GetManhattenDistance(Node startNode, Node endNode)
    {
        Vector3 temp = endNode.position - startNode.position;
        float result = 0;
        result += Mathf.Abs(temp.x);
        result += Mathf.Abs(temp.y);
        result += Mathf.Abs(temp.z);
        return result;
    }

    void GetFinalPath(Node startNode, Node targetNode)  //최종 경로 입력
    {
        Node currentNode = targetNode;

        while (currentNode.parent != startNode && currentNode.parent != null)
        {
            path.Add(currentNode.parent);
            currentNode = currentNode.parent;
        }
    }
}

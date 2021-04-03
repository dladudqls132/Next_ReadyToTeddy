using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AirTest : MonoBehaviour
{
    [SerializeField] private NodeManager nodeManager;
    [SerializeField] private Transform target;
    [SerializeField] private List<Node> path = new List<Node>();
    [SerializeField] private GameObject pathObject;
    [SerializeField] private float pathFindingDelay;

    private Vector3 destPos;
    private Node[][][] node;

    // Start is called before the first frame update
    void Start()
    {
        node = nodeManager.node;

        StartCoroutine(PathFinding());
    }

    private void Update()
    {
        if(path.Count > 0)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, path[path.Count - 1].position, Time.deltaTime * 3);
        }
    }

    // Update is called once per frame
    IEnumerator PathFinding()
    {
        while (true)
        {
            yield return new WaitForSeconds(pathFindingDelay);
            int t = 0;

            Vector3 temp = this.transform.position - nodeManager.transform.position;
            Node startNode = node[Mathf.RoundToInt(Mathf.Abs(temp.x))][Mathf.RoundToInt(Mathf.Abs(temp.y))][Mathf.RoundToInt(Mathf.Abs(temp.z))];
            Node targetNode = GetNode(target.position);

            if (targetNode.nodeType == NodeType.Wall)
                continue;

            HashSet<Node> closeList = new HashSet<Node>();
            List<Node> openList = new List<Node>();

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                t++;
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
                        if (currentNode.g + 10 < neighborNode.g)
                        {
                            neighborNode.parent = currentNode;
                            neighborNode.g = currentNode.g + 10;
                        }
                    }
                }
            }
            Debug.Log(t);
        }
    }

    Node GetNode(Vector3 pos)
    {
        Vector3 temp = pos - nodeManager.transform.position;
        return node[Mathf.RoundToInt(Mathf.Abs(temp.x))][Mathf.RoundToInt(Mathf.Abs(temp.y))][Mathf.RoundToInt(Mathf.Abs(temp.z))];
    }

    List<Node> GetNeighborNode(Node node)
    {
        List<Node> neighborNode = new List<Node>();

        if (node.nodePosX > 0)
            neighborNode.Add(this.node[node.nodePosX - 1][node.nodePosY][node.nodePosZ]);
        if (node.nodePosX < nodeManager.gridSizeX - 1)
            neighborNode.Add(this.node[node.nodePosX + 1][node.nodePosY][node.nodePosZ]);
        if (node.nodePosY > 0)
            neighborNode.Add(this.node[node.nodePosX][node.nodePosY - 1][node.nodePosZ]);
        if (node.nodePosY < nodeManager.gridSizeY - 1)
            neighborNode.Add(this.node[node.nodePosX][node.nodePosY + 1][node.nodePosZ]);
        if (node.nodePosZ > 0)
            neighborNode.Add(this.node[node.nodePosX][node.nodePosY][node.nodePosZ - 1]);
        if (node.nodePosZ < nodeManager.gridSizeZ - 1)
            neighborNode.Add(this.node[node.nodePosX][node.nodePosY][node.nodePosZ + 1]);

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

    void GetFinalPath(Node startNode, Node targetNode)
    {
        Node currentNode = targetNode;

        while (currentNode.parent != startNode)
        {
            path.Add(currentNode.parent);
            currentNode = currentNode.parent;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NodeManager : MonoBehaviour
{
    public int gridSizeX;
    public int gridSizeY;
    public int gridSizeZ;
    public float nodeRadius;

    private bool isStart;

    public Node[][][] node;

    // Start is called before the first frame update
    void Awake()
    {
        node = new Node[gridSizeX][][];

        for(int i = 0; i < gridSizeX; i++)
        {
            node[i] = new Node[gridSizeY][];
            for (int j = 0; j < gridSizeY; j++)
            {
                node[i][j] = new Node[gridSizeZ];
                for(int k = 0; k < gridSizeZ; k++)
                {
                    node[i][j][k] = new Node(this.transform.position + new Vector3(i, j, k));
                    node[i][j][k].SetNodePos(i, j, k);

                    if (Physics.CheckSphere(node[i][j][k].position, nodeRadius, 1 << LayerMask.NameToLayer("Enviroment")))
                    {
                        node[i][j][k].nodeType = NodeType.Wall;
                    }
                    else
                        node[i][j][k].nodeType = NodeType.None;

                }
            }
        }

        isStart = true;
    }

    //private void OnDrawGizmos()
    //{
  
    //    for (int i = 0; i < gridSizeX; i++)
    //    {
    //        for (int j = 0; j < gridSizeY; j++)
    //        {
    //            for (int k = 0; k < gridSizeZ; k++)
    //            {
    //                if (isStart)
    //                {
    //                    if (node[i][j][k].nodeType == NodeType.None)
    //                        Gizmos.color = Color.green;
    //                    else
    //                        Gizmos.color = Color.red;

    //                    Gizmos.DrawWireCube(node[i][j][k].position, Vector3.one);
    //                }
    //            }
    //        }
    //    }
    //}
}

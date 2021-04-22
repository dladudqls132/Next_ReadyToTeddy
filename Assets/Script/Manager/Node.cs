using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    None,
    Wall
}

public class Node
{
    public Vector3 position;
    public int nodePosX, nodePosY, nodePosZ;

    public NodeType nodeType;
    public Node parent;

    public float g, h;
    public float f { get { return g + h; } }

    public Node(Vector3 pos)
    {
        position = pos;
        nodeType = NodeType.None;
        parent = null;

        g = h = 0;
    }

    public void SetNodePos(int x, int y, int z)
    {
        nodePosX = x;
        nodePosY = y;
        nodePosZ = z;
    }
}

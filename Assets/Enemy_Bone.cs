using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bone : MonoBehaviour
{
    public Transform root;
    // Start is called before the first frame update
    void Start()
    {
        root = this.transform;

        while(!root.CompareTag("Enemy"))
        {
            root = root.parent;
        }
    }
}

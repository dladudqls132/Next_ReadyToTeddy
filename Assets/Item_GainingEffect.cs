using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_GainingEffect : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.LookRotation(Vector3.up);
    }
}

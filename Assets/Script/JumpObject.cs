using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().SetIsJumpByObject(true, 10);
            //other.GetComponent<Rigidbody>().AddForce(this.transform.up * 10, ForceMode.VelocityChange);
        }
    }
}

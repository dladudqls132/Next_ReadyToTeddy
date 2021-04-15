using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    [SerializeField] private float jumpPower = 15;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().SetIsJumpByObject(true, jumpPower);
            other.GetComponent<PlayerController>().SetCanJump(true);
            //other.GetComponent<Rigidbody>().AddForce(this.transform.up * 10, ForceMode.VelocityChange);
        }
    }
}

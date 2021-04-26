using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_RagdollController : MonoBehaviour
{
    [SerializeField] private Rigidbody spineRigid;

    public void AddForce(Vector3 force)
    {
        spineRigid.AddForce(force, ForceMode.Impulse);
    }
}

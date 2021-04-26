using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_RagdollController : MonoBehaviour
{
    [SerializeField] private Rigidbody spineRigid;
    private float disappearTime = 10.0f;
    private float currentDisappearTime;

    private void Update()
    {
        currentDisappearTime += Time.deltaTime;

        if(currentDisappearTime >= disappearTime)
        {
            currentDisappearTime = 0;
            this.gameObject.SetActive(false);
        }
    }

    public void AddForce(Vector3 force)
    {
        spineRigid.AddForce(force, ForceMode.Impulse);
    }
}

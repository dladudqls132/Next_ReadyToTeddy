using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    private Transform target;
    private bool canRot;

    private void Start()
    {
        target = GameManager.Instance.GetPlayer().transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (canRot)
        {
            Vector3 dir = (target.position + target.GetComponent<Rigidbody>().velocity +  - this.transform.position).normalized;
            dir.y = 0;
            this.transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void SetCanRot(bool value) { canRot = value; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematicTest : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    [SerializeField] private float speed = 0;

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        this.transform.position = this.transform.position + new Vector3(h, 0, v) * Time.deltaTime * speed;

        Vector3 dir = (target.transform.position - this.transform.position).normalized;
        Quaternion rot = Quaternion.FromToRotation(Vector3.left, dir);
        Quaternion rot2 = Quaternion.Euler(90, rot.eulerAngles.y, rot.eulerAngles.z);
        this.transform.rotation = rot2;

    }
}

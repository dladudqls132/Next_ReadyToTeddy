using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Magazine : MonoBehaviour
{
    private Vector3 originPos;
    private Quaternion originRot;
    private Transform originParent;

    // Start is called before the first frame update
    void Start()
    {
        originParent = this.transform.parent;
        originPos = this.transform.localPosition;
        originRot = this.transform.localRotation;
    }

    public void ResetInfo()
    {
        this.transform.parent = originParent;
        this.transform.localPosition = originPos;
        this.transform.localRotation = originRot;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Magazine : MonoBehaviour
{
    [SerializeField]
    private Vector3 originPos;
    [SerializeField]
    private Quaternion originRot;
    [SerializeField]
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

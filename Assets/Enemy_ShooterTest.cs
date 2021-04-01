using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Enemy_ShooterTest : Enemy
{
    [SerializeField] private bool isAiming;
    [SerializeField] private Rig aimRig;
    [SerializeField] private Transform target;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckingHp();
        if (isAiming)
        {
            aimRig.weight = Mathf.Lerp(aimRig.weight, 1, Time.deltaTime * 15);

        }
        else
        {
            aimRig.weight = Mathf.Lerp(aimRig.weight, 0, Time.deltaTime * 15);
        }
        //if (Vector3.Dot(this.transform.forward, target.position - this.transform.position) > 0.5f)
        //{
        //    if (isAiming)
        //    {
        //        aimRig.weight = Mathf.Lerp(aimRig.weight, 1, Time.deltaTime * 15);

        //    }
        //    else
        //    {
        //        aimRig.weight = Mathf.Lerp(aimRig.weight, 0, Time.deltaTime * 15);
        //    }

        //    anim.SetBool("isAiming", isAiming);
        //}
        //else
        //{
        //    aimRig.weight = Mathf.Lerp(aimRig.weight, 0, Time.deltaTime * 10);

        //    anim.SetBool("isAiming", false);
        //}

        Quaternion temp = Quaternion.LookRotation(target.transform.position - this.transform.position);
        temp = Quaternion.Euler(0, temp.eulerAngles.y, 0);
        this.transform.rotation = temp;

        anim.SetBool("isAiming", isAiming);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_Energyball : Boss_Skill
{
    [SerializeField] private Transform body;
    [SerializeField] private Transform target;
    [SerializeField] private Transform hand_left;
    [SerializeField] private Transform hand_right;

    private Quaternion tempRot_body;
    private Quaternion tempRot_leftHand;
    private Quaternion tempRot_rightHand;
    private Vector3 originRot_leftHand;
    private Vector3 originRot_rightHand;

    protected override void Start()
    {
        originRot_leftHand = hand_left.localEulerAngles;
        originRot_rightHand = hand_right.localEulerAngles;

        tempRot_body = body.rotation;
        tempRot_leftHand = hand_left.rotation;
        tempRot_rightHand = hand_right.rotation;

        base.Start();
    }

    public override void Use()
    {
        base.Use();

        tempRot_body = body.rotation;
        tempRot_leftHand = hand_left.rotation;
        tempRot_rightHand = hand_right.rotation;
    }

    protected override void ResetInfo()
    {
        anim.SetBool("isAttack_EnergyBall", false);

        this.GetComponent<Boss_TypeX>().SetRot(tempRot_body, tempRot_leftHand, tempRot_rightHand);

        base.ResetInfo();
    }

    public override void Update()
    {
        currentAttackTime -= Time.deltaTime;

        anim.SetBool("isAttack_EnergyBall", true);

        if (currentAttackTime <= 0)
        {
            ResetInfo();
        }
    }

    //private void LateUpdate()
    //{
       
    //        Vector3 dir = (target.position - this.transform.position).normalized;
    //        tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(dir), Time.deltaTime * 10);
    //        body.rotation = Quaternion.Euler(tempRot_body.eulerAngles + body.rotation.eulerAngles;

    //        dir = (target.position - hand_left.position).normalized;
    //        tempRot_leftHand = Quaternion.Lerp(tempRot_leftHand, Quaternion.LookRotation(dir) * Quaternion.Euler(originRot_leftHand), Time.deltaTime * 10);

    //        hand_left.rotation = tempRot_leftHand;

    //        dir = (target.position - hand_right.position).normalized;
    //        tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, Quaternion.LookRotation(dir) * Quaternion.Euler(originRot_rightHand), Time.deltaTime * 10);

    //        hand_right.rotation = tempRot_rightHand;
        

    //}
}

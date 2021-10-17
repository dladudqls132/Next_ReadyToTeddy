using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Boss_TypeX_State
{
    Waiting,
    Ready,
    Idle,
    Attack
}

public class Boss_TypeX : MonoBehaviour
{
    [SerializeField] private Boss_TypeX_State state;
    [SerializeField] private Transform body;
    [SerializeField] private Transform target;
    [SerializeField] private Transform hand_left;
    [SerializeField] private Transform hand_right;
  

    private Quaternion tempRot_body;
    private Quaternion tempRot_leftHand;
    private Quaternion tempRot_rightHand;
    private Quaternion originRot_body;
    private Vector3 originRot_leftHand;
    private Vector3 originRot_rightHand;

    private Animator anim;
    [SerializeField] private bool isDetect;
    [SerializeField] private Boss_Skill[] skills;
    [SerializeField] private Boss_Skill currentSkill;

    // Start is called before the first frame update
    void Start()
    {
        state = Boss_TypeX_State.Waiting;
        anim = this.GetComponent<Animator>();

        anim.SetBool("isWaiting", true);
        originRot_body = body.localRotation;
        originRot_leftHand = hand_left.localEulerAngles;
        originRot_rightHand = hand_right.localEulerAngles;

        tempRot_body = body.rotation;
        tempRot_leftHand = hand_left.rotation;
        tempRot_rightHand = hand_right.rotation;

        for(int i = 0; i < skills.Length; i++)
        {
            skills[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            isDetect = true;
        }

        if (!isDetect) return;

        if (anim.GetBool("isWaiting"))
        {
            anim.SetBool("isWaiting", false);
        }

        if (!anim.GetBool("isCombat")) return;


        state = Boss_TypeX_State.Idle;


        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].CoolDown())
            {
                if (currentSkill == null)
                {
                    currentSkill = skills[i];
                    currentSkill.Use();
                }
            }
        }

        if(currentSkill != null)
        {
            state = Boss_TypeX_State.Attack;

            if (!currentSkill.enabled)
            {
                currentSkill = null;
            }
        }
        else
        {
            state = Boss_TypeX_State.Idle;
        }
    }

    private void LateUpdate()
    {
        if (isDetect)
        {

                

                if (anim.GetBool("isAttack_EnergyBall"))
                {
                    Vector3 dir = (target.position - body.position).normalized;
                    Vector3 temp = body.localRotation.eulerAngles - originRot_body.eulerAngles;

                    tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(dir), Time.deltaTime * 10);
                    body.rotation = tempRot_body;
                    body.localRotation = Quaternion.Euler(body.localEulerAngles + temp);

                    dir = (target.position - hand_left.position).normalized;
                    tempRot_leftHand = Quaternion.Lerp(tempRot_leftHand, Quaternion.LookRotation(dir) * Quaternion.Euler(originRot_leftHand), Time.deltaTime * 10);

                    hand_left.rotation = tempRot_leftHand;

                    dir = (target.position - hand_right.position).normalized;
                    tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, Quaternion.LookRotation(dir) * Quaternion.Euler(originRot_rightHand), Time.deltaTime * 10);

                    hand_right.rotation = tempRot_rightHand;
                }
                else
                {
                    Vector3 dir = (target.position - body.position).normalized;
                    tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(dir), Time.deltaTime * 10);
                    body.rotation = tempRot_body;

                    tempRot_leftHand = Quaternion.Lerp(tempRot_leftHand, hand_left.rotation, Time.deltaTime * 10);
                    tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, hand_right.rotation, Time.deltaTime * 10);

                    hand_left.rotation = tempRot_leftHand;
                    hand_right.rotation = tempRot_rightHand;
                }
            
        }

    }
}

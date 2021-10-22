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
    [SerializeField] protected Renderer[] renderers;
    [SerializeField] protected Color emissionColor_normal;
    [SerializeField] protected Color emissionColor_angry;


    private Quaternion tempRot_body;
    private Quaternion tempRot_leftHand;
    private Quaternion tempRot_rightHand;
    private Vector3 tempPos_leftHand;
    private Vector3 tempPos_rightHand;

    private Animator anim;
    [SerializeField] private bool isDetect;
    [SerializeField] private Boss_Skill[] skills;
    [SerializeField] private Boss_Skill currentSkill;
    bool isStuned;
    int faceNum;
    // Start is called before the first frame update
    void Start()
    {
        state = Boss_TypeX_State.Waiting;
        anim = this.GetComponent<Animator>();

        anim.SetBool("isWaiting", true);
        
        tempRot_body = body.localRotation;
        tempRot_leftHand = hand_left.localRotation;
        tempRot_rightHand = hand_right.localRotation;
        tempPos_leftHand = hand_left.localPosition;
        tempPos_rightHand = hand_right.localPosition;

        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            isDetect = true;
        }
        else if(Input.GetKeyDown(KeyCode.U))
        {
            isStuned = !isStuned;
            anim.enabled = !anim.enabled;
            hand_left.GetComponent<Rigidbody>().useGravity = !hand_left.GetComponent<Rigidbody>().useGravity;
            hand_right.GetComponent<Rigidbody>().useGravity = !hand_right.GetComponent<Rigidbody>().useGravity;
            hand_left.GetComponent<Rigidbody>().velocity = Vector3.zero;
            hand_right.GetComponent<Rigidbody>().velocity = Vector3.zero;
            hand_left.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            hand_right.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        }
        else if(Input.GetKeyDown(KeyCode.I))
        {
            faceNum++;
        }

        anim.SetFloat("FaceType", Mathf.Lerp(anim.GetFloat("FaceType"), (float)faceNum / 3, Time.deltaTime * 10));

        if (!isDetect) return;

        foreach (Renderer r in renderers)
        {
            r.material.SetColor("_EmissionColor", Color.Lerp(r.material.GetColor("_EmissionColor"), (emissionColor_angry * 35f), Time.deltaTime * 6));
        }

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

        if (currentSkill != null)
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

    Vector3 tempDir;
    void SetTempBodyDir()
    {
        tempDir = (target.position - body.position).normalized;
        tempDir.y = 0;
    }

    private void LateUpdate()
    {
        if (anim.GetBool("isCombat"))
        {
            //이동
            tempPos_leftHand = Vector3.Lerp(tempPos_leftHand, hand_left.localPosition, Time.deltaTime * 15);
            hand_left.localPosition = tempPos_leftHand;

            tempPos_rightHand = Vector3.Lerp(tempPos_rightHand, hand_right.localPosition, Time.deltaTime * 15);
            hand_right.localPosition = tempPos_rightHand;

            if (isStuned) return;

            if (!anim.GetBool("isIdle"))
            {
                if (currentSkill != null)
                {
                    if (currentSkill.GetComponent<Boss_TypeX_Skill_Energyball>())
                    {
                        Vector3 dir = (target.position - body.position).normalized;

                        tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(dir) * Quaternion.Euler(body.localEulerAngles), Time.deltaTime * 13);
                        body.rotation = tempRot_body;

                        if (anim.GetBool("isAttack_EnergyBall_LeftHand"))
                        {
                            //회전
                            dir = (target.position - hand_left.position).normalized;

                            if (anim.GetBool("isReload"))
                                tempRot_leftHand = Quaternion.Lerp(tempRot_leftHand, hand_left.parent.rotation * Quaternion.Euler(hand_left.localEulerAngles), Time.deltaTime * 13);
                            else
                                tempRot_leftHand = Quaternion.Lerp(tempRot_leftHand, Quaternion.LookRotation(dir) * Quaternion.Euler(hand_left.localEulerAngles), Time.deltaTime * 13);

                            hand_left.rotation = tempRot_leftHand;


                            tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, hand_right.parent.rotation * Quaternion.Euler(hand_right.localEulerAngles), Time.deltaTime * 13);
                            hand_right.rotation = tempRot_rightHand;
                        }
                        else if (anim.GetBool("isAttack_EnergyBall_RightHand"))
                        {
                            //회전
                            dir = (target.position - hand_right.position).normalized;

                            if (anim.GetBool("isReload"))
                                tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, hand_right.parent.rotation * Quaternion.Euler(hand_right.localEulerAngles) , Time.deltaTime * 13);
                            else
                                tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, Quaternion.LookRotation(dir) * Quaternion.Euler(hand_right.localEulerAngles), Time.deltaTime * 13);

                            hand_right.rotation = tempRot_rightHand;

                            tempRot_leftHand = Quaternion.Lerp(tempRot_leftHand, hand_left.parent.rotation * Quaternion.Euler(hand_left.localEulerAngles), Time.deltaTime * 13);
                            hand_left.rotation = tempRot_leftHand;
                        }
                    }
                }
                else
                {
                    //회전
                    Vector3 dir = (target.position - body.position).normalized;
                    dir.y = 0;
                    tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(tempDir) * Quaternion.Euler(body.localEulerAngles), Time.deltaTime * 13);

                    body.rotation = tempRot_body;

                    tempRot_leftHand = Quaternion.Lerp(tempRot_leftHand, hand_left.parent.rotation * Quaternion.Euler(hand_left.localEulerAngles), Time.deltaTime * 53);
                    hand_left.rotation = hand_left.parent.rotation * Quaternion.Euler(hand_left.localEulerAngles);


                    tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, hand_right.parent.rotation * Quaternion.Euler(hand_right.localEulerAngles), Time.deltaTime * 53);
                    hand_right.rotation = hand_right.parent.rotation * Quaternion.Euler(hand_right.localEulerAngles);
                }
            }
            else
            {
                Vector3 dir = (target.position - body.position).normalized;
                tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(dir) * Quaternion.Euler(body.localEulerAngles), Time.deltaTime * 13);
                body.rotation = tempRot_body;

                tempRot_leftHand = Quaternion.Lerp(tempRot_leftHand, hand_left.parent.rotation * Quaternion.Euler(hand_left.localEulerAngles), Time.deltaTime * 13);
                hand_left.rotation = tempRot_leftHand;


                tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, hand_right.parent.rotation * Quaternion.Euler(hand_right.localEulerAngles), Time.deltaTime * 13);
                hand_right.rotation = tempRot_rightHand;
            }
        }
    }
}

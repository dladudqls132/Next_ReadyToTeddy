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
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color emissionColor_normal;
    [SerializeField] private Color emissionColor_angry;
    [SerializeField] private Transform shield_left;
    [SerializeField] private Transform shield_right;

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
    float animLerpSpeed;
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
        if (Input.GetKeyDown(KeyCode.Y)) //발견
        {
            isDetect = true;
        }
        else if(Input.GetKeyDown(KeyCode.U)) //스턴
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
        else if (Input.GetKeyDown(KeyCode.I)) //얼굴 돌리기
        {
            faceNum++;
        }

        anim.SetFloat("FaceType", Mathf.Lerp(anim.GetFloat("FaceType"), (float)faceNum / 3, Time.deltaTime * 10));

        if (!isDetect) return;

        foreach (Renderer r in renderers)
        {
            r.material.SetColor("_EmissionColor", Color.Lerp(r.material.GetColor("_EmissionColor"), (emissionColor_angry * 35f), Time.deltaTime / 10));
        }

        if (anim.GetBool("isWaiting"))
        {
            anim.SetBool("isWaiting", false);
        }

        if (!anim.GetBool("isCombat")) return;

        if (isStuned) return;

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

    public void ResetAnimLerpSpeed()
    {
        animLerpSpeed = 10;
    }

    void PickUpShield()
    {
        shield_left.GetComponent<Boss_TypeX_Shield>().SetPickUp();
        shield_right.GetComponent<Boss_TypeX_Shield>().SetPickUp();
    }

    void PickUpShieldLeft()
    {
        shield_left.GetComponent<Boss_TypeX_Shield>().SetPickUp();
    }

    void PickUpShieldRight()
    {
        shield_right.GetComponent<Boss_TypeX_Shield>().SetPickUp();
    }

    private void LateUpdate()
    {
        if (anim.GetBool("isCombat"))
        {
            //이동

     


            if (isStuned)
            {
                animLerpSpeed = 13;
                tempPos_leftHand = hand_left.localPosition;
                tempPos_rightHand = hand_right.localPosition;
                return;
            }
            else
            {
                if (anim.GetBool("isIdle"))
                {
                    tempPos_leftHand = Vector3.Lerp(tempPos_leftHand, hand_left.localPosition, Time.deltaTime * 13);
                    tempPos_rightHand = Vector3.Lerp(tempPos_rightHand, hand_right.localPosition, Time.deltaTime * 13);
                }
                else
                {
                    tempPos_leftHand = Vector3.Lerp(tempPos_leftHand, hand_left.localPosition, Time.deltaTime * 53);
                    tempPos_rightHand = Vector3.Lerp(tempPos_rightHand, hand_right.localPosition, Time.deltaTime * 53);
                }

                hand_left.localPosition = tempPos_leftHand;
                hand_right.localPosition = tempPos_rightHand;
            }

            if (!anim.GetBool("isIdle"))
            {
                animLerpSpeed += Time.deltaTime * 40;
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

                    tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(dir) * Quaternion.Euler(body.localEulerAngles), Time.deltaTime * animLerpSpeed);

                    body.rotation = tempRot_body;

                    tempRot_leftHand = Quaternion.Lerp(tempRot_leftHand, hand_left.parent.rotation * Quaternion.Euler(hand_left.localEulerAngles), Time.deltaTime * animLerpSpeed);
                    hand_left.rotation = tempRot_leftHand;


                    tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, hand_right.parent.rotation * Quaternion.Euler(hand_right.localEulerAngles), Time.deltaTime * animLerpSpeed);
                    hand_right.rotation = tempRot_rightHand;

                    ////회전
                    //Vector3 dir = (target.position - body.position).normalized;
                    //dir.y = 0;
                    //tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(tempDir) * Quaternion.Euler(body.localEulerAngles), Time.deltaTime * 53);

                    //body.rotation = tempRot_body;

                    //Quaternion rotTemp = Quaternion.Euler(0, hand_left.parent.eulerAngles.y, hand_left.parent.eulerAngles.z);

                    //tempRot_leftHand = Quaternion.Lerp(tempRot_leftHand, rotTemp * Quaternion.Euler(hand_left.localEulerAngles), Time.deltaTime * 53);
                    //hand_left.rotation = tempRot_leftHand;

                    //rotTemp = Quaternion.Euler(0, hand_right.parent.eulerAngles.y, hand_right.parent.eulerAngles.z);
                    //tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, rotTemp * Quaternion.Euler(hand_right.localEulerAngles), Time.deltaTime * 53);
                    //hand_right.rotation = tempRot_rightHand;
                }
            }
            else
            {
                animLerpSpeed = 13;

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

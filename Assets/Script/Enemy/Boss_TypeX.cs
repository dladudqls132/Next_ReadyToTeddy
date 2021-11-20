using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Boss_TypeX_State
{
    Waiting,
    Ready,
    Idle,
    Attack
}

public class Boss_TypeX : Enemy
{
    //[SerializeField] private new Boss_TypeX_State state;
    [SerializeField] private Transform body;
    [SerializeField] private Transform hand_left;
    [SerializeField] private Transform hand_right;
    [SerializeField] private Transform shield_left;
    [SerializeField] private Transform shield_right;

    private Quaternion tempRot_body;
    private Quaternion tempRot_leftHand;
    private Quaternion tempRot_rightHand;
    private Vector3 tempPos_leftHand;
    private Vector3 tempPos_rightHand;

    [SerializeField] private List<Boss_Skill> skills = new List<Boss_Skill>();
    [SerializeField] private Boss_Skill currentSkill;
    [SerializeField] private Queue<Boss_Skill> skillOrder = new Queue<Boss_Skill>();

    int faceNum = 1;
    float animLerpSpeed;
    [SerializeField] bool canRot;
    [SerializeField] private int currentPhase;
    [SerializeField] private float coolTime;
    [SerializeField] private Animator timelineMeshAnim;
    [SerializeField] private BGMController bgmController;
    private Timeline_Boss_TypeX timeLine;
    private float currentCoolTime;
    private bool isOn;
    private Image f;

    public Boss_Skill GetCurrentSkill() { return currentSkill; }
    public void SetPhase(int phase) { currentPhase = phase; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //state = Boss_TypeX_State.Waiting;

        anim.SetBool("isWaiting", true);

        tempRot_body = body.localRotation;
        tempRot_leftHand = hand_left.localRotation;
        tempRot_rightHand = hand_right.localRotation;
        tempPos_leftHand = hand_left.localPosition;
        tempPos_rightHand = hand_right.localPosition;
        currentCoolTime = coolTime;

        skills.AddRange(this.GetComponents<Boss_Skill>());

        //for (int i = 0; i < skills.Length; i++)
        //{
        //    skills[i].enabled = false;
        //}

        f = GameObject.Find("F").GetComponent<Image>();
        isGod = true;
        timeLine = GameObject.FindGameObjectWithTag("Timeline").GetComponent<Timeline_Boss_TypeX>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRigidity)
        {

            if (anim.enabled)
            {
                hand_left.GetComponent<Rigidbody>().useGravity = true;
                hand_right.GetComponent<Rigidbody>().useGravity = true;
                hand_left.GetComponent<Rigidbody>().velocity = Vector3.zero;
                hand_right.GetComponent<Rigidbody>().velocity = Vector3.zero;
                hand_left.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                hand_right.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }

            anim.enabled = false;

            currentRigidityTime += Time.deltaTime;

            if (currentRigidityTime >= rigidityTime)
            {
                isRigidity = false;
                anim.enabled = true;
                hand_left.GetComponent<Rigidbody>().useGravity = false;
                hand_right.GetComponent<Rigidbody>().useGravity = false;
                hand_left.GetComponent<Rigidbody>().velocity = Vector3.zero;
                hand_right.GetComponent<Rigidbody>().velocity = Vector3.zero;
                hand_left.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                hand_right.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }

        }

        if (!isOn)
        {
            foreach (Renderer r in renderers)
            {
                r.material.SetColor("_ASEOutlineColor", Color.red);
            }

            return;
        }

        foreach (Renderer r in renderers)
        {
            r.material.SetColor("_ASEOutlineColor", Color.black);
        }

        foreach (Renderer r in renderers)
        {
            r.material.SetColor("_EmissionColor", Color.Lerp(r.material.GetColor("_EmissionColor"), (emissionColor_angry * 35f), Time.deltaTime / 4));
        }

        if (anim.GetBool("isWaiting"))
        {
            anim.SetBool("isWaiting", false);
        }

        if (isRigidity || currentPhase == 5 || !anim.GetBool("isCombat"))
        {
            if (currentPhase == 5)
            {
                this.GetComponent<BoxCollider>().enabled = true;
                bgmController.StopBGM();
            }
            return;
        }

        if ((currentHp / maxHp) * 100 <= 10)
        {
            if (currentPhase != 4)
            {
                isGod = true;

                if (currentSkill == null)
                {
                    timeLine.PlayTimeline(3, true);
                    currentPhase = 4;
                    faceNum = 4;
                    skillOrder.Clear();
                    currentSkill = null;
                }
            }

        }
        else if ((currentHp / maxHp) * 100 <= 40)
        {
            if (currentPhase != 3)
            {
                timeLine.PlayTimeline(2, true);
                currentPhase = 3;
                faceNum = 3;
            }
        }
        else if ((currentHp / maxHp) * 100 <= 70)
        {
            if (currentPhase != 2)
            {
                timeLine.PlayTimeline(1, true);
                currentPhase = 2;
                faceNum = 2;
            }
        }

        anim.SetFloat("Phase", Mathf.Lerp(anim.GetFloat("Phase"), currentPhase, Time.deltaTime * 10));
        anim.SetFloat("FaceType", Mathf.Lerp(anim.GetFloat("FaceType"), faceNum, Time.deltaTime * 10));

        if (currentPhase >= 4)
        {
            if (currentPhase == 4 && currentSkill == null)
            {
                if (!timeLine.GetIsPlay())
                {
                    for (int i = 0; i < skills.Count; i++)
                    {
                        if (skills[i].GetActivePhase() == currentPhase)
                        {
                            currentSkill = skills[i];
                            currentSkill.Use();
                        }
                    }
                }
            }

            return;
        }


            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i] != currentSkill)
                {
                    if (skills[i].CoolDown())
                    {
                        if (!skillOrder.Contains(skills[i]))
                        {
                            skillOrder.Enqueue(skills[i]);
                        }
                    }
                }
            }


            if (currentSkill != null)
            {
                if (!currentSkill.enabled)
                {
                    currentSkill = null;
                }
            }
            else
            {
                currentCoolTime -= Time.deltaTime;

                if (currentCoolTime <= 0 && skillOrder.Count != 0)
                {
                    currentSkill = skillOrder.Dequeue();
                    currentSkill.Use();
                    currentCoolTime = coolTime;
                }

                canRot = true;
                //state = Boss_TypeX_State.Idle;
            }
        
    }

    void PlayWakeUpAudio()
    {
        GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Boss_TypeX_WakeUp, this.transform.position + Vector3.up * 2, false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOn)
        {
            if (currentPhase == 5)
            {
                if (other.CompareTag("Player"))
                {
                    if (Vector3.Dot(Camera.main.transform.forward, (body.position - Camera.main.transform.position).normalized) >= 0.9f)
                    {
                        f.enabled = true;
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            f.enabled = false;
                            timeLine.PlayTimeline(4, true);
                        }
                    }
                    else
                        f.enabled = false;
                }
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                if (Vector3.Dot(Camera.main.transform.forward, (body.position - Camera.main.transform.position).normalized) >= 0.9f)
                {
                    f.enabled = true;
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        bgmController.PlayBGM(0, true);
                        f.enabled = false;
                        isOn = true;
                        isDetect = true;
                        this.GetComponent<BoxCollider>().enabled = false;
                        timeLine.PlayTimeline(0, true);
                    }
                }
                else
                    f.enabled = false;
            }
        }

 
    }

    private void OnTriggerExit(Collider other)
    {
        f.enabled = false;
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

    //void PickUpShield()
    //{
    //    shield_left.GetComponent<Boss_TypeX_Shield>().SetPickUp();
    //    shield_right.GetComponent<Boss_TypeX_Shield>().SetPickUp();
    //}

    void PickUpShieldLeft()
    {
        shield_left.GetComponent<Boss_TypeX_Shield>().SetPickUp();
    }

    void PickUpShieldRight()
    {
        shield_right.GetComponent<Boss_TypeX_Shield>().SetPickUp();
    }

    void SetCanRotFalse()
    {
        canRot = false;

        SetTempBodyDir();
    }

    void SetIsGodTrue()
    {
        isGod = true;
    }

    void SetIsGodFalse()
    {
        isGod = false;
    }

    public int GetCurrentPhase()
    {
        return currentPhase;
    }

    private void LateUpdate()
    {
        if (currentPhase == 5)
        {
            if (!canRot)
            {
                tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(tempDir) * Quaternion.Euler(body.localEulerAngles), Time.deltaTime * animLerpSpeed);

                body.rotation = tempRot_body;
            }

            return;
        }

        if (anim.GetBool("isCombat"))
        {
            //이동

            if (isRigidity)
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
                    if (anim.GetBool("isAttack_EnergyBall_LeftHand") || anim.GetBool("isAttack_EnergyBall_RightHand") || anim.GetBool("isAttack_MultiShot_LeftHand") || anim.GetBool("isAttack_MultiShot_RightHand"))
                    {
                        Vector3 dir = (target.position - body.position).normalized;

                        tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(dir) * Quaternion.Euler(body.localEulerAngles), Time.deltaTime * 13);
                        body.rotation = tempRot_body;

                        if (anim.GetBool("isAttack_EnergyBall_LeftHand") || anim.GetBool("isAttack_MultiShot_LeftHand"))
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
                        else if (anim.GetBool("isAttack_EnergyBall_RightHand") || anim.GetBool("isAttack_MultiShot_RightHand"))
                        {
                            //회전
                            dir = (target.position - hand_right.position).normalized;

                            if (anim.GetBool("isReload"))
                                tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, hand_right.parent.rotation * Quaternion.Euler(hand_right.localEulerAngles), Time.deltaTime * 13);
                            else
                                tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, Quaternion.LookRotation(dir) * Quaternion.Euler(hand_right.localEulerAngles), Time.deltaTime * 13);

                            hand_right.rotation = tempRot_rightHand;

                            tempRot_leftHand = Quaternion.Lerp(tempRot_leftHand, hand_left.parent.rotation * Quaternion.Euler(hand_left.localEulerAngles), Time.deltaTime * 13);
                            hand_left.rotation = tempRot_leftHand;
                        }
                    }
                    else
                    {
                        Vector3 dir = (target.position - body.position).normalized;
                        dir.y = 0;

                        if (canRot)
                            tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(dir) * Quaternion.Euler(body.localEulerAngles), Time.deltaTime * animLerpSpeed);
                        else
                            tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(tempDir) * Quaternion.Euler(body.localEulerAngles), Time.deltaTime * animLerpSpeed);
                        body.rotation = tempRot_body;

                        tempRot_leftHand = Quaternion.Lerp(tempRot_leftHand, hand_left.parent.rotation * Quaternion.Euler(hand_left.localEulerAngles), Time.deltaTime * animLerpSpeed);
                        hand_left.rotation = tempRot_leftHand;


                        tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, hand_right.parent.rotation * Quaternion.Euler(hand_right.localEulerAngles), Time.deltaTime * animLerpSpeed);
                        hand_right.rotation = tempRot_rightHand;
                    }
                }
                else
                {
                    //회전
                    Vector3 dir = (target.position - body.position).normalized;
                    dir.y = 0;

                    if (canRot)
                        tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(dir) * Quaternion.Euler(body.localEulerAngles), Time.deltaTime * animLerpSpeed);
                    else
                        tempRot_body = Quaternion.Lerp(tempRot_body, Quaternion.LookRotation(tempDir) * Quaternion.Euler(body.localEulerAngles), Time.deltaTime * animLerpSpeed);

                    body.rotation = tempRot_body;

                    tempRot_leftHand = Quaternion.Lerp(tempRot_leftHand, hand_left.parent.rotation * Quaternion.Euler(hand_left.localEulerAngles), Time.deltaTime * animLerpSpeed);
                    hand_left.rotation = tempRot_leftHand;


                    tempRot_rightHand = Quaternion.Lerp(tempRot_rightHand, hand_right.parent.rotation * Quaternion.Euler(hand_right.localEulerAngles), Time.deltaTime * animLerpSpeed);
                    hand_right.rotation = tempRot_rightHand;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform mainCam;
    [SerializeField] private Transform camPos;
    [SerializeField] private CapsuleCollider groundCollider;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform hand_Origin;
    [SerializeField] private GameObject weapon_gameObject;
    [SerializeField] private Gun weapon;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float slidingCoolTime;
    [SerializeField] private float currentSlidingCoolTime;
    [SerializeField] private bool isSlide;
    [SerializeField] private bool isSlope;
    [SerializeField] private bool isRun;
    private bool firstRun;
    [SerializeField] private bool isCrouch;
    [SerializeField] private bool isJump;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float jumpPower;
    private float currentJumpPower;
    [SerializeField] private bool isClimbing;
    [SerializeField] private float climbPower;
    private float currentClimbPower;
    [SerializeField] private bool canClimb;
    [SerializeField] private bool isClimbUp;
    [SerializeField] private bool isCombat;
    [SerializeField] private float combatTime;
    [SerializeField] private float currentCombatTime;

    private Vector3 climbUpPos;
    private Vector2 moveInput;
    private Rigidbody rigid;
    private Vector3 moveDirection;
    private Vector3 slidingDirection;
    private float headBobValue;
    private float headOriginY;
    private Vector3 handOriginPos;
    private Quaternion handOriginRot;

    [SerializeField] private PhysicMaterial walk_defaultPm;
    [SerializeField] private PhysicMaterial sliding_groundPm;
    [SerializeField] private PhysicMaterial sliding_slopePm;

    private bool isInit;

    public void SetIsGrounded(bool value) { isGrounded = value; }
    public GameObject GetWeaponGameObject() { return weapon_gameObject; }
    public Gun GetWeapon() { return weapon; }

    // Start is called before the first frame update
    public void Init()
    {
        rigid = this.GetComponent<Rigidbody>();
        currentSlidingCoolTime = 0;
        headBobValue = 0;
        headOriginY = camPos.localPosition.y;
        mainCam = Camera.main.transform;
        hand = mainCam.Find("HandPos").Find("Hand");
        hand_Origin = mainCam.Find("HandPos");
        handOriginPos = hand.parent.localPosition;
        handOriginRot = hand.parent.localRotation;
        currentCombatTime = combatTime;
        isInit = true;
    }

    void Start()
    {
        if (!isInit)
        {
            rigid = this.GetComponent<Rigidbody>();
            currentSlidingCoolTime = 0;
            headBobValue = 0;
            headOriginY = camPos.localPosition.y;
            mainCam = Camera.main.transform;
            hand = mainCam.Find("HandPos").Find("Hand");
            hand_Origin = mainCam.Find("HandPos");
            handOriginPos = hand.localPosition;
            handOriginRot = hand.localRotation;
            currentCombatTime = combatTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hand.GetChild(0) != null)
        {
            if (hand.GetChild(0) != weapon)
            {
                weapon_gameObject = hand.GetChild(0).gameObject;
                weapon = weapon_gameObject.GetComponent<Gun>();
            }
        }

        Vector3 forward = mainCam.forward;
        Vector3 right = mainCam.right;

        forward.y = 0; right.y = 0;

        forward.Normalize();
        right.Normalize();

        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

        RaycastHit hit;

        //Debug.DrawLine(this.transform.position, this.transform.position + Vector3.down * 0.2f);
        //Debug.DrawLine(this.transform.position + moveDirection * 0.2f, this.transform.position + moveDirection * 0.2f + Vector3.down * 0.5f);
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 0.2f, 1 << LayerMask.NameToLayer("Enviroment")))
        {
            groundCollider.enabled = true;
            if (currentJumpPower <= 0)
                isJump = false;
            isGrounded = true;
            isClimbing = false;
            canClimb = true;
            Vector3 slopeResult = Vector3.Cross(hit.normal, Vector3.Cross(rigid.velocity.normalized, hit.normal));
            Vector3 result = Vector3.Cross(hit.normal, Vector3.Cross(moveDirection.normalized, hit.normal));

            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (rigid.velocity.magnitude > walkSpeed && !isSlide && !isJump)
                {
                    if (Vector3.Dot(moveDirection, forward) > 0)
                    {
                        mainCam.GetComponent<FPPCamController>().FovMove(mainCam.GetComponent<FPPCamController>().GetOriginFov() + 10, 0.1f, 1000);

                        if (!isCrouch)
                        {
                            isSlide = true;

                            if(currentSlidingCoolTime <= 0)
                                rigid.AddForce(slopeResult.normalized * rigid.velocity.magnitude * 0.7f, ForceMode.VelocityChange);
                        }

                        slidingDirection = moveDirection;
                    }
                }

                if (!isSlide && !isJump)
                    isCrouch = true;

            }

            if (Input.GetKey(KeyCode.LeftShift) && !isSlide && !isCombat)
            {
                if (!isRun)
                {
                    firstRun = true;
                    isRun = true;
                }

            }
            if (Input.GetKeyUp(KeyCode.LeftShift) && isRun)
            {
                if (firstRun)
                {
                    firstRun = false;
                }
                else
                {
                    isRun = false;
                }
            }
            if (moveDirection == Vector3.zero)
            {
                isRun = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSlide = false;
                isJump = true;
                currentJumpPower = rigid.velocity.y / 2 + jumpPower;
            }

            if (isCrouch)
            {
                isRun = false;
            }

            if (Vector3.Dot(forward, moveDirection) < 0.5f)
            {
                isRun = false;
            }

            if (isSlide)
            {
                if (slopeResult.y < 0)
                {
                    groundCollider.material = sliding_slopePm;
                    isSlope = true;
                }
                else
                {
                    groundCollider.material = sliding_groundPm;
                    isSlope = false;
                }

                if (moveDirection != Vector3.zero)
                {
                    if (Vector3.Dot(new Vector3(rigid.velocity.normalized.x, moveDirection.y, rigid.velocity.normalized.z), slidingDirection) < 0 || Vector3.Dot(new Vector3(rigid.velocity.normalized.x, moveDirection.y, rigid.velocity.normalized.z), moveDirection) < -0.75f)
                    {
                        isSlide = false;
                    }
                    else
                    {
                        rigid.velocity = rigid.velocity + moveDirection * 0.01f;
                    }
                }
            }
            else
            {
                groundCollider.material = walk_defaultPm;

                if (rigid.velocity.magnitude > walkSpeed)
                {
                    if (isRun)
                    {
                        rigid.velocity = Vector3.Lerp(rigid.velocity, result.normalized * runSpeed, Time.deltaTime * 8);
                    }
                    else
                    {
                        rigid.velocity = Vector3.Lerp(rigid.velocity, result.normalized * walkSpeed, Time.deltaTime * 8);
                    }
                }
                else
                {
                    if (isRun)
                    {
                        rigid.velocity = Vector3.Lerp(rigid.velocity, result.normalized * runSpeed, Time.deltaTime * 20);
                    }
                    else
                    {
                        if (isCrouch)
                            rigid.velocity = Vector3.Lerp(rigid.velocity, result.normalized * walkSpeed * 0.65f, Time.deltaTime * 20);
                        else
                        {
                            rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(0, rigid.velocity.y, 0) + result.normalized * walkSpeed, Time.deltaTime * 20);
                        }
                    }
                }
            }
        }
        else
        {
            groundCollider.enabled = false;

            if (isGrounded && !isJump)
            {
                rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
            }

            isGrounded = false;

            if (!isClimbUp)
            {
                if (!isClimbing)
                {
                    if (rigid.velocity.magnitude >= walkSpeed)
                        rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(moveDirection.x * new Vector2(rigid.velocity.x, rigid.velocity.z).magnitude, rigid.velocity.y, moveDirection.z * new Vector2(rigid.velocity.x, rigid.velocity.z).magnitude), Time.deltaTime * 6);
                    else
                        rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(moveDirection.x * walkSpeed, rigid.velocity.y, moveDirection.z * walkSpeed), Time.deltaTime * 4);
                }
                else
                {
                    rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(moveDirection.x * walkSpeed / 4, rigid.velocity.y, moveDirection.z * walkSpeed / 3), Time.deltaTime * 4);
                }
            }

            RaycastHit wallHit;
            for (int i = 0; i < 12; i++)
            {
                Debug.DrawRay(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f, forward * 0.35f);
                if (Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f, forward, out wallHit, 0.35f, 1 << LayerMask.NameToLayer("Enviroment")))
                {
                    if (Mathf.Abs(wallHit.normal.y) <= 0.3f && Vector3.Dot(moveDirection, forward) > 0.7f)
                    {
                        if (Input.GetKey(KeyCode.Space))
                        {
                            isJump = false;
                            isClimbing = true;
                            isSlide = false;
                            rigid.velocity = new Vector3(rigid.velocity.x, currentClimbPower, rigid.velocity.z);
                        }
                    }
                }
                else if(moveDirection != Vector3.zero)
                {
                    bool isCheckObject = false;
                    if (!Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * (i - 1)) + Vector3.up * 0.35f, forward, 0.35f, 1 << LayerMask.NameToLayer("Enviroment")))
                    {
                        isCheckObject = true;
                    }

                    if (Physics.BoxCast(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f, new Vector3(0.5f, 0.5f, 0.5f), Vector3.up, Quaternion.identity, 1.6f, 1 << LayerMask.NameToLayer("Enviroment")))
                    {
                        isCheckObject = true;
                    }

                    for (int j = 0; j < 20; j++)
                    {
                        if (Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f + (Vector3.up * 0.1f * j), forward, 0.35f, 1 << LayerMask.NameToLayer("Enviroment")))
                        {
                            isCheckObject = true;
                        }
                    }

                    if (!isCheckObject)
                    {
                        isClimbUp = true;
                        rigid.velocity = Vector3.zero;
                        climbUpPos = this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f + forward * 0.38f;
                    }

                    if (isClimbing)
                    {
                        canClimb = false;
                    }

                    isClimbing = false;
                }
            }

            if (!isJump && !isClimbing)
            {
                if (Physics.Raycast(this.transform.position + moveDirection * new Vector2(rigid.velocity.x, rigid.velocity.z).magnitude * Time.deltaTime, Vector3.down, out hit, 0.4f, 1 << LayerMask.NameToLayer("Enviroment")))
                {
                    Vector3 result = Vector3.Cross(hit.normal, Vector3.Cross(moveDirection.normalized, hit.normal));
                    Vector3 slopeResult = Vector3.Cross(hit.normal, Vector3.Cross(rigid.velocity.normalized, hit.normal));

                    if (isSlide)
                    {
                        if (rigid.velocity.y >= 0)
                            rigid.velocity = (slopeResult.normalized * rigid.velocity.magnitude);
                    }
                    else
                    {
                        if (rigid.velocity.y >= 0)
                            rigid.velocity = (result.normalized * rigid.velocity.magnitude);
                    }
                }
            }
        }

        if(Input.GetMouseButtonDown(0) && !isClimbUp && !isClimbing)
        {
            weapon.Fire();
            if (isGrounded)
            {
                isCombat = true;
                isRun = false;
            }
            
            currentCombatTime = combatTime;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(weapon.CanReload())
                weapon.SetIsReload(true);
        }

        //if (Input.GetKeyDown(KeyCode.LeftShift) || moveDirection == Vector3.zero)
        //{
        //    isRun = false;
        //}
        //if (moveDirection == Vector3.zero)
        //{
        //    isRun = false;
        //}

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isSlide = false;

            //currentSlidingCoolTime = slidingCoolTime;
            isCrouch = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isClimbing)
            {
                canClimb = false;
            }

            isClimbing = false;
        }

        //if (isRun && !isJump && isGrounded && !isSlide)
        //{
        //    if (!weapon.GetIsReload())
        //        hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(15.479f, -62.062f, 0), Time.deltaTime * 14);
        //    else
        //        hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(handOriginRot.eulerAngles.x, handOriginRot.eulerAngles.y, -23), Time.deltaTime * 12);
        //}
        //else
        //{
        //    if (!weapon.GetIsReload())
        //        hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(handOriginRot.eulerAngles.x, handOriginRot.eulerAngles.y, -moveInput.x * 1.7f), Time.deltaTime * 12);
        //    else
        //        hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(handOriginRot.eulerAngles.x, handOriginRot.eulerAngles.y, -23), Time.deltaTime * 12);
        //}

        if(weapon.GetIsReload())
        {
            hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(handOriginRot.eulerAngles.x, handOriginRot.eulerAngles.y, -23), Time.deltaTime * 12);
        }
        else
        {
            if(weapon.GetIsShot() || isCombat)
            {
                hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(handOriginRot.eulerAngles.x, handOriginRot.eulerAngles.y, -moveInput.x * 1.7f), Time.deltaTime * 12);
            }
            else
            {
                if(isRun)
                {
                    hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(15.479f, -62.062f, 0), Time.deltaTime * 14);
                }
                else
                {
                    hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(handOriginRot.eulerAngles.x, handOriginRot.eulerAngles.y, -moveInput.x * 1.7f), Time.deltaTime * 12);
                }
            }
        }

        if (isCombat)
        {
            currentCombatTime -= Time.deltaTime;

            if(currentCombatTime <= 0)
            {
                isCombat = false;
                currentCombatTime = combatTime;
            }
        }

        if (isClimbUp)
        {
            isSlide = false;
            isClimbing = false;
            isJump = false;

            rigid.velocity = (climbUpPos - this.transform.position).normalized * Vector3.Distance(climbUpPos, this.transform.position) * 9f;

            if (Vector3.Distance(this.transform.position, climbUpPos) <= 0.15f)
            {
                isClimbUp = false;
                rigid.velocity = Vector3.zero;
            }
        }

        if (isClimbing)
        {
            currentClimbPower += Time.deltaTime * (Physics.gravity.y / 3.2f);
        }
        else
        {
            if (canClimb)
            {
                if (rigid.velocity.y >= 0)
                {
                    currentClimbPower = climbPower;
                }
                else
                    currentClimbPower = rigid.velocity.y / 4 + climbPower;
            }
            else
            {
                currentClimbPower = rigid.velocity.y;
            }
        }

        if (isJump)
        {
            if (Physics.Raycast(mainCam.position, Vector3.up, 0.5f, 1 << LayerMask.NameToLayer("Enviroment")))
                isJump = false;

            rigid.velocity = new Vector3(rigid.velocity.x, currentJumpPower, rigid.velocity.z);
            currentJumpPower += Time.deltaTime * Physics.gravity.y;
        }

        if (isSlide)
        {
            //isRun = false;
            currentSlidingCoolTime = slidingCoolTime;

            headBobValue = 0;
            camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY / 2, 0), Time.deltaTime * 8);

            if (moveDirection != Vector3.zero)
            {
                if (rigid.velocity.magnitude <= walkSpeed)
                {
                    isSlide = false;
                }
            }
            else
            {
                if (rigid.velocity.magnitude <= 0.5f)
                {
                    isSlide = false;
                }
            }
        }
        else
        {
            if (currentSlidingCoolTime > 0)
            {
                currentSlidingCoolTime -= Time.deltaTime;
            }

            mainCam.GetComponent<FPPCamController>().FovReset();

            if (isGrounded)
            {
                if (moveDirection == Vector3.zero)
                {
                    groundCollider.material = walk_defaultPm;

                    headBobValue += Time.deltaTime * 1.0f;

                    if (isCrouch)
                        camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY / 2 + Mathf.Abs(Mathf.Sin(headBobValue)) / 30, camPos.localPosition.z), Time.deltaTime * 8);
                    else
                        camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY + Mathf.Abs(Mathf.Sin(headBobValue)) / 30, camPos.localPosition.z), Time.deltaTime * 8);
                }
                else
                {
                    if (isRun)
                    {
                        headBobValue += Time.deltaTime * runSpeed * 0.8f;
                    }
                    else
                    {
                        headBobValue += Time.deltaTime * walkSpeed * 1.0f;
                    }

                    if (isCrouch)
                        camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY / 2 + Mathf.Abs(Mathf.Sin(headBobValue)) / 6, 0), Time.deltaTime * 8);
                    else
                    {
                        if (isRun)
                            camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(Mathf.Sin(headBobValue) / 30, headOriginY + Mathf.Abs(Mathf.Sin(headBobValue)) / 3.5f, camPos.localPosition.z), Time.deltaTime * 10);
                        else
                            camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(Mathf.Sin(headBobValue) / 50, headOriginY + Mathf.Abs(Mathf.Sin(headBobValue)) / 6, camPos.localPosition.z), Time.deltaTime * 8);
                    }
                }
            }
            else
            {
                if (!isCrouch && !isSlide)
                {
                    camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY, camPos.localPosition.z), Time.deltaTime * 8);
                    headBobValue = 0;
                }
            }

            isSlope = false;
        }

        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 28.0f);

        this.transform.rotation = Quaternion.LookRotation(forward);
    }
}

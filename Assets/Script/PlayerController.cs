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
    private float currentSlidingCoolTime;
    [SerializeField] private bool isSlide;
    private float slideSpeed;
    [SerializeField] private bool isSlope;
    [SerializeField] private bool isRun;
    [SerializeField] private float WPressTime;
    private float currentWPressTime;
    private bool isPressW;
    [SerializeField] private bool isCrouch;
    [SerializeField] private bool canJump;
    [SerializeField] private bool isJump;
    [SerializeField] private bool isJumpByObject;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float jumpPower;
    [SerializeField] private float currentJumpPower;
    [SerializeField] private bool isClimbing;
    [SerializeField] private float climbPower;
    private float currentClimbPower;
    private bool canClimb;
    private float climbUpPower;
    
    [SerializeField] private bool isClimbUp;
    [SerializeField] private bool isCombat;
    [SerializeField] private bool isAiming;
    [SerializeField] private float combatTime;
    private float currentCombatTime;
    [SerializeField] private bool isDash;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashTime;
    private float currentDashTime;
    [SerializeField] private float currentDashPower;
    [SerializeField]  private bool useGravity;

    private Vector3 climbUpPos;
    private Vector2 moveInput;
    private Rigidbody rigid;
    private Vector3 moveDirection;
    private Vector3 slidingDirection;
    private Vector3 dashDirection;
    private float headBobValue;
    private float headOriginY;
    private Vector3 handOriginPos;
    private Quaternion handOriginRot;
    Vector3 result;

    private bool isInit;

    public void SetIsGrounded(bool value) { isGrounded = value; }
    public GameObject GetWeaponGameObject() { return weapon_gameObject; }
    public Gun GetWeapon() { return weapon; }
    public bool GetIsAiming() { return isAiming; }
    public void SetIsJumpByObject(bool value, float power) { isJumpByObject = value; currentJumpPower = power; }

    // Start is called before the first frame update
    public void Init()
    {
        rigid = this.GetComponent<Rigidbody>();
        currentSlidingCoolTime = 0;
        headBobValue = 0;
        headOriginY = camPos.localPosition.y;
        mainCam = Camera.main.transform;
        hand = mainCam.Find("HandPos").Find("WeaponPos");
        hand_Origin = mainCam.Find("HandPos");
        handOriginPos = hand.parent.localPosition;
        handOriginRot = hand.parent.localRotation;
        currentCombatTime = combatTime;
        currentWPressTime = WPressTime;
        currentDashTime = dashTime;
        currentDashPower = dashPower;
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
            hand = mainCam.Find("HandPos").Find("WeaponPos");
            hand_Origin = mainCam.Find("HandPos");
            handOriginPos = hand.localPosition;
            handOriginRot = hand.localRotation;
            currentCombatTime = combatTime;
            currentWPressTime = WPressTime;
            currentDashTime = dashTime;
            currentDashPower = dashPower;
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
            {
                isJump = false;
                isJumpByObject = false;
            }
            canJump = true;
            isGrounded = true;
            isClimbing = false;
            canClimb = true;
            useGravity = false;
            Vector3 slopeResult = Vector3.Cross(hit.normal, Vector3.Cross(rigid.velocity.normalized, hit.normal));
            result = Vector3.Cross(hit.normal, Vector3.Cross(moveDirection.normalized, hit.normal));

            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (rigid.velocity.magnitude > walkSpeed && !isSlide && !isJump && !isJumpByObject)
                {
                    if (Vector3.Dot(moveDirection, forward) > 0)
                    {
                        if(!isAiming)
                            mainCam.GetComponent<FPPCamController>().FovMove(mainCam.GetComponent<FPPCamController>().GetOriginFov() + 10, 0.1f, 1000);

                        if (!isCrouch)
                        {
                            isSlide = true;

                            if (currentSlidingCoolTime <= 0)
                            {
                                rigid.AddForce(slopeResult.normalized * new Vector3(rigid.velocity.x, rigid.velocity.y / 2, rigid.velocity.z).magnitude * 0.9f, ForceMode.VelocityChange);
                                slideSpeed = rigid.velocity.magnitude * 1.5f;
                            }
                        }

                        slidingDirection = moveDirection;
                    }
                }

                if (!isClimbing && !isClimbUp && !isSlide && !isJump && !isJumpByObject)
                    isCrouch = true;

            }

            if(Input.GetKeyDown(KeyCode.W))
            {
                if (isPressW)
                {
                    isRun = true;
                    isPressW = false;
                    currentWPressTime = WPressTime;
                }

                if (!isSlide && !isCombat && !isAiming && !isRun)
                {
                    isPressW = true;
                }
            }

            if(isPressW)
            {
                currentWPressTime -= Time.deltaTime;

                if(currentWPressTime <= 0)
                {
                    currentWPressTime = WPressTime;
                    isPressW = false;
                }
            }

            if (isCrouch)
            {
                isRun = false;
            }

            if (Vector3.Dot(forward, moveDirection) < 0.5f)
            {
                isRun = false;
            }
            if (slopeResult.y < 0)
            {
                isSlope = true;
            }
            else
            {
                isSlope = false;
            }

            if (isSlide)
            {
                if (moveDirection != Vector3.zero)
                {
                    if (Vector3.Dot(new Vector3(rigid.velocity.normalized.x, moveDirection.y, rigid.velocity.normalized.z), slidingDirection) < 0 || Vector3.Dot(new Vector3(rigid.velocity.normalized.x, moveDirection.y, rigid.velocity.normalized.z), moveDirection) < -0.75f)
                    {
                        isSlide = false;
                    }
                    else
                    {
                        if (isSlope)
                        {
                            slideSpeed += Time.deltaTime * Mathf.Abs(slopeResult.y) * 4;

                            rigid.velocity = Vector3.Lerp(rigid.velocity, slopeResult.normalized * slideSpeed + (right * moveInput.x).normalized, Time.deltaTime * 3);
                            
                        }
                        else
                            rigid.velocity = Vector3.Lerp(rigid.velocity, Vector3.zero, Time.deltaTime);
                    }
                }
            }
            else if (!isDash)
            {
                if (rigid.velocity.magnitude > walkSpeed)
                {
                    if (isRun && !isCombat)
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
                    if (isRun && !isCombat)
                    {
                   
                            rigid.velocity = Vector3.Lerp(rigid.velocity, result.normalized * runSpeed, Time.deltaTime * 20);
                    }
                    else
                    {
                        if (isCrouch)
                            rigid.velocity = Vector3.Lerp(rigid.velocity, result.normalized * walkSpeed * 0.65f, Time.deltaTime * 20);
                        else
                        {
               
                                rigid.velocity = Vector3.Lerp(rigid.velocity, result.normalized * walkSpeed, Time.deltaTime * 20);
                        }
                    }
                }
            }
        }
        else
        {

            if(!isDash)
            useGravity = true;
            groundCollider.enabled = false;
            if (isGrounded && !isJump && !isJumpByObject)
            {
                rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
            }

            isGrounded = false;

            if (!isClimbUp)
            {
                if (!isClimbing)
                {
                    if (rigid.velocity.magnitude >= walkSpeed)
                    {

                            rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(moveDirection.x * new Vector2(rigid.velocity.x, rigid.velocity.z).magnitude, rigid.velocity.y, moveDirection.z * new Vector2(rigid.velocity.x, rigid.velocity.z).magnitude), Time.deltaTime * 6);
                    }
                    else
                    {
               
                            rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(moveDirection.x * walkSpeed, rigid.velocity.y, moveDirection.z * walkSpeed), Time.deltaTime * 4);

                    }
                }
                else
                {
                    rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(moveDirection.x * walkSpeed / 4, rigid.velocity.y, moveDirection.z * walkSpeed / 3), Time.deltaTime * 4);
                }
            }

            RaycastHit wallHit;
            for (int i = 0; i < 12; i++)
            {
                //Debug.DrawRay(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f, forward * 0.35f);
                if (Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f, forward, out wallHit, 0.38f, 1 << LayerMask.NameToLayer("Enviroment")))
                {
                    if (Mathf.Abs(wallHit.normal.y) <= 0.3f && Vector3.Dot(moveDirection, forward) > 0.7f)
                    {
                        if (Input.GetKey(KeyCode.Space))
                        {
                            isJump = false;
                            isJumpByObject = false;
                            isClimbing = true;
                            isSlide = false;
                            isDash = false;
                            rigid.velocity = new Vector3(rigid.velocity.x, currentClimbPower, rigid.velocity.z);
                        }
                    }
                }
                else if(moveDirection != Vector3.zero)
                {
                    bool isCheckObject = false;
                    if (!Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * (i - 1)) + Vector3.up * 0.35f, forward, 0.38f, 1 << LayerMask.NameToLayer("Enviroment")))
                    {
                        isCheckObject = true;
                    }

                    if (Physics.BoxCast(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f, new Vector3(0.5f, 0.5f, 0.5f), Vector3.up, Quaternion.identity, 1.6f, 1 << LayerMask.NameToLayer("Enviroment")))
                    {
                        isCheckObject = true;
                    }

                    for (int j = 0; j < 20; j++)
                    {
                        if (Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f + (Vector3.up * 0.1f * j), forward, 0.38f, 1 << LayerMask.NameToLayer("Enviroment")))
                        {
                            isCheckObject = true;
                        }
                    }

                    if (!isCheckObject)
                    {
                        isClimbUp = true;
                        rigid.velocity = Vector3.zero;
                        climbUpPos = this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f + forward * 0.38f;
                        climbUpPower = Vector3.Distance(this.transform.position, climbUpPos) * 9;
                    }

                    if (isClimbing)
                    {
                        canClimb = false;
                    }

                    isClimbing = false;
                }
            }

            if (!isJump && !isJumpByObject && !isClimbing)
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSlide && !isClimbing && !isClimbUp && moveDirection != Vector3.zero)
        {
            isDash = true;
            if (Vector3.Dot(forward, moveDirection) > 0.5f)
                mainCam.GetComponent<FPPCamController>().FovMove(mainCam.GetComponent<FPPCamController>().GetOriginFov() - 2.0f, 0.1f, 1000);
            if (Vector3.Dot(forward, moveDirection) < 0)
                mainCam.GetComponent<FPPCamController>().FovMove(mainCam.GetComponent<FPPCamController>().GetOriginFov() + 2.0f, 0.1f, 1000);

            if (isJump)
            {
                canJump = false;
            }
            isJump = false;
            isJumpByObject = false;

            if(isGrounded)
                dashDirection = result;
            else
                dashDirection = moveDirection;

            // rigid.AddForce(moveDirection * dashPower, ForceMode.VelocityChange);
        }

        if(useGravity)
        {
            if(!isJump && !isJumpByObject)
            rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(rigid.velocity.x, rigid.velocity.y + Physics.gravity.y, rigid.velocity.z), Time.deltaTime);
        }

        if (isDash)
        {
            if (!isGrounded)
                rigid.velocity = new Vector3(dashDirection.x * currentDashPower, 0, dashDirection.z * currentDashPower);
            else
                rigid.velocity = dashDirection * currentDashPower;

            useGravity = false;
            currentDashTime -= Time.deltaTime;
            currentDashPower -= (Time.deltaTime * dashPower)/ dashTime;

            if(currentDashPower <= walkSpeed + 1.5f)
                mainCam.GetComponent<FPPCamController>().FovReset();

            if (currentDashPower <= walkSpeed)
            {
                isDash = false;
                currentDashTime = dashTime;
                currentDashPower = dashPower;
            }
        }

        if (Input.GetMouseButtonDown(0) && !isClimbUp && !isClimbing)
        {
            weapon.Fire();
            if (isGrounded && !isSlide)
            {
                isCombat = true;
            }
            
            currentCombatTime = combatTime;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(weapon.CanReload())
                weapon.SetIsReload(true);
        }

        if(Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming;

            if (isAiming)
            {
                weapon.SetIsReload(false);
                mainCam.GetComponent<FPPCamController>().FovMove(mainCam.GetComponent<FPPCamController>().GetOriginFov() - 10, 0.1f, 1000);
            }
        }

        if (moveDirection == Vector3.zero)
        {
            isRun = false;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isSlide = false;

            isCrouch = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isDash && !isJump && canJump)
        {
            isSlide = false;
            isJump = true;
            canJump = false;

            if (isJumpByObject)
            {
                rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
                currentJumpPower = jumpPower;
            }
            else
            {
                if(!isSlope)
                    rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);

                currentJumpPower = rigid.velocity.y / 2 + jumpPower;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isClimbing)
            {
                canClimb = false;
            }

            isClimbing = false;
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
            isJumpByObject = false;
            isDash = false;
            
            if(climbUpPower <= 2)
            {
                climbUpPower = 2;
            }
            else
            {
                climbUpPower -= Time.deltaTime * 15;
            }

            rigid.velocity = (climbUpPos - this.transform.position).normalized  * Mathf.Clamp(Vector3.Distance(this.transform.position, climbUpPos) * 9, 2, 100);

            if (Vector3.Distance(this.transform.position, climbUpPos) <= 0.1f)
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

        if (isJump || isJumpByObject)
        {
            if (Physics.Raycast(mainCam.position, Vector3.up, 0.5f, 1 << LayerMask.NameToLayer("Enviroment")))
            {
                isJump = false;
                isJumpByObject = false;
            }

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

            if (!isAiming && !isDash)
                mainCam.GetComponent<FPPCamController>().FovReset();

            HeadBob();

            isSlope = false;
        }

        HandAnimation();

        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 28.0f);

        this.transform.rotation = Quaternion.LookRotation(forward);
    }

    private void HeadBob()
    {
        if (isGrounded)
        {
            if (moveDirection == Vector3.zero)
            {
                headBobValue += Time.deltaTime * 1.0f;

                if (isCrouch)
                    camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY / 2 + Mathf.Abs(Mathf.Sin(headBobValue)) / 30, camPos.localPosition.z), Time.deltaTime * 8);
                else
                    camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY + Mathf.Abs(Mathf.Sin(headBobValue)) / 30, camPos.localPosition.z), Time.deltaTime * 8);
            }
            else if(!isDash)
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
    }

    private void HandAnimation()
    {
        if (isAiming)
        {
            if (isGrounded)
                isRun = false;

            hand_Origin.localPosition = Vector3.Lerp(hand_Origin.localPosition, new Vector3(0, -0.08f, 0.087f), Time.deltaTime * 35);
        }
        else
        {
            hand_Origin.localPosition = Vector3.Lerp(hand_Origin.localPosition, handOriginPos, Time.deltaTime * 16);
        }

        if (weapon.GetIsReload())
        {
            hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(handOriginRot.eulerAngles.x, handOriginRot.eulerAngles.y, -23), Time.deltaTime * 12);
        }
        else
        {
            if (isCombat || isAiming)
            {
                if (isAiming)
                    hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(handOriginRot.eulerAngles.x, handOriginRot.eulerAngles.y, -moveInput.x * 1.05f), Time.deltaTime * 30);
                else
                    hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(handOriginRot.eulerAngles.x, handOriginRot.eulerAngles.y, -moveInput.x * 1.7f), Time.deltaTime * 12);
            }
            else
            {
                if (weapon.GetIsShot())
                {
                    if (!isGrounded)
                    {
                        hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(handOriginRot.eulerAngles.x, handOriginRot.eulerAngles.y, -moveInput.x * 1.7f), Time.deltaTime * 12);
                    }
                    else
                    {
                        hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(handOriginRot.eulerAngles.x, handOriginRot.eulerAngles.y, -moveInput.x * 1.7f), Time.deltaTime * 12);
                    }
                }
                else
                {
                    if (isRun)
                    {
                        hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(15.479f, -62.062f, 0), Time.deltaTime * 14);
                    }
                    else
                    {
                        hand_Origin.localRotation = Quaternion.Lerp(hand_Origin.localRotation, Quaternion.Euler(handOriginRot.eulerAngles.x, handOriginRot.eulerAngles.y, -moveInput.x * 1.7f), Time.deltaTime * 12);
                    }
                }
            }
        }
    }
}
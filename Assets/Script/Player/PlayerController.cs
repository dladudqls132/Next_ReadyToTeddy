using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private FPPCamController mainCam = null;
    [SerializeField] private Transform camPos = null;
    [SerializeField] private Transform aimPos;
    [SerializeField] private CapsuleCollider bodyCollider = null;
    [SerializeField] private CapsuleCollider groundCollider = null;
    [SerializeField] private Transform hand = null;

    [SerializeField] private GameObject weapon_gameObject = null;
    [SerializeField] private Gun weapon = null;
    [SerializeField] private Transform checkingGroundRayPos;

    [SerializeField] private bool isDead = false;
    [SerializeField] private float maxHP = 0;
    private float currentHP = 0;
    [SerializeField] private int maxCombo = 0;
    [SerializeField] private int currentCombo = 0;
    [SerializeField] private float resetComboTime = 0;
    [SerializeField] private float keepComboTime = 0;
    [SerializeField] private float downComboTime = 0;
    private float currentResetComboTime = 0;

    [SerializeField] private float decreaseHpValuePerSecond = 0;
    [SerializeField] private float walkSpeed = 0;
    [SerializeField] private float runSpeed = 0;
    [SerializeField] private float slidingCoolTime = 0;
    private float currentSlidingCoolTime = 0;
    private bool isSlide = false;
    private float slideSpeed = 0;
    [SerializeField] private bool isSlope = false;
    private bool isRun = false;
    [SerializeField] private float WPressTime = 0;
    private float currentWPressTime = 0;
    private bool isPressW = false;
    private bool isCrouch = false;
    [SerializeField] private bool canJump = false;
    private bool isJump = false;
    [SerializeField] private bool isJumpByObject = false;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private float jumpPower = 0;
    [SerializeField] private float currentJumpPower = 0;
    [SerializeField] private bool isClimbing = false;
    [SerializeField] private float climbPower = 0;
    private float currentClimbPower = 0;
    private bool canClimb = false;

    [SerializeField] private bool isClimbUp = false;
    [SerializeField] private float climbUpTime = 0;
    [SerializeField] private float climbUpPower = 0;
    private float currentClimbuUpPower = 0;
    private float currentClimbUpTime = 0;
    private bool isCombat = false;
    private bool isAiming = false;
    [SerializeField] private float combatTime = 0;
    private float currentCombatTime = 0;
    private bool isDash;
    [SerializeField] private float dashPower = 0;
    [SerializeField] private float dashTime = 0;
    private float currentDashPower = 0;
    [SerializeField] private bool useGravity = false;
    private bool isLanding = false;
    [SerializeField] private float landingReboundSpeed = 0;
    [SerializeField] private float skill1_range;
    [SerializeField] private float kickWallTime;
    [SerializeField] private float currentKickWallTime;
   
    [SerializeField] private int currentWeaponNum;
    private bool isSwap;

    private bool isMoveAim;
    private Vector3 climbUpPos = Vector3.zero;
    private Vector2 moveInput = Vector2.zero;
    private Rigidbody rigid = null;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 slidingDirection = Vector3.zero;
    private Vector3 dashDirection = Vector3.zero;
    private float headBobValue = 0;
    private float headOriginY = 0;
    private Vector3 handOriginPos = Vector3.zero;
    private Quaternion handOriginRot = Quaternion.identity;
    private Vector3 result = Vector3.zero;
    private Vector3 originBodyColliderCenter;
    private float originBodyColliderHeight;
    private RaycastHit wallHit;
    private bool isPushed;
    private Vector3 originAimPos;
    private Quaternion handFireRot;


    private bool isInit = false;

    public void SetIsGrounded(bool value) { isGrounded = value; }
    public GameObject GetWeaponGameObject() { return weapon_gameObject; }
    public Gun GetWeapon() { return weapon; }
    public bool GetIsAiming() { return isAiming; }
    public void SetIsJumpByObject(bool value, float power) { isJumpByObject = value; currentJumpPower = power; }
    public float GetMaxHp() { return maxHP; }
    public float GetCurrentHp() { return currentHP; }
    public float GetMaxCombo() { return maxCombo; }
    public float GetCurrentCombo() { return currentCombo; }
    public float GetResetComboTime() { return resetComboTime; }
    public float GetKeepComboTime() { return keepComboTime; }
    public float GetDownComboTime() { return downComboTime; }
    public float GetCurrentResetComboTime() { return currentResetComboTime; }
    public Transform GetCamPos() { return camPos; }
    public bool GetIsCrouch() { return isCrouch; }
    public void SetCanJump(bool value) { canJump = value; }
    public float GetWalkSpeed() { return walkSpeed; }
    public bool GetIsDead() { return isDead; }
    public bool GetIsRun() { return isRun; }
    public bool GetIsGrounded() { return isGrounded; }
    public bool GetIsSlide() { return isSlide; }
    public bool GetIsDash() { return isDash; }
    public bool GetIsClimbing() { return isClimbing; }
    public bool GetIsClimbUp() { return isClimbUp; }
    public void SetIsPushed(bool value) { isPushed = value; }
    public bool GetIsPushed() { return isPushed; }
    public Transform GetAimPos() { return aimPos; }
    public int GetCurrentWeaponNum() { return currentWeaponNum; }
    public void SetIsSwap(bool value) { isSwap = value; } 

    // Start is called before the first frame update
    public void Init()
    {
        rigid = this.GetComponent<Rigidbody>();
        currentSlidingCoolTime = 0;
        headBobValue = 0;
        headOriginY = camPos.localPosition.y;
        mainCam = Camera.main.transform.GetComponent<FPPCamController>();
        hand = mainCam.transform.Find("HandPos");
        currentHP = maxHP;
        handOriginPos = hand.localPosition;
        handOriginRot = hand.localRotation;
        currentCombatTime = combatTime;
        currentWPressTime = WPressTime;
        currentDashPower = dashPower;
        currentClimbUpTime = climbUpTime;
        currentClimbuUpPower = climbUpPower;
        originBodyColliderCenter = bodyCollider.center;
        originBodyColliderHeight = bodyCollider.height;
        currentKickWallTime = 0;
        originAimPos = aimPos.localPosition;
        currentWeaponNum = 1;

        hand.GetChild(currentWeaponNum - 1).gameObject.SetActive(true);

        weapon_gameObject = hand.GetChild(currentWeaponNum - 1).GetChild(0).GetChild(0).gameObject;
        weapon = weapon_gameObject.GetComponent<Gun>();

        if (weapon.GetOwner() == null)
            weapon.SetOwner(this.gameObject, hand);

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
            mainCam = Camera.main.transform.GetComponent<FPPCamController>();
            hand = mainCam.transform.Find("HandPos");
            currentHP = maxHP;
            handOriginPos = hand.parent.localPosition;
            handOriginRot = hand.parent.localRotation;
            currentCombatTime = combatTime;
            currentWPressTime = WPressTime;
            currentDashPower = dashPower;
            currentClimbUpTime = climbUpTime;
            currentClimbuUpPower = climbUpPower;
            originBodyColliderCenter = bodyCollider.center;
            originBodyColliderHeight = bodyCollider.height;
            currentKickWallTime = 0;
            originAimPos = aimPos.position;
            currentWeaponNum = 1;

            hand.GetChild(currentWeaponNum - 1).gameObject.SetActive(true);

            weapon_gameObject = hand.GetChild(currentWeaponNum - 1).GetChild(0).GetChild(0).gameObject;
            weapon = weapon_gameObject.GetComponent<Gun>();

            if (weapon.GetOwner() == null)
                weapon.SetOwner(this.gameObject, hand);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || GameManager.Instance.GetIsPause() || isPushed)
        {
            if (isDead)
                rigid.velocity = Vector3.zero;

            if (isPushed)
            {
                rigid.velocity = Vector3.Lerp(rigid.velocity, Vector3.zero, Time.deltaTime * 6);

                if (rigid.velocity.magnitude <= 6.0f)
                {
                    isPushed = false;
                }
            }

            return;
        }

        if (!weapon.GetIsReload() && !isSwap)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwapWeapon(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwapWeapon(2);
            }
        }

        

        //if (hand.childCount > 0)
        //{
        //    for (int i = 0; i < hand.childCount; i++)
        //    {
        //        if (hand.GetChild(i).gameObject.activeSelf)
        //        {
        //            weapon_gameObject = hand.GetChild(i).GetChild(0).gameObject;
        //            weapon = weapon_gameObject.GetComponent<Gun>();

        //            if (weapon.GetOwner() == null)
        //                weapon.SetOwner(this.gameObject, hand);
        //        }
        //    }
        //}

        Vector3 forward = mainCam.transform.forward;
        Vector3 right = mainCam.transform.right;

        forward.y = 0; right.y = 0;

        forward.Normalize();
        right.Normalize();

        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

        RaycastHit hit;


        if (Physics.SphereCast(checkingGroundRayPos.position, 0.25f, Vector3.down, out hit, 0.2f, 1 << LayerMask.NameToLayer("Enviroment")))
        {
            if (currentJumpPower <= jumpPower / 2)
            {
                isJump = false;
                isJumpByObject = false;
                canJump = true;
            }

            if (!isJump && isClimbing && !isClimbUp)
                this.transform.position = new Vector3(this.transform.position.x, hit.point.y + 0.04f, this.transform.position.z);

            groundCollider.enabled = true;
            isClimbing = false;
            canClimb = true;
            useGravity = false;

            Vector3 slopeResult = Vector3.Cross(hit.normal, Vector3.Cross(rigid.velocity.normalized, hit.normal));
            Vector3 temp = Vector3.Cross(Vector3.Cross(hit.normal, moveDirection), hit.normal);
            result = temp;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                //슬라이딩
                //if (rigid.velocity.magnitude > walkSpeed && !isSlide && !isJump && !isJumpByObject)
                //{
                //    if (Vector3.Dot(moveDirection, forward) > 0)
                //    {

                //        if (!isCrouch)
                //        {
                //            if (!isAiming)
                //            {
                //                mainCam.FovMove(mainCam.GetRealOriginFov() + 10, 0.1f, 1000);
                //                mainCam.SetOriginFov(mainCam.GetRealOriginFov() + 10);
                //            }
                //            isSlide = true;

                //            if (currentSlidingCoolTime <= 0)
                //            {
                //                rigid.AddForce(slopeResult.normalized * new Vector3(rigid.velocity.x, rigid.velocity.y / 2, rigid.velocity.z).magnitude * 0.75f, ForceMode.VelocityChange);
                //                slideSpeed = rigid.velocity.magnitude * 1.5f;
                //            }
                //        }

                //        slidingDirection = moveDirection;
                //    }
                //}

                if (!isClimbing && !isClimbUp && !isSlide && !isJump && !isJumpByObject)
                {
                    isCrouch = true;

                    bodyCollider.center = new Vector3(0, 0.3922825f, 0);
                    bodyCollider.height = 0.9702759f;
                }

            }

            if (!isGrounded && !isSlide && rigid.velocity.y < -3.0f)
            {
                isLanding = true;
                headBobValue = Mathf.PI;
            }

            isGrounded = true;

            //달리기
            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    if (isPressW)
            //    {
            //        isRun = true;
            //        isPressW = false;
            //        currentWPressTime = WPressTime;
            //    }

            //    if (!isSlide && !isCombat && !isAiming && !isRun)
            //    {
            //        isPressW = true;
            //    }
            //}

            if (isPressW)
            {
                currentWPressTime -= Time.deltaTime;

                if (currentWPressTime <= 0)
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
                        mainCam.SetOriginFov(mainCam.GetRealOriginFov());
                        mainCam.FovReset();
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

            if (!isDash)
                useGravity = true;
            groundCollider.enabled = false;
            if (isGrounded && !isJump && !isJumpByObject)
            {
                rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
            }

            isGrounded = false;
            isLanding = false;

            if (!isClimbUp)
            {
                if (!isClimbing)
                {
                    Vector2 tempVel = new Vector2(rigid.velocity.x, rigid.velocity.z);
                    if (tempVel.magnitude >= walkSpeed)
                    {
                        rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(moveDirection.x * tempVel.magnitude, rigid.velocity.y, moveDirection.z * tempVel.magnitude), Time.deltaTime * 6);
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

            if (!isClimbUp)
            {
                for (int i = 0; i < 12; i++)
                {
                    //Debug.DrawRay(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f, forward * 0.35f);
                    if (Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f, forward, out wallHit, 0.38f, 1 << LayerMask.NameToLayer("Enviroment")))
                    {
                        if (Mathf.Abs(wallHit.normal.y) <= 0.3f && Vector3.Dot(moveDirection, forward) > 0.7f)
                        {
                            if (Input.GetKey(KeyCode.Space))
                            {
                                if (!isClimbing)
                                {
                                    currentKickWallTime = 0;
                                }
                                isJump = false;
                                isJumpByObject = false;
                                isClimbing = true;
                                isSlide = false;
                                isDash = false;
                                isJump = false;
                                isJumpByObject = false;
                                //canJump = false;

                                rigid.velocity = new Vector3(rigid.velocity.x, currentClimbPower, rigid.velocity.z);
                                mainCam.SetOriginFov(mainCam.GetRealOriginFov());
                                mainCam.FovReset();
                            }
                        }
                    }
                    else if (moveDirection != Vector3.zero)
                    {
                        if (!Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * (i - 1)) + Vector3.up * 0.35f, forward, 0.38f, 1 << LayerMask.NameToLayer("Enviroment")))
                        {
                            isClimbing = false;
                            break;
                        }

                        if (!Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * (i - 1)) + Vector3.up * 0.35f, forward, 0.38f, 1 << LayerMask.NameToLayer("Enviroment")))
                        {
                            isClimbing = false;
                            break;
                        }

                        if (Physics.BoxCast(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f, new Vector3(0.5f, 0.5f, 0.5f), Vector3.up, Quaternion.identity, 1.6f, 1 << LayerMask.NameToLayer("Enviroment")))
                        {
                            isClimbing = false;
                            break;
                        }

                        isClimbUp = true;
                        rigid.velocity = Vector3.zero;
                        climbUpPos = this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.35f + forward * 0.38f;

                        if (isClimbing)
                        {
                            canClimb = false;
                        }

                        isClimbing = false;
                    }
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
                mainCam.FovMove(mainCam.GetOriginFov() - 2.0f, 0.1f, 0.1f);
            if (Vector3.Dot(forward, moveDirection) < 0)
                mainCam.FovMove(mainCam.GetOriginFov() + 2.0f, 0.1f, 0.1f);

            if (isJump)
            {
                //canJump = false;
            }
            isJump = false;
            isJumpByObject = false;

            if (isGrounded)
                dashDirection = result;
            else
                dashDirection = moveDirection.normalized;
        }

        if (useGravity)
        {
            if (!isJump && !isJumpByObject)
                rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(rigid.velocity.x, rigid.velocity.y + Physics.gravity.y, rigid.velocity.z), Time.deltaTime);
        }

        if (isDash)
        {
            if (!isGrounded)
                rigid.velocity = new Vector3(dashDirection.x * currentDashPower, 0, dashDirection.z * currentDashPower);
            else
                rigid.velocity = dashDirection * currentDashPower;

            useGravity = false;
            currentDashPower -= (Time.deltaTime * dashPower) / dashTime;

            if (currentDashPower <= walkSpeed)
            {
                isDash = false;
                currentDashPower = dashPower;
            }
        }
        else
        {
            currentDashPower = dashPower;
        }

        if (weapon != null)
        {
            if (Input.GetMouseButton(0) && !isClimbUp && !isClimbing && !isSwap)
            {
                if (weapon.Fire())
                {
                    handFireRot = weapon.GetHandFireRot();

                    if (isGrounded && !isSlide)
                    {
                        isCombat = true;
                        isRun = false;
                    }

                    currentCombatTime = combatTime;
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (weapon.CanReload() && !isSwap)
            {
                isAiming = false;
                mainCam.SetOriginFov(mainCam.GetRealOriginFov());
                mainCam.FovReset();
                weapon.SetIsReload(true);
                hand.localRotation = handOriginRot;
                //hand.localPosition = handOriginPos;
                handFireRot = Quaternion.Euler(Vector3.zero);
            }
        }

        if (Input.GetMouseButtonDown(1) && !weapon.GetIsReload())
        {
            isAiming = !isAiming;
            isMoveAim = true;

            if (isAiming)
            {
                weapon.SetIsReload(false);
                mainCam.FovMove(mainCam.GetOriginFov() - mainCam.GetOriginFov() / 4f, 0.07f, 1000);
                mainCam.SetOriginFov(mainCam.GetOriginFov() - mainCam.GetOriginFov() / 4f);
            }
            else
            {
                mainCam.SetOriginFov(mainCam.GetRealOriginFov());
                mainCam.FovReset();
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
            bodyCollider.center = originBodyColliderCenter;
            bodyCollider.height = originBodyColliderHeight;
            mainCam.SetOriginFov(mainCam.GetRealOriginFov());
            mainCam.FovReset();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isDash && canJump && isGrounded)
        {
            isSlide = false;
            if (!isAiming)
            {
                mainCam.SetOriginFov(mainCam.GetRealOriginFov());
                mainCam.FovReset();
            }
            isJump = true;

            //if(!isGrounded)
            canJump = false;

            if (isJumpByObject)
            {
                isJumpByObject = false;
                rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
                currentJumpPower = jumpPower;
            }
            else
            {
                if (!isSlope)
                    rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);

                currentJumpPower = rigid.velocity.y / 2 + jumpPower;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isClimbing)
            {
                if (canClimb)
                {
                    if (currentKickWallTime < kickWallTime)
                    {
                        if (rigid.velocity.magnitude >= walkSpeed)
                            rigid.AddForce((wallHit.normal + Vector3.up * 0.2f).normalized * rigid.velocity.magnitude, ForceMode.Impulse);
                        else
                            rigid.AddForce((wallHit.normal + Vector3.up * 0.2f).normalized * 8, ForceMode.Impulse);
                    }
                }
                canClimb = false;
            }

            isClimbing = false;
        }

        //스킬
        //if(Input.GetKeyDown(KeyCode.Q))
        //{
        //    if (currentCombo == maxCombo)
        //    {
        //        currentCombo = 0;
        //        currentResetComboTime = 0;

        //        Collider[] target = Physics.OverlapSphere(this.transform.position, skill1_range, 1 << LayerMask.NameToLayer("Enemy"));

        //        for (int i = 0; i < target.Length; i++)
        //        {
        //            if(Physics.Raycast(camPos.position, (target[i].transform.position - camPos.position).normalized, skill1_range, 1 << LayerMask.NameToLayer("Enemy")))
        //                target[i].GetComponent<Enemy>().DecreaseHp(200, false);
        //        }
        //    }
        //}

        if (isCombat)
        {
            currentCombatTime -= Time.deltaTime;

            if (currentCombatTime <= 0)
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
            mainCam.SetOriginFov(mainCam.GetRealOriginFov());
            mainCam.FovReset();
            currentClimbUpTime -= Time.deltaTime;


            if (currentClimbuUpPower <= 2)
            {
                currentClimbuUpPower = 2;
            }
            else
            {
                currentClimbuUpPower -= Time.deltaTime * 20;
            }

            rigid.velocity = (climbUpPos - this.transform.position).normalized * currentClimbuUpPower;

            if (Vector3.Distance(this.transform.position, climbUpPos) <= 0.13f || currentClimbUpTime <= 0)
            {
                currentClimbuUpPower = climbUpPower;
                currentClimbUpTime = climbUpTime;
                isClimbUp = false;
                rigid.velocity = Vector3.zero;
            }

        }

        if (isClimbing)
        {
            currentClimbPower += Time.deltaTime * (Physics.gravity.y / 3.2f);
            currentKickWallTime += Time.deltaTime;
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
            if (Physics.Raycast(mainCam.transform.position, Vector3.up, 0.5f, 1 << LayerMask.NameToLayer("Enviroment")))
            {
                isJump = false;
                isJumpByObject = false;
            }

            rigid.velocity = new Vector3(rigid.velocity.x, currentJumpPower, rigid.velocity.z);
            currentJumpPower += Time.deltaTime * Physics.gravity.y;
        }
        else
        {
            currentJumpPower = 0;
        }

        if (isSlide)
        {
            //isRun = false;
            currentSlidingCoolTime = slidingCoolTime;

            if (moveDirection != Vector3.zero)
            {
                if (rigid.velocity.magnitude <= walkSpeed)
                {
                    isSlide = false;
                    mainCam.SetOriginFov(mainCam.GetRealOriginFov());
                    mainCam.FovReset();
                }
            }
            else
            {
                if (rigid.velocity.magnitude <= 0.5f)
                {
                    isSlide = false;
                    mainCam.SetOriginFov(mainCam.GetRealOriginFov());
                    mainCam.FovReset();
                }
            }
        }
        else
        {
            if (currentSlidingCoolTime > 0)
            {
                currentSlidingCoolTime -= Time.deltaTime;
            }

            isSlope = false;
        }



        HeadBob();
        HandAnimation();
        //DecreaseHpPerSecond();
        //UpdateComboDamage();

        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 28.0f);

        this.transform.rotation = Quaternion.LookRotation(forward);
    }

    private void HeadBob()
    {
        if (!isSlide)
        {
            if (isLanding)
            {
                if (!isCrouch)
                {
                    headBobValue += Time.deltaTime * landingReboundSpeed;

                    camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(camPos.localPosition.x, headOriginY + Mathf.Sin(headBobValue) / 3.0f, camPos.localPosition.z), Time.deltaTime * 22);

                    if (headBobValue >= 2 * Mathf.PI)
                        isLanding = false;
                }
                else
                {
                    isLanding = false;
                }
            }
            else
            {
                if (isGrounded)
                {
                    if (moveDirection == Vector3.zero)
                    {
                        headBobValue += Time.deltaTime * 1.0f;

                        if (isCrouch)
                            camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY / 2 + Mathf.Abs(Mathf.Sin(headBobValue)) / 30, 0), Time.deltaTime * 8);
                        else
                            camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY + Mathf.Abs(Mathf.Sin(headBobValue)) / 30, 0), Time.deltaTime * 8);
                    }
                    else if (!isDash)
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
                                camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(Mathf.Sin(headBobValue) / 30, headOriginY + Mathf.Abs(Mathf.Sin(headBobValue)) / 3.5f, 0), Time.deltaTime * 10);
                            else
                                camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(Mathf.Sin(headBobValue) / 50, headOriginY + Mathf.Abs(Mathf.Sin(headBobValue)) / 6, 0), Time.deltaTime * 8);
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
        }
        else
        {
            isLanding = false;
            headBobValue = 0;
            camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY / 2, 0), Time.deltaTime * 8);
        }

        if (isCrouch)
        {
            aimPos.localPosition = new Vector3(0, 0.542f, 0);
        }
        else
        {
            aimPos.localPosition = originAimPos;
        }
    }

    private void HandAnimation()
    {


        if (!isSwap)
        {
            if (isAiming)
            {
                if (isGrounded)
                    isRun = false;

                if (isMoveAim)
                {

                    hand.localPosition = Vector3.Lerp(hand.localPosition, new Vector3(0, -0.08f, 0.087f), Time.deltaTime * 35);
                    hand.localRotation = Quaternion.Lerp(hand.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 35);


                    if (Vector3.Distance(hand.localPosition, new Vector3(0, -0.08f, 0.087f)) < 0.001f)
                    {
                        hand.localPosition = new Vector3(0, -0.08f, 0.087f);
                        hand.localRotation = Quaternion.Euler(0, 0, 0);
                        isMoveAim = false;
                    }


                }
            }
            else
            {
                if (isMoveAim)
                {
                    hand.localPosition = Vector3.Lerp(hand.localPosition, handOriginPos, Time.deltaTime * 25);
                    hand.localRotation = Quaternion.Lerp(hand.localRotation, handOriginRot, Time.deltaTime * 35);

                    if (Vector3.Distance(hand.localPosition, handOriginPos) < 0.001f)
                    {
                        hand.localPosition = handOriginPos;
                        hand.localRotation = handOriginRot;
                        isMoveAim = false;
                    }

                }
            }
        }

        if (weapon.GetIsReload())
        {
            //hand.localRotation = Quaternion.Lerp(hand.localRotation, Quaternion.Euler(handOriginRot.eulerAngles.x, handOriginRot.eulerAngles.y, -30), Time.deltaTime * 12);
            
            
        }
        else
        {
                    handFireRot = Quaternion.Lerp(handFireRot, Quaternion.Euler(0, 0, 0), Time.deltaTime * 15);
            if (!isMoveAim)
            {
                if (!isSwap)
                {
                    if (isAiming)
                    {
                        hand.localRotation = Quaternion.Lerp(hand.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 10);
                        hand.localPosition = Vector3.Lerp(hand.localPosition, new Vector3(0, -0.08f, 0.087f), Time.deltaTime * 35);
                    }
                    else
                    {
                        hand.localRotation = Quaternion.Lerp(hand.localRotation, Quaternion.Euler(handOriginRot.eulerAngles + handFireRot.eulerAngles), Time.deltaTime * 10);
                        hand.localPosition = Vector3.Lerp(hand.localPosition, handOriginPos, Time.deltaTime * 25);

                    }
                }
                

            }
        }
    }

    private void LateUpdate()
    {
        if(weapon.GetIsReload())
        {
            hand.localPosition = Vector3.Lerp(hand.localPosition, handOriginPos, Time.deltaTime * 25);
        }
    }

    private void SwapWeapon(int num)
    {
        if (currentWeaponNum == num)
            return;

        currentWeaponNum = num;

        isSwap = true;

        hand.GetComponent<Animator>().applyRootMotion = false;

        hand.localPosition = handOriginPos;
        hand.localRotation = handOriginRot;
        handFireRot = Quaternion.Euler(0, 0, 0);

        isAiming = false;
        mainCam.SetOriginFov(mainCam.GetRealOriginFov());
        mainCam.FovReset();

        hand.GetComponent<Animator>().SetBool("isSwap", true);

        weapon_gameObject = hand.GetChild(currentWeaponNum - 1).GetChild(0).GetChild(0).gameObject;
        weapon = weapon_gameObject.GetComponent<Gun>();

        if (weapon.GetOwner() == null)
            weapon.SetOwner(this.gameObject, hand);
    }

    public void SwapWeapon()
    {
        hand.GetComponent<Animator>().applyRootMotion = true;
        for (int i = 0; i < hand.childCount; i++)
        {
            hand.GetChild(i).gameObject.SetActive(false);
        }

        hand.GetChild(currentWeaponNum - 1).gameObject.SetActive(true);

    
    }

    private void CheckingHp()
    {
        if (currentHP <= 0)
        {
            currentHP = 0;

            isDead = true;

            GameManager.Instance.SetIsGameOver(true);
        }
        else if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    private void DecreaseHpPerSecond()
    {
        currentHP -= decreaseHpValuePerSecond * Time.deltaTime;

        CheckingHp();
    }

    public void DecreaseHp(float value)
    {
        currentHP -= value;

        CheckingHp();
    }

    public void IncreaseHp(float value)
    {
        currentHP += value;

        CheckingHp();
    }

    void CheckingCombo()
    {
        if (currentCombo < 0)
        {
            currentCombo = 0;
        }
        else if (currentCombo > maxCombo)
            currentCombo = maxCombo;
    }

    public void IncreaseCombo(int value)
    {
        //currentResetComboTime = resetComboTime / (1 + ((float)currentCombo / maxCombo));
        if (currentResetComboTime > keepComboTime || currentCombo == 0)
            currentCombo += value;
        currentResetComboTime = resetComboTime;

        CheckingCombo();
    }

    public void DecreaseCombo(int value)
    {
        currentCombo -= value;

        CheckingCombo();
    }

    public void UpdateComboDamage()
    {
        if (currentCombo > 0)
        {
            currentResetComboTime -= Time.deltaTime;
            if (currentResetComboTime <= 0)
            {
                DecreaseCombo(1);
                //currentResetComboTime = resetComboTime / 2;
                if (currentCombo > 0)
                    currentResetComboTime = downComboTime;
                else
                    currentResetComboTime = 0;
            }
        }

        //콤보가 오르면 데미지 증가
        //weapon.SetDamagePerBullet(weapon.GetDamagePerBullet_Origin() + (float)currentCombo * 2);
    }
}
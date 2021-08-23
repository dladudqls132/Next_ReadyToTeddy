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

    private Quaternion lastAngle_hand;
    private Vector3 lastPos_hand;

    [SerializeField] private GameObject weapon_gameObject = null;
    [SerializeField] private GameObject tempWeapon;
    [SerializeField] private Gun gun = null;
    [SerializeField] private Projectile projectile = null;
    private ThrowProjectile projectileController;
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
    [SerializeField] private float walkSpeed_min = 0;
    [SerializeField] private float walkSpeed_max = 0;
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
    private bool isPressMouseButton = false;
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
    //[SerializeField] private float kickWallTime;
    //[SerializeField] private float currentKickWallTime;
    private bool isSwap;
    private bool isAimingProjectile;

    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject[] weapons;

    private bool isMoveAim;
    private Vector3 climbUpPos = Vector3.zero;
    public Vector2 moveInput = Vector2.zero;
    private Rigidbody rigid = null;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 slidingDirection = Vector3.zero;
    private Vector3 dashDirection = Vector3.zero;
    private float headBobValue = 0;
    private float headOriginY = 0;
    [SerializeField] private Vector3 handOriginPos = Vector3.zero;
    private Quaternion handOriginRot = Quaternion.identity;
    private Vector3 result = Vector3.zero;
    private Vector3 originBodyColliderCenter;
    private float originBodyColliderHeight;
    private RaycastHit wallHit;
    private bool isPushed;
    private Vector3 originAimPos;
    private Quaternion handFireRot;
    private float pickupTime = 0.5f;
    private float currentPickupTime;
    private bool isPickup;
    [SerializeField] private bool isFever;
    [SerializeField] private float feverTime;
    [SerializeField] private float currentFeverTime;

    private bool isInit = false;

    private List<GameObject> collisionWeapon = new List<GameObject>();

    [SerializeField] private float dashRefillTime;
    private float currentDashRefillTime;
    [SerializeField] private int dashCount;
    [SerializeField] private int currentDashCount;

    public void SetIsGrounded(bool value) { isGrounded = value; }
    public GameObject GetWeaponGameObject() { return weapon_gameObject; }
    public GameObject GetTempWeaponGameObject() { return tempWeapon; }
    public Gun GetGun() { return gun; }
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
    public void SetIsSwap(bool value) { isSwap = value; }
    public Transform GetHand() { return hand; }
    public Inventory GetInventory() { return inventory; }
    public int GetCurrentDashCount() { return currentDashCount; }
    public bool GetIsSwap() { return isSwap; }
    public void SetIsAiming(bool value) { isAiming = value; }

    private static PlayerController instance;

    //private void Awake()
    //{
    //    Debug.Log(isDead);

    //    if (instance == null)
    //    {
    //        DontDestroyOnLoad(this);
    //        instance = this;
    //    }
    //    else
    //    {
    //        if(isDead)
    //        {
    //            Debug.Log(this.gameObject);
    //            Destroy(instance.gameObject);
    //            instance = this;
    //        }
    //        else
    //            Destroy(gameObject);
    //    }
    //}



    // Start is called before the first frame update
    public void Init()
    {
        rigid = this.GetComponent<Rigidbody>();
        currentSlidingCoolTime = 0;
        headBobValue = 0;
        headOriginY = camPos.localPosition.y;
        mainCam = Camera.main.transform.GetComponent<FPPCamController>();
        if (hand == null)
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
        //currentKickWallTime = 0;
        originAimPos = aimPos.localPosition;
        projectileController = this.GetComponent<ThrowProjectile>();
        currentDashCount = dashCount;
        //currentWeaponNum = 1;
        //for (int i = 0; i < hand.childCount - 1; i++)
        //{
        //    slot.Add(hand.GetChild(i));
        //}
        ////hand.GetChild(currentWeaponNum - 1).gameObject.SetActive(true);

        //weapon_gameObject = hand.GetChild(currentWeaponNum - 1).GetChild(0).gameObject;
        //gun = weapon_gameObject.GetComponent<Gun>();

        //if (gun.GetOwner() == null)
        //    gun.SetOwner(this.gameObject, hand);

        inventory = this.GetComponent<Inventory>();
        inventory.Init();

        isInit = true;
    }

    void Start()
    {
        //for (int i = 0; i < weapons.Length; i++)
        //{
        //    GameObject temp = Instantiate(weapons[i]);

        //    inventory.AddWeapon(temp);
        //}

        GameObject temp = Instantiate(weapons[0]);
        inventory.AddWeapon(temp);
        inventory.SwapWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFever)
        {
            currentFeverTime -= Time.deltaTime;

            if (currentFeverTime <= 0)
            {
                currentFeverTime = feverTime;
                isFever = false;
            }
        }
        else
        {
            walkSpeed = Mathf.Lerp(walkSpeed, walkSpeed_min, Time.deltaTime);
        }

        if (currentDashCount < dashCount)
        {
            currentDashRefillTime += Time.deltaTime;

            if (currentDashRefillTime >= dashRefillTime)
            {
                currentDashCount++;
                currentDashRefillTime = 0;
            }
        }
        else
        {
            currentDashRefillTime = 0;
        }

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

        if (!isSwap)
        {
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    SwapWeapon(1);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    SwapWeapon(2);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha4))
            //{

            //    SwapWeapon(4);


            //}
        }


        //if (Input.GetKey(KeyCode.E))
        //{
        //    if (!isPickup)
        //    {
        //        currentPickupTime += Time.deltaTime;

        //        if (currentPickupTime >= pickupTime)
        //        {
        //            currentPickupTime = 0;
        //            isPickup = true;
        //            PickUpWeapon_Change();
        //        }
        //    }
        //}
        //else if (Input.GetKeyUp(KeyCode.E))
        //{
        //    if (!isPickup)
        //    {
        //        PickUpWeapon();
        //    }

        //    currentPickupTime = 0;
        //    isPickup = false;
        //}

        //if (!isSwap)
        //{
        //    if (Input.GetKeyDown(KeyCode.G))
        //    {
        //        inventory.DropWeapon();
        //    }
        //}

        //if (projectile != null)
        //{

        //    if (projectile.GetHaveNum() == 0)
        //    {
        //        SwapWeapon(1);
        //    }

        //}



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

            if (!isGrounded && !isSlide && rigid.velocity.y < -1.0f)
            {
                if(!isLanding)
                    GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Land);

                isLanding = true;
                hand.GetComponent<Animator>().SetTrigger("Landing");
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
                        if (isCrouch || isAiming)
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
            //if (isGrounded && !isJump && !isJumpByObject)
            //{
            //    rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
            //}

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
                        //if (Mathf.Abs(wallHit.normal.y) <= 0.3f && Vector3.Dot(moveDirection, forward) > 0.7f)
                        //{
                        //    if (Input.GetKey(KeyCode.Space))
                        //    {
                        //        //if (!isClimbing)
                        //        //{
                        //        //    currentKickWallTime = 0;
                        //        //}
                        //        isJump = false;
                        //        isJumpByObject = false;
                        //        isClimbing = true;
                        //        isSlide = false;
                        //        isDash = false;
                        //        isJump = false;
                        //        isJumpByObject = false;
                        //        //canJump = false;

                        //        rigid.velocity = new Vector3(rigid.velocity.x, currentClimbPower, rigid.velocity.z);
                        //        mainCam.SetOriginFov(mainCam.GetRealOriginFov());
                        //        mainCam.FovReset();
                        //    }
                        //}
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSlide && !isClimbing && !isClimbUp && moveDirection != Vector3.zero && currentDashCount > 0 && !isDash)
        {
            GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Dash);

            isDash = true;
            currentDashCount--;

            if(moveInput.x > 0)
            {
                mainCam.GetComponent<Animator>().SetTrigger("Dash_Right");
            }
            else if(moveInput.x < 0)
            {
                mainCam.GetComponent<Animator>().SetTrigger("Dash_Left");
            }

            if (Vector3.Dot(forward, moveDirection) > 0.5f)
            {
                mainCam.FovMove(mainCam.GetCurrentFov() - 5.0f, 0.05f, 0.16f, 0.04f);
            }
            if (Vector3.Dot(forward, moveDirection) < 0)
            {
                mainCam.FovMove(mainCam.GetCurrentFov() + 5.0f, 0.05f, 0.16f, 0.04f);
            }
            

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
                //mainCam.GetComponent<Animator>().SetBool("isDash_Right", false);
                //mainCam.GetComponent<Animator>().SetBool("isDash_Left", false);
                isDash = false;
                currentDashPower = dashPower;
            }
        }
        else
        {
            currentDashPower = dashPower;
        }

        if (Input.GetMouseButton(0) && !isClimbUp && !isClimbing && !isSwap && !inventory.isOpen)
        {
            isPressMouseButton = true;
            if (gun != null)
            {
                if (gun.GetGunType() == GunType.ChainLightning)
                {
                    handFireRot = gun.GetHandFireRot();
                    gun.GetComponent<Gun_ChainLightning>().Charging();
                }
                else
                {
                    if (gun.Fire())
                    {
                        handFireRot = gun.GetHandFireRot();

                        if (isGrounded && !isSlide)
                        {
                            isCombat = true;
                            isRun = false;
                        }

                        currentCombatTime = combatTime;
                    }
                }
            }
            else if (projectile != null)
            {
                //if (projectileController.AimingProjectile())
                //    isAimingProjectile = true;
                //else
                //{
                //    isAimingProjectile = false;
                //}
            }
        }
        else if (!Input.GetMouseButtonDown(0) && isPressMouseButton)
        {
            isPressMouseButton = false;
            if (gun != null)
            {
                if (gun.GetGunType() == GunType.ChainLightning)
                {
                    if (!isSwap)
                        gun.GetComponent<Gun_ChainLightning>().Fire();
                }
                else if (gun.GetGunType() == GunType.Flamethrower)
                {
                    gun.GetComponent<Gun_FlameThrower>().Off();
                }
            }
            //if (isAimingProjectile)
            //{
            //    projectileController.LaunchProjectile();
            //    projectile.DecreaseHaveNum();
            //    isAimingProjectile = false;

            //    if (projectile.GetHaveNum() == 0)
            //    {
            //        inventory.DestroyWeapon(4);
            //    }
            //}
            //else
            //{
            //    projectileController.ResetInfo();
            //}
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gun != null)
            {
                if (gun.CanReload() && gun.GetGunType() != GunType.Flamethrower && !isSwap)
                {
                    isAiming = false;

                    mainCam.FovReset();
                    gun.SetIsReload(true);
                    //hand.localRotation = handOriginRot;
                    //hand.localPosition = handOriginPos;
                    //lastAngle_hand = handOriginRot;
                    //lastPos_hand = handOriginPos;
                    handFireRot = Quaternion.Euler(Vector3.zero);
                }
            }

        }


        //if (gun != null)
        //{
        //    if (Input.GetMouseButtonDown(1) && !gun.GetIsReload() && gun.GetGunType() != GunType.Flamethrower && gun.GetGunType() != GunType.ChainLightning)
        //    {
        //        if (tempWeapon.GetComponent<Gun>())
        //        {
        //            if (tempWeapon.GetComponent<Gun>().GetGunType() != GunType.ChainLightning)
        //            {
        //                isAiming = !isAiming;
        //                isMoveAim = true;

        //                if (isAiming)
        //                {

        //                    //mainCam.FovMove(52, 0.07f, 1000);
        //                    //mainCam.SetOriginFov(52);
        //                    gun.SetIsReload(false);
        //                }
        //                else
        //                {
        //                    mainCam.FovReset();
        //                }
        //            }
        //        }
        //    }
        //}


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
            //mainCam.SetOriginFov(mainCam.GetRealOriginFov());
            //mainCam.FovReset();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isDash && canJump)
        {
            isSlide = false;
            if (!isAiming)
            {
                mainCam.SetOriginFov(mainCam.GetRealOriginFov());
                mainCam.FovReset();
            }

            if (!isJump)
            {
                if (isGrounded)
                {
                    canJump = true;
                    GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Jump);
                }
                else
                {
                    canJump = false;
  
                }
            }
            else
                canJump = false;

                isJump = true;
            hand.GetComponent<Animator>().SetTrigger("Jump");
            //if(!isGrounded)
            
   

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

                currentJumpPower =  jumpPower;
            }
        }

        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    if (isClimbing)
        //    {
        //        if (canClimb)
        //        {
        //            if (currentKickWallTime < kickWallTime)
        //            {
        //                if (rigid.velocity.magnitude >= walkSpeed)
        //                    rigid.AddForce((wallHit.normal + Vector3.up * 0.2f).normalized * rigid.velocity.magnitude, ForceMode.Impulse);
        //                else
        //                    rigid.AddForce((wallHit.normal + Vector3.up * 0.2f).normalized * 8, ForceMode.Impulse);
        //            }
        //        }
        //        canClimb = false;
        //    }

        //    isClimbing = false;
        //}

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
            //currentKickWallTime += Time.deltaTime;
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

        //if (isSlide)
        //{
        //    //isRun = false;
        //    currentSlidingCoolTime = slidingCoolTime;

        //    if (moveDirection != Vector3.zero)
        //    {
        //        if (rigid.velocity.magnitude <= walkSpeed)
        //        {
        //            isSlide = false;
        //            mainCam.SetOriginFov(mainCam.GetRealOriginFov());
        //            mainCam.FovReset();
        //        }
        //    }
        //    else
        //    {
        //        if (rigid.velocity.magnitude <= 0.5f)
        //        {
        //            isSlide = false;
        //            mainCam.SetOriginFov(mainCam.GetRealOriginFov());
        //            mainCam.FovReset();
        //        }
        //    }
        //}
        //else
        //{
        //    if (currentSlidingCoolTime > 0)
        //    {
        //        currentSlidingCoolTime -= Time.deltaTime;
        //    }

        //    isSlope = false;
        //}


        HeadBob();
        HandAnimation();
        //DecreaseHpPerSecond();
        //UpdateComboDamage();

        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 28.0f);

        //if (moveInput == Vector2.zero && isGrounded)
        //{
        //    rigid.velocity = Vector3.zero;
        //}

        this.transform.rotation = Quaternion.LookRotation(forward);
    }

    public bool SetWeapon(SlotType type, GameObject weapon)
    {

        if (weapon == this.weapon_gameObject)
        {
            hand.GetComponent<Animator>().SetBool("isSwap", false);
            return false;
        }

        if (type == SlotType.Projectile)
        {
            if (weapon.GetComponent<Projectile>().GetHaveNum() == 0)
                return false;
        }

        if (gun != null)
        {
            if (gun.GetIsReload())
                gun.SetIsReload(false);
            if (gun.GetIsAiming())
                gun.SetIsAiming(false);
        }

        tempWeapon = weapon;

        if (tempWeapon == null)
        {
            //this.weapon_gameObject = tempWeapon;
            this.gun = null;
            this.projectile = null;
        }

        if (projectile != null)
            projectileController.ResetInfo();

        isSwap = true;

        handFireRot = Quaternion.Euler(0, 0, 0);

        isAiming = false;
        mainCam.SetOriginFov(mainCam.GetRealOriginFov());
        mainCam.FovReset();

        hand.GetComponent<Animator>().SetBool("isSwap", true);
        //hand.GetComponent<Animator>().SetTrigger("isSwap_test");

        return true;
    }

    public bool SwapWeapon()
    {
        if ((this.weapon_gameObject != null && this.weapon_gameObject.transform.parent != null) || (tempWeapon == null && this.weapon_gameObject.transform.parent != null))
        {
            this.weapon_gameObject.SetActive(false);
        }

        this.weapon_gameObject = tempWeapon;

        if (this.weapon_gameObject != null)
        {
            this.weapon_gameObject.SetActive(true);

            if (tempWeapon.GetComponent<Gun>() != null)
            {
                this.gun = tempWeapon.GetComponent<Gun>();
                projectileController.SetProjectile(null);
                this.projectile = null;
            }
            else if (tempWeapon.GetComponent<Projectile>() != null)
            {
                this.projectile = tempWeapon.GetComponent<Projectile>();
                projectileController.SetProjectile(projectile.projectile.GetComponent<Rigidbody>());
                this.gun = null;
            }

            return true;
        }
        else
        {
            this.projectile = null;
            this.gun = null;
            return false;
        }
    }

    private bool footstep;

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
                        //headBobValue += Time.deltaTime * 1.0f;
                        headBobValue = 0;

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
                            if(isCrouch || isAiming)
                                headBobValue += Time.deltaTime * walkSpeed * 0.85f;
                            else
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
                        //headBobValue = Mathf.Lerp(headBobValue, 0, Time.deltaTime * 15);
                    }
                }
            }
        }
        else
        {
            isLanding = false;
            //headBobValue = Mathf.Lerp(headBobValue, 0, Time.deltaTime * 15);
            camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY / 2, 0), Time.deltaTime * 8);
        }

        mainCam.GetComponent<Animator>().SetFloat("horizontal", Mathf.Lerp(mainCam.GetComponent<Animator>().GetFloat("horizontal"), moveInput.x, Time.deltaTime * 10));

        if (isCrouch)
        {
            aimPos.localPosition = new Vector3(0, 0.542f, 0);
        }
        else
        {
            aimPos.localPosition = originAimPos;
        }

        if (isGrounded)
        {
            if (Mathf.Abs(Mathf.Sin(headBobValue)) / 6 <= 0.05f)
            {
                if (moveInput != Vector2.zero)
                {
                    if (!footstep)
                    {
                        GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Walk);
                        footstep = true;
                    }
                }
            }
            else
            {
                footstep = false;
            }
        }
        else
        {
            footstep = false;
        }

        //if (!firstStep)
        //{
        //    if (moveInput != Vector2.zero)
        //    {
        //        GameManager.Instance.GetSoundManager().AudioPlayOneShot(AudioSourceType.SFX, SoundType.Walk_1);
        //        firstStep = true;
        //        footstep = true;
        //    }
        //}
        //else
        //{
        //    currentFootstepTime += Time.deltaTime;

        //    if(currentFootstepTime >= footstepTime)
        //    {
        //        currentFootstepTime = 0;
        //        GameManager.Instance.GetSoundManager().AudioPlayOneShot(AudioSourceType.SFX, SoundType.Walk_1);
        //    }

        //    if (moveInput == Vector2.zero)
        //    {
        //        currentFootstepTime = 0;
              
        //        firstStep = false;
        //    }
        //}
    }

    private void HandAnimation()
    {
        if (gun != null)
        {
            if (gun.GetIsReload())
            {
                gun.SetIsAiming(false);
                lastAngle_hand = hand.localRotation;
                lastPos_hand = Vector3.Lerp(lastPos_hand, handOriginPos, Time.deltaTime * 15);
                handFireRot = Quaternion.Euler(Vector3.zero);

                hand.localPosition = lastPos_hand;
                hand.localRotation = lastAngle_hand;
            }
            else
            {
                handFireRot = Quaternion.Lerp(handFireRot, Quaternion.Euler(0, 0, 0), Time.deltaTime * 15);

                if (!isSwap)
                {
                    if (isAiming)
                    {
                        
                        gun.SetIsAiming(true);
                        if (!isDash)
                        {
                            mainCam.FovMove(52, 0.07f, 1000);
                            mainCam.SetOriginFov(52);
                        }

                        float temp = hand.localRotation.eulerAngles.z;
                        if (temp > 180)
                            temp -= 360;
                        lastAngle_hand = Quaternion.Lerp(lastAngle_hand, Quaternion.Euler(new Vector3(hand.localRotation.eulerAngles.x, 0, temp / 3) + handFireRot.eulerAngles * 3), Time.deltaTime * 20);

                        if (!isLanding)
                            lastPos_hand = Vector3.Lerp(lastPos_hand, new Vector3(0, -0.08f, 0.087f) + new Vector3(Mathf.Sin(headBobValue) / 600, Mathf.Abs(Mathf.Sin(headBobValue)) / 500f, 0), Time.deltaTime * 30);

                        //if (moveInput == Vector2.zero)
                        //{
                        //    lastPos_hand = Vector3.Lerp(lastPos_hand, new Vector3(0, -0.08f, 0.087f - (0.2245002f - hand.localPosition.z)), Time.deltaTime * 30);
                        //}
                        //else
                        //{
                        //    lastPos_hand = Vector3.Lerp(lastPos_hand, new Vector3(0, -0.08f, 0.087f - (0.2245002f - hand.localPosition.z)), Time.deltaTime * 30);
                        //    lastPos_hand = Vector3.Lerp(lastPos_hand, new Vector3(lastPos_hand.x, -0.08f, lastPos_hand.z) + new Vector3(Mathf.Sin(headBobValue) / 500, Mathf.Abs(Mathf.Sin(headBobValue)) / 300f, 0), Time.deltaTime * 10);

                        //}

                        hand.localRotation = lastAngle_hand;
                        hand.localPosition = lastPos_hand;
                    }
                    else
                    {
                        gun.SetIsAiming(false);
                        lastAngle_hand = Quaternion.Lerp(lastAngle_hand, Quaternion.Euler(hand.localRotation.eulerAngles + handFireRot.eulerAngles), Time.deltaTime * 30);

                        if (!isLanding)
                        {
                            if(gun.GetGunType() == GunType.ChainLightning)
                                lastPos_hand = Vector3.Lerp(lastPos_hand, hand.localPosition + new Vector3(Mathf.Sin(headBobValue) / 200, Mathf.Abs(Mathf.Sin(headBobValue)) / 100f, 0) + mainCam.shakeVec / 2, Time.deltaTime * 20);
                            else
                            {
                                lastPos_hand = Vector3.Lerp(lastPos_hand, hand.localPosition + new Vector3(Mathf.Sin(headBobValue) / 200, Mathf.Abs(Mathf.Sin(headBobValue)) / 100f, 0), Time.deltaTime * 20);
                            }
                        }
                        //if (moveInput == Vector2.zero)
                        //{
                        //    lastPos_hand = Vector3.Lerp(lastPos_hand, hand.localPosition, Time.deltaTime * 15);
                        //}
                        //else
                        //{
                        //    lastPos_hand = Vector3.Lerp(lastPos_hand, hand.localPosition + new Vector3(Mathf.Sin(headBobValue) / 200, Mathf.Abs(Mathf.Sin(headBobValue)) / 50f, 0), Time.deltaTime * 15);
                        //    //lastPos_hand = Vector3.Lerp(lastPos_hand, new Vector3(lastPos_hand.x, handOriginPos.y, lastPos_hand.z) + new Vector3(Mathf.Sin(headBobValue) / 200, Mathf.Abs(Mathf.Sin(headBobValue)) / 50f, 0), Time.deltaTime * 10);

                        //}
                        hand.localRotation = lastAngle_hand;
                        hand.localPosition = lastPos_hand;
                    }
                }
                else
                {
                    lastAngle_hand = hand.localRotation;
                    lastPos_hand = Vector3.Lerp(lastPos_hand, hand.localPosition, Time.deltaTime * 30);

                    hand.localPosition = lastPos_hand;
                    hand.localRotation = lastAngle_hand;
                }
            }
        }
        else
        {
            lastAngle_hand = hand.localRotation;
            lastPos_hand = Vector3.Lerp(lastPos_hand, hand.localPosition, Time.deltaTime * 30);

            hand.localPosition = lastPos_hand;
            hand.localRotation = lastAngle_hand;
        }
    }

    private void LateUpdate()
    {
       
        //HandAnimation();
    }

    //public void SwapWeapon(GameObject weapon)
    //{
    //    if (weapon.GetComponent<Gun>() != null)
    //    {
    //        if (weapon.GetComponent<Gun>().GetIsReload())
    //            return;

    //        this.weapon.gameObject.SetActive(false);

    //        this.weapon = weapon.GetComponent<Gun>();
    //        this.projectile = null;

    //        this.weapon.gameObject.SetActive(true);
    //    }
    //    else if (weapon.GetComponent<Projectile>() != null)
    //    {
    //        this.projectile.gameObject.SetActive(false);
    //        this.weapon = null;
    //        this.projectile = weapon.GetComponent<Projectile>();
    //        this.projectile.gameObject.SetActive(true);
    //    }
    //    //if (currentWeaponNum == num || hand.childCount < num || hand.GetChild(num - 1).childCount == 0)
    //    //    return;

    //    //if (currentWeaponNum != 4)
    //    //{
    //    //    if (weapon.GetIsReload())
    //    //        return;
    //    //}

    //    //if (num == 4)
    //    //{
    //    //    GameObject temp = hand.GetChild(num - 1).GetChild(0).gameObject;
    //    //    if (temp.GetComponent<Projectile>().GetHaveNum() == 0)
    //    //        return;
    //    //}

    //    //tempWeaponNum = num;

    //    isSwap = true;

    //    handFireRot = Quaternion.Euler(0, 0, 0);

    //    isAiming = false;
    //    mainCam.SetOriginFov(mainCam.GetRealOriginFov());
    //    mainCam.FovReset();

    //    hand.GetComponent<Animator>().SetBool("isSwap", true);

    //    ////weapon_gameObject = hand.GetChild(currentWeaponNum - 1).GetChild(0).GetChild(0).gameObject;
    //    ////weapon = weapon_gameObject.GetComponent<Gun>();

    //    ////if (weapon.GetOwner() == null)
    //    ////    weapon.SetOwner(this.gameObject, hand);
    //    ///

    //}



    private void CheckingHp()
    {
        if ((int)currentHP <= 0)
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

    private bool isDamaged;

    void IsDamagedFalse()
    {
        isDamaged = false;
    }

    public bool GetIsDamaged()
    {
        return isDamaged;
    }

    public void DecreaseHp(float value)
    {
        isDamaged = true;
  
        Invoke("IsDamagedFalse", 0.8f);

        currentHP -= value;

        CheckingHp();
    }

    public void IncreaseHp(float value)
    {
        currentHP += value;

        CheckingHp();
    }

    public void IncreaseSpeed()
    {
        isFever = true;
        currentFeverTime = feverTime;
        if (walkSpeed <= walkSpeed_max)
            walkSpeed += 0.5f;
        else
            walkSpeed = walkSpeed_max;
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

    public void PickUpWeapon_Change()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, 3, 1 << LayerMask.NameToLayer("Weapon")))
        {
            inventory.ChangeWeapon(hit.transform.gameObject);
        }
    }

    public void PickUpWeapon()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, 3, 1 << LayerMask.NameToLayer("Weapon")))
        {
            inventory.AddWeapon(hit.transform.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).Equals("Weapon") && !collisionWeapon.Contains(other.gameObject))
            collisionWeapon.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).Equals("Weapon") && collisionWeapon.Contains(other.gameObject))
            collisionWeapon.Remove(other.gameObject);
    }

}
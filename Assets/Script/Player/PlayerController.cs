using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private FPPCamController mainCam = null;
    [SerializeField] private Transform camPos = null;
    [SerializeField] private Transform aimPos;
    [SerializeField] private CapsuleCollider bodyCollider = null;

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

    [SerializeField] private float walkSpeed = 0;
    [SerializeField] private float walkSpeed_min = 0;
    [SerializeField] private float walkSpeed_max = 0;

    [SerializeField] private bool isSlope = false;

    private bool isPressMouseButton = false;
    private bool isCrouch = false;
    [SerializeField] private bool canJump = false;
    private bool isJump = false;
    [SerializeField] private bool isJumpByObject = false;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private float jumpPower = 0;
    [SerializeField] private float currentJumpPower = 0;

    private RaycastHit wallHit;
    [SerializeField] private bool isClimbUp = false;
    [SerializeField] private float climbUpTime = 0;
    [SerializeField] private float climbUpPower = 0;
    private float currentClimbuUpPower = 0;
    private float currentClimbUpTime = 0;

    private bool isDash;
    [SerializeField] private float dashPower = 0;
    [SerializeField] private float dashTime = 0;
    private float currentDashPower = 0;
    [SerializeField] private float dashRefillTime;
    private float currentDashRefillTime;
    [SerializeField] private int dashCount;
    [SerializeField] private int currentDashCount;
    [SerializeField] private bool useGravity = false;
    private bool isLanding = false;
    [SerializeField] private float landingReboundSpeed = 0;

    //[SerializeField] private float kickWallTime;
    //[SerializeField] private float currentKickWallTime;
    private bool isSwap;

    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject[] weapons;

    private Vector3 climbUpPos = Vector3.zero;
    public Vector2 moveInput = Vector2.zero;
    private Rigidbody rigid = null;
    private Vector3 moveDirection = Vector3.zero;

    private Vector3 dashDirection = Vector3.zero;

    [SerializeField] private Vector3 handOriginPos = Vector3.zero;
    private Quaternion handOriginRot = Quaternion.identity;
    private Vector3 result = Vector3.zero;
    private Vector3 originBodyColliderCenter;
    private float originBodyColliderHeight;

    private float headOriginY;
    private Vector3 originAimPos;
    private Quaternion handFireRot;
    //private float pickupTime = 0.5f;
    //private float currentPickupTime;
    //private bool isPickup;
    [SerializeField] private bool isFever;
    [SerializeField] private float feverTime;
    [SerializeField] private float currentFeverTime;

    private bool isInit = false;
    [SerializeField] private float canJumpTime = 0.1f;
    [SerializeField] private float currentCanJumpTime = 0.0f;
    private bool isGod;
    [SerializeField] private float accelation = 1;
    //private List<GameObject> collisionWeapon = new List<GameObject>();

    public void SetIsGrounded(bool value) { isGrounded = value; }
    public GameObject GetWeaponGameObject() { return weapon_gameObject; }
    public GameObject GetTempWeaponGameObject() { return tempWeapon; }
    public Gun GetGun() { return gun; }

    public void SetIsJumpByObject(bool value, float power) { isJumpByObject = value; currentJumpPower = power; }
    public float GetMaxHp() { return maxHP; }
    public float GetCurrentHp() { return currentHP; }
    public Transform GetCamPos() { return camPos; }
    public bool GetIsCrouch() { return isCrouch; }
    public void SetCanJump(bool value) { canJump = value; }
    public float GetWalkSpeed() { return walkSpeed; }
    public float GetWalkSpeed_Min() { return walkSpeed_min; }
    public float GetWalkSpeed_Max() { return walkSpeed_max; }
    public bool GetIsDead() { return isDead; }

    public bool GetIsGrounded() { return isGrounded; }

    public bool GetIsDash() { return isDash; }

    public bool GetIsClimbUp() { return isClimbUp; }


    public Transform GetAimPos() { return aimPos; }
    public void SetIsSwap(bool value) { isSwap = value; }
    public Transform GetHand() { return hand; }
    public Inventory GetInventory() { return inventory; }
    public int GetCurrentDashCount() { return currentDashCount; }
    public bool GetIsSwap() { return isSwap; }
    public FPPCamController GetCam() { return mainCam; }

    // Start is called before the first frame update
    public void Init()
    {
        rigid = this.GetComponent<Rigidbody>();


        mainCam = Camera.main.transform.GetComponent<FPPCamController>();
        if (hand == null)
            hand = mainCam.transform.Find("HandPos");

        currentHP = maxHP;
        handOriginPos = hand.localPosition;
        handOriginRot = hand.localRotation;

        currentDashPower = dashPower;
        currentClimbUpTime = climbUpTime;
        currentClimbuUpPower = climbUpPower;
        originBodyColliderCenter = bodyCollider.center;
        originBodyColliderHeight = bodyCollider.height;

        originAimPos = aimPos.localPosition;
        projectileController = this.GetComponent<ThrowProjectile>();
        currentDashCount = dashCount;
        currentCanJumpTime = canJumpTime;

        headOriginY = camPos.localPosition.y;

        inventory = this.GetComponent<Inventory>();
        inventory.Init();

        isInit = true;
        //this.transform.position = new Vector3(0, 0, -30);
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
            walkSpeed = Mathf.Lerp(walkSpeed, walkSpeed_min, Time.deltaTime * 2);
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

        if (isDead || GameManager.Instance.GetIsPause())
        {
            if (isDead)
                rigid.velocity = Vector3.zero;

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

        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            isGod = true;
        }

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

            if (!isJump && !isClimbUp)
                this.transform.position = new Vector3(this.transform.position.x, hit.point.y + 0.04f, this.transform.position.z);

            useGravity = false;

            Vector3 slopeResult = Vector3.Cross(hit.normal, Vector3.Cross(rigid.velocity.normalized, hit.normal));
            Vector3 temp = Vector3.Cross(Vector3.Cross(hit.normal, moveDirection), hit.normal);

            RaycastHit hit2;
            if (Physics.Raycast(checkingGroundRayPos.position, Vector3.down, out hit2, 0.5f, 1 << LayerMask.NameToLayer("Enviroment")))
                result = temp;
            else
                result = moveDirection;

            if (Input.GetKey(KeyCode.LeftControl))
            {


                if (!isClimbUp && !isJump && !isJumpByObject)
                {
                    isCrouch = true;
                    camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY / 1.5f, 0), Time.deltaTime * 8);
                    bodyCollider.center = new Vector3(0, 0.4691301f, 0);
                    bodyCollider.height = 0.9497328f;
                }

            }

            if (isCrouch)
            {
                camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(camPos.localPosition.x, headOriginY / 2, camPos.localPosition.z), Time.deltaTime * 8);
            }
            else
            {
                camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(camPos.localPosition.x, headOriginY, camPos.localPosition.z), Time.deltaTime * 8);
            }

            if (!isGrounded && rigid.velocity.y < -1.0f)
            {
                if (!isLanding)
                    GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Land);

                isLanding = true;

                mainCam.GetComponent<Animator>().SetTrigger("Jump");
                hand.GetComponent<Animator>().SetTrigger("Landing");
            }

            isGrounded = true;
            currentCanJumpTime = canJumpTime;

            if (slopeResult.y < 0)
            {
                isSlope = true;
            }
            else
            {
                isSlope = false;
            }

            if (!isDash)
            {
                if (rigid.velocity.magnitude > walkSpeed)
                {
                    rigid.velocity = Vector3.Lerp(rigid.velocity, result.normalized * walkSpeed, Time.deltaTime * 8);
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
        else
        {

            if (!isDash)
                useGravity = true;

            //if (isGrounded && !isJump && !isJumpByObject)
            //{
            //    rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
            //}

            currentCanJumpTime -= Time.deltaTime;
            isGrounded = false;
            isLanding = false;

            if (!isClimbUp)
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

            if (!isClimbUp)
            {

                for (int i = 12; i >= 0; i--)
                {
                    //Debug.DrawRay(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.45f, forward * 0.35f);
                    if (!Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.45f, moveDirection, out wallHit, 0.38f, 1 << LayerMask.NameToLayer("Enviroment")))
                    {


                        if (moveDirection != Vector3.zero && Vector3.Dot(moveDirection, new Vector3(mainCam.transform.forward.x, moveDirection.y, mainCam.transform.forward.z)) >= 0.5f)
                        {
                            if (!Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * (i - 1) + Vector3.up * 0.45f), moveDirection, 0.38f, 1 << LayerMask.NameToLayer("Enviroment")))
                            {

                                continue;
                            }



                            if (Physics.BoxCast(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.45f, new Vector3(0.5f, 0.5f, 0.5f), Vector3.up, Quaternion.identity, 1.6f, 1 << LayerMask.NameToLayer("Enviroment")))
                            {

                                break;
                            }

                            if (!gun.GetIsReload())
                                mainCam.GetComponent<Animator>().SetTrigger("Parkour");
                            GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Parkour);

                            isClimbUp = true;
                            rigid.velocity = Vector3.zero;
                            climbUpPos = this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.45f + moveDirection * 0.38f;


                            break;
                        }
                    }

                }
            }

            if (!isJump && !isJumpByObject)
            {
                if (Physics.Raycast(this.transform.position + moveDirection * new Vector2(rigid.velocity.x, rigid.velocity.z).magnitude * Time.deltaTime, Vector3.down, out hit, 0.4f, 1 << LayerMask.NameToLayer("Enviroment")))
                {
                    Vector3 result = Vector3.Cross(hit.normal, Vector3.Cross(moveDirection.normalized, hit.normal));
                    Vector3 slopeResult = Vector3.Cross(hit.normal, Vector3.Cross(rigid.velocity.normalized, hit.normal));


                    if (rigid.velocity.y >= 0)
                        rigid.velocity = (result.normalized * rigid.velocity.magnitude);

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isClimbUp && moveDirection != Vector3.zero && currentDashCount > 0 && !isDash)
        {
            GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Dash);

            isDash = true;
            currentDashCount--;

            if (moveInput.x > 0)
            {
                mainCam.GetComponent<Animator>().SetTrigger("Dash_Right");
            }
            else if (moveInput.x < 0)
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

            accelation += Time.deltaTime / 1.55f;
            accelation = Mathf.Clamp(accelation, 0, 2);
            if (!isJump && !isJumpByObject)
                rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(rigid.velocity.x, rigid.velocity.y + Physics.gravity.y * accelation, rigid.velocity.z), Time.deltaTime);
        }
        else
        {
            accelation = 1.5f;
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

        if (Input.GetMouseButton(0) && !isSwap && !inventory.isOpen)
        {
            isPressMouseButton = true;
            if (gun != null)
            {
                if (gun.GetGunType() == GunType.ChainLightning)
                {
                    //handFireRot = gun.GetHandFireRot();
                    gun.GetComponent<Gun_ChainLightning>().Charging();
                }
                else
                {
                    if (gun.Fire())
                    {
                        handFireRot = gun.GetHandFireRot();
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
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gun != null)
            {
                if (gun.CanReload() && gun.GetGunType() != GunType.Flamethrower && !isSwap)
                {
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


        if (moveDirection != Vector3.zero)
        {
            hand.GetComponent<Animator>().SetBool("isMove", true);
        }
        else
            hand.GetComponent<Animator>().SetBool("isMove", false);

        if (!isCrouch)
            hand.GetComponent<Animator>().SetFloat("walkSpeed", walkSpeed / walkSpeed_min);
        else
            hand.GetComponent<Animator>().SetFloat("walkSpeed", walkSpeed * 0.85f / walkSpeed_min);

        hand.GetComponent<Animator>().SetBool("isGrounded", isGrounded);


        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouch = false;
            bodyCollider.center = originBodyColliderCenter;
            bodyCollider.height = originBodyColliderHeight;
            //mainCam.SetOriginFov(mainCam.GetRealOriginFov());
            //mainCam.FovReset();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isDash && canJump)
        {
            mainCam.GetComponent<Animator>().SetTrigger("Jump");

            mainCam.SetOriginFov(mainCam.GetRealOriginFov());
            mainCam.FovReset();

            if (!isJump)
            {
                if (isGrounded)
                {
                    canJump = true;
                    GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Jump);
                }
                else
                {
                    if (currentCanJumpTime > 0)
                    {
                        canJump = true;
                        GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Jump);
                    }
                    else
                    {
                        accelation = 1.5f;
                        canJump = false;
                        GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.DoubleJump);
                    }

                }
            }
            else
            {
                accelation = 1.5f;
                canJump = false;
                GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.DoubleJump);
            }

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

                currentJumpPower = jumpPower;
            }
        }

        if (isClimbUp)
        {
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



        if (isJump || isJumpByObject)
        {
            if (Physics.Raycast(mainCam.transform.position, Vector3.up, 0.5f, 1 << LayerMask.NameToLayer("Enviroment")))
            {
                isJump = false;
                isJumpByObject = false;
            }

            rigid.velocity = new Vector3(rigid.velocity.x, currentJumpPower, rigid.velocity.z);
            currentJumpPower += Time.deltaTime * Physics.gravity.y * accelation;
        }
        else
        {
            currentJumpPower = 0;
        }


        HandAnimation();

        mainCam.GetComponent<Animator>().SetFloat("horizontal", Mathf.Lerp(mainCam.GetComponent<Animator>().GetFloat("horizontal"), moveInput.x, Time.deltaTime * 10));
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 28.0f);


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

            if(gun.GetComponent<Gun_ChainLightning>() != null)
                gun.GetComponent<Gun_ChainLightning>().ResetValue();
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
                GameManager.Instance.GetCrosshairController().SetCrosshair(this.gun.GetGunType());
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

                    gun.SetIsAiming(false);


                    lastAngle_hand = Quaternion.Lerp(lastAngle_hand, Quaternion.Euler(hand.localRotation.eulerAngles + -handFireRot.eulerAngles), Time.deltaTime * 30);


                    if (gun.GetGunType() == GunType.ChainLightning)
                        lastPos_hand = Vector3.Lerp(lastPos_hand, hand.localPosition + mainCam.shakeVec / 2, Time.deltaTime * 20);
                    else
                    {
                        if (isGrounded)
                            lastPos_hand = Vector3.Lerp(lastPos_hand, hand.localPosition /*+ new Vector3(Mathf.Sin(headBobValue) / 200, Mathf.Abs(Mathf.Sin(headBobValue)) / 100f, 0)*/, Time.deltaTime * 25);
                        else
                            lastPos_hand = Vector3.Lerp(lastPos_hand, hand.localPosition, Time.deltaTime * 25);
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
        if (isGod || value == 0) return;

        isDamaged = true;

        Invoke("IsDamagedFalse", 0.8f);

        mainCam.Shake(0.1f, 0.8f, true);

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
        walkSpeed = walkSpeed_max;
    }

    //public void PickUpWeapon_Change()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, 3, 1 << LayerMask.NameToLayer("Weapon")))
    //    {
    //        inventory.ChangeWeapon(hit.transform.gameObject);
    //    }
    //}

    //public void PickUpWeapon()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, 3, 1 << LayerMask.NameToLayer("Weapon")))
    //    {
    //        inventory.AddWeapon(hit.transform.gameObject);
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (LayerMask.LayerToName(other.gameObject.layer).Equals("Weapon") && !collisionWeapon.Contains(other.gameObject))
    //        collisionWeapon.Add(other.gameObject);
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (LayerMask.LayerToName(other.gameObject.layer).Equals("Weapon") && collisionWeapon.Contains(other.gameObject))
    //        collisionWeapon.Remove(other.gameObject);
    //}

}
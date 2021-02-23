using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTest : MonoBehaviour
{
    [SerializeField] private Transform mainCam;
    [SerializeField] private Transform camPos;
    [SerializeField] private CapsuleCollider groundCollider;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float slidingCoolTime;
    [SerializeField] private float currentSlidingCoolTime;
    [SerializeField] private bool isSlide;
    [SerializeField] private bool isSlope;
    [SerializeField] private bool isRun;
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

    private Vector3 climbUpPos;
    private Vector2 moveInput;
    private Rigidbody rigid;
    private Vector3 moveDirection;
    private Vector3 slidingDirection;
    private float headBobValue;
    private float headOriginY;

    [SerializeField] private PhysicMaterial walk_defaultPm;
    [SerializeField] private PhysicMaterial sliding_groundPm;
    [SerializeField] private PhysicMaterial sliding_slopePm;

    public void SetIsGrounded(bool value) { isGrounded = value; }

    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        currentSlidingCoolTime = 0;
        headBobValue = 0;
        headOriginY = camPos.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
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
                if (rigid.velocity.magnitude > walkSpeed && !isSlide)
                {
                    if (Vector3.Dot(moveDirection, forward) > 0)
                    {
                        mainCam.GetComponent<FPPCamController>().FovMove(mainCam.GetComponent<FPPCamController>().GetOriginFov() + 10, 0.1f, 1000);

                        if (currentSlidingCoolTime <= 0 && !isCrouch)
                        {
                            isSlide = true;
                            rigid.AddForce(slopeResult.normalized * rigid.velocity.magnitude * 0.85f, ForceMode.VelocityChange);
                        }

                        slidingDirection = moveDirection;
                    }
                }

                if (!isSlide)
                    isCrouch = true;

            }

            if (Input.GetKey(KeyCode.LeftShift) && !isSlide)
            {
                isRun = true;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
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
                            rigid.velocity = Vector3.Lerp(rigid.velocity, result.normalized * walkSpeed, Time.deltaTime * 20);
                    }
                }
            }
        }
        else
        {
            groundCollider.enabled = false;

            if (isGrounded)
                rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);

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
                //Debug.DrawRay(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.5f, forward * 0.32f);
                if (Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.5f, forward, out wallHit, 0.32f, 1 << LayerMask.NameToLayer("Enviroment")))
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
                    if (!Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * (i - 1)) + Vector3.up * 0.5f, forward, 0.32f, 1 << LayerMask.NameToLayer("Enviroment")))
                    {
                        isCheckObject = true;
                    }

                    if (Physics.BoxCast(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.5f, new Vector3(0.5f, 0.5f, 0.5f), Vector3.up, Quaternion.identity, 1.6f, 1 << LayerMask.NameToLayer("Enviroment")))
                    {
                        isCheckObject = true;
                    }

                    for (int j = 0; j < 20; j++)
                    {
                        if (Physics.Raycast(this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.5f + (Vector3.up * 0.1f * j), forward, 0.32f, 1 << LayerMask.NameToLayer("Enviroment")))
                        {
                            isCheckObject = true;
                        }
                    }

                    if (!isCheckObject)
                    {
                        isClimbUp = true;
                        rigid.velocity = Vector3.zero;
                        climbUpPos = this.transform.position + (Vector3.up * 0.1f * i) + Vector3.up * 0.5f + forward * 0.45f;
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
                if (Physics.Raycast(this.transform.position + moveDirection * new Vector2(rigid.velocity.x, rigid.velocity.z).magnitude * Time.deltaTime, Vector3.down, out hit, 0.5f, 1 << LayerMask.NameToLayer("Enviroment")))
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

        if (Input.GetKeyUp(KeyCode.LeftShift) || moveDirection == Vector3.zero)
        {
            isRun = false;
        }

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
            rigid.velocity = new Vector3(rigid.velocity.x, currentJumpPower, rigid.velocity.z);
            currentJumpPower += Time.deltaTime * Physics.gravity.y;
        }

        if (isSlide)
        {
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
                        camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY / 2 + Mathf.Abs(Mathf.Sin(headBobValue)) / 30, 0), Time.deltaTime * 8);
                    else
                        camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY + Mathf.Abs(Mathf.Sin(headBobValue)) / 30, 0), Time.deltaTime * 8);
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
                            camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(Mathf.Sin(headBobValue) / 30, headOriginY + Mathf.Abs(Mathf.Sin(headBobValue)) / 3.5f, 0), Time.deltaTime * 8);
                        else
                            camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(Mathf.Sin(headBobValue) / 50, headOriginY + Mathf.Abs(Mathf.Sin(headBobValue)) / 6, 0), Time.deltaTime * 8);
                    }
                }
            }
            else
            {
                if (!isCrouch && !isSlide)
                {
                    camPos.localPosition = Vector3.Lerp(camPos.localPosition, new Vector3(0, headOriginY, 0), Time.deltaTime * 8);
                    headBobValue = 0;
                }
            }

            isSlope = false;
        }

        this.transform.rotation = Quaternion.LookRotation(forward);
    }
}

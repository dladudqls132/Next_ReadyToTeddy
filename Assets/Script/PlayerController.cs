using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform mainCam;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float gravity;

    [SerializeField] private bool isSprint;
    [SerializeField] private bool isCrouch;
    [SerializeField] private bool isSliding;
    [SerializeField] private bool isSlide;

    private Vector2 moveInput;
    private Vector3 moveDirection;
    private Vector3 destSpeedToDirection;
    private Vector3 tempDirection;
    [SerializeField] private Vector3 currentSpeedToDirection;
    private Vector2 anim_destMoveValue;
    private Vector2 anim_currentMoveValue;
    private Vector3 slidingDirection;

    private Rigidbody rigid;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        anim = this.GetComponent<Animator>();
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

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (currentSpeedToDirection.magnitude > sprintSpeed - 1.0f)
            {
                if (!isSlide)
                {
                    currentSpeedToDirection = currentSpeedToDirection * 1.5f;
                    isSliding = true;
                    isSlide = true;
                }
            }
        }
        else
        {
            if (isSliding)
            {
                isSliding = false;
            }
        }

        anim.SetBool("slide_start", isSliding);
        //움직일때
        if (moveInput != Vector2.zero)
        {
            moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

            if(!isSlide)
            {
                if (Input.GetKey(KeyCode.LeftShift) && moveInput.y >= 0)
                {
                    isSprint = true;

                    destSpeedToDirection = moveDirection * sprintSpeed;
                    anim_destMoveValue = new Vector2(0, 1);
                }
                else
                {
                    isSprint = false;

                    destSpeedToDirection = moveDirection * walkSpeed;
                    anim_destMoveValue = new Vector2(moveInput.x / 2, moveInput.y / 2);
                }

                if (Vector3.Dot(this.transform.forward, forward) > 0)
                {
                    currentSpeedToDirection = Vector3.MoveTowards(currentSpeedToDirection, destSpeedToDirection, Time.deltaTime * 50);
                }
                else
                {
                    currentSpeedToDirection = Vector3.MoveTowards(currentSpeedToDirection, destSpeedToDirection, Time.deltaTime * 10);
                }
            }

            anim_currentMoveValue = Vector2.MoveTowards(anim_currentMoveValue, anim_destMoveValue, Time.deltaTime * 6.0f);
        }
        else //안움직일때
        {
            if (!isSlide)
            {
                isSprint = false;

                destSpeedToDirection = Vector3.zero;
                anim_destMoveValue = Vector2.zero;

                currentSpeedToDirection = Vector3.MoveTowards(currentSpeedToDirection, destSpeedToDirection, Time.deltaTime * 30);
            }

            anim_currentMoveValue = Vector2.MoveTowards(anim_currentMoveValue, anim_destMoveValue, Time.deltaTime * 5);
        }

        if (isSlide)
        {
            destSpeedToDirection = moveDirection + forward;
            anim_destMoveValue = new Vector2(0, 0.5f);
            
            if (currentSpeedToDirection.magnitude > 1.0f)
                currentSpeedToDirection = Vector3.MoveTowards(currentSpeedToDirection, destSpeedToDirection.normalized, Time.deltaTime * 4);
            else
            {

                isSliding = false;
            }

            isSprint = false;
        }

        anim.SetFloat("horizontal", anim_currentMoveValue.x);
        anim.SetFloat("vertical", anim_currentMoveValue.y);

        if (!isSlide)
        {
            if (!isSprint)
            {
                if (moveInput != Vector2.zero)
                {
                    if(moveInput.y <= 0)
                    tempDirection = forward;
                    else
                        tempDirection = destSpeedToDirection;
                }
                else
                {
                    if(Vector3.Dot(this.transform.forward, forward) < 0)
                    {
                        tempDirection = forward;
                    }

                }

         
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(tempDirection), Time.deltaTime * 8);
            }
            else
            {
                if (destSpeedToDirection != Vector3.zero)
                {
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(destSpeedToDirection), Time.deltaTime * 10);
                }
            }
        }
        else
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(forward), Time.deltaTime * 8);
        }
    }

    private void FixedUpdate()
    {
        CalculateVelocity();
        //rigid.velocity = new Vector3(currentSpeedToDirection.x, currentSpeedToDirection.y + gravity, currentSpeedToDirection.z);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetLookAtWeight(1, 0, 0.5f, 0.5f, 0.7f);
        anim.SetLookAtPosition(mainCam.position + mainCam.forward);
    }

    void CalculateVelocity()
    {
        RaycastHit hit;

        if (Physics.Raycast(rigid.position, Vector3.down, out hit, 0.2f, 1 << LayerMask.NameToLayer("Ground")))
        {
            Vector3 result = Vector3.Cross(hit.normal, Vector3.Cross(currentSpeedToDirection.normalized, hit.normal));
            result = result * currentSpeedToDirection.magnitude;

            gravity = 0;

            rigid.velocity = new Vector3(result.x, result.y + gravity, result.z);
        }
        else
        {
            gravity += Physics.gravity.y * Time.deltaTime;

            rigid.velocity = new Vector3(currentSpeedToDirection.x, currentSpeedToDirection.y + gravity, currentSpeedToDirection.z); 
        }

    }

    void SlidingFalse()
    {
        Vector3 forward = mainCam.forward;
        Vector3 right = mainCam.right;

        forward.y = 0; right.y = 0;

        forward.Normalize();
        right.Normalize();

        isSlide = false;
        //currentSpeedToDirection = forward;
        destSpeedToDirection = forward;
        tempDirection = forward;
    }
}

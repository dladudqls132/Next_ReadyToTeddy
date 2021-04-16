using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    Camera mainCam;
    Vector2 moveInput;
    Vector3 moveDirection;
    Rigidbody rigid;
    RaycastHit hit;
    Vector3 gravity;
    [SerializeField] bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        rigid = this.GetComponent<Rigidbody>();
        gravity = Physics.gravity;
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    Vector3 forward = mainCam.transform.forward;
    //    Vector3 right = mainCam.transform.right;

    //    forward.y = 0; right.y = 0;

    //    forward.Normalize();
    //    right.Normalize();

    //    moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    //    moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

    //    if (isGrounded)
    //    {
    //        Vector3 result = Vector3.Cross(hit.normal, Vector3.Cross(moveDirection.normalized, hit.normal));
    //        rigid.velocity = result * 10;
    //    }
    //    else
    //    {
    //        rigid.velocity += gravity * Time.deltaTime;
    //    }
    //}

    private void Update()
    {
        Vector3 forward = mainCam.transform.forward;
        Vector3 right = mainCam.transform.right;

        forward.y = 0; right.y = 0;

        forward.Normalize();
        right.Normalize();

        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;


        Vector3 result = Vector3.Cross(hit.normal, Vector3.Cross(moveDirection.normalized, hit.normal));


        if (Physics.Raycast(this.transform.position + result * 10 * Time.deltaTime + Vector3.up * 0.1f, Vector3.down, out hit, 0.4f, 1 << LayerMask.NameToLayer("Enviroment")))
        {
            //this.transform.position = hit.point + Vector3.up * 0.1f;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded)
        {


            //rigid.velocity = result * 10;
            RaycastHit hit2;
            if (Physics.CapsuleCast(this.transform.position, this.transform.position + Vector3.up * 0.5f, 0.25f, result, out hit2, (result * 10 * Time.deltaTime).magnitude, 1 << LayerMask.NameToLayer("Enviroment")))
            {

            }
            else
            {
                this.transform.position += result * 10 * Time.deltaTime;
                this.transform.position = new Vector3(this.transform.position.x, hit.point.y, this.transform.position.z);
            }
        }
        else
        {
            //rigid.velocity += gravity * Time.deltaTime;
            this.transform.position += gravity * Time.deltaTime;
        }

        //if (Physics.Raycast(this.transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 0.4f, 1 << LayerMask.NameToLayer("Enviroment")))
        //{
        //    this.transform.position = hit.point + Vector3.up * 0.1f;
        //    isGrounded = true;
        //}
        //else
        //{
        //    isGrounded = false;
        //}
    }
}

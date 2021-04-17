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
    [SerializeField] private Transform rayPoint;

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
    private void FixedUpdate()
    {
        //if (Physics.SphereCast(rayPoint.position, 0.24f, Vector3.down, out hit, 0.5f, 1 << LayerMask.NameToLayer("Enviroment")))
        //{
        //    isGrounded = true;
        //}
        //else
        //    isGrounded = false;

        //if (isGrounded)
        //{
        //    Vector3 result = Vector3.Cross(hit.normal, Vector3.Cross(moveDirection.normalized, hit.normal));
        //    this.transform.position += result * 10 * Time.deltaTime;
        //    this.transform.position = new Vector3(this.transform.position.x, hit.point.y + 0.1f, this.transform.position.z);
        //}
        //else
        //{
        //    this.transform.position += gravity * Time.deltaTime;
        //}
    }
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
        //this.transform.position += result * 5 * Time.deltaTime;

        if (Physics.SphereCast(rayPoint.position, 0.25f, Vector3.down, out hit, 0.3f, 1 << LayerMask.NameToLayer("Enviroment")))
        {
            if (Vector3.Dot(Vector3.down, hit.normal) < -0.65f)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }

            //Debug.Log(Vector3.Dot(Vector3.down, hit.normal));
        }
        else
            isGrounded = false;

        if (isGrounded)
        {
            this.transform.position = new Vector3(this.transform.position.x, hit.point.y + 0.05f, this.transform.position.z);
            rigid.velocity = result * 10.0f;
        }
        else
        {
            rigid.velocity += gravity * Time.deltaTime;
        }

    }

    private void OnDrawGizmos()
    {
        float maxDistance = 0.2f;
        RaycastHit hit;
        // Physics.SphereCast (레이저를 발사할 위치, 구의 반경, 발사 방향, 충돌 결과, 최대 거리)
        bool isHit = Physics.SphereCast(rayPoint.position, 0.25f, Vector3.down, out hit, maxDistance);

        Gizmos.color = Color.red;
        if (isHit)
        {
            Gizmos.DrawRay(rayPoint.position, Vector3.down * hit.distance);
            Gizmos.DrawWireSphere(rayPoint.position + Vector3.down * hit.distance, 0.25f);
        }
        else
        {
            Gizmos.DrawRay(rayPoint.position, Vector3.down * maxDistance);
        }
    }
}

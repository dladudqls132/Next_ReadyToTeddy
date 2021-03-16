using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPCamController : MonoBehaviour
{
    [SerializeField] private float cameraMoveSpeed = 120.0f;
    [SerializeField] Transform cameraFollow;
    private Camera mainCamera;
    
    private float clampAngle = 72.0f;
    private float inputSensitivity = 150.0f;
    private float mouseX;
    private float mouseY;
    private float rotY = 0.0f;
    private float rotX = 0.0f;

    private bool isFovMove;
    private float timeToDest;
    private float timeToOrigin;
    private float fovTimer;
    private float originFov;
    private float destFov;
    private float fovStopTime;

    public float GetOriginFov() { return originFov; }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCamera = Camera.main;
        originFov = Camera.main.fieldOfView;
    }

    private void Update()
    {
        if (isFovMove)
        {
            if (Mathf.Abs(Camera.main.fieldOfView - destFov) <= 0.1f)
            {
                fovTimer = 0;
                mainCamera.fieldOfView = destFov;

                fovStopTime -= Time.deltaTime;

                if (fovStopTime <= 0)
                {
                    isFovMove = false;
                    fovStopTime = 0;
                }
            }
            else
            {
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, destFov, Time.deltaTime / timeToDest);
                fovTimer += Time.deltaTime / timeToDest;
            }
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, originFov, Time.deltaTime * 10);

            if (Mathf.Abs(mainCamera.fieldOfView - originFov) <= 0.1f)
            {
                fovTimer = 0;
                mainCamera.fieldOfView = originFov;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotY += mouseX * cameraMoveSpeed * Time.fixedDeltaTime;
        rotX += mouseY * cameraMoveSpeed * Time.fixedDeltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        this.transform.position = cameraFollow.position;
    }

    public void FovMove(float destination, float timeToDest, float stopTime)
    {
        isFovMove = true;
        destFov = destination;
        this.timeToDest = timeToDest;
        fovStopTime = stopTime;

        fovTimer = 0;
    }

    public void FovMove(float destination, float timeToDest, float timeToOrigin, float stopTime)
    {
        isFovMove = true;
        destFov = destination;
        this.timeToDest = timeToDest;
        this.timeToOrigin = timeToOrigin;
        fovStopTime = stopTime;

        fovTimer = 0;
    }

    public void FovReset()
    {
        isFovMove = false;

        destFov = 0;
        this.timeToDest = 0;
        this.timeToOrigin = 0;
        fovStopTime = 0;
        fovTimer = 0;
    }
}

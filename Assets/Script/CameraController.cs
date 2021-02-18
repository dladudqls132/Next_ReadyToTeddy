using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float CameraMoveSpeed = 120.0f;
    [SerializeField] Transform CameraFollow;
    Camera mainCamera;

    private float clampAngle = 70.0f;
    private float inputSensitivity = 150.0f;
    private float mouseX;
    private float mouseY;
    private float rotY = 0.0f;
    private float rotX = 0.0f;

    private bool shake;
    private float timer;
    private float shakeDuration;
    private float shakeAmount;
    private Vector3 originPos;

    private bool isFovMove;
    private float timeToDest;
    private float timeToOrigin;
    private float fovTimer;
    private float originFov;
    private float destFov;
    private float fovStopTime;

    public bool GetIsShake() { return shake; }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainCamera = transform.GetChild(0).GetComponent<Camera>();

        originFov = Camera.main.fieldOfView;
    }

    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotY += mouseX * CameraMoveSpeed * Time.deltaTime;
        rotX += mouseY * CameraMoveSpeed * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

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
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, destFov, fovTimer);
                fovTimer += Time.deltaTime / timeToDest;
            }
        }
        else
        {
            fovTimer += Time.deltaTime / timeToOrigin;

            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, originFov, fovTimer);

            if (Mathf.Abs(mainCamera.fieldOfView - originFov) <= 0.1f)
            {
                fovTimer = 0;
                mainCamera.fieldOfView = originFov;
            }
        }
    }

    private void LateUpdate()
    {
        if (CameraFollow != null)
        {
            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            transform.rotation = localRotation;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

            if (shake)
            {
                timer += Time.deltaTime;

                originPos = CameraFollow.position;
                transform.localPosition = (Vector3)Random.insideUnitCircle * shakeAmount + originPos;

                if (timer >= shakeDuration)
                {
                    shake = false;
                    timer = 0;
                }
            }
            else
                this.transform.position = CameraFollow.position;
        }
    }

    public void Shake(float _amount, float _duration)
    {
        shake = true;
        shakeDuration = _duration;
        shakeAmount = _amount;
        this.originPos = this.transform.position;
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
        mainCamera.fieldOfView = originFov;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPCamController : MonoBehaviour
{
    [SerializeField] private float cameraMoveSpeed = 120.0f;
    [SerializeField] Transform cameraFollow = null;
    private Camera mainCamera;

    private float clampAngle = 72.0f;
    private float mouseX;
    private float mouseY;
    [SerializeField] private float rotY = 0.0f;
    [SerializeField] private float rotX = 0.0f;
    private float tempRotX;
    private float tempRotY;

    [SerializeField] private bool isFovMove;
    private float timeToDest;
    private float timeToOrigin;
    private float fovTimer;
    private float originFov;
    private float destFov;
    private float fovStopTime;
    private float realOriginFov;
    [SerializeField] private bool isShake;
    [SerializeField] private float shakeTime;
    private float currentShakeTime;
    [SerializeField] private float shakeAmount;
    //[SerializeField] private Transform enemyHp;
    //[SerializeField] private Vector3 enemyHp_pos;
    [SerializeField] private bool isRecoil;
    [SerializeField] private bool isFire;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float returnSpeed;
    private float currentReturnSpeed;
    private Vector3 tempDir;

    [Header("Hipfire: ")]
    [SerializeField] private Vector3 recoilRotation = new Vector3(2f, 2f, 2f);

    [Header("Aiming: ")]
    [SerializeField] private Vector3 recoilRotationAiming = new Vector3(0.5f, 0.5f, 1.0f);

    public bool isAiming;

    [SerializeField] private Vector3 currentRotation;
    [SerializeField] private Vector3 rot;

    public float GetOriginFov() { return originFov; }
    public float GetRealOriginFov() { return realOriginFov; }
    public void SetOriginFov(float value) { originFov = value; }

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
        realOriginFov = originFov;
        cameraFollow = GameManager.Instance.GetPlayer().GetCamPos();

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
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, originFov, Time.deltaTime * 13);

            if (Mathf.Abs(mainCamera.fieldOfView - originFov) <= 0.1f)
            {
                fovTimer = 0;
                mainCamera.fieldOfView = originFov;
            }
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            cameraMoveSpeed += 5;
        }
        else if (Input.GetKeyDown(KeyCode.PageDown))
        {
            cameraMoveSpeed -= 5;
        }

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Enemy"))))
        //{
        //    enemyHp.gameObject.SetActive(true);
        //    enemyHp.position = hit.transform.position + Vector3.up * 3;
        //    enemyHp.rotation = this.transform.rotation;
        //}
        //else
        //{
        //    enemyHp.gameObject.SetActive(false);
        //}
    }
    // return : -180 ~ 180 degree (for unity)
    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }
    private void LateUpdate()
    {
        if (GameManager.Instance.GetPlayer().GetWeapon().GetIsReload())
        {
            //Debug.Log(this.GetComponent<Animator>().rootRotation.eulerAngles);
            this.GetComponent<Animator>().SetBool("isReload", true);
            transform.localRotation = Quaternion.Euler(rot + new Vector3(rotX, rotY) + this.GetComponent<Animator>().rootRotation.eulerAngles);
        }
        else
        {
            this.GetComponent<Animator>().SetBool("isReload", false);
            transform.localRotation = Quaternion.Euler(rot + new Vector3(rotX, rotY));
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


        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, (returnSpeed + currentReturnSpeed) * Time.deltaTime);
        rot = Vector3.Slerp(rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);



        currentReturnSpeed += Time.deltaTime;



        if (isShake)

        {

            transform.position = Random.insideUnitSphere * shakeAmount + cameraFollow.position;

            currentShakeTime -= Time.deltaTime;

            if (currentShakeTime <= 0)
            {
                isShake = false;
            }

        }

        else

        {
            this.transform.position = cameraFollow.position;

            //canvas.renderMode = RenderMode.ScreenSpaceCamera;

        }


    }

    public Vector3 SetFireRecoilRot(Vector3 rot, float rotSpeed, float returnSpeed)
    {
        currentReturnSpeed = 0;
        this.returnSpeed = returnSpeed;
        this.rotationSpeed = rotSpeed;

        Vector3 rnd = new Vector3(-rot.x, Random.Range(-rot.y, rot.y), Random.Range(-rot.z, rot.z));

        currentRotation += rnd;
        return rnd;
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

    public void Shake(float shakeTime, float shakeAmount)
    {
        this.isShake = true;
        this.shakeTime = shakeTime;
        this.shakeAmount = shakeAmount;

        currentShakeTime = shakeTime;
    }
}

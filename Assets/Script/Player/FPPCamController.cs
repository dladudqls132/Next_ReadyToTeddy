using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPCamController : MonoBehaviour
{
    private static FPPCamController instance;
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
    [SerializeField] private float aimingMoveRndTime;
    private float currentAimingMoveRndTime;
    private float currentReturnSpeed;
    
    private Vector2 tempDir;
    private Vector2 currentTempDir;
    private Vector2 smoothTemp;

    public Vector3 shakeVec;
    private bool overWrite;

    [Header("Hipfire: ")]
    [SerializeField] private Vector3 recoilRotation = new Vector3(2f, 2f, 2f);

    [Header("Aiming: ")]
    [SerializeField] private Vector3 recoilRotationAiming = new Vector3(0.5f, 0.5f, 1.0f);

    private bool isAiming;

    [SerializeField] private Vector3 currentRotation;
    [SerializeField] private Vector3 rot;

    public float GetCurrentFov() { return Camera.main.fieldOfView; }
    public float GetOriginFov() { return originFov; }
    public float GetRealOriginFov() { return realOriginFov; }
    public void SetOriginFov(float value) { originFov = value; }
    public void SetCameraMoveSpeed(float value) { cameraMoveSpeed = value; }

  

    // Start is called before the first frame update
    //private void Awake()
    //{
    //    DontDestroyOnLoad(this);

    //    if(instance == null)
    //    {
    //        instance = this;
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

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
            if(!GameManager.Instance.GetIsPause())
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, originFov, Time.deltaTime / timeToOrigin);

            if (Mathf.Abs(mainCamera.fieldOfView - originFov) <= 0.1f)
            {
                fovTimer = 0;
                mainCamera.fieldOfView = originFov;
            }
        }

        ////////////////////////////////////////////////////
        isAiming = GameManager.Instance.GetPlayer().GetIsAiming();
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        if (!GameManager.Instance.GetPlayer().GetInventory().isOpen && !GameManager.Instance.GetIsPause())
        {
            rotY += mouseX * cameraMoveSpeed * Time.fixedDeltaTime;
            rotX += mouseY * cameraMoveSpeed * Time.fixedDeltaTime;
        }
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);


        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, (returnSpeed + currentReturnSpeed) * Time.deltaTime);
        rot = Vector3.Slerp(rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);

        currentReturnSpeed += Time.deltaTime;
        Vector3 temp = this.transform.right * currentTempDir.x + this.transform.up * (currentTempDir.y / 2);
        if (isShake)

        {
            shakeVec = Random.insideUnitSphere * shakeAmount;
            transform.position = shakeVec + cameraFollow.position + temp;

            currentShakeTime -= Time.deltaTime;

            if (currentShakeTime <= 0)
            {
                isShake = false;
            }

        }

        else

        {
            shakeVec = Vector3.Lerp(shakeVec, Vector3.zero, Time.deltaTime * 15);
            if (!isAiming)
            {
                this.transform.position = cameraFollow.position + temp;
                currentAimingMoveRndTime = 0;
                tempDir = Vector2.zero;
                currentTempDir = Vector2.Lerp(currentTempDir, tempDir, Time.deltaTime * 15f);
            }
            else
            {
                if (!Input.GetMouseButton(0) && GameManager.Instance.GetPlayer().GetIsGrounded())
                {
                    this.transform.position = cameraFollow.position + temp;

                    //currentTempDir = Vector2.Lerp(currentTempDir, tempDir, Time.deltaTime * 0.5f);
                    currentTempDir = Vector2.SmoothDamp(currentTempDir, tempDir, ref smoothTemp, 1);
                    currentAimingMoveRndTime += Time.deltaTime;
                    if (currentAimingMoveRndTime > aimingMoveRndTime)
                    {
                        currentAimingMoveRndTime = 0;
                        tempDir = Random.insideUnitCircle * 0.1f;

                    }
                }
                else
                {
                    currentAimingMoveRndTime = 0;
                    tempDir = currentTempDir;

                    this.transform.position = cameraFollow.position + temp;
                }
            }

            //canvas.renderMode = RenderMode.ScreenSpaceCamera;

        }


        //if (Input.GetKeyDown(KeyCode.PageUp))
        //{
        //    cameraMoveSpeed += 5;
        //}
        //else if (Input.GetKeyDown(KeyCode.PageDown))
        //{
        //    cameraMoveSpeed -= 5;
        //}

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
        if (GameManager.Instance.GetPlayer().GetGun() != null)
        {
            if (GameManager.Instance.GetPlayer().GetGun().GetIsReload() && (GameManager.Instance.GetPlayer().GetGun().GetGunType() != GunType.Flamethrower))
            {
                //Debug.Log(this.GetComponent<Animator>().rootRotation.eulerAngles);
                if (GameManager.Instance.GetPlayer().GetGun().GetGunType() == GunType.ChainLightning)
                {
                    if (!this.GetComponent<Animator>().GetBool("isReload_CL"))
                        this.GetComponent<Animator>().SetBool("isReload_CL", true);
                }
                else
                {
                    if (!this.GetComponent<Animator>().GetBool("isReload"))
                        this.GetComponent<Animator>().SetBool("isReload", true);
                }

                if(transform.localRotation.eulerAngles.y >= 200 || transform.localRotation.eulerAngles.y <= 168)
                    transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + rot + new Vector3(rotX, rotY) + new Vector3(transform.localRotation.eulerAngles.x, 0, transform.localRotation.eulerAngles.z));
                else
                    transform.localRotation = Quaternion.Euler(rot + new Vector3(rotX, rotY) + new Vector3(transform.localRotation.eulerAngles.x, 0, transform.localRotation.eulerAngles.z));
            }
            else
            {
                if (this.GetComponent<Animator>().GetBool("isReload"))
                    this.GetComponent<Animator>().SetBool("isReload", false);
                if (this.GetComponent<Animator>().GetBool("isReload_CL"))
                    this.GetComponent<Animator>().SetBool("isReload_CL", false);

                transform.localRotation = Quaternion.Euler(rot + new Vector3(rotX, rotY) + new Vector3(transform.localRotation.eulerAngles.x, 0, transform.localRotation.eulerAngles.z));
            }
        }
        else
        {
            transform.localRotation = Quaternion.Euler(rot + new Vector3(rotX, rotY) + new Vector3(0, 0, transform.localRotation.eulerAngles.z));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //isAiming = GameManager.Instance.GetPlayer().GetIsAiming();
        //mouseX = Input.GetAxis("Mouse X");
        //mouseY = Input.GetAxis("Mouse Y");

        //if (!GameManager.Instance.GetPlayer().GetInventory().isOpen && !GameManager.Instance.GetIsPause())
        //{
        //    rotY += mouseX * cameraMoveSpeed * Time.fixedDeltaTime;
        //    rotX += mouseY * cameraMoveSpeed * Time.fixedDeltaTime;
        //}
        //rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);


        //currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, (returnSpeed + currentReturnSpeed) * Time.deltaTime);
        //rot = Vector3.Slerp(rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);

        //currentReturnSpeed += Time.deltaTime;
        //Vector3 temp = this.transform.right * currentTempDir.x + this.transform.up * (currentTempDir.y / 2);
        //if (isShake)

        //{
        //    shakeVec = Random.insideUnitSphere * shakeAmount;
        //    transform.position = shakeVec + cameraFollow.position + temp;

        //    currentShakeTime -= Time.deltaTime;

        //    if (currentShakeTime <= 0)
        //    {
        //        isShake = false;
        //    }

        //}

        //else

        //{
        //    shakeVec = Vector3.Lerp(shakeVec, Vector3.zero, Time.deltaTime * 15);
        //    if (!isAiming)
        //    {
        //        this.transform.position = cameraFollow.position + temp;
        //        currentAimingMoveRndTime = 0;
        //        tempDir = Vector2.zero;
        //        currentTempDir = Vector2.Lerp(currentTempDir, tempDir, Time.deltaTime * 15f);
        //    }
        //    else
        //    {
        //        if (!Input.GetMouseButton(0) && GameManager.Instance.GetPlayer().GetIsGrounded())
        //        {
        //            this.transform.position = cameraFollow.position + temp;

        //            //currentTempDir = Vector2.Lerp(currentTempDir, tempDir, Time.deltaTime * 0.5f);
        //            currentTempDir = Vector2.SmoothDamp(currentTempDir, tempDir, ref smoothTemp, 1);
        //       currentAimingMoveRndTime += Time.deltaTime;
        //            if (currentAimingMoveRndTime > aimingMoveRndTime)
        //            {
        //                currentAimingMoveRndTime = 0;
        //                tempDir = Random.insideUnitCircle * 0.1f;

        //            }
        //        }
        //        else
        //        {
        //            currentAimingMoveRndTime = 0;
        //            tempDir = currentTempDir;
                    
        //            this.transform.position = cameraFollow.position + temp;
        //        }
        //    }

        //    //canvas.renderMode = RenderMode.ScreenSpaceCamera;

        //}


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

        originFov = realOriginFov;
        destFov = 0;
        this.timeToDest = 0;
        this.timeToOrigin = 0.1f;
        fovStopTime = 0;
        fovTimer = 0;
    }

    public void Shake(float shakeTime, float shakeAmount, bool overWrite)
    {
        if (!overWrite && isShake) return;

        this.isShake = true;
        this.shakeTime = shakeTime;
        this.shakeAmount = shakeAmount;

        currentShakeTime = shakeTime;
    }
}

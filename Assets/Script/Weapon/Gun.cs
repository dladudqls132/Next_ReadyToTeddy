using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    AR,
    ShotGun
}

public class Gun : MonoBehaviour
{
    [SerializeField] protected GunType gunType;
    [SerializeField] protected Vector3 originPos;
    [SerializeField] protected Vector3 originRot;
    [SerializeField] protected Vector3 aimingPos;
    [SerializeField] protected Vector3 aimingRot;

    [SerializeField] protected GameObject item;
    [SerializeField] protected GameObject owner;
    [SerializeField] protected Transform hand;
    [SerializeField] protected Transform shotPos;
    [SerializeField] protected Sprite sprite;

    [SerializeField] protected int fireNum = 0;
    [SerializeField] protected bool isShot;
    [SerializeField] protected bool canShot;
    [SerializeField] protected bool isRecoil;
    [SerializeField] protected float shotDelay;
    [SerializeField] protected float currentShotDelay;
    [SerializeField] protected bool isReload;
    [SerializeField] protected float reloadTime = 1;
    protected float currentReloadTime;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected float damagePerBullet;
    
    [SerializeField] protected ParticleSystem muzzleFlash;
    protected FPPCamController mainCam;
    protected Vector3 handFireRot;
    [SerializeField] protected float spreadAngle_normal = 3;
    [SerializeField] protected float spreadAngle_aiming = 0.5f;
    [SerializeField] protected float currentSpreadAngle;

    [SerializeField] protected bool isDroped;
    [SerializeField] protected bool isPlayerEnter;

    //[SerializeField] private Transform meshRoot;
    [SerializeField] MeshRenderer[] mesh;

    protected Rigidbody rigid;

    public Vector3 GetOriginPos() { return originPos; }
    public Vector3 GetOriginRot() { return originRot; }
    public Sprite GetSprite() { return sprite; }
    public float GetShotDelay() { return shotDelay; }

    protected Vector3 direction;

    public void SetInfo(GunType gunType, float damagePerBullet, int maxAmmo, int fireNum, float spreadAngle_normal, float spreadAngle_aiming, float shotDelay)
    {
        this.gunType = gunType;
        this.damagePerBullet = damagePerBullet;
        this.maxAmmo = maxAmmo;
        this.currentAmmo = maxAmmo;
        this.fireNum = fireNum;
        this.spreadAngle_normal = spreadAngle_normal;
        this.spreadAngle_aiming = spreadAngle_aiming;
        this.shotDelay = shotDelay;
    }

    protected virtual void Start()
    {
        currentAmmo = maxAmmo;
        currentReloadTime = reloadTime;
        currentShotDelay = shotDelay;
        mainCam = Camera.main.transform.GetComponent<FPPCamController>();
        rigid = this.GetComponent<Rigidbody>();

        //MeshRenderer[] tempMesh = meshRoot.GetComponentsInChildren<MeshRenderer>();

        //foreach(MeshRenderer m in tempMesh)
        //{
        //    m.enabled = false;
        //}


        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].enabled = false;
        }
    }

    public void SetIsAiming(bool value)
    {
        if (value)
        {
            mainCam.FovMove(mainCam.GetOriginFov() - mainCam.GetOriginFov() / 4f, 0.07f, 1000);

            this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(aimingRot), Time.deltaTime * 25);
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, aimingPos, Time.deltaTime * 25);
        }
        else
        {
            mainCam.FovReset();

            this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(originRot), Time.deltaTime * 25);
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, originPos, Time.deltaTime * 25);
        }
    }

    public GunType GetGunType() { return gunType; }
    public int GetMaxAmmoCount() { return maxAmmo; }
    public int GetCurrentAmmoCount() { return currentAmmo; }
    public bool GetIsReload() { return isReload; }
    public bool GetIsShot() { return isShot; }
    public void SetIsShot(bool value) { isShot = value; } 
    public bool GetIsRecoil() { return isRecoil; }
    public void SetIsRecoil(bool value) { isRecoil = value; }

    public void SetOwner(GameObject who, Transform hand, Transform parent) 
    { 
        owner = who; 
        this.hand = hand;

        if (owner != null)
        {
            Collider[] c = this.GetComponents<Collider>();
            foreach(Collider temp in c)
            {
                temp.enabled = false;
            }
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            rigid.useGravity = false;

           // this.gameObject.SetActive(false);
            this.transform.SetParent(parent);

            this.transform.localPosition = originPos;
            this.transform.localRotation = Quaternion.Euler(originRot);

            for(int i = 0; i < mesh.Length; i++)
            {
                mesh[i].enabled = true;
            }

            item.SetActive(false);
        }
        else
        {
            //this.gameObject.SetActive(true);
            this.transform.SetParent(null);

            Collider[] c = this.GetComponents<Collider>();
            foreach (Collider temp in c)
            {
                temp.enabled = true;
            }
            rigid.useGravity = true;
            rigid.AddForce(mainCam.transform.forward * 5, ForceMode.Impulse);

            //this.transform.position = dropPos;
            //this.transform.rotation = dropRot;

            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i].enabled = false;
            }

            item.SetActive(true);
        }
    }
    public GameObject GetOwner() { return owner; }

    public float GetDamagePerBullet() { return damagePerBullet; }
    public void SetDamagePerBullet(float value) { damagePerBullet = value; }

    public virtual void SetIsReload(bool value)
    {
        mainCam.SetOriginFov(mainCam.GetRealOriginFov());
        mainCam.FovReset();
        isReload = value;
        hand.GetComponent<Animator>().SetFloat("Reload_Time", 1 / reloadTime);
        hand.GetComponent<Animator>().ResetTrigger("Fire_Auto");
    }

    virtual public void SetIsReloadFinish() { }

    public Quaternion GetHandFireRot() { return Quaternion.Euler(handFireRot); }

    virtual public bool Fire() { return false; }
    public bool CanReload() { if (currentAmmo < maxAmmo && !isReload) return true; return false; }

    virtual public void ResetInfo() { owner = null; hand = null; isShot = false; canShot = false; isRecoil = false; currentShotDelay = shotDelay; isReload = false; currentReloadTime = reloadTime; }

    //protected void CheckingParent()
    //{
    //    if(!this.transform.root.CompareTag("Player"))
    //    {
    //        isDroped = true;
    //    }
    //}

}

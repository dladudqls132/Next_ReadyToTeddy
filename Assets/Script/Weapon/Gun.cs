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

    [SerializeField] protected GameObject owner;
    [SerializeField] protected Transform hand;
    [SerializeField] protected Transform shotPos;
    
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
    }

    public GunType GetGunType() { return gunType; }
    public int GetMaxAmmoCount() { return maxAmmo; }
    public int GetCurrentAmmoCount() { return currentAmmo; }
    public bool GetIsReload() { return isReload; }
    public bool GetIsShot() { return isShot; }
    public void SetIsShot(bool value) { isShot = value; } 
    public bool GetIsRecoil() { return isRecoil; }
    public void SetIsRecoil(bool value) { isRecoil = value; }
    public void SetOwner(GameObject who, Transform hand) 
    { 
        owner = who; 
        this.hand = hand;
 
        //for (int i = 0; i < hand.childCount; i++)
        //{
        //    if (!hand.GetChild(i).gameObject.activeSelf)
        //    {
        //        this.transform.SetParent(hand);
        //    }
        //}
    }
    public GameObject GetOwner() { return owner; }

    public float GetDamagePerBullet() { return damagePerBullet; }
    public void SetDamagePerBullet(float value) { damagePerBullet = value; }

    public virtual void SetIsReload(bool value)
    {
        isReload = value;
        hand.GetComponent<Animator>().SetFloat("Reload_Time", 1 / reloadTime);
        hand.GetComponent<Animator>().ResetTrigger("Fire_Auto");
    }

    virtual public void SetIsReloadFinish() { }

    public Quaternion GetHandFireRot() { return Quaternion.Euler(handFireRot); }

    virtual public bool Fire() { return false; }
    public bool CanReload() { if (currentAmmo < maxAmmo && !isReload) return true; return false; }

    protected virtual void ResetInfo() { owner = null; hand = null; isShot = false; canShot = false; isRecoil = false; currentShotDelay = shotDelay; isReload = false; currentReloadTime = reloadTime; }

    //protected void CheckingParent()
    //{
    //    if(!this.transform.root.CompareTag("Player"))
    //    {
    //        isDroped = true;
    //    }
    //}

}

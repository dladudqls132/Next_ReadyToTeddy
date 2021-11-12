using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    AR,
    ShotGun,
    ChainLightning,
    Flamethrower,
    Sniper
}

public class Gun : MonoBehaviour
{
    [SerializeField] protected GunType gunType;
    [SerializeField] protected UI_GunSound UI_gunsound;
    [SerializeField] protected Vector3 originPos;
    [SerializeField] protected Vector3 originRot;
    [SerializeField] protected Vector3 aimingPos;
    [SerializeField] protected Vector3 aimingRot;
    [SerializeField] protected Vector3 recoil;
    protected float recoilMagnitude;

    [SerializeField] protected GameObject owner;
    [SerializeField] protected Transform hand;
    [SerializeField] protected Transform shotPos;
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected string text;

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
    [SerializeField] protected int maxAmmo_aMag;
    protected int haveAmmo;
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

    [SerializeField] protected float stunTime;
    [SerializeField] protected float refillTime;

    //[SerializeField] private Transform meshRoot;
    [SerializeField] MeshRenderer[] mesh;
    public AnimationClip weaponAnimation;

    public Vector3 GetOriginPos() { return originPos; }
    public Vector3 GetOriginRot() { return originRot; }
    public Sprite GetSprite() { return sprite; }
    public float GetShotDelay() { return shotDelay; }
    public float GetReloadTime() { return reloadTime; }
    public string GetText() { return text; }
    public float GetRecoil() { return recoilMagnitude; }
    public UI_GunSound GetGunSound() { return UI_gunsound; }

    protected Vector3 direction;
    [SerializeField] protected bool isAiming;
    protected bool isAiminged;
    [SerializeField] public Transform mag;

    public void SetInfo(GunType gunType, Sprite sprite, float damagePerBullet, int maxAmmo, int maxAmmo_aMag, int fireNum, float spreadAngle_normal, float spreadAngle_aiming, float shotDelay, float stunTime, float refillTime)
    {
        this.gunType = gunType;
        this.sprite = sprite;
        this.damagePerBullet = damagePerBullet;
        this.maxAmmo = maxAmmo;
        this.maxAmmo_aMag = maxAmmo_aMag;
        this.currentAmmo = maxAmmo_aMag;
        this.fireNum = fireNum;
        this.spreadAngle_normal = spreadAngle_normal;
        this.spreadAngle_aiming = spreadAngle_aiming;
        this.shotDelay = shotDelay;
        this.stunTime = stunTime;
        this.refillTime = refillTime;
    }



    protected virtual void Awake()
    {
        currentAmmo = maxAmmo_aMag;
        haveAmmo = maxAmmo;

        currentReloadTime = reloadTime;
        currentShotDelay = shotDelay;
        mainCam = Camera.main.transform.GetComponent<FPPCamController>();

        recoilMagnitude = recoil.magnitude;

        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].enabled = false;
        }
    }

    public void SetIsAiming(bool value)
    {
        isAiming = value;

        //if (value)
        //{
        //    //mainCam.FovMove(mainCam.GetOriginFov() - mainCam.GetOriginFov() / 4f, 0.07f, 1000);
        //    //mainCam.FovMove(52, 0.07f, 1000);
        //    //mainCam.SetOriginFov(52);
        //    this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(aimingRot), Time.deltaTime * 25);
        //    this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, aimingPos, Time.deltaTime * 25);
        //}
        //else
        //{
        //    //mainCam.FovReset();
        //    if(isAiming)
        //        isAiminged = true;
        //    this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(originRot), Time.deltaTime * 25);
        //    this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, originPos, Time.deltaTime * 25);
        //}
    }



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

           // this.gameObject.SetActive(false);
            this.transform.SetParent(parent);

            this.transform.localPosition = originPos;
            this.transform.localRotation = Quaternion.Euler(originRot);

            for(int i = 0; i < mesh.Length; i++)
            {
                mesh[i].enabled = true;
            }
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

            //this.transform.position = dropPos;
            //this.transform.rotation = dropRot;

            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i].enabled = false;
            }
        }
    }

    public virtual void SetIsReload(bool value)
    {
        if (value)
        {
            mainCam.SetOriginFov(mainCam.GetRealOriginFov());
            mainCam.FovReset();
            hand.GetComponent<Animator>().ResetTrigger("Fire_AR");
            hand.GetComponent<Animator>().ResetTrigger("Fire_SG");

            if (isAiming)
                isAiminged = true;
        }
        else
            hand.GetComponent<HandAnimController>().ResetMag();

        isReload = value;
        //hand.GetComponent<Animator>().SetFloat("Reload_Time", 1 / reloadTime);
    }

    virtual public void SetIsReloadFinish() 
    {
        isReload = false;

        if (isAiminged)
        {
            SetIsAiming(true);
        }

        isAiminged = false;

        if (gunType == GunType.AR)
        {
            currentAmmo = maxAmmo_aMag;
        }
        else
        {
            if (haveAmmo >= (maxAmmo_aMag - currentAmmo))
            {
                haveAmmo -= maxAmmo_aMag - currentAmmo;
                currentAmmo = maxAmmo_aMag;
            }
            else
            {
                currentAmmo += haveAmmo;
                haveAmmo = 0;
            }
        }
    }

    public bool GetIsAiming() { return isAiming; }
    public GunType GetGunType() { return gunType; }
    public int GetMaxAmmoCount() { return maxAmmo; }
    public int GetMaxAmmo_aMagCount() { return maxAmmo_aMag; }
    public void SetMaxAmmoCount(int ammo) { maxAmmo = ammo; }
    public void AddAmmo(int ammo) { haveAmmo += ammo; if (haveAmmo > maxAmmo) haveAmmo = maxAmmo; }
    public int GetHaveAmmoCount() { return haveAmmo; }
    public int GetCurrentAmmoCount() { return currentAmmo; }
    public bool GetIsReload() { return isReload; }
    public bool GetIsShot() { return isShot; }
    public void SetIsShot(bool value) { isShot = value; }
    public bool GetIsRecoil() { return isRecoil; }
    public void SetIsRecoil(bool value) { isRecoil = value; }

    public GameObject GetOwner() { return owner; }

    public float GetDamagePerBullet() { return damagePerBullet; }
    public void SetDamagePerBullet(float value) { damagePerBullet = value; }

    public Quaternion GetHandFireRot() { return Quaternion.Euler(handFireRot); }

    virtual public bool Fire() { return false; }
    public bool CanReload() { if (haveAmmo > 0 && currentAmmo < maxAmmo_aMag && !isReload) { return true; } return false; }
    public Vector3 GetShotPos() { return shotPos.position; }

    virtual public void ResetInfo() { owner = null; hand = null; isShot = false; canShot = false; isRecoil = false; currentShotDelay = shotDelay; isReload = false; currentReloadTime = reloadTime; }

    //protected void CheckingParent()
    //{
    //    if(!this.transform.root.CompareTag("Player"))
    //    {
    //        isDroped = true;
    //    }
    //}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] protected GameObject owner;
    [SerializeField] protected Transform shotPos;
    [SerializeField] protected bool isShot;
    [SerializeField] protected bool canShot;
    [SerializeField] protected bool isRecoil;
    [SerializeField] protected float shotDelay;
    [SerializeField] protected float currentShotDelay;
    [SerializeField] protected bool isReload;
    [SerializeField] protected float reloadTime;
    [SerializeField] protected float currentReloadTime;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected float damagePerBullet;
    [SerializeField] protected float speed;
    [SerializeField] protected ParticleSystem muzzleFlash;

    protected Vector3 direction;

    protected virtual void Start()
    {
        currentAmmo = maxAmmo;
        currentReloadTime = reloadTime;
        currentShotDelay = shotDelay;
    }

    public int GetAmmoCount() { return currentAmmo; }
    public bool GetIsReload() { return isReload; }
    public bool GetIsShot() { return isShot; }
    public bool GetIsRecoil() { return isRecoil; }
    public void SetIsRecoil(bool value) { isRecoil = value; }
    public void SetOwner(GameObject who) { owner = who; }

    public void SetIsReload(bool value) { isReload = value; }

    virtual public bool Fire() { return false; }
    public bool CanReload() { if (currentAmmo < maxAmmo) return true; return false; }
}

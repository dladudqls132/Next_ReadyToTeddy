using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] protected Transform shotPos;
    [SerializeField] protected bool isHitScan;
    [SerializeField] protected bool isReload;
    [SerializeField] protected float reloadTime;
    [SerializeField] protected float currentReloadTime;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected float minDamage;
    [SerializeField] protected float maxDamage;
    [SerializeField] protected float speed;

    protected virtual void Start()
    {
        currentReloadTime = reloadTime;
    }
}

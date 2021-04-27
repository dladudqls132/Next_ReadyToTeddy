using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected int haveNum;
    [SerializeField] protected float damage;
    [SerializeField] protected int maxHaveNum;
    [SerializeField] protected float remainingTime;
    [SerializeField] protected float explosionRadius;
    [SerializeField] protected float explosionPower;
    [SerializeField] protected ParticleSystem particle;
    protected float currentRemainingTime;
    protected bool isThrown;
    protected Rigidbody rigid;
    protected GameObject owner;
    protected Transform hand;

    public void SetHaveNum(int value) { haveNum = value; }
    public int GetHaveNum() { return haveNum; }

    public void DecreaseHaveNum() { haveNum -= 1; }
    public void IncreaseHaveNum() { haveNum += 1; }

    public void SetInfo(float damage, float explosionPower, float explosionRadius, int maxHaveNum, float remainingTime)
    {
        this.damage = damage;
        this.explosionPower = explosionPower;
        this.explosionRadius = explosionRadius;
        this.maxHaveNum = maxHaveNum;
        this.remainingTime = remainingTime;
    }

    public void SetOwner(GameObject owner, Transform hand) { this.owner = owner; this.hand = hand; }

    virtual protected void Start()
    {
        currentRemainingTime = remainingTime;
        rigid = this.GetComponent<Rigidbody>();
    }

    public void SetIsThrown(bool value)
    {
        isThrown = value;
    }
}

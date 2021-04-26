using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected int haveNum;
    [SerializeField] protected float remainingTime;
    [SerializeField] protected float explosionRadius;
    [SerializeField] protected float explosionPower;
    protected float currentRemainingTime;
    protected bool isThrown;
    protected Rigidbody rigid;

    public void SetHaveNum(int value) { haveNum = value; }
    public int GetHaveNum() { return haveNum; }

    public void DecreaseHaveNum() { haveNum -= 1; }
    public void IncreaseHaveNum() { haveNum += 1; }

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

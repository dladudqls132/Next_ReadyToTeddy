﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected GameObject item;
    [SerializeField] public GameObject projectile;
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected string text;
    [SerializeField] protected Vector3 originPos;
    [SerializeField] protected Vector3 originRot;
    [SerializeField] protected Vector3 aimingPos;
    [SerializeField] protected Vector3 aimingRot;

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
    protected FPPCamController mainCam;

    [SerializeField] MeshRenderer[] mesh;

    public GameObject GetItem() { return item; }
    public Sprite GetSprite() { return sprite; }

    public void SetHaveNum(int value) { haveNum = value; }
    public int GetHaveNum() { return haveNum; }
    public int GetMaxHaveNum() { return maxHaveNum; }
    public float GetDamage() { return damage; }
    public string GetText() { return text; }

    public void DecreaseHaveNum() { haveNum -= 1; if (haveNum == 0) this.gameObject.SetActive(false); }
    public void IncreaseHaveNum() { haveNum += 1; }

    public void SetInfo(Sprite sprite, float damage, float explosionPower, float explosionRadius, int maxHaveNum, float remainingTime)
    {
        this.sprite = sprite;
        this.damage = damage;
        this.explosionPower = explosionPower;
        this.explosionRadius = explosionRadius;
        this.maxHaveNum = maxHaveNum;
        this.remainingTime = remainingTime;
    }

    public void SetOwner(GameObject who, Transform hand, Transform parent)
    {
        owner = who;
        this.hand = hand;

        if (owner != null)
        {
            Collider[] c = this.GetComponents<Collider>();
            foreach (Collider temp in c)
            {
                temp.enabled = false;
            }

            rigid.velocity = Vector3.zero;
            rigid.useGravity = false;

            this.transform.SetParent(parent);

            this.transform.localPosition = originPos;
            this.transform.localRotation = Quaternion.Euler(originRot);

            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i].enabled = true;
            }

            item.SetActive(false);
        }
        else
        {
            this.transform.SetParent(null);

            Collider[] c = this.GetComponents<Collider>();
            foreach (Collider temp in c)
            {
                temp.enabled = true;
            }

            rigid.useGravity = true;
            rigid.AddForce(mainCam.transform.forward * 5, ForceMode.Impulse);

            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i].enabled = false;
            }

            item.SetActive(true);
        }
    }

    virtual protected void Awake()
    {
        currentRemainingTime = remainingTime;
        rigid = this.GetComponent<Rigidbody>();
        mainCam = Camera.main.transform.GetComponent<FPPCamController>();

        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].enabled = false;
        }

        particle = Instantiate(particle);
        particle.gameObject.SetActive(false);
    }

    public void SetIsThrown(bool value)
    {
        isThrown = value;
    }
}

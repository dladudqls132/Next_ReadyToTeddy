using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected bool isDead;
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected ParticleSystem damagedEffect;

    public float GetCurrentHP() { return currentHP; }
    public void SetCurrentHP(float value) { currentHP = value; }

    virtual protected void Start()
    {
        currentHP = maxHP;
    }

    protected void CheckingHp()
    {
        if(currentHP <= 0)
        {
            isDead = true;

            this.gameObject.SetActive(false);
        }
    }

    public void DecreaseHp(float value)
    {
        currentHP -= value;

        CheckingHp();
    }

    public void DecreaseHp(float value, Vector3 damagedPos)
    {
        currentHP -= value;
        Instantiate(damagedEffect, damagedPos, Quaternion.identity);

        CheckingHp();
    }
}

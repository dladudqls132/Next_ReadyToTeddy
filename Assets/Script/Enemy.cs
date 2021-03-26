using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected bool isDead;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    [SerializeField] protected float increaseHp;
    [SerializeField] protected float increaseCombo;
    [SerializeField] protected Pool_DamagedEffect pool_damagedEffect;
    [SerializeField] protected GameObject spreadBlood;
    private GameObject whoAttackThis;

    public float GetCurrentHP() { return currentHp; }
    public void SetCurrentHP(float value) { currentHp = value; }

    virtual protected void Start()
    {
        currentHp = maxHp;

        if (GameObject.Find("Pool").transform.Find("Pool_Effect") != null)
            pool_damagedEffect = GameObject.Find("Pool").transform.Find("Pool_Effect").GetComponent<Pool_DamagedEffect>();
    }

    protected void CheckingHp()
    {
        if(currentHp <= 0)
        {
            isDead = true;
            GameObject temp = Instantiate(spreadBlood, this.GetComponent<Collider>().bounds.center, Quaternion.identity);
            temp.GetComponent<particle_test>().SetTarget(whoAttackThis.transform);
            //temp.GetComponent<ParticleSystem>().emission.SetBursts(new[] { new ParticleSystem.Burst(0.0f, increaseCombo) });
            temp.GetComponent<ParticleSystem>().emission.SetBursts(new[] { new ParticleSystem.Burst(0.0f, 1) });

            this.gameObject.SetActive(false);
        }
    }

    public void DecreaseHp(float value)
    {
        currentHp -= value;

        whoAttackThis = null;

        CheckingHp();
    }

    public void DecreaseHp(GameObject attackObj, float value, Vector3 damagedPos)
    {
        currentHp -= value;
        //Instantiate(damagedEffect, damagedPos, Quaternion.identity);
        GameObject effect = pool_damagedEffect.GetDamagedEffect(Pool_DamagedEffect.Material.Iron);

        effect.transform.SetParent(null);
        effect.transform.position = damagedPos;
        effect.transform.rotation = Quaternion.identity;
        effect.SetActive(true);

        whoAttackThis = attackObj;

        CheckingHp();
    }
}

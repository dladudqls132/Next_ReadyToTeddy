using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_Laser : MonoBehaviour
{
    private float attackTick;
    [SerializeField] private float currentTick;
    private float damage;

    public void SetAttackTrue(float damage, float attackTick)
    {
        this.GetComponent<CapsuleCollider>().enabled = true;
        this.damage = damage;
        this.attackTick = attackTick;
        currentTick = 0;
    }

    public void SetAttackFalse()
    {
        this.GetComponent<CapsuleCollider>().enabled = false;
    }

    private void Update()
    {
        currentTick += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (attackTick <= currentTick)
            {
                other.GetComponent<PlayerController>().DecreaseHp(damage);
                currentTick = 0;
            }
        }
    }
}

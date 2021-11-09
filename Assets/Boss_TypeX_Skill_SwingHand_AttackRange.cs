using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_SwingHand_AttackRange : MonoBehaviour
{
    [SerializeField] private GameObject coll;

    private float damage;
    private bool isAttack;

    public void ActiveTrue()
    {
        this.gameObject.SetActive(true);
    }

    public void ActiveFalse()
    {
        isAttack = false;
        coll.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void Attack(float damage)
    {
        this.damage = damage;
        coll.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttack)
        {
            if (other.transform.CompareTag("Player"))
            {
                isAttack = true;
                other.GetComponent<PlayerController>().DecreaseHp(damage);
            }
        }
    }
}

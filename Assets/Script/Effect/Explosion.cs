using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float damage;
    private bool isAttack;

    private void OnEnable()
    {
        isAttack = false;

        StartCoroutine(SetDestroy());
    }

    IEnumerator SetDestroy()
    {
        yield return new WaitForSeconds(0.01f);

        damage = 0;
        isAttack = true;
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttack) return;

        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DecreaseHp(damage);
            isAttack = true;
        }
    }
}

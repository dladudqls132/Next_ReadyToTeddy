using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFog : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DecreaseHp(100);
        }
    }
}

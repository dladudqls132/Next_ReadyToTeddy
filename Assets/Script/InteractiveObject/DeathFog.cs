using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFog : MonoBehaviour
{
    [SerializeField] private Transform returnPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DecreaseHp(10);
            other.transform.position = returnPos.position;
        }
    }
}

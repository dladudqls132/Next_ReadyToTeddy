using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomController : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    private bool isStart;

    void StartRoom()
    {
        boss.GetComponent<Enemy>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isStart)
        {
            isStart = true;
            this.GetComponent<Animator>().SetTrigger("Start");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] private bool isPressed;
    [SerializeField] private bool isEnterPlayer;
    private Transform player;

    private void Update()
    {
        if(isEnterPlayer)
        {
            if(Vector3.Dot(player.GetComponent<PlayerController>().GetCamPos().forward, (this.transform.position - player.GetComponent<PlayerController>().GetCamPos().position).normalized) > 0.75f)
            {
                if(Input.GetKeyDown(KeyCode.F))
                {
                    isPressed = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.transform;
            isEnterPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            isEnterPlayer = false;
        }
    }
}

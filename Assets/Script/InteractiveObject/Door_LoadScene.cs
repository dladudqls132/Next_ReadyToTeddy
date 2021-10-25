using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_LoadScene : MonoBehaviour
{
    private bool isOpened;

    private void OnTriggerStay(Collider other)
    {
        if (isOpened) return;

        Transform temp = other.transform;

        if (temp.CompareTag("Player"))
        {

            if (Input.GetKeyDown(KeyCode.F))
            {
                isOpened = true;
                LoadingSceneController.LoadScene("Bosszone");
            }

        }
    }
}

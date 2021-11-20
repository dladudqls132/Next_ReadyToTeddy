using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door_LoadScene : MonoBehaviour
{
    private bool isOpened;
    private Image f;

    private void Start()
    {
        f = GameObject.Find("F").GetComponent<Image>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOpened) return;

        Transform temp = other.transform;

        if (temp.CompareTag("Player"))
        {
            f.enabled = true;

            if (Input.GetKeyDown(KeyCode.F))
            {
                this.GetComponent<SavePlayerData>().SaveData();
                isOpened = true;
                LoadingSceneController.LoadScene("Bosszone");
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        f.enabled = false;
    }
}

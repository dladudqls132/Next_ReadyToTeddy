using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Exit : MonoBehaviour
{
    private bool isOpened;
    private Animator anim;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOpened) return;

        Transform temp = other.transform;

        if (temp.CompareTag("Player"))
        {

                if (Input.GetKeyDown(KeyCode.F))
                {
                    isOpened = true;
                    anim.SetBool("isOpened", true);
                }
            
        }
    }
}

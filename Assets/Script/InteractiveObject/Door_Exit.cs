using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door_Exit : MonoBehaviour
{
    private bool isOpened;
    private Animator anim;
    private Image f;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        f = GameObject.Find("F").GetComponent<Image>();
    }

    void ShakeCam()
    {
        GameManager.Instance.GetPlayer().GetCam().Shake(1.3f, 0.2f, true);
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
                    isOpened = true;
                    anim.SetBool("isOpened", true);
                }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        f.enabled = false;
    }
}

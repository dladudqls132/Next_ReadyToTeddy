using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door_Exit : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private GameObject ui_text;
    private bool isOpened;
    private Animator anim;
    private Image f;

    private void Awake()
    {
        ui_text = GameObject.FindGameObjectWithTag("UI_Text");
    }

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        f = GameObject.Find("F").GetComponent<Image>();
        ui_text.SetActive(false);
    }

    void ShakeCam()
    {
        GameManager.Instance.GetPlayer().GetCam().Shake(1.3f, 0.2f, true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOpened)
        {
            f.enabled = false;
            return;
        }

        if (other.transform.CompareTag("Player"))
        {
            if (!item.activeSelf)
            {
                ui_text.SetActive(false);
                f.enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    isOpened = true;
                    anim.SetBool("isOpened", true);
                }
            }
            else
            {
                ui_text.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        f.enabled = false;
        ui_text.SetActive(false);
    }
}

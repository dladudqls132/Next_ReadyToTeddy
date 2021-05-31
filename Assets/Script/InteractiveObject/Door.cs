using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private bool isEnterPlayer;
    [SerializeField] private bool isOpened;
    private Animator anim;

    private void Start()
    {
        player = GameManager.Instance.GetPlayer().transform;
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.GetIsCombat())
        {
            isOpened = false;
        }

        anim.SetBool("isOpened", isOpened);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!GameManager.Instance.GetIsCombat())
            {
                isOpened = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!GameManager.Instance.GetIsCombat())
            {
                isOpened = false;
            }
        }
    }
}

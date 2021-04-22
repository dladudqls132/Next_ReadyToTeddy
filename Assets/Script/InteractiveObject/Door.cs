using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private Transform button;
    [SerializeField] private Transform player;
    [SerializeField] private bool isEnterPlayer;
    [SerializeField] private bool isOpened;

    private void Start()
    {
        player = GameManager.Instance.GetPlayer().GetCamPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetIsCombat())
        {
            door.GetComponent<Animator>().SetBool("isOpened", false);
        }
        else
        {
            if(isOpened)
            {
                door.GetComponent<Animator>().SetBool("isOpened", true);
            }

            if (!isOpened)
            {
                if (isEnterPlayer)
                {
                    if (Vector3.Dot((button.position - player.position).normalized, player.forward) >= 0.5f)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            isOpened = true;
                            door.GetComponent<Animator>().SetBool("isOpened", true);
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnterPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isEnterPlayer = false;
    }
}

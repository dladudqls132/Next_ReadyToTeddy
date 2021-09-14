using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckingGround : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private List<Transform> ground = new List<Transform>();
    public bool canJump;

    private void Start()
    {
        player = this.transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.transform.gameObject.layer).Equals("Enviroment"))
        {
            if (!ground.Contains(other.transform))
            {
                ground.Add(other.transform);

                canJump = true;
                player.SetCanJump(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.transform.gameObject.layer).Equals("Enviroment"))
        {
            if (ground.Contains(other.transform))
            {
                ground.Remove(other.transform);
            }

            if (ground.Count == 0)
            {
                //canJump = false;
                //player.SetCanJump(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlayerController player;

    public PlayerController GetPlayer() { return player; }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.Init();
    }
}

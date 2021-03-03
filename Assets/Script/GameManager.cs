using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Pool_Bullet bulletManager;

    public Pool_Bullet GetBulletManager() { return bulletManager; }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        bulletManager = GameObject.FindGameObjectWithTag("BulletManager").GetComponent<Pool_Bullet>();
        bulletManager.Init();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [System.Serializable]
    protected struct enemySpawnInfo
    {
        [SerializeField] private float spawnRate;
        [SerializeField] private GameObject enemyPrefab;
    }

    [SerializeField] private float successRate;
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private List<enemySpawnInfo> enemySpawnInfos = new List<enemySpawnInfo>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

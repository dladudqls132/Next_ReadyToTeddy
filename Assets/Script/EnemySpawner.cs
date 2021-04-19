using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Stage currentStage;
    [SerializeField] private float spawnTime;
    [SerializeField] private float currentSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        //if(this.transform.parent.GetComponent<Stage>() != null)
            currentStage = this.transform.root.GetComponent<Stage>();
        spawnTime = currentStage.GetRandomSpawnTime();
        currentSpawnTime = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
            currentSpawnTime -= Time.deltaTime;
        if (currentStage.GetIsStart() && !currentStage.GetIsFull())
        {

            if (currentSpawnTime <= 0)
            {
                for (int i = 0; i < currentStage.GetEnemySpawnInfo().Count; i++)
                {
                    float r = Random.Range(0.0f, 100.0f);

                    if(r <= currentStage.GetEnemySpawnInfo()[i].spawnRate)
                    {
                        currentStage.SpawnEnemy(currentStage.GetEnemySpawnInfo()[i].enemyType, this.transform.position);
                        currentSpawnTime = spawnTime;
                        break;
                    }
                }
            }
        }
    }
}

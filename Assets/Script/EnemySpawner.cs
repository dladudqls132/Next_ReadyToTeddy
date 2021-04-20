using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Enemy enemy;
    [SerializeField] private Stage currentStage;
    [SerializeField] private Transform destPos;
    [SerializeField] private float spawnTime;
    [SerializeField] private float currentSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        //if(this.transform.parent.GetComponent<Stage>() != null)
        currentStage = this.transform.root.GetComponent<Stage>();
        spawnTime = currentStage.GetRandomSpawnTime();
        currentSpawnTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (currentStage.GetIsStart() && !currentStage.GetIsFull())
        {
            if(enemy == null || enemy.GetIsDead())
            {
                currentSpawnTime -= Time.deltaTime;
            }

            if (currentSpawnTime <= 0)
            {
                //for (int i = 0; i < currentStage.GetEnemySpawnInfo().Count; i++)
                //{
                //    float r = Random.Range(0.0f, 100.0f);

                //    if(r <= currentStage.GetEnemySpawnInfo()[i].spawnRate)
                //    {
                //        currentStage.SpawnEnemy(currentStage.GetEnemySpawnInfo()[i].enemyType, this.transform.position);
                //        currentSpawnTime = spawnTime;
                //        break;
                //    }
                //}


                if(enemyType == EnemyType.Gunner_Easy)
                {
                    enemy = currentStage.SpawnEnemy(enemyType, this.transform.position, false);
                    enemy.GetComponent<Enemy_ShooterTest>().SetDestPos(destPos.position);
                }
                else
                {
                    if(currentStage.GetBoss())
                        enemy = currentStage.SpawnEnemy(enemyType, this.transform.position, true);
                }

                if(enemyType == EnemyType.Boss)
                {
                    enemy = currentStage.SpawnEnemy(enemyType, this.transform.position, true);
                    currentStage.SetBoss(enemy);
                }

                currentSpawnTime = spawnTime;

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemySpawnInfo
{
    public float spawnRate;
    public EnemyType enemyType;
}

public class Stage : MonoBehaviour
{
    [SerializeField] private bool isStart;
    [SerializeField] private Pool_Enemy pool_Enemy;
    [SerializeField] private float successRate;
    //[SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private List<EnemySpawnInfo> enemySpawnInfo = new List<EnemySpawnInfo>();
    [SerializeField] private int maxEnemyNum;
    [SerializeField] private int currentEnemyNum;
    [SerializeField] private bool isFull;
    [SerializeField] private float randomSpawnTime_min;
    [SerializeField] private float randomSpawnTime_max;

    public void SetIsStart(bool value) { isStart = value; }
    public bool GetIsStart() { return isStart; }
    public void SetSuccessRate(float value) { successRate = value; }
    public float GetSuccessRate() { return successRate; }
    public int GetMaxEnemyNum() { return maxEnemyNum; }
    public int GetCurrentEnemyNum() { return currentEnemyNum; }
    public float GetRandomSpawnTime() { return Random.Range(randomSpawnTime_min, randomSpawnTime_max); }
    public void DecreaseEnemyNum() { currentEnemyNum--; }
    public bool GetIsFull()
    {
        if (currentEnemyNum >= maxEnemyNum)
            return true;
        else
            return false;
    }
    public List<EnemySpawnInfo> GetEnemySpawnInfo() { return enemySpawnInfo; }
    public void SpawnEnemy(EnemyType enemyType, Vector3 position)
    {
        GameObject tempEnemy = pool_Enemy.GetEnemy(enemyType);
        if (tempEnemy == null)
            return;
        tempEnemy.transform.position = position;
        tempEnemy.SetActive(true);
        tempEnemy.GetComponent<Enemy>().SetIsDead(false);
        tempEnemy.GetComponent<Enemy>().SetCurrentStage(this);

        currentEnemyNum++;
    }

    protected void Update()
    {
        if(successRate >= 100.0f)
        {
            isStart = false;
        }
    }
}

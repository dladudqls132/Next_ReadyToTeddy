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
    [SerializeField] private bool isClear;
    [SerializeField] private bool isStart;
    [SerializeField] private Enemy boss;
    [SerializeField] private Pool_Enemy pool_Enemy;
    [SerializeField] private float successRate;
    [SerializeField] private float clearTime;
    private float currentClearTime;
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

    public void SetBoss(Enemy enemy) { boss = enemy; }
    public Enemy GetBoss() { return boss; }

    public void SetIsClear(bool value) { isClear = value; }
    public bool GetIsClear() { return isClear; }

    public float GetCurrentClearTime() { return currentClearTime; }

    public List<EnemySpawnInfo> GetEnemySpawnInfo() { return enemySpawnInfo; }
    public Enemy SpawnEnemy(EnemyType enemyType, Vector3 position, bool addNum)
    {
        GameObject tempEnemy = pool_Enemy.GetEnemy(enemyType);
        if (tempEnemy == null)
            return null;
        tempEnemy.transform.position = position;
        tempEnemy.SetActive(true);
        tempEnemy.GetComponent<Enemy>().SetIsDead(false);
        tempEnemy.GetComponent<Enemy>().SetCurrentStage(this);

        if(addNum)
            currentEnemyNum++;

        return tempEnemy.GetComponent<Enemy>();
    }

    private void Start()
    {
        pool_Enemy = GameManager.Instance.GetPoolEnemy();
        currentClearTime = clearTime;
    }

    protected void Update()
    {
        if(isStart)
            currentClearTime -= Time.deltaTime;

        if(currentClearTime <= 0)
        {
            GameManager.Instance.SetIsGameOver(true);
        }

        if(successRate >= 100.0f)
        {
            if (boss != null)
            {
                if (boss.GetIsDead())
                    isStart = false;
            }
            else
                isStart = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Ragdoll : MonoBehaviour
{
    [System.Serializable]
    struct EnemyInfo
    {
        public GameObject prefab;
        public EnemyType enemyType;

        public EnemyInfo(EnemyInfo info, Transform parent)
        {
            this.prefab = Instantiate(info.prefab, parent);
            this.enemyType = info.enemyType;
            this.prefab.SetActive(false);
        }
    }

    [SerializeField] private int enemyNum = 0;
    [SerializeField] private EnemyInfo[] enemyInfos;
    private List<EnemyInfo> enemys = new List<EnemyInfo>();

    public void Init()
    {
        for (int i = 0; i < enemyInfos.Length; i++)
        {
            for (int j = 0; j < enemyNum; j++)
            {
                EnemyInfo temp = new EnemyInfo(enemyInfos[i], this.transform);
                enemys.Add(temp);
            }
        }
    }

    public GameObject GetEnemyRagdoll(EnemyType enemyType)
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            if (enemys[i].enemyType == enemyType)
            {
                if (!enemys[i].prefab.activeSelf)
                    return enemys[i].prefab;
            }
        }

        return null;
    }
}

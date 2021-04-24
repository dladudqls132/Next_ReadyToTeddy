using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Warrior_Easy,
    Warrior_Normal,
    Warrior_Hard,
    Gunner_Easy,
    Gunner_Normal,
    Gunner_Hard,
    Air_Easy,
    Air_Normal,
    Air_Hard,
    Boss
}

public class Pool_Enemy : MonoBehaviour
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

    //// Start is called before the first frame update
    //void Start()
    //{
    //    for (int i = 0; i < enemyInfos.Length; i++)
    //    {
    //        for (int j = 0; j < enemyNum; j++)
    //        {
    //            EnemyInfo temp = new EnemyInfo(enemyInfos[i], this.transform);
    //            enemys.Add(temp);
    //        }
    //    }
    //}

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

    public GameObject GetEnemy(EnemyType enemyType)
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

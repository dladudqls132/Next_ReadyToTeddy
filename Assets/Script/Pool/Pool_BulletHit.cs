using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletHitType
{
    Normal
}

public class Pool_BulletHit : MonoBehaviour
{
    [System.Serializable]
    struct BulletHitInfo
    {
        public GameObject prefab;
        public BulletHitType bulletType;

        public BulletHitInfo(BulletHitInfo info, Transform parent)
        {
            this.prefab = Instantiate(info.prefab, parent);
            this.bulletType = info.bulletType;
            this.prefab.SetActive(false);
        }
    }

    [SerializeField] private int bulletNum = 0;
    [SerializeField] private BulletHitInfo[] bulletHitInfos;
    private List<BulletHitInfo> bulletHits = new List<BulletHitInfo>();
    [SerializeField] private int currentNum;

    public void Init()
    {
        for (int i = 0; i < bulletHitInfos.Length; i++)
        {
            for (int j = 0; j < bulletNum; j++)
            {
                BulletHitInfo temp = new BulletHitInfo(bulletHitInfos[i], this.transform);
                bulletHits.Add(temp);
            }
        }
    }

    public GameObject GetBulletHit(BulletHitType bulletType)
    {
        GameObject temp = bulletHits[currentNum].prefab;
        if (bulletHits[currentNum].bulletType != bulletType)
        {
            currentNum++;

            if (currentNum >= bulletHits.Count)
            {
                currentNum = 0;
            }

            return null;
        }
        else
        {
            currentNum++;

            if (currentNum >= bulletHits.Count)
            {
                currentNum = 0;
            }

            return temp;
        }
    }
}

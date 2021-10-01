using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Normal,
    CL,
    Energy,
    Normal_small
}

public class Pool_Bullet : MonoBehaviour
{
    [System.Serializable]
    struct BulletInfo
    {
        public GameObject prefab;
        public BulletType bulletType;

        public BulletInfo(BulletInfo info, Transform parent)
        {
            this.prefab = Instantiate(info.prefab, parent);
            this.bulletType = info.bulletType;
            this.prefab.transform.position = Vector3.up * 1000;
            //this.prefab.transform.position = Vector3.zero;
            //this.prefab.SetActive(false);
        }
    }

    [SerializeField] private int bulletNum = 0;
    [SerializeField] private BulletInfo[] bulletInfos;
    private List<BulletInfo> bullets = new List<BulletInfo>();

    public void Init()
    {
        for (int i = 0; i < bulletInfos.Length; i++)
        {
            for (int j = 0; j < bulletNum; j++)
            {
                BulletInfo temp = new BulletInfo(bulletInfos[i], this.transform);
                bullets.Add(temp);
            }
        }

        Invoke("SetActiveFalseAll", 0.2f);
    }

    void SetActiveFalseAll()
    {
        for (int i = 0; i < bullets.Count; i++)
        {

            bullets[i].prefab.SetActive(false);
        }
    }

    public GameObject GetBullet(BulletType bulletType)
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i].bulletType == bulletType)
            {
                if (!bullets[i].prefab.activeSelf)
                    return bullets[i].prefab;
            }
        }

        return null;
    }
}

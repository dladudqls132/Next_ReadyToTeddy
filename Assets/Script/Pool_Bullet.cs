using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private int bulletNum;
    [SerializeField] private List<GameObject> pool_bullet = new List<GameObject>();

    public void Init()
    {
        for (int i = 0; i < bulletNum; i++)
        {
            GameObject temp = Instantiate(bullet, this.transform);
            temp.GetComponent<Bullet>().Init();
            temp.SetActive(false);
            pool_bullet.Add(temp);
        }
    }

    public GameObject GetBullet()
    {
        for(int i = 0; i < bulletNum; i++)
        {
            if(!pool_bullet[i].activeSelf)
            {
                return pool_bullet[i];
            }
        }

        return null;
    }
}

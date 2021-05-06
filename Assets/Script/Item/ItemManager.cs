using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] List<GameObject> items = new List<GameObject>();

    public void Init()
    {

    }

    public void ResetItem()
    {
        this.items.Clear();
    }

    public void AddItem(GameObject item)
    {
        this.items.Add(item);
    }

    public void SpawnItem(ItemType type, Vector3 pos, Quaternion rot)
    {
        float itemDropRate = Random.Range(0.0f, 100.0f);
        float temp1 = 0;
        float temp2 = 0;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].GetComponent<Item>().GetItemType() == type)
            {
                temp2 += items[i].GetComponent<Item>().GetDropRate();

                if (itemDropRate <= temp2 && itemDropRate > temp1)
                {
                    Instantiate(items[i], pos, rot, null);
                    return;
                }

                temp1 += items[i].GetComponent<Item>().GetDropRate();
            }
        }
    }
}

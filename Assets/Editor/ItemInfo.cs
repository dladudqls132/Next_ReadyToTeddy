using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "Scriptable Object Asset/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    [System.Serializable]
    public struct ItemInfos
    {
        public GameObject prefab;
        public ItemType itemType;
        public float dropRate;
    }

    public void SetInfo()
    {
        itemManager.GetComponent<ItemManager>().ResetItem();

        for (int i = 0; i < items.Length; i++)
        {
            Item temp = items[i].prefab.GetComponent<Item>();
            temp.SetInfo(items[i].itemType, items[i].dropRate);
            EditorUtility.SetDirty(temp);

            itemManager.GetComponent<ItemManager>().AddItem(items[i].prefab);
            EditorUtility.SetDirty(itemManager);
        }
    }

    public GameObject itemManager;
    public ItemInfos[] items;
}

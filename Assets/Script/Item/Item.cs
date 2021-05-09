using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Potion,
    Magazine
}

public class Item : MonoBehaviour
{
    [SerializeField] protected float dropRate;
    [SerializeField] protected ItemType itemType;
    [SerializeField] protected float moveSpeed;

    protected PlayerController player;
    protected Rigidbody rigid;

    public float GetDropRate() { return dropRate; }
    public ItemType GetItemType() { return itemType; }

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
        rigid = this.GetComponent<Rigidbody>();
    }

    public void SetInfo(ItemType itemType, float dropRate)
    {
        this.itemType = itemType;
        this.dropRate = dropRate;
    }

    protected void UpdateMoveSpeed()
    {
        moveSpeed += Time.deltaTime * 10;
    }
}

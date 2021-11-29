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

    protected float delay = 1.0f;
    protected float moveSpeed = 15.0f;
    protected Collider collider;

    protected PlayerController player;
    protected Rigidbody rigid;
    protected bool canMove;

    public float GetDropRate() { return dropRate; }
    public ItemType GetItemType() { return itemType; }

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
        rigid = this.GetComponent<Rigidbody>();
        collider = this.GetComponent<Collider>();
    }

    public void SetInfo(ItemType itemType, float dropRate)
    {
        this.itemType = itemType;
        this.dropRate = dropRate;
    }

    public void ResetInfo()
    {
        if (rigid == null)
            rigid = this.GetComponent<Rigidbody>();

        if (collider == null)
            collider = this.GetComponent<Collider>();

        collider.isTrigger = false;
        canMove = false;
        rigid.velocity = Vector3.zero;
        moveSpeed = 15.0f;
    }

    public IEnumerator CanMoveDelay()
    {
        yield return new WaitForSeconds(delay);

        canMove = true;
    }

    protected void UpdateMoveSpeed()
    {
        //moveSpeed += Time.deltaTime * 10;
        //moveSpeed = Mathf.Clamp(moveSpeed, 5.0f, 15.0f);
    }
}

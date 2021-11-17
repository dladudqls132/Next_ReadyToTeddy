using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Box : InteractiveObject
{
    enum ItemBoxType
    {
        Potion,
        Magazine
    }

    [SerializeField] private ItemBoxType boxType;
    [SerializeField] private GameObject[] dropItems_prefab;
    [SerializeField] private Rigidbody[] parts;
    [SerializeField] private ParticleSystem particle;
    private List<GameObject> dropItems = new List<GameObject>();

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        for (int i = 0; i < dropItems_prefab.Length; i++)
        {
            GameObject tempItem = Instantiate(dropItems_prefab[i]);
            dropItems.Add(tempItem);
            tempItem.transform.position = this.transform.position;
            tempItem.SetActive(false);
        }

        parts = this.GetComponentsInChildren<Rigidbody>();
    }

    override protected void ActiveFalse()
    {
        isDestroyed = true;
        currentHp = 0;

        for (int i = 0; i < dropItems.Count; i++)
        {
            if (boxType == ItemBoxType.Magazine)
            {
                if (GameManager.Instance.GetPlayer().GetInventory().GetWeapon(GunType.AR))
                {
                    if(dropItems[i].GetComponent<Item_Magazine>().GetMagType() == GunType.AR)
                    {
                        dropItems[i].SetActive(true);
                    }
                }
                if(GameManager.Instance.GetPlayer().GetInventory().GetWeapon(GunType.ShotGun))
                {
                    if (dropItems[i].GetComponent<Item_Magazine>().GetMagType() == GunType.ShotGun)
                    {
                        dropItems[i].SetActive(true);
                    }
                }
                if(GameManager.Instance.GetPlayer().GetInventory().GetWeapon(GunType.Sniper))
                {
                    if (dropItems[i].GetComponent<Item_Magazine>().GetMagType() == GunType.Sniper)
                    {
                        dropItems[i].SetActive(true);
                    }
                }
                if(GameManager.Instance.GetPlayer().GetInventory().GetWeapon(GunType.ChainLightning))
                {
                    if (dropItems[i].GetComponent<Item_Magazine>().GetMagType() == GunType.ChainLightning)
                    {
                        dropItems[i].SetActive(true);
                    }
                }
                if(GameManager.Instance.GetPlayer().GetInventory().GetWeapon(GunType.Flamethrower))
                {
                    if (dropItems[i].GetComponent<Item_Magazine>().GetMagType() == GunType.Flamethrower)
                    {
                        dropItems[i].SetActive(true);
                    }
                }
            }
            else if (boxType == ItemBoxType.Potion)
            {
                dropItems[i].SetActive(true);
            }
        }

        this.GetComponent<BoxCollider>().enabled = false;
        for(int i = 0; i < parts.Length - 1; i++)
        {
            parts[i].isKinematic = false;

            parts[i].angularVelocity = Random.insideUnitSphere * 5;
            parts[i].AddForce((parts[i].transform.position - this.transform.position).normalized * 4, ForceMode.Impulse);
        }

        parts[parts.Length - 1].gameObject.SetActive(false);
        particle.Play();

        StartCoroutine(DelayActiveFalse());
    }

    IEnumerator DelayActiveFalse()
    {
        yield return new WaitForSeconds(5.0f);

        this.gameObject.SetActive(false);
    }
}

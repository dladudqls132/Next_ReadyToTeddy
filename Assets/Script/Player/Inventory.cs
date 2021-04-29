using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotType
{
    Hand,
    Gun,
    Projectile
}

[System.Serializable]
public class Slot
{
    public SlotType slotType;
    public Transform transform;
    public GameObject weapon;
    public bool isEmpty;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Slot> slots = new List<Slot>();
    [SerializeField] private int currentSlotNum;
    [SerializeField] private PlayerController player;

    public int GetCurrentSlotNum() { return currentSlotNum; }

    // Start is called before the first frame update
    public void Init()
    {
        player = GameManager.Instance.GetPlayer();

        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].slotType != SlotType.Hand)
            {
                slots[i].isEmpty = true;
                slots[i].transform = player.GetHand().GetChild(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {

            SwapWeapon(0);

        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {

            SwapWeapon(1);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            SwapWeapon(2);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {


            SwapWeapon(3);

        }
    }

    public void DropWeapon()
    {
        if(slots[currentSlotNum].slotType == SlotType.Gun)
        {
            if(slots[currentSlotNum].weapon != null)
            {
                slots[currentSlotNum].weapon.GetComponent<Gun>().SetOwner(null, null, null);
                slots[currentSlotNum].weapon = null;
                slots[currentSlotNum].isEmpty = true;
                SetAnyWeapon();
            }
        }
    }

    public void ChangeWeapon(GameObject weapon)
    {
        if (weapon.GetComponent<Gun>() != null)
        {
            if (slots[currentSlotNum].slotType == SlotType.Gun)
            {
                slots[currentSlotNum].weapon.GetComponent<Gun>().SetOwner(null, null, null);

                slots[currentSlotNum].weapon = weapon;
                weapon.GetComponent<Gun>().SetOwner(player.gameObject, player.GetHand(), slots[currentSlotNum].transform);
                weapon.gameObject.SetActive(false);
                SwapWeapon(currentSlotNum);
            }
        }
    }

    public void AddWeapon(GameObject weapon)
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if(slots[i].isEmpty)
            {
                if (weapon.GetComponent<Gun>() != null)
                {
                    if(slots[i].slotType == SlotType.Gun)
                    {
                        slots[i].weapon = weapon;
                        slots[i].isEmpty = false;
                        weapon.gameObject.SetActive(false);
                        weapon.GetComponent<Gun>().SetOwner(player.gameObject, player.GetHand(), slots[i].transform);

                        SwapWeapon(i);
                        return;
                    }
                }
                else if(weapon.GetComponent<Projectile>() != null)
                {
                    if (slots[i].slotType == SlotType.Projectile)
                    {
                        slots[i].weapon = weapon;
                        slots[i].isEmpty = false;
                        weapon.gameObject.SetActive(false);
                        weapon.GetComponent<Projectile>().SetOwner(player.gameObject, player.GetHand(), slots[i].transform);

                        return;
                    }
                }
            }
            else
            {
                if (weapon.GetComponent<Projectile>() != null)
                {
                    if (slots[i].slotType == SlotType.Projectile)
                    {
                        weapon.gameObject.SetActive(false);
                        slots[i].weapon.GetComponent<Projectile>().IncreaseHaveNum();
                        return;
                    }
                }
            }
        }
    }

    public void SetAnyWeapon()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].isEmpty)
            {
                if (player.SetWeapon(slots[i].slotType, slots[i].weapon))
                {
                    currentSlotNum = i;
                    return;
                }
            }
        }
    }

        private void SwapWeapon(int slotNum)
    {
        if (slots[slotNum].slotType == SlotType.Gun)
        {
            if (slots[slotNum].weapon != null)
            {
                if (slots[slotNum].weapon == player.GetWeaponGameObject())
                    return;

                if (player.SetWeapon(slots[slotNum].slotType, slots[slotNum].weapon))
                {
                    currentSlotNum = slotNum;
                }
            }
        }
        else if(slots[slotNum].slotType == SlotType.Projectile)
        {
            if (slots[slotNum].weapon != null)
            {
                if (slots[slotNum].weapon == player.GetWeaponGameObject())
                {
                    return;
                }
                else
                {
                    if (player.SetWeapon(slots[slotNum].slotType, slots[slotNum].weapon))
                    {
                        currentSlotNum = slotNum;
                    }
                }
            }
        }
        else if(slots[slotNum].slotType == SlotType.Hand)
        {
            if (player.SetWeapon(slots[slotNum].slotType, slots[slotNum].weapon))
            {
                currentSlotNum = slotNum;
            }
        }
    }

}

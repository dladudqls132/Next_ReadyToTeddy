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
    [SerializeField] public bool isOpen;
    [SerializeField] private UI_Inventory UI_Inventory;

    public List<Slot> GetSlots() { return slots; }
    public int GetCurrentSlotNum() { return currentSlotNum; }

    // Start is called before the first frame update
    public void Init()
    {
        player = GameManager.Instance.GetPlayer();

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].slotType != SlotType.Hand)
            {
                slots[i].isEmpty = true;
                slots[i].transform = player.GetHand().GetChild(i);
            }
        }

        UI_Inventory = FindObjectOfType<UI_Inventory>();
        UI_Inventory.transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwapWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (UI_Inventory != null)
            {
                UI_Inventory.transform.GetChild(0).gameObject.SetActive(!UI_Inventory.transform.GetChild(0).gameObject.activeSelf);
                isOpen = UI_Inventory.transform.GetChild(0).gameObject.activeSelf;
            }

            if (isOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public Gun GetWeapon(GunType type)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].weapon != null)
            {
                if (slots[i].weapon.GetComponent<Gun>() != null)
                {
                    if (slots[i].weapon.GetComponent<Gun>().GetGunType() == type)
                    {
                        return slots[i].weapon.GetComponent<Gun>();
                    }
                }
            }
        }

        return null;
    }

    public void DropWeapon()
    {
        if (slots[currentSlotNum].slotType == SlotType.Gun)
        {
            if (slots[currentSlotNum].weapon != null)
            {
                slots[currentSlotNum].weapon.GetComponent<Gun>().SetOwner(null, null, null);
                slots[currentSlotNum].weapon = null;
                slots[currentSlotNum].isEmpty = true;
                UI_Inventory.UpdateSlot(currentSlotNum);
                SetAnyWeapon();
            }
        }
    }

    public void DropWeapon(int slotNum)
    {
        if (slots[slotNum].slotType == SlotType.Gun)
        {
            if (slots[slotNum].weapon != null)
            {
                slots[slotNum].weapon.SetActive(true);
                slots[slotNum].weapon.GetComponent<Gun>().SetOwner(null, null, null);
                slots[slotNum].weapon = null;
                slots[slotNum].isEmpty = true;
                UI_Inventory.UpdateSlot(slotNum);
                SetAnyWeapon();
            }
        }
        else if(slots[slotNum].slotType == SlotType.Projectile)
        {
            if (slots[slotNum].weapon != null)
            {
                slots[slotNum].weapon.SetActive(true);
                slots[slotNum].weapon.GetComponent<Projectile>().SetOwner(null, null, null);
                slots[slotNum].weapon = null;
                slots[slotNum].isEmpty = true;
                UI_Inventory.UpdateSlot(slotNum);
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
                UI_Inventory.UpdateSlot(currentSlotNum);
                SwapWeapon(currentSlotNum);
            }
        }
    }

    public bool ChangeWeapon(int changeSlotNum, int changedSlotNum)
    {
        if (slots[changeSlotNum].weapon != null)
        {
            if (slots[changeSlotNum].weapon.GetComponent<Gun>() != null)
            {
                if (slots[changedSlotNum].slotType == SlotType.Gun)
                {
                    if (slots[changedSlotNum].weapon != null)
                    {
                        GameObject temp = slots[changedSlotNum].weapon;

                        slots[changedSlotNum].weapon = slots[changeSlotNum].weapon;
                        slots[changeSlotNum].weapon = temp;

                        slots[changedSlotNum].weapon.GetComponent<Gun>().SetOwner(player.gameObject, player.GetHand(), slots[changedSlotNum].transform);
                        slots[changeSlotNum].weapon.GetComponent<Gun>().SetOwner(player.gameObject, player.GetHand(), slots[changeSlotNum].transform);

                        UI_Inventory.UpdateSlot(changedSlotNum);
                        UI_Inventory.UpdateSlot(changeSlotNum);
                        SwapWeapon(currentSlotNum);

                        return true;
                    }
                    else
                    {
                        slots[changedSlotNum].weapon = slots[changeSlotNum].weapon;
                        slots[changedSlotNum].weapon.GetComponent<Gun>().SetOwner(player.gameObject, player.GetHand(), slots[changedSlotNum].transform);

                        slots[changeSlotNum].weapon = null;
                        slots[changeSlotNum].isEmpty = true;

                        UI_Inventory.UpdateSlot(changedSlotNum);
                        UI_Inventory.UpdateSlot(changeSlotNum);
                        SwapWeapon(2);

                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void AddWeapon(GameObject weapon)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].isEmpty)
            {
                if (weapon.GetComponent<Gun>() != null)
                {
                    if (slots[i].slotType == SlotType.Gun)
                    {
                        slots[i].weapon = weapon;
                        slots[i].isEmpty = false;
                        weapon.gameObject.SetActive(false);
                        weapon.GetComponent<Gun>().SetOwner(player.gameObject, player.GetHand(), slots[i].transform);
                        UI_Inventory.UpdateSlot(i);

                        SwapWeapon(i);
                        return;
                    }
                }
                else if (weapon.GetComponent<Projectile>() != null)
                {
                    if (slots[i].slotType == SlotType.Projectile)
                    {
                        slots[i].weapon = weapon;
                        slots[i].isEmpty = false;
                        weapon.gameObject.SetActive(false);
                        weapon.GetComponent<Projectile>().SetOwner(player.gameObject, player.GetHand(), slots[i].transform);
                        UI_Inventory.UpdateSlot(i);

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

    public void DestroyWeapon(int slotNum)
    {
        if (slots[slotNum].slotType == SlotType.Projectile)
        {
            if (slots[slotNum].weapon != null)
            {
                //Destroy(slots[slotNum].weapon);
                slots[slotNum].weapon.SetActive(false);
                slots[slotNum].weapon.GetComponent<Projectile>().SetOwner(null, null, null);
                slots[slotNum].weapon = null;
                slots[slotNum].isEmpty = true;
                UI_Inventory.UpdateSlot(slotNum);
                SetAnyWeapon();
            }
        }
    }

    public void SetAnyWeapon()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].isEmpty)
            {
                SwapWeapon(i);
                return;
            }
        }
    }

    private void SwapWeapon(int slotNum)
    {
        if (slots[slotNum].slotType == SlotType.Gun)
        {
            if (slots[slotNum].weapon != null)
            {
                if (player.SetWeapon(slots[slotNum].slotType, slots[slotNum].weapon))
                {
                    currentSlotNum = slotNum;
                }
            }
        }
        else if (slots[slotNum].slotType == SlotType.Projectile)
        {
            if (slots[slotNum].weapon != null)
            {

                if (player.SetWeapon(slots[slotNum].slotType, slots[slotNum].weapon))
                {
                    currentSlotNum = slotNum;
                }

            }
        }
        else if (slots[slotNum].slotType == SlotType.Hand)
        {
            if(player != null)
            if (player.SetWeapon(slots[slotNum].slotType, slots[slotNum].weapon))
            {
                currentSlotNum = slotNum;
            }
        }
    }

}

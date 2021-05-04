using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[System.Serializable]
//class UI_Slot
//{
//    public Image slot;
//    public Sprite sprite;
//    public float damage;
//    public float shotDelay;
//    public int maxAmmo;
//    public int currentAmmo;
//}

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    private List<Slot> slots;
    [SerializeField] private List<UI_Slot> ui_slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameManager.Instance.GetPlayer().GetInventory();
        slots = inventory.GetSlots();
    }

    public void UpdateSlot(int slotNum)
    {
        if (slots[slotNum].weapon != null)
        {
            if (slots[slotNum].weapon.GetComponent<Gun>() != null)
            {
                //ui_slots[slotNum].SetSlot(slots[slotNum].weapon.GetComponent<Gun>().GetSprite(), slots[slotNum].weapon.GetComponent<Gun>().GetDamagePerBullet(), slots[slotNum].weapon.GetComponent<Gun>().GetShotDelay(), slots[slotNum].weapon.GetComponent<Gun>().GetMaxAmmoCount());
                ui_slots[slotNum].SetSlot(slots[slotNum].weapon.GetComponent<Gun>().GetSprite(), slots[slotNum].weapon.GetComponent<Gun>().GetDamagePerBullet(), slots[slotNum].weapon.GetComponent<Gun>().GetRecoil(), slots[slotNum].weapon.GetComponent<Gun>().GetReloadTime(), slots[slotNum].weapon.GetComponent<Gun>().GetShotDelay(), slots[slotNum].weapon.GetComponent<Gun>().GetMaxAmmoCount(), slots[slotNum].weapon.GetComponent<Gun>().GetText());
            }
            else if(slots[slotNum].weapon.GetComponent<Projectile>() != null)
            {
                ui_slots[slotNum].SetSlot(slots[slotNum].weapon.GetComponent<Projectile>().GetSprite(), slots[slotNum].weapon.GetComponent<Projectile>().GetDamage(), 0, 0, 0, slots[slotNum].weapon.GetComponent<Projectile>().GetMaxHaveNum(), slots[slotNum].weapon.GetComponent<Projectile>().GetText());
            }
        }
        else
        {
            ui_slots[slotNum].ResetSlot();
        }
    }
}

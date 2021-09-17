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
    [SerializeField] Camera cam;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Image[] image_backgrounds;
    [SerializeField] private UI_Slot[] slots;
    Transform temp = null;
    private bool isSwap;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameManager.Instance.GetPlayer().GetInventory();
    }

    private void Update()
    {
        if(!inventory.isOpen)
        {
            for(int i = 0; i < image_backgrounds.Length; i++)
            {
                image_backgrounds[i].enabled = false;
            }

            for(int i = 0; i < slots.Length; i++)
            {
                slots[i].ResetImage();
            }

            if(temp != null && !isSwap)
            {
                isSwap = true;
                inventory.SwapWeapon(inventory.GetWeaponNum(temp.GetComponent<UI_Slot>().gunType));
            }
        }
        else
        {
            isSwap = false;

            for (int i = 0; i < image_backgrounds.Length; i++)
            {
                image_backgrounds[i].enabled = true;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                //slots[i].gameObject.SetActive(true);
                slots[i].UpdateSlot();
            }


            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (temp != null)
                {
                    if (temp != hit.transform)
                    {
                        temp.GetComponent<UI_Slot>().PointerExit();
                    }
                }

                temp = hit.transform;
                hit.transform.GetComponent<UI_Slot>().PointerOver();
            }
            else
            {
                if (temp != null)
                {
                    temp.GetComponent<UI_Slot>().PointerExit();
                }

                temp = null;
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_Slot : MonoBehaviour
{
    public GunType gunType;
    [SerializeField] private Image image_notHave;
    [SerializeField] private Image image_have_off;
    [SerializeField] private Image image_have_on;

    [SerializeField] private bool isHave;
    [SerializeField] private bool isMouseOver;
    [SerializeField] private Text text;
    [SerializeField] private Text text_background;
    private Color gray = new Color(171 / 255f, 171 / 255f, 171 / 255f);

    private Inventory inventory;

    void Start()
    {
        inventory = GameManager.Instance.GetPlayer().GetInventory();
        ResetImage();
    }

    private void Update()
    {
        if (inventory.isOpen)
        {
            if (!text.enabled)
            {
                text.enabled = true;
                text_background.enabled = true;
            }

            if (inventory.GetWeapon(gunType) != null)
            {
                text.text = inventory.GetWeapon(gunType).GetCurrentAmmoCount().ToString() + " / " + inventory.GetWeapon(gunType).GetHaveAmmoCount();
                text_background.text = text.text;
            }
            else
            {
                text.text = null;
                text_background.text = null;
            }
        }
        else
        {
            if (text.enabled)
            {
                text.enabled = false;
                text_background.enabled = false;
                text.color = gray;
            }
        }
    }

    public void ResetImage()
    {
        isMouseOver = false;

        image_notHave.enabled = false;
        image_have_off.enabled = false;
        image_have_on.enabled = false;
    }

    public void UpdateSlot()
    {
        if(inventory.GetWeapon(gunType))
        {
            isHave = true;
        }
        else
        {
            isHave = false;
        }

        if(isMouseOver)
        {
            ResetImage();

            image_have_on.enabled = true;
        }
        else
        {
            ResetImage();

            if (isHave)
                image_have_off.enabled = true;
            else
                image_notHave.enabled = true;
        }
    }

    public void PointerOver()
    {
        if (GameManager.Instance.GetIsPause()) return;

        if (isHave)
            isMouseOver = true;

        text.color = Color.white;
    }

    public void PointerExit()
    {
        if (GameManager.Instance.GetIsPause()) return;

        if (isHave)
            isMouseOver = false;

        text.color = gray;
    }
}

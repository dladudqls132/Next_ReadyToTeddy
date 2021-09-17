using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GunType gunType;
    [SerializeField] private Image image_notHave;
    [SerializeField] private Image image_have_off;
    [SerializeField] private Image image_have_on;

    [SerializeField] private bool isHave;
    [SerializeField] private bool isMouseOver;

    private Inventory inventory;

    void Start()
    {
        inventory = GameManager.Instance.GetPlayer().GetInventory();
        ResetImage();
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
        if(isHave)
            isMouseOver = true;
    }

    public void PointerExit()
    {
        if (isHave)
            isMouseOver = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isHave)
        {
            //isMouseOver = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if (isHave)
        //{
        //    isMouseOver = false;
        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Slot : MonoBehaviour, IDragHandler, IDropHandler, IEndDragHandler
{
    public GameObject tempSlot;
    public int slotNum;
    public Image slot;
    public Sprite sprite;
    public float damage;
    public float shotDelay;
    public int maxAmmo;

    private Canvas canvas;

    // Start is called before the first frame update
    void Init()
    {
        if (slot == null || canvas == null)
        {
            slot = this.transform.GetChild(0).GetComponent<Image>();
            canvas = this.transform.root.GetComponent<Canvas>();
        }
    }

    public void ResetSlot()
    {
        slot.sprite = null;
        this.sprite = null;
        this.damage = 0;
        this.shotDelay = 0;
        this.maxAmmo = 0;

        slot.enabled = false;
    }

    public void SetSlot(Sprite sprite, float damage, float shotDelay, int maxAmmo)
    {
        Init();

        slot.sprite = sprite;
        this.sprite = sprite;
        this.damage = damage;
        this.shotDelay = shotDelay;
        this.maxAmmo = maxAmmo;

        slot.enabled = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.sprite == null)
        {
            return;
        }

        tempSlot.SetActive(true);
        tempSlot.GetComponent<UI_DragDrop>().SetDragInfo(slotNum, sprite);
        tempSlot.GetComponent<RectTransform>().anchoredPosition = eventData.position - canvas.GetComponent<CanvasScaler>().referenceResolution / 2;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(tempSlot.activeSelf)
        {
            GameManager.Instance.GetPlayer().GetInventory().ChangeWeapon(tempSlot.GetComponent<UI_DragDrop>().slotNum, slotNum);
            tempSlot.SetActive(false);
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        tempSlot.SetActive(false);
        if (eventData.pointerCurrentRaycast.gameObject.GetComponent<UI_Slot>() == null && eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<UI_Slot>() == null)
        {
            GameManager.Instance.GetPlayer().GetInventory().DropWeapon(tempSlot.GetComponent<UI_DragDrop>().slotNum);
            tempSlot.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Slot : MonoBehaviour, IDragHandler, IDropHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tempSlot;
    public UI_ItemInfo itemInfo;
    public int slotNum;
    public Image slot;
    public Sprite sprite;
    public float damage;
    public float recoil;
    public float reloadTime;
    public float shotDelay;
    public int maxAmmo;
    public string text;

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

    public void SetSlot(Sprite sprite, float damage, float recoil, float reloadTime, float shotDelay, int maxAmmo, string text)
    {
        Init();

        slot.enabled = true;
        slot.sprite = sprite;
        this.sprite = sprite;
        this.damage = damage;
        this.recoil = recoil;
        this.reloadTime = reloadTime;
        this.shotDelay = shotDelay;
        this.maxAmmo = maxAmmo;
        this.text = text;
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

        if (this.sprite == null)
            return;

        itemInfo.gameObject.SetActive(true);
        itemInfo.GetComponent<RectTransform>().anchoredPosition = eventData.position - canvas.GetComponent<CanvasScaler>().referenceResolution / 2;
        itemInfo.SetItemInfo(sprite, damage, recoil, reloadTime, shotDelay, maxAmmo, text);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        tempSlot.SetActive(false);
  
        if (eventData.pointerCurrentRaycast.gameObject == null)
        {
            GameManager.Instance.GetPlayer().GetInventory().DropWeapon(tempSlot.GetComponent<UI_DragDrop>().slotNum);
            tempSlot.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.sprite == null || tempSlot.activeSelf)
            return;

        itemInfo.gameObject.SetActive(true);
        itemInfo.GetComponent<RectTransform>().anchoredPosition = eventData.position - canvas.GetComponent<CanvasScaler>().referenceResolution / 2;
        itemInfo.SetItemInfo(sprite, damage, recoil, reloadTime, shotDelay, maxAmmo, text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfo.gameObject.SetActive(false);
    }
}

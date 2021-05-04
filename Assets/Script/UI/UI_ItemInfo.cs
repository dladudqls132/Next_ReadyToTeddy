using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemInfo : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text damage;
    [SerializeField] private Text recoil;
    [SerializeField] private Text reloadTime;
    [SerializeField] private Text shotDelay;
    [SerializeField] private Text maxHaveNum;
    [SerializeField] private Text text;

    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;
    [SerializeField] RectTransform canvasRect;


    public void SetItemInfo(Sprite sprite, float damage, float recoil, float reloadTime, float shotDelay, int maxHaveNum, string text)
    {
        this.image.sprite = sprite;
        this.damage.text = "데미지: " + damage.ToString();
        this.recoil.text = "반동: " + recoil.ToString();
        this.reloadTime.text = "재장전 시간: " + reloadTime.ToString();
        this.shotDelay.text = "발사 속도: " + shotDelay.ToString();
        this.maxHaveNum.text = "용량: " + maxHaveNum.ToString();
        this.text.text = text;
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        this.GetComponent<RectTransform>().anchoredPosition = mousePos - this.transform.root.GetComponent<Canvas>().GetComponent<CanvasScaler>().referenceResolution / 2;
    }
}

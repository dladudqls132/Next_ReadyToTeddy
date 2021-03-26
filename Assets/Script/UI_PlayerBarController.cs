using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerBarController : MonoBehaviour
{
    enum BarType
    {
        HpBar,
        ComboBar
    }

    [SerializeField] private BarType barType;
    private PlayerController player;
    private Image image;
    private Image image_comboResetTime;
    private Text text;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();

        if (this.transform.Find("Image") != null)
            image = this.transform.Find("Image").GetComponent<Image>();
        if (this.transform.Find("Image_comboResetTime") != null)
            image_comboResetTime = this.transform.Find("Image_comboResetTime").GetComponent<Image>();
        if (this.transform.Find("Text") != null)
            text = this.transform.Find("Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetPlayer() != null)
        {
            if (GameManager.Instance.GetPlayer() != player)
            {
                player = GameManager.Instance.GetPlayer();
            }
        }

        if (barType == BarType.HpBar)
        {
            text.text = player.GetCurrentHp().ToString();
            image.rectTransform.sizeDelta = Vector2.Lerp(image.rectTransform.sizeDelta, new Vector2(rectTransform.rect.width * (player.GetCurrentHp() / player.GetMaxHp()), image.rectTransform.rect.height), Time.deltaTime * 15);
        }
        else if (barType == BarType.ComboBar)
        {
            text.text = player.GetCurrentCombo().ToString();
            image.rectTransform.sizeDelta = Vector2.Lerp(image.rectTransform.sizeDelta, new Vector2(rectTransform.rect.width * (player.GetCurrentCombo() / player.GetMaxCombo()), image.rectTransform.rect.height), Time.deltaTime * 15);
            image_comboResetTime.rectTransform.sizeDelta = Vector2.Lerp(image_comboResetTime.rectTransform.sizeDelta, new Vector2(rectTransform.rect.width * (player.GetCurrentResetComboTime() / player.GetResetComboTime()), image_comboResetTime.rectTransform.rect.height), Time.deltaTime * 15);
        }
    }
}

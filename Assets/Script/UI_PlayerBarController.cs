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

    [SerializeField] private BarType barType = 0;
    private PlayerController player = null;
    private Image image = null;
    private Image image_comboResetTime = null;
    private Text text = null;
    private RectTransform rectTransform = null;

    private void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();

        if (this.transform.Find("Image_Hp") != null)
            image = this.transform.Find("Image_Hp").GetComponent<Image>();
        if (this.transform.Find("Image_ComboResetTime") != null)
            image_comboResetTime = this.transform.Find("Image_ComboResetTime").GetComponent<Image>();
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
            text.text = player.GetCurrentHp().ToString() + "/ " + player.GetMaxHp().ToString();
            image.rectTransform.sizeDelta = Vector2.Lerp(image.rectTransform.sizeDelta, new Vector2(rectTransform.rect.width * (player.GetCurrentHp() / player.GetMaxHp()), image.rectTransform.rect.height), Time.deltaTime * 15);
        }
        else if (barType == BarType.ComboBar)
        {
            if(player.GetCurrentCombo() < 10)
                text.text = "0" + player.GetCurrentCombo().ToString();
            else
                text.text = player.GetCurrentCombo().ToString();
            //image.rectTransform.sizeDelta = Vector2.Lerp(image.rectTransform.sizeDelta, new Vector2(rectTransform.rect.width * (player.GetCurrentCombo() / player.GetMaxCombo()), image.rectTransform.rect.height), Time.deltaTime * 15);
            if (player.GetCurrentResetComboTime() <= player.GetResetComboTime() / 3)
                image_comboResetTime.color = Color.red;
            else
                image_comboResetTime.color = Color.white;
            image_comboResetTime.rectTransform.sizeDelta = Vector2.Lerp(image_comboResetTime.rectTransform.sizeDelta, new Vector2(rectTransform.rect.width * (player.GetCurrentResetComboTime() / player.GetResetComboTime()), image_comboResetTime.rectTransform.rect.height), Time.deltaTime * 15);
        }
    }
}

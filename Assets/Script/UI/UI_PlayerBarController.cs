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
    [SerializeField] private Image image = null;
    //private Image image_comboResetTime = null;
    [SerializeField] private Text text = null;
    [SerializeField] private Text text_background = null;
    private RectTransform rectTransform = null;
    private float maxWidth;

    private void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();


        //if (this.transform.Find("Image_Hp") != null)
        //    image = this.transform.Find("Image_Hp").GetComponent<Image>();
        //if (this.transform.Find("Image_ComboResetTime") != null)
        //    image = this.transform.Find("Image_ComboResetTime").GetComponent<Image>();
        //if (this.transform.Find("Text") != null)
        //    text = this.transform.Find("Text").GetComponent<Text>();

        maxWidth = image.rectTransform.rect.width;
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
            text.text = ((int)player.GetCurrentHp()).ToString();
            text_background.text = text.text;
            //image.rectTransform.sizeDelta = Vector2.Lerp(image.rectTransform.sizeDelta, new Vector2(maxWidth * (player.GetCurrentHp() / player.GetMaxHp()), image.rectTransform.rect.height), Time.deltaTime * 15);
            image.fillAmount = Mathf.Lerp(image.fillAmount, player.GetCurrentHp() / player.GetMaxHp(), Time.deltaTime * 15);
        }
    }
}

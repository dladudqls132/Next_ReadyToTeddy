using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossBarController : MonoBehaviour
{
    enum BarType
    {
        HpBar,
        SheildBar
    }

    [SerializeField] private BarType barType = 0;
    [SerializeField] private GameObject boss = null;
    [SerializeField] private Image image = null;

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
        if (GameObject.FindGameObjectWithTag("Boss") != null)
        {
            if(boss == null)
            {
                boss = GameObject.FindGameObjectWithTag("Boss");
            }
        }

        if (boss != null)
        {
            if (barType == BarType.HpBar)
            {
                image.fillAmount = Mathf.Lerp(image.fillAmount, boss.GetComponent<Enemy>().GetCurrentHp() / boss.GetComponent<Enemy>().GetMaxHp(), Time.deltaTime * 15);
            }
        }
    }
}

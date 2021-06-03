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
        if (GameObject.FindGameObjectWithTag("Boss") != null)
        {
            if (boss == null)
            {
                boss = GameObject.FindGameObjectWithTag("Boss");

                for (int i = 0; i < this.transform.childCount; i++)
                {
                    this.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

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
       

        if (boss != null && boss.GetComponent<Enemy>().enabled)
        {
            if (barType == BarType.HpBar)
            {
                if (!this.transform.GetChild(0).gameObject.activeSelf)
                {
                    for (int i = 0; i < this.transform.childCount; i++)
                    {
                        this.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }

                image.fillAmount = Mathf.Lerp(image.fillAmount, boss.GetComponent<Enemy>().GetCurrentHp() / boss.GetComponent<Enemy>().GetMaxHp(), Time.deltaTime * 15);
            }
            else if(barType == BarType.SheildBar)
            {
                if (boss.GetComponent<Boss_Teddy>().GetCurrentBehavior() == BossBehavior.Shield)
                {
                    if (!this.transform.GetChild(0).gameObject.activeSelf)
                    {
                        for (int i = 0; i < this.transform.childCount; i++)
                        {
                            this.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }

                    image.fillAmount = Mathf.Lerp(image.fillAmount, boss.GetComponent<Enemy>().GetEnergyShield().GetComponent<Boss_EnergyShield>().GetCurrentShieldHp() / boss.GetComponent<Enemy>().GetEnergyShield().GetComponent<Boss_EnergyShield>().GetMaxShieldHp(), Time.deltaTime * 15);
                }
                else
                {
                    for (int i = 0; i < this.transform.childCount; i++)
                    {
                        this.transform.GetChild(i).gameObject.SetActive(false);
                    }


                }
            }
        }
        else
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Boss_HpBar : MonoBehaviour
{
    [SerializeField] private Enemy boss;
    [SerializeField] private Image gauge;

    // Update is called once per frame
    void Update()
    {
        if (boss.GetIsDetect())
        {
            if (!this.transform.GetChild(0).gameObject.activeSelf)
            {
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    this.transform.GetChild(i).gameObject.SetActive(true);
                }
            }

            gauge.fillAmount = boss.GetCurrentHp() / boss.GetMaxHp();
        }
    }
}

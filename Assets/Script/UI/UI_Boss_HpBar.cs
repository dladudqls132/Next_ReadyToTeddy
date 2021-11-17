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
        gauge.fillAmount = boss.GetCurrentHp() / boss.GetMaxHp();
    }
}

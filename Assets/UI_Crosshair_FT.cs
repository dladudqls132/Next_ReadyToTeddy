using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crosshair_FT : MonoBehaviour
{
    [SerializeField] private Image gauge;
    private float currentGauge;

    private void Update()
    {
        gauge.fillAmount = Mathf.Lerp(gauge.fillAmount, currentGauge, Time.deltaTime * 15);
    }

    public void SetGauge(float value)
    {
        currentGauge = value;

        if (!this.gameObject.activeSelf)
            gauge.fillAmount = value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crosshair_CL : MonoBehaviour
{
    [SerializeField] private Image[] image_charge;

    [SerializeField] private Color[] originColor;
    [SerializeField] private bool isCharge;
    private int chargeNum;
    private bool isStart;

    private void OnEnable()
    {
        if (!isStart)
        {
            for (int i = 0; i < image_charge.Length; i++)
            {
                originColor[i] = image_charge[i].color;
                image_charge[i].color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0);
            }
        }

        isStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isCharge)
        {
            for(int i = 0; i < chargeNum; i++)
            {
                image_charge[i].color = Color.Lerp(image_charge[i].color, originColor[i], Time.deltaTime * 6);
            }
        }
        else
        {
            for (int i = 0; i < image_charge.Length; i++)
            {
                image_charge[i].color = Color.Lerp(image_charge[i].color, new Color(255 / 255f, 255 / 255f, 255 / 255f, 0), Time.deltaTime * 10);
            }
        }
    }

    public void SetCharging(int chargeNum)
    {
        if(chargeNum == 0)
        {
            isCharge = false;
            this.chargeNum = chargeNum;
        }
        else
        {
            isCharge = true;
            this.chargeNum = chargeNum;
        }
    }
}

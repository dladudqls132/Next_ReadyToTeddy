using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageClearTime : MonoBehaviour
{
    [SerializeField] private Stage currentStage;
    [SerializeField] private Text remaningTime;

    // Update is called once per frame
    void Update()
    {
        if(currentStage == null)
        {
            currentStage = GameManager.Instance.GetPlayer().GetCurrentStage();
            remaningTime.enabled = false;
        }
        else
        {
            if (currentStage.GetIsStart())
            {
                if (!currentStage.GetIsClear())
                {
                    remaningTime.enabled = true;
                    remaningTime.text = "Remaining Time: " + ((int)currentStage.GetCurrentClearTime()).ToString();
                }
                else
                {
                    remaningTime.enabled = false;
                }
            }
            else
            {
                remaningTime.enabled = false;
            }
        }
    }
}

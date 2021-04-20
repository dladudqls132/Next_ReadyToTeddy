using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageSuccessRate : MonoBehaviour
{
    [SerializeField] Stage currentStage = null;
    [SerializeField] private Image image_gauge;
    [SerializeField] private Image image_backGround;
    [SerializeField] private float maxGaugeWidth = 500;

    // Update is called once per frame
    void Update()
    {
        if (currentStage == null)
        {
            currentStage = GameManager.Instance.GetPlayer().GetCurrentStage();
            image_gauge.enabled = false;
            image_backGround.enabled = false;
        }
        else
        {
            if(currentStage.GetIsStart())
            {
                if (!currentStage.GetIsClear())
                {
                    image_gauge.enabled = true;
                    image_backGround.enabled = true;

                    if(currentStage.GetBoss() != null)
                        image_gauge.rectTransform.sizeDelta = Vector2.Lerp(image_gauge.rectTransform.sizeDelta, new Vector2(maxGaugeWidth * (currentStage.GetBoss().GetCurrentHP() / currentStage.GetBoss().GetMaxHp()), image_gauge.rectTransform.sizeDelta.y), Time.deltaTime * 20);
                }
            }
            else
            {
                image_gauge.enabled = false;
                image_backGround.enabled = false;

                image_gauge.rectTransform.sizeDelta = new Vector2(0, image_gauge.rectTransform.sizeDelta.y);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_Pulse : MonoBehaviour
{
    [SerializeField] private int activePhase;
    [SerializeField] private GameObject pulse;
    [SerializeField] private float damage;
    [SerializeField] private float delay;
    [SerializeField] private GameObject[] lightning;

    private void Update()
    {
        if (this.GetComponent<Boss_TypeX>().GetCurrentPhase() == 5)
        {
            pulse.SetActive(false);
            return;
        }

        if (this.GetComponent<Boss_TypeX>().GetCurrentPhase() >= activePhase)
        {
            if (!this.GetComponent<Boss_TypeX_Skill_RandomShot>().enabled)
            {
                if (!pulse.activeSelf)
                {
                    pulse.GetComponent<Boss_TypeX_Pulse>().SetActiveTrue(damage, delay);
                    for(int i = 0; i < lightning.Length; i++)
                    {
                        lightning[i].SetActive(true);
                    }
                }
                else
                {
                    if(this.GetComponent<Boss_TypeX>().GetCurrentPhase() == 4)
                    {
                        pulse.GetComponent<Boss_TypeX_Pulse>().SetDelay(0);
                    }
                }
            }
            else
            {
                if (pulse.activeSelf)
                    pulse.GetComponent<Boss_TypeX_Pulse>().SetActiveFalse();
            }
        }
    }
}

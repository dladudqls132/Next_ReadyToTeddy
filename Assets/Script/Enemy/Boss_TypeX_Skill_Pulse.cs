using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_Pulse : MonoBehaviour
{
    [SerializeField] private int activePhase;
    [SerializeField] private GameObject pulse;
    [SerializeField] private float damage;
    [SerializeField] private float delay;

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
                    pulse.GetComponent<Boss_TypeX_Pulse>().SetActiveTrue(damage, delay);
            }
            else
            {
                if (pulse.activeSelf)
                    pulse.GetComponent<Boss_TypeX_Pulse>().SetActiveFalse();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GunSound_FT : UI_GunSound
{
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < infos_attack.Length; i++)
        {
            if (infos_attack[i].image.gameObject.activeSelf)
            {
                if (infos_attack[i].time >= aliveTime)
                {
                    infos_attack[i].image.gameObject.SetActive(false);
                    break;
                }

                infos_attack[i].time += Time.deltaTime;
            }
        }

        for (int i = 0; i < infos_reload.Length; i++)
        {
            if (infos_reload[i].image.gameObject.activeSelf)
            {
                if (infos_reload[i].time >= aliveTime)
                {
                    infos_reload[i].image.gameObject.SetActive(false);
                    break;
                }

                infos_reload[i].time += Time.deltaTime;
            }
        }
    }

    public override void DisplayImage_Attack()
    {
        num = num % infos_attack.Length;
        infos_attack[num].time = 0;
        infos_attack[num].image.gameObject.SetActive(true);

        num++;
    }

    public override void DisplayImage_Reload()
    {
        num = num % infos_reload.Length;

        infos_reload[num].time = 0;
        infos_reload[num].image.gameObject.SetActive(true);

        num++;
    }
}

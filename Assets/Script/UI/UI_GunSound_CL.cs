using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GunSound_CL : UI_GunSound
{
  

    [SerializeField] protected UI_GunSoundInfo[] infos_charge1;
    [SerializeField] protected UI_GunSoundInfo[] infos_charge2;


    // Update is called once per frame
    void Update()
    {
       
            for (int i = 0; i < infos_charge1.Length; i++)
            {
                if (infos_charge1[i].image.gameObject.activeSelf)
                {
                    if (infos_charge1[i].time >= aliveTime)
                    {
                        infos_charge1[i].image.gameObject.SetActive(false);
                        break;
                    }

                    infos_charge1[i].time += Time.deltaTime;
                }
            }
        
            for (int i = 0; i < infos_charge2.Length; i++)
            {
                if (infos_charge2[i].image.gameObject.activeSelf)
                {
                    if (infos_charge2[i].time >= aliveTime)
                    {
                        infos_charge2[i].image.gameObject.SetActive(false);
                        break;
                    }

                    infos_charge2[i].time += Time.deltaTime;
                }
            }
        
       
            for (int i = 0; i < infos_attack.Length; i++)
            {
                if (infos_attack[i].image.gameObject.activeSelf)
                {
                    if (infos_attack[i].time >= aliveTime * 2)
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
                if (infos_reload[i].time >= aliveTime * 6)
                {
                    infos_reload[i].image.gameObject.SetActive(false);
                    break;
                }

                infos_reload[i].time += Time.deltaTime;
            }
        }

    }

    public override void DisplayImage_Charge1()
    {
        num = num % infos_charge1.Length;
        infos_charge1[num].time = 0;
        infos_charge1[num].image.gameObject.SetActive(true);

        num++;
    }

    public override void DisplayImage_Charge2()
    {
        num = num % infos_charge2.Length;
        infos_charge2[num].time = 0;
        infos_charge2[num].image.gameObject.SetActive(true);

        num++;
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

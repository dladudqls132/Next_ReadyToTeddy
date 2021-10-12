using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GunSound : MonoBehaviour
{
    [System.Serializable]
    protected struct UI_GunSoundInfo
    {
        public Image image;
        public float time;
    }

    [SerializeField] protected float aliveTime;
    [SerializeField] protected UI_GunSoundInfo[] infos_attack;
    [SerializeField] protected UI_GunSoundInfo[] infos_reload;
    protected int num;

    public virtual void DisplayImage_Attack()
    {

    }

    public virtual void DisplayImage_Charge1()
    {

    }

    public virtual void DisplayImage_Charge2()
    {

    }

    public virtual void DisplayImage_Reload()
    {

    }
}

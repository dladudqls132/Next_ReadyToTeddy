using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GunSoundManager : MonoBehaviour
{
    [SerializeField] private UI_GunSound AR;
    [SerializeField] private UI_GunSound SG;
    [SerializeField] private UI_GunSound CL;
    [SerializeField] private UI_GunSound FT;

    public void Init()
    {

    }

    public UI_GunSound GetGunSound(GunType gunType)
    {
        switch (gunType)
        {
            case GunType.AR:
                return AR;
            case GunType.ShotGun:
                return SG;
            case GunType.ChainLightning:
                return CL;
            case GunType.Flamethrower:
                return FT;
        }

        return null;
    }

}

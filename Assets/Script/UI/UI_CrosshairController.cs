using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CrosshairController : MonoBehaviour
{
    [SerializeField] private GameObject AR;
    [SerializeField] private GameObject SG;
    [SerializeField] private GameObject CL;
    [SerializeField] private GameObject FT;

    [SerializeField] private Image image_attack_kill;
    [SerializeField] private Image image_attack_normal;
    [SerializeField] private Color originColor_attack_kill;
    [SerializeField] private Color originColor_attack_normal;
    [SerializeField] private float attackImageTime;

    private float currentAttackImageTime;
    private bool isAttack;
    private bool isKill;

    public bool GetIsKill() { return isKill; }

    public void ResetAttack()
    {
        SetAttack_Kill(false);
        SetAttack_Normal(false);
    }

    public void SetAttack_Kill(bool value)
    {
        currentAttackImageTime = attackImageTime;
        image_attack_kill.color = originColor_attack_kill;
        image_attack_kill.enabled = value;
        isAttack = value;
        isKill = value;
    }

    public void SetAttack_Normal(bool value)
    {
        currentAttackImageTime = attackImageTime;
        image_attack_normal.color = originColor_attack_normal;
        image_attack_normal.enabled = value;
        isAttack = value;
    }

    public void Init()
    {
        currentAttackImageTime = attackImageTime;
        originColor_attack_kill = image_attack_kill.color;
        originColor_attack_normal = image_attack_normal.color;
    }

    private void Update()
    {
        if (isAttack)
        {
            currentAttackImageTime -= Time.deltaTime;

            if (currentAttackImageTime <= 0)
            {
                isAttack = false;
                isKill = false;
                currentAttackImageTime = attackImageTime;
            }
        }
        else
        {
            image_attack_normal.color = Color.Lerp(image_attack_normal.color, new Color(255 / 255f, 255 / 255f, 255 / 255f, 0f), Time.deltaTime * 15);
            image_attack_kill.color = Color.Lerp(image_attack_kill.color, new Color(255 / 255f, 0 / 255f, 0 / 255f, 0 / 255f), Time.deltaTime * 15);
        }
    }

    void ResetCrosshair()
    {
        AR.SetActive(false);
        SG.SetActive(false);
        CL.SetActive(false);
        FT.SetActive(false);
    }

    public void SetCrosshair(GunType gunType)
    {
        ResetCrosshair();

        switch (gunType)
        {
            case GunType.AR:
                AR.SetActive(true);
                break;
            case GunType.ShotGun:
                SG.SetActive(true);
                break;
            case GunType.ChainLightning:
                CL.SetActive(true);
                break;
            case GunType.Flamethrower:
                FT.SetActive(true);
                break;
        }
    }

    public GameObject GetCrosshair(GunType gunType)
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

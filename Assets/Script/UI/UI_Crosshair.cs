using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crosshair : MonoBehaviour
{
    [System.Serializable]
    struct AimInfo
    {
        public Image image;
        [HideInInspector] public Vector3 originPos;
        public Vector3 destPos_small;
        public Vector3 destPos_large;
        [HideInInspector] public RectTransform rectTransform;
    }

    private PlayerController player;
    [SerializeField] private Image[] images;
    [SerializeField] private AimInfo[] aimInfos;
    [SerializeField] private Image image_attack_kill;
    [SerializeField] private Image image_attack_normal;
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
        image_attack_kill.enabled = value;
        isAttack = value;
        isKill = value;
    }

    public void SetAttack_Normal(bool value)
    {
        currentAttackImageTime = attackImageTime;
        image_attack_normal.enabled = value;
        isAttack = value;
    }

    public void Init()
    {
        player = GameManager.Instance.GetPlayer();
        images = this.GetComponentsInChildren<Image>();

        for(int i = 0; i < aimInfos.Length; i++)
        {
            aimInfos[i].rectTransform = aimInfos[i].image.rectTransform;
            aimInfos[i].originPos = aimInfos[i].rectTransform.localPosition;
        }

        currentAttackImageTime = attackImageTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttack)
        {
            currentAttackImageTime -= Time.deltaTime;

            if(currentAttackImageTime <= 0)
            {
                isAttack = false;
                SetAttack_Kill(false);
                SetAttack_Normal(false);
            }
        }

        if(player.GetIsAiming())
        {
            foreach (Image child in images)
            {
                if (!child.enabled || child == image_attack_kill || child == image_attack_normal)
                    break;
                child.enabled = false;
            }
        }
        else
        {
            foreach (Image child in images)
            {
                if (child.enabled || child == image_attack_kill || child == image_attack_normal)
                    break;
                child.enabled = true;
            }
        }

        if (!player.GetIsGrounded() || player.GetIsRun() || player.GetIsSlide() || player.GetIsDash() || player.GetIsClimbing() || player.GetIsClimbUp())
        {
            for(int i = 0; i < aimInfos.Length; i++)
            {
                if (Vector3.Magnitude(aimInfos[i].destPos_large - aimInfos[i].rectTransform.localPosition) > 1.0f)
                {
                    aimInfos[i].rectTransform.localPosition = Vector3.Lerp(aimInfos[i].rectTransform.localPosition, aimInfos[i].destPos_large, Time.deltaTime * 12);
                }
                else
                {
                    aimInfos[i].rectTransform.localPosition = aimInfos[i].destPos_large;
                }
            }
        }
        else if (player.GetIsCrouch())
        {
            for (int i = 0; i < aimInfos.Length; i++)
            {
                if (Vector3.Magnitude(aimInfos[i].destPos_small - aimInfos[i].rectTransform.localPosition) > 1.0f)
                {
                    aimInfos[i].rectTransform.localPosition = Vector3.Lerp(aimInfos[i].rectTransform.localPosition, aimInfos[i].destPos_small, Time.deltaTime * 12);
                }
                else
                {
                    aimInfos[i].rectTransform.localPosition = aimInfos[i].destPos_small;
                }
            }

        }
        else
        {
            ResetAim();
        }
    }

    private void ResetAim()
    {
        for (int i = 0; i < aimInfos.Length; i++)
        {
            if (Vector3.Magnitude(aimInfos[i].originPos - aimInfos[i].rectTransform.localPosition) > 1.0f)
            {
                aimInfos[i].rectTransform.localPosition = Vector3.Lerp(aimInfos[i].rectTransform.localPosition, aimInfos[i].originPos, Time.deltaTime * 12);
            }
            else
            {
                aimInfos[i].rectTransform.localPosition = aimInfos[i].originPos;
            }
        }
    }
}

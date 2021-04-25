using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimController : MonoBehaviour
{
    [SerializeField] private Gun currentWeapon;
    [SerializeField] private PlayerController player;

    private void Update()
    {
        currentWeapon = GameManager.Instance.GetPlayer().GetWeapon();
        player = GameManager.Instance.GetPlayer();
    }

    public void SetIsReloadFinish()
    {
        currentWeapon.SetIsReloadFinish();
    }

    public void SetSwapGun()
    {
        player.SwapWeapon();
    }

    public void SetIsSwapFinish()
    {
        this.GetComponent<Animator>().SetBool("isSwap", false);
        player.SetIsSwap(false);
    }
}

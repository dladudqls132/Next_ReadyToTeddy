using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimController : MonoBehaviour
{
    [SerializeField] private Gun currentWeapon;

    private void Update()
    {
        currentWeapon = GameManager.Instance.GetPlayer().GetWeapon();
    }

    public void SetIsReloadFinish()
    {
        currentWeapon.SetIsReloadFinish();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Potion : Item
{
    enum PotionType
    {
        Hp
    }

    [SerializeField] private PotionType potionType;
    [SerializeField] private float increaseValue;

    private void FixedUpdate()
    {
        //rigid.position = Vector3.Lerp(rigid.position, player.GetAimPos().position, Time.deltaTime * 12);
        rigid.velocity = (player.GetAimPos().position - rigid.position).normalized * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoveSpeed();
        //this.transform.position = Vector3.Lerp(this.transform.position, player.GetAimPos().position, Time.deltaTime * 8);

        if (Vector3.Distance(this.transform.position, player.GetAimPos().position) < 0.2f)
        {
            if (potionType == PotionType.Hp)
            {
                player.IncreaseHp(increaseValue);
                Destroy(this.gameObject);
            }
        }
    }
}

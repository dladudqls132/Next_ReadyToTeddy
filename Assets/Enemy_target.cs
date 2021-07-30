using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_target : Enemy
{


    // Start is called before the first frame update
    protected override void Start()
    {
        if (FindObjectOfType<Pool_DamagedEffect>() != null)
            pool_damagedEffect = FindObjectOfType<Pool_DamagedEffect>();

        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SetDead(bool value)
    {
        isDead = value;

        if (isDead)
        {
            Destroy(this.gameObject);
        }
    }
}

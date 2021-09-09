using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_target : Enemy
{


    // Start is called before the first frame update
    protected override void Start()
    {
        currentHp = maxHp;
        anim = this.GetComponent<Animator>();
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

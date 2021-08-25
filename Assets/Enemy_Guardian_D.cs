using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Guardian_D : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (FindObjectOfType<Pool_DamagedEffect>() != null)
            pool_damagedEffect = FindObjectOfType<Pool_DamagedEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Type_A : Enemy
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
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(new Vector3(target.position.x, this.transform.position.y, target.position.z) - this.transform.position), Time.deltaTime *10);
    }

    public override void SetDead(bool value)
    {
        isDead = value;

        if (isDead)
        {
            if (effect_explosion != null)
            {
                effect_explosion.SetActive(true);
                effect_explosion.transform.position = this.transform.position;
                effect_explosion.GetComponent<ParticleSystem>().Play();

                if (Vector3.Distance(this.transform.position, target.position) <= attackRange)
                    GameManager.Instance.GetPlayer().DecreaseHp(damage);
            }

            this.gameObject.SetActive(false);
        }
    }
}

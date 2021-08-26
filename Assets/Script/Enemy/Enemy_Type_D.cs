using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Type_D : Enemy
{
    [SerializeField] private GameObject effect_prefab;
    private GameObject effect;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (FindObjectOfType<Pool_DamagedEffect>() != null)
            pool_damagedEffect = FindObjectOfType<Pool_DamagedEffect>();

        if (effect_prefab != null)
        {
            effect = Instantiate(effect_prefab);
            effect.GetComponent<ParticleSystem>().Play();
            effect.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);

        if(Vector3.Distance(this.transform.position, target.position) <= attackRange)
        {
            SetDead(true);
        }
    }

    public override void SetDead(bool value)
    {
        isDead = value;

        if (isDead)
        {
            isDead = false;

            if (effect != null)
            {
                effect.SetActive(true);
                effect.transform.position = this.transform.position;
                effect.GetComponent<ParticleSystem>().Play();

                if (Vector3.Distance(this.transform.position, target.position) <= attackRange)
                    GameManager.Instance.GetPlayer().DecreaseHp(damage);
            }

            this.gameObject.SetActive(false);
        }
    }
}

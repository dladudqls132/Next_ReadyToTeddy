using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Type_D : Enemy
{
    [SerializeField] private float attackReadyRange;
    [SerializeField] private GameObject effect_prefab;
    [SerializeField] private Color emissionColor_normal;
    [SerializeField] private Color emissionColor_angry;
    private GameObject effect;
    //private List<Renderer> renderers = new List<Renderer>();
    private Renderer[] renderers;
    private bool isAngry;
    [SerializeField] private float attackTimer;
    private float currentAttackTimer;

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


        renderers = this.GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);

        if (Vector3.Distance(this.transform.position, target.position) <= attackReadyRange)
        {
            if (!isAngry)
            {
                foreach (Renderer r in renderers)
                {
                    r.material.SetColor("_EmissionColor", emissionColor_angry * 35f);
                }
            }

            isAngry = true;
        }

        if(isAngry)
        {
            currentAttackTimer += Time.deltaTime;

            if(currentAttackTimer >= attackTimer)
            {
                SetDead(true);
            }
        }
    }

    public override void SetDead(bool value)
    {
        isDead = value;

        if (isDead)
        {
            if (effect != null)
            {
                effect.SetActive(true);
                effect.transform.position = this.transform.position;
                effect.GetComponent<ParticleSystem>().Play();

                if (Vector3.Distance(this.transform.position, target.position) <= attackRange)
                    GameManager.Instance.GetPlayer().DecreaseHp(damage);
            }

            foreach (Renderer r in renderers)
            {
                r.material.SetColor("_EmissionColor", emissionColor_normal * 35f);
            }

            isDead = false;
            isAngry = false;
            currentAttackTimer = 0;

            this.gameObject.SetActive(false);
        }
    }
}

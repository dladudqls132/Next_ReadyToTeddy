using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Type_A : Enemy
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
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(new Vector3(target.position.x, this.transform.position.y, target.position.z) - this.transform.position), Time.deltaTime *10);
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

            this.gameObject.SetActive(false);
        }
    }
}

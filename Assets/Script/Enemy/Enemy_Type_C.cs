using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Type_C : Enemy
{
    [SerializeField] private GameObject effect_prefab;
    private GameObject effect;
    [SerializeField] private float coolTime_dodge;
    private float currentCoolTime_dodge;
    [SerializeField] private Transform mesh;

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

        foreach (Renderer r in renderers)
        {
            r.material.SetColor("_EmissionColor", emissionColor_angry * 35f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentCoolTime_dodge += Time.deltaTime;

        if(currentCoolTime_dodge >= coolTime_dodge)
        {
            int rndNum = Random.Range(0, 2);

            if(rndNum == 0)
            {
                anim.SetTrigger("Dodge_Left");
            }
            else
            {
                anim.SetTrigger("Dodge_Right");
            }

            currentCoolTime_dodge = 0;
        }

        temp = (target.position - mesh.position).normalized;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(target.position - new Vector3(this.transform.position.x, target.position.y, this.transform.position.z)), Time.deltaTime * 10);
        //mesh.localRotation = Quaternion.Euler(mesh.localRotation.eulerAngles + Quaternion.Euler(Quaternion.LookRotation(temp).eulerAngles.x, 0, 0).eulerAngles);
    }

    Vector3 temp;


    private void LateUpdate()
    {
        mesh.localRotation = Quaternion.Euler(mesh.localRotation.eulerAngles + Quaternion.Euler(Quaternion.LookRotation(temp).eulerAngles.x, 0, 0).eulerAngles);
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

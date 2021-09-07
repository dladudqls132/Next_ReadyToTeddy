using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Type_D : Enemy
{
    [SerializeField] private float attackReadyRange;

    //private List<Renderer> renderers = new List<Renderer>();
    private bool isAngry;
    [SerializeField] private float attackTimer;
    private float currentAttackTimer;
    private Vector3 targetOffset;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        targetOffset = new Vector3(Random.Range(0.0f, 0.5f), 0, Random.Range(0.0f, 0.5f));

    }

    // Update is called once per frame
    void Update()
    {
        CheckingPlayer();

        if (!canSee) return;

        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, target.position + targetOffset, NavMesh.AllAreas, path))
        {
            bool isvalid = true;
            if (path.status != NavMeshPathStatus.PathComplete) isvalid = false;
            if (isvalid)
            {
                agent.SetDestination(target.position + targetOffset);
            }

        }
        else
        {
            agent.SetDestination(target.position);
        }

        //agent.SetDestination(target.position + targetOffset);

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
        else
        {
            currentAttackTimer = 0;

            if(isAngry)
            {
                foreach (Renderer r in renderers)
                {
                    r.material.SetColor("_EmissionColor", emissionColor_normal * 35f);
                }
            }

            isAngry = false;
        }

        if(isAngry)
        {
            currentAttackTimer += Time.deltaTime;

            if(currentAttackTimer >= attackTimer)
            {
                SetDead(true);
            }
        }
        //Debug.Log(agent.velocity.magnitude);
        //if(agent.velocity.magnitude > 4.0f)
        //{
        //    anim.SetBool("isRoll", true);
        //}
        //else
        //{
        //    anim.SetBool("isRoll", false);
        //}

        anim.SetFloat("rollSpeed", agent.velocity.magnitude / 5.5f);
    }

    public override void SetDead(bool value)
    {
        isDead = value;

        if (isDead)
        {
            //if (effect_explosion != null)
            //{
            //    effect_explosion.SetActive(true);
            //    effect_explosion.transform.position = this.transform.position;
            //    effect_explosion.GetComponent<ParticleSystem>().Play();

            //    if (Vector3.Distance(this.transform.position, target.position) <= attackRange)
            //        GameManager.Instance.GetPlayer().DecreaseHp(damage);
            //}

            GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Explosion_bomb);
            temp.transform.position = this.transform.position;
            temp.SetActive(true);

            if (Vector3.Distance(this.transform.position, target.position) <= attackRange)
                GameManager.Instance.GetPlayer().DecreaseHp(damage);

            foreach (Renderer r in renderers)
            {
                r.material.SetColor("_EmissionColor", emissionColor_normal * 35f);
            }

            isAngry = false;
            currentAttackTimer = 0;

            this.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Type_D : Enemy
{
    [SerializeField] private float attackReadyRange;

    //private List<Renderer> renderers = new List<Renderer>();
    [SerializeField] private bool isAngry;
    [SerializeField] private float attackTimer;
    private float currentAttackTimer;
    private Vector3 targetOffset;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        targetOffset = new Vector3(Random.Range(0.0f, 0.5f), 0, Random.Range(0.0f, 0.5f));

        GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Enemy_roll, this.transform, true);

        StartCoroutine(PlayTimer());
    }

    // Update is called once per frame
    void Update()
    {
        CheckingPlayer();

        if (isRigidity)
        {
            currentRigidityTime += Time.deltaTime;
            if (currentRigidityTime >= rigidityTime)
            {
                isRigidity = false;
                currentRigidityTime = 0;
                agent.isStopped = false;
            }
        }

        if (!isDetect || isDead || isRigidity)
        {

            agent.SetDestination(this.transform.position);
            return;
        }

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

        if (Vector3.Distance(this.transform.position, target.position) <= attackReadyRange)
        {
            isAngry = true;
        }
        else
        {
            currentAttackTimer = 0;

            isAngry = false;
        }

        if (isAngry)
        {
            currentAttackTimer += Time.deltaTime;

            if (currentAttackTimer >= attackTimer)
            {
                SetDead(true);
            }

            if (!isDead)
            {
                foreach (Renderer r in renderers)
                {
                    r.material.SetColor("_EmissionColor", Color.Lerp(r.material.GetColor("_EmissionColor"), (emissionColor_angry * 35f) * (currentAttackTimer / attackTimer), Time.deltaTime * 4));
                }
            }
        }
        else
        {
            if (!isDead)
            {
                foreach (Renderer r in renderers)
                {
                    r.material.SetColor("_EmissionColor", Color.Lerp(r.material.GetColor("_EmissionColor"), (emissionColor_normal * 35f), Time.deltaTime * 6));
                }
            }
        }

        anim.SetFloat("rollSpeed", agent.velocity.magnitude / 5.5f);
    }

    IEnumerator PlayTimer()
    {
        while (true)
        {
            float temp = 0.43f - currentAttackTimer / 2 / attackTimer;

            temp = Mathf.Clamp(temp, 0.1f, 0.43f);

            yield return new WaitForSeconds(temp);

            if(isAngry)
            {
                GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Timer, this.transform.position, false);
            }
        }
    }

    public override void SetDead(bool value)
    {
        isDead = value;

        if (isDead)
        {
            GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Explosion_bomb_small);
            temp.transform.position = this.transform.position;
            temp.SetActive(true);

            if (Vector3.Distance(this.transform.position, target.position) <= attackRange)
                GameManager.Instance.GetPlayer().DecreaseHp(damage);

            isAngry = false;
            currentAttackTimer = 0;

            if (currentHp > 0)
                GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion, this.transform.position, false);
            else
                GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion_1, this.transform.position, false);

            this.gameObject.SetActive(false);
        }
    }
}

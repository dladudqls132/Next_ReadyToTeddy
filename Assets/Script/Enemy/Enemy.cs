using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Enemy_State
{
    None,
    Patrol,
    Search,
    Targeting,
    Chase,
    RunAway,
    Return
}

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyType enemyType;
    [SerializeField] protected Enemy_State state;
    [SerializeField] protected CharacterMaterial material;
    [SerializeField] protected Transform eye;
    [SerializeField] protected Transform target;
    [SerializeField] protected GameObject spreadBlood;
    [SerializeField] protected bool canSee;
    [SerializeField] protected bool isDead;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    protected float increaseHp;
    [SerializeField] protected float damage;
    [SerializeField] protected float speed_min;
    [SerializeField] protected float speed_max;
    [SerializeField] protected float speed;
    [SerializeField] protected float increaseCombo;
    protected Pool_DamagedEffect pool_damagedEffect;
    [SerializeField] protected float detectRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float combatTime;
    protected float currentCombatTime;
    protected float returnToPatorlTime;
    protected float currentReturnToPatrolTime;
    protected Transform[] patrolNode;
    protected Transform currentDestPatrolNode;
    protected int currentDestPatrolNodeIndex;
    protected bool isRunAway;
    [SerializeField] protected float increaseSuccessRate;
    [SerializeField] protected float potionDropRate;
    [SerializeField] protected float magazineDropRate;

    private GameObject whoAttackThis;
    protected Animator anim;
    protected float originAttackRange;
    protected NavMeshAgent agent;

    ParticleSystem.Burst[] bursts;

    public float GetCurrentHP() { return currentHp; }
    public void SetCurrentHP(float value) { currentHp = value; }
    public float GetMaxHp() { return maxHp; }
    public void SetIsDead(bool value) { isDead = value; }
    public bool GetIsDead() { return isDead; }

    public void SetInfo(EnemyType enemyType, CharacterMaterial material, float hp, float speed_min, float speed_max, float detectRange, float attackRange)
    {
        this.enemyType = enemyType;
        this.material = material;
        this.maxHp = hp;
        this.speed_min = speed_min;
        this.speed_max = speed_max;
        this.detectRange = detectRange;
        this.attackRange = attackRange;
    }

    virtual protected void Start()
    {
        currentHp = maxHp;

        if(FindObjectOfType<Pool_DamagedEffect>() != null)
            pool_damagedEffect = FindObjectOfType<Pool_DamagedEffect>();

        bursts = new[] { new ParticleSystem.Burst(0.0f, increaseCombo) };
        originAttackRange = attackRange;

        currentCombatTime = combatTime;
        currentReturnToPatrolTime = returnToPatorlTime;

        target = GameManager.Instance.GetPlayer().transform;

        speed = Random.Range(speed_min, speed_max);

        if (this.GetComponent<NavMeshAgent>() != null)
            agent = this.GetComponent<NavMeshAgent>();

        //if (patrolNode.Length != 0)
        //{
        //    currentDestPatrolNode = patrolNode[0];
        //    currentDestPatrolNodeIndex = 0;
        //}
    }

    protected virtual void SetDead(bool value) {}

    public void SetRagdoll(Transform damagedTrs, Vector3 damagedVelocity)
    {
        //GameObject temp = GameManager.Instance.GetPoolRagdoll().GetEnemyRagdoll(enemyType);

        //temp.SetActive(true);
        //temp.transform.position = this.transform.position;
        //temp.transform.rotation = this.transform.rotation;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        if(this.GetComponent<Enemy_RagdollController>() != null)
            this.GetComponent<Enemy_RagdollController>().AddForce(damagedTrs, damagedVelocity);
        anim.enabled = false;
    }

    protected void GoToPatrolNode()
    {
        if (patrolNode.Length != 0)
        {
            if (state == Enemy_State.Search)
            {
                currentDestPatrolNode = patrolNode[0];
                currentDestPatrolNodeIndex = 0;

                for (int i = 0; i < patrolNode.Length; i++)
                {
                    if (Vector3.Distance(this.transform.position, patrolNode[i].position) < Vector3.Distance(this.transform.position, currentDestPatrolNode.position))
                    {
                        currentDestPatrolNode = patrolNode[i];
                        currentDestPatrolNodeIndex = i;
                    }
                }
            }

            if (state == Enemy_State.Patrol)
            {
                if (Vector3.Distance(this.transform.position, currentDestPatrolNode.position) < 1.0f)
                {
                    currentDestPatrolNodeIndex++;
                    currentDestPatrolNodeIndex = currentDestPatrolNodeIndex % patrolNode.Length;

                    currentDestPatrolNode = patrolNode[currentDestPatrolNodeIndex];
                }

                agent.SetDestination(currentDestPatrolNode.position);
            }
        }
    }

    protected void CheckingHp(Transform damagedTrs, Vector3 damagedVelocity)
    {
        if (!isDead)
        {

            if (currentHp <= 0)
            {
                //if (isUpCombo)
                //{
                //    //GameObject temp = Instantiate(spreadBlood, this.GetComponent<Collider>().bounds.center, Quaternion.LookRotation(this.transform.position - whoAttackThis.transform.position));
                //    //temp.GetComponent<particle_test>().SetTarget(whoAttackThis.transform);
                //    //temp.GetComponent<ParticleSystem>().emission.SetBursts(new[] { new ParticleSystem.Burst(0.0f, increaseCombo) });
                //    //temp.GetComponent<ParticleSystem>().emission.SetBursts(bursts);
                //}
                //SetRagdoll(damagedVelocity);
                SetRagdoll(damagedTrs, damagedVelocity);
                SetDead(true);

                float itemDropRate = Random.Range(0.0f, 100.0f);
                Debug.Log(itemDropRate);
                if(itemDropRate <= magazineDropRate)
                {
                    GameManager.Instance.GetItemManager().SpawnItem(ItemType.Magazine, this.transform.position, this.transform.rotation);
                }
                else if(itemDropRate <= magazineDropRate + potionDropRate)
                {
                    GameManager.Instance.GetItemManager().SpawnItem(ItemType.Potion, this.transform.position, this.transform.rotation);
                }
                //agent.isStopped = true;
                //this.gameObject.SetActive(false);
            }
        }
    }

    protected void CheckingHp()
    {
        if (!isDead)
        {
            if (currentHp <= 0)
            {
                isDead = true;
            }
        }
    }

    public void DecreaseHp(float value)
    {
        if (!GameManager.Instance.GetIsCombat())
            return;

        currentHp -= value;

        whoAttackThis = null;

        CheckingHp();
    }

    public void DecreaseHp(GameObject attackObj, float damage, Vector3 damagedPos, Transform damagedTrs, Vector3 damagedVelocity)
    {
        if (!GameManager.Instance.GetIsCombat())
        {
            return;
        }

        currentHp -= damage;

        GameObject effect = pool_damagedEffect.GetDamagedEffect(material);

        if (effect == null)
            return;

        effect.transform.SetParent(null);
        effect.transform.position = damagedPos;
        effect.transform.rotation = Quaternion.identity;
        effect.SetActive(true);

        whoAttackThis = attackObj;

        CheckingHp(damagedTrs, damagedVelocity);
    }

    //public void DecreaseHp(GameObject attackObj, float damage, Vector3 damagedPos, Vector3 damagedVelocity)
    //{
    //    //if (!GameManager.Instance.GetIsCombat())
    //    //{
    //    //    return;
    //    //}

    //    //currentHp -= damage;

    //    //GameObject effect = pool_damagedEffect.GetDamagedEffect(material);

    //    //if (effect == null)
    //    //    return;

    //    //effect.transform.SetParent(null);
    //    //effect.transform.position = damagedPos;
    //    //effect.transform.rotation = Quaternion.identity;
    //    //effect.SetActive(true);

    //    //whoAttackThis = attackObj;

    //    //CheckingHp(damagedVelocity);
    //}
}

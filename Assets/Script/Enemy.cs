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
    RunAway
}

public class Enemy : MonoBehaviour
{
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
    [SerializeField] protected float speed;
    [SerializeField] protected float increaseCombo;
    protected Pool_DamagedEffect pool_damagedEffect;
    [SerializeField] protected float detectRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float combatTime;
    protected float currentCombatTime;
    [SerializeField] protected float returnToPatorlTime;
    protected float currentReturnToPatrolTime;
    [SerializeField] protected Transform[] patrolNode;
    [SerializeField] protected Transform currentDestPatrolNode;
    protected int currentDestPatrolNodeIndex;
    [SerializeField] protected bool isRunAway;
    [SerializeField] protected float increaseSuccessRate;
    [SerializeField] protected Stage currentStage;

    private GameObject whoAttackThis;
    protected Animator anim;
    protected float originAttackRange;
    protected NavMeshAgent agent;

    ParticleSystem.Burst[] bursts;

    public float GetCurrentHP() { return currentHp; }
    public void SetCurrentHP(float value) { currentHp = value; }
    public void SetIsDead(bool value) { isDead = value; }
    public void SetCurrentStage(Stage stage) { currentStage = stage; }

    virtual protected void Start()
    {
        currentHp = maxHp;

        if (GameObject.Find("Pool").transform.Find("Pool_Effect") != null)
            pool_damagedEffect = GameObject.Find("Pool").transform.Find("Pool_Effect").GetComponent<Pool_DamagedEffect>();

        bursts = new[] { new ParticleSystem.Burst(0.0f, increaseCombo) };
        originAttackRange = attackRange;

        currentCombatTime = combatTime;
        currentReturnToPatrolTime = returnToPatorlTime;

        target = GameManager.Instance.GetPlayer().GetCamPos();

        if (this.GetComponent<NavMeshAgent>() != null)
            agent = this.GetComponent<NavMeshAgent>();

        if (patrolNode.Length != 0)
        {
            currentDestPatrolNode = patrolNode[0];
            currentDestPatrolNodeIndex = 0;
        }
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

    protected void CheckingHp(bool isUpCombo)
    {
        if (!isDead)
        {
            if (currentHp <= 0)
            {
                if (isUpCombo)
                {
                    GameObject temp = Instantiate(spreadBlood, this.GetComponent<Collider>().bounds.center, Quaternion.LookRotation(this.transform.position - whoAttackThis.transform.position));
                    temp.GetComponent<particle_test>().SetTarget(whoAttackThis.transform);
                    //temp.GetComponent<ParticleSystem>().emission.SetBursts(new[] { new ParticleSystem.Burst(0.0f, increaseCombo) });
                    temp.GetComponent<ParticleSystem>().emission.SetBursts(bursts);
                }

                currentStage.DecreaseEnemyNum();
                currentStage.SetSuccessRate(currentStage.GetSuccessRate() + increaseSuccessRate);
                isDead = true;
                //agent.isStopped = true;
                //this.gameObject.SetActive(false);
            }
        }
    }

    public void DecreaseHp(float value, bool isUpCombo)
    {
        currentHp -= value;

        whoAttackThis = null;

        CheckingHp(isUpCombo);
    }

    public void DecreaseHp(GameObject attackObj, float value, Vector3 damagedPos, bool isUpCombo)
    {
        currentHp -= value;

        GameObject effect = pool_damagedEffect.GetDamagedEffect(material);

        if (effect == null)
            return;

        effect.transform.SetParent(null);
        effect.transform.position = damagedPos;
        effect.transform.rotation = Quaternion.identity;
        effect.SetActive(true);

        whoAttackThis = attackObj;

        CheckingHp(isUpCombo);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemy_State
{
    None,
    Patrol,
    Search,
    Targeting,
    Chase
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
    [SerializeField] protected float increaseCombo;
    protected Pool_DamagedEffect pool_damagedEffect;
    [SerializeField] protected float detectRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float combatTime;
    [SerializeField] protected float currentCombatTime;
    private GameObject whoAttackThis;
    protected Animator anim;
    protected float originAttackRange;

    ParticleSystem.Burst[] bursts;

    public float GetCurrentHP() { return currentHp; }
    public void SetCurrentHP(float value) { currentHp = value; }

    virtual protected void Start()
    {
        currentHp = maxHp;

        if (GameObject.Find("Pool").transform.Find("Pool_Effect") != null)
            pool_damagedEffect = GameObject.Find("Pool").transform.Find("Pool_Effect").GetComponent<Pool_DamagedEffect>();

        state = Enemy_State.None;

        bursts = new[] { new ParticleSystem.Burst(0.0f, increaseCombo) };
        originAttackRange = attackRange;
    }

    protected void CheckingHp(bool isUpCombo)
    {
        if(currentHp <= 0)
        {
            isDead = true;

            if (isUpCombo)
            {
                GameObject temp = Instantiate(spreadBlood, this.GetComponent<Collider>().bounds.center, Quaternion.LookRotation(this.transform.position - whoAttackThis.transform.position));
                temp.GetComponent<particle_test>().SetTarget(whoAttackThis.transform);
                //temp.GetComponent<ParticleSystem>().emission.SetBursts(new[] { new ParticleSystem.Burst(0.0f, increaseCombo) });
                temp.GetComponent<ParticleSystem>().emission.SetBursts(bursts);
            }

            this.gameObject.SetActive(false);
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

        effect.transform.SetParent(null);
        effect.transform.position = damagedPos;
        effect.transform.rotation = Quaternion.identity;
        effect.SetActive(true);

        whoAttackThis = attackObj;

        CheckingHp(isUpCombo);
    }
}

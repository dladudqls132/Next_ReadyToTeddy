using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Air_Easy,
    Boss,
    A,
    B,
    C,
    D
}

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
    //[SerializeField] protected CharacterMaterial material;
    [SerializeField] protected Transform eye;
    [SerializeField] protected Transform target;
    
    [SerializeField] protected bool canSee;
    [SerializeField] protected bool isDead;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    protected float increaseHp;
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    //protected float increaseCombo;
   
    [SerializeField] protected float detectRange;
    private float detectTime = 4.0f;
    private float currentDetectTime;
    [SerializeField] protected bool isDetect;
    [SerializeField] protected float attackRange;
    //[SerializeField] protected float combatTime;
    //protected float currentCombatTime;
    //protected float returnToPatorlTime;
    //protected float currentReturnToPatrolTime;
    //protected Transform[] patrolNode;
    //protected Transform currentDestPatrolNode;
    protected int currentDestPatrolNodeIndex;
    protected bool isRunAway;
    protected float increaseSuccessRate;
    [SerializeField] protected float potionDropRate;
    [SerializeField] protected float magazineDropRate;
    [SerializeField] protected bool isRigidity;
    [SerializeField] protected float rigidityTime;

    [SerializeField] protected GameObject dropItem;
    [SerializeField] protected GameObject dropWeapon;


    protected float currentRigidityTime;

    //private GameObject whoAttackThis;
    protected Animator anim;
    protected float originAttackRange;
    protected NavMeshAgent agent;
    protected Rigidbody rigid;

    protected bool isGod;

    [SerializeField] protected GameObject energyShield_prefab;
    protected GameObject energyShield;
    [SerializeField] protected int shieldHp;

    [SerializeField] protected Renderer[] renderers;
    [SerializeField] protected Color emissionColor_normal;
    [SerializeField] protected Color emissionColor_angry;

    public float GetCurrentHp() { return currentHp; }
    public void SetCurrentHp(float value) { currentHp = value; }
    public float GetMaxHp() { return maxHp; }
    public void SetIsDead(bool value) { isDead = value; }
    public bool GetIsDead() { return isDead; }
    public GameObject GetEnergyShield() { return energyShield; }

    public void SetInfo(EnemyType enemyType,/* EffectType effectType, */float damage, float hp, float speed, float detectRange, float attackRange, float potionDropRate, float magazineDropRate)
    {
        this.enemyType = enemyType;
        //this.material = material;
        this.speed = speed;
        this.damage = damage;
        this.maxHp = hp;
        this.detectRange = detectRange;
        this.attackRange = attackRange;
        this.potionDropRate = potionDropRate;
        this.magazineDropRate = magazineDropRate;
    }

    virtual protected void Start()
    {
        currentHp = maxHp;

        originAttackRange = attackRange;

        if(target == null && GameManager.Instance.GetPlayer().transform != null)
            target = GameManager.Instance.GetPlayer().GetAimPos();

        if (this.GetComponent<NavMeshAgent>() != null)
        {
            agent = this.GetComponent<NavMeshAgent>();
            agent.speed = speed;
        }

        if(this.GetComponent<Rigidbody>() != null)
            rigid = this.GetComponent<Rigidbody>();

        if(this.GetComponent<Animator>() != null)
            anim = this.GetComponent<Animator>();

        //float itemDropRate = Random.Range(0.0f, 100.0f);

        //if (itemDropRate <= magazineDropRate)
        //{
        //    dropItem = GameManager.Instance.GetItemManager().SetDropItem(ItemType.Magazine);
        //}
        //else if (itemDropRate <= magazineDropRate + potionDropRate)
        //{
        //    dropItem = GameManager.Instance.GetItemManager().SetDropItem(ItemType.Potion);
        //}

        dropItem = GameManager.Instance.GetItemManager().SetDropItem(ItemType.Magazine);

        if (dropItem != null)
            dropItem.SetActive(false);

        renderers = this.transform.GetChild(0).GetComponentsInChildren<Renderer>();
    }

    public virtual void SetDead(bool value) {}

    protected void CheckingPlayer()
    {
        RaycastHit hit;
        if(Physics.Raycast(this.transform.position, (target.position - this.transform.position).normalized, out hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Root") | 1 << LayerMask.NameToLayer("Shield"))))
        {
            if(hit.transform.CompareTag("Player"))
            {
                canSee = true;
            }
            else
            {

                if (Vector3.Distance(this.transform.position, target.position) > detectRange)
                {
                    canSee = false;
                }
            }
        }
        else
        {
            //canSee = false;
            if (Vector3.Distance(this.transform.position, target.position) > detectRange)
            {
                canSee = false;
            }
        }

        //if (Vector3.Distance(this.transform.position, target.position) <= detectRange)
        //{
        //    canSee = true;
        //}

        if (canSee)
        {
            currentDetectTime = detectTime;

            isDetect = true;
        }
        else
        {
            currentDetectTime -= Time.deltaTime;

            if(currentDetectTime <= 0)
            {
                isDetect = false;
            }
        }
    }

    protected void CheckingHp()
    {
        if (!isDead)
        {
            //Debug.Log(currentHp);

            if (currentHp <= 0)
            {
                
                SetDead(true);


                //if (enemyType == EnemyType.Air_Easy)
                //{
                //    GameManager.Instance.GetItemManager().SpawnMagazine(GunType.ChainLightning, this.transform.position + Vector3.up, this.transform.rotation);
                //}
                //else
                //{
                //    float itemDropRate = Random.Range(0.0f, 100.0f);

                //    if (itemDropRate <= magazineDropRate)
                //    {
                //        GameManager.Instance.GetItemManager().SpawnItem(ItemType.Magazine, this.transform.position + Vector3.up, this.transform.rotation);
                //    }
                //    else if (itemDropRate <= magazineDropRate + potionDropRate)
                //    {
                //        GameManager.Instance.GetItemManager().SpawnItem(ItemType.Potion, this.transform.position + Vector3.up, this.transform.rotation);
                //    }
                //}


                if (dropItem != null)
                {
                    dropItem.transform.position = this.transform.position + Vector3.up;
                    dropItem.transform.rotation = this.transform.rotation;

                    dropItem.SetActive(true);
                }
                if(dropWeapon != null)
                {
                    dropWeapon.transform.position = this.transform.position + Vector3.up;
                    dropWeapon.transform.rotation = this.transform.rotation;

                    dropWeapon.SetActive(true);
                }

                //agent.isStopped = true;
                //this.gameObject.SetActive(false);
            }
        }
    }

    public void IncreaseHp(float value)
    {
        currentHp += value;

        CheckingHp();
    }

    public void DecreaseHp(float value)
    {
        //if (!GameManager.Instance.GetIsCombat())
        //    return;
        if (!this.enabled) return;

        if (isDead || isGod) return;
        currentHp -= value;

        isDetect = true;
       // whoAttackThis = null;

        CheckingHp();
    }

    public void DecreaseHp(float damage, Vector3 damagedPos, Transform damagedTrs, Vector3 damagedVelocity, EffectType effectType)
    {
        //if (!GameManager.Instance.GetIsCombat())
        //{
        //    return;
        //}

        if (!this.enabled) return;

        if (isDead || isGod) return;

        currentHp -= damage;
     
        GameObject effect = GameManager.Instance.GetPoolEffect().GetEffect(effectType);

        if (effect == null)
            return;

        if (effectType != EffectType.Damaged_lightning)
            effect.transform.SetParent(null);
        else
            effect.GetComponent<HitEffect>().SetHitEffect(this.transform, 3.0f);

        effect.transform.position = damagedPos;
        effect.transform.rotation = Quaternion.LookRotation(damagedVelocity.normalized);
        effect.transform.rotation = Quaternion.Euler(effect.transform.eulerAngles.x - 90, effect.transform.eulerAngles.y, effect.transform.eulerAngles.z);
        effect.SetActive(true);

        isDetect = true;
        // whoAttackThis = attackObj;
        if (anim != null)
        {
            anim.SetTrigger("Damaged");
        }
        CheckingHp();
    }

    public void DecreaseHp(float damage, Vector3 damagedPos, Transform damagedTrs, Vector3 damagedVelocity, EffectType effectType, float stunTime)
    {
        //if (!GameManager.Instance.GetIsCombat())
        //{
        //    return;
        //}
        if (!this.enabled) return;

        if (isDead || isGod) return;

        currentHp -= damage;

        GameObject effect = GameManager.Instance.GetPoolEffect().GetEffect(effectType);

        if (effect == null)
            return;

        if (effectType != EffectType.Damaged_lightning)
            effect.transform.SetParent(null);
        else
        {
            effect.GetComponent<HitEffect>().SetHitEffect(this.transform, stunTime);
        }

        SetRigidity(true, stunTime);

        effect.transform.position = damagedPos;
        effect.transform.rotation = Quaternion.identity;
        effect.SetActive(true);

        isDetect = true;
        // whoAttackThis = attackObj;
        anim.SetTrigger("Damaged");
        CheckingHp();
    }

    public void SetRigidity(bool value, float time)
    {
        isRigidity = value;
        rigidityTime = time;
        currentRigidityTime = 0;
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

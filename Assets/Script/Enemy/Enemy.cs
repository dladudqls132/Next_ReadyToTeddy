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


    [SerializeField] protected float potionDropRate;
    [SerializeField] protected float magazineDropRate;
    [SerializeField] protected bool isRigidity;
    [SerializeField] protected float rigidityTime;

    [SerializeField] protected GameObject[] dropItem_prefab;
    protected List<GameObject> dropItem = new List<GameObject>();

    protected float currentRigidityTime;

    //private GameObject whoAttackThis;
    protected Animator anim;
    protected float originAttackRange;
    protected NavMeshAgent agent;
    protected Rigidbody rigid;

    protected bool isGod;

    [SerializeField] protected Renderer[] renderers;
    [SerializeField] protected Color emissionColor_normal;
    [SerializeField] protected Color emissionColor_angry;

    public float GetCurrentHp() { return currentHp; }
    public void SetCurrentHp(float value) { currentHp = value; }
    public float GetMaxHp() { return maxHp; }
    public void SetIsDead(bool value) { isDead = value; }
    public bool GetIsDead() { return isDead; }
    public EnemyType GetEnemyType() { return enemyType; }

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

        if (target == null && GameManager.Instance.GetPlayer().transform != null)
            target = GameManager.Instance.GetPlayer().GetAimPos();

        if (this.GetComponent<NavMeshAgent>() != null)
        {
            agent = this.GetComponent<NavMeshAgent>();
            agent.speed = speed;
        }

        if (this.GetComponent<Rigidbody>() != null)
            rigid = this.GetComponent<Rigidbody>();

        if (this.GetComponent<Animator>() != null)
            anim = this.GetComponent<Animator>();

        for (int i = 0; i < dropItem_prefab.Length; i++)
        {
            dropItem.Add(Instantiate(dropItem_prefab[i], this.transform.position + Vector3.up, Quaternion.identity, this.transform));
            dropItem[i].SetActive(false);
        }

        renderers = this.transform.GetChild(0).GetComponentsInChildren<Renderer>();
    }

    public virtual void SetDead(bool value) { }

    protected void CheckingPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, (target.position - this.transform.position).normalized, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.CompareTag("Player") && Vector3.Distance(this.transform.position, target.position) < detectRange * 2)
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

            if (currentDetectTime <= 0)
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
                if (dropItem.Count != 0)
                {
                    for (int i = 0; i < dropItem.Count; i++)
                    {
                        dropItem[i].transform.SetParent(null);
                        dropItem[i].SetActive(true);
                    }
                }

                SetDead(true);
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

        if (effect != null)
        {
            if (effectType != EffectType.Damaged_lightning)
                effect.transform.SetParent(null);
            else
                effect.GetComponent<HitEffect>().SetHitEffect(this.transform, 3.0f);

            effect.transform.position = damagedPos;
            effect.transform.rotation = Quaternion.LookRotation(damagedVelocity.normalized);
            effect.transform.rotation = Quaternion.Euler(effect.transform.eulerAngles.x - 90, effect.transform.eulerAngles.y, effect.transform.eulerAngles.z);
            effect.SetActive(true);
        }

        foreach (Renderer r in renderers)
        {
            r.material.SetColor("_Color", Color.white * 35);
        }
      
        //Invoke("ResetColor", 0.02f);
        StartCoroutine(ResetColor());

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

        if (effect != null)
        {
            if (effectType != EffectType.Damaged_lightning)
                effect.transform.SetParent(null);
            else
            {
                effect.GetComponent<HitEffect>().SetHitEffect(this.transform, stunTime);
            }

            effect.transform.position = damagedPos;
            effect.transform.rotation = Quaternion.identity;
            effect.SetActive(true);
        }

        SetRigidity(true, stunTime);

        isDetect = true;
        // whoAttackThis = attackObj;
        anim.SetTrigger("Damaged");
        CheckingHp();
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.02f);

        foreach (Renderer r in renderers)
        {
            r.material.SetColor("_Color", Color.white);
        }
    }

    //void ResetColor()
    //{
    //    foreach (Renderer r in renderers)
    //    {
    //        r.material.SetColor("_Color", Color.white);
    //    }
    //}

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

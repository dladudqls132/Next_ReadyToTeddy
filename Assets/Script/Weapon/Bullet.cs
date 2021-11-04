using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected Vector3 dir;
    private Vector3 tempDir;
    [SerializeField] protected float speed;
    protected float damage;
    [SerializeField] protected bool isFire;
    protected Rigidbody rigid;
    [SerializeField] protected float lifeTime = 8.0f;
    [SerializeField] protected bool isDestroyed;
    protected SphereCollider coll;
    [SerializeField] protected TrailRenderer trail;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform target;
    [SerializeField] protected float stunTime;
    private float turnSpeed;
    private float limitTurnSpeed;

    public float GetDamage() { return damage; }

    protected void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        coll = this.GetComponent<SphereCollider>();

        if (trail == null)
            trail = this.GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(ActiveFalse_LifeTime());
    }

    virtual protected void Update()
    {
        if (isFire)
        {

            if (target)
            {
                turnSpeed += Time.deltaTime * 4;
                turnSpeed = Mathf.Clamp(turnSpeed, 0, limitTurnSpeed);

                if (Vector3.Dot(tempDir, dir) > 0.3f)
                    dir = Vector3.MoveTowards(dir, (target.position - this.transform.position).normalized, turnSpeed * Time.deltaTime);
            }

            rigid.velocity = dir * speed;
        }

        if (isDestroyed)
        {
            if (trail != null)
            {
                if (trail.positionCount <= 0)
                {
                    ResetInfo();
                }
            }
            else
            {
                ResetInfo();
            }
        }
        //else
        //{
        //    currentLifeTime += Time.deltaTime;
        //    if (currentLifeTime >= lifeTime)
        //    {
        //        ActiveFalse();
        //    }
        //}
    }

    public void SetFire(Vector3 startPos, Vector3 direction, float speed, float damage)
    {
        isFire = true;
        this.dir = direction;
        this.speed = speed;
        this.damage = damage;

        this.transform.position = startPos;
        this.transform.rotation = Quaternion.LookRotation(direction);

        this.gameObject.SetActive(true);
    }

    public void SetFire(Vector3 startPos, Vector3 direction, Transform target, float speed, float damage, float limitTurnSpeed)
    {
        isFire = true;
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.dir = direction;
        tempDir = direction;

        this.transform.position = startPos;
        this.transform.rotation = Quaternion.LookRotation((target.position - startPos).normalized);

        this.limitTurnSpeed = limitTurnSpeed;

        this.gameObject.SetActive(true);
    }

    public void SetFire(Vector3 startPos, Vector3 direction, float speed, float damage, float stunTime)
    {
        isFire = true;
        this.dir = direction;
        this.speed = speed;
        this.damage = damage;
        this.stunTime = stunTime;

        this.transform.position = startPos;
        this.transform.rotation = Quaternion.LookRotation(direction);

        this.gameObject.SetActive(true);
    }

    public void SetFire(Vector3 startPos, Transform target, float speed, float damage, float stunTime)
    {
        isFire = true;
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.stunTime = stunTime;
        this.dir = (target.position - this.transform.position).normalized;

        this.transform.position = startPos;
        this.transform.rotation = Quaternion.LookRotation((target.position - startPos).normalized);

        this.gameObject.SetActive(true);
    }

    virtual protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || /*LayerMask.LayerToName(other.gameObject.layer).Equals("Default") ||*/ LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Wall"))
        //else if (other.CompareTag("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment"))
        {

            if (!other.isTrigger)
            {
                GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.BulletHit_normal);
                if (temp != null)
                {
                    temp.transform.position = this.transform.position;
                    temp.GetComponent<Explosion>().SetDamage(damage);
                    temp.SetActive(true);
                }

                GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion_2, this.transform.position, false);
                ActiveFalse();
            }
        }
    }

    virtual protected IEnumerator ActiveFalse_LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        if (!isDestroyed)
        {
            GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.BulletHit_normal);
            if (temp != null)
            {
                temp.transform.position = this.transform.position;
                temp.GetComponent<Explosion>().SetDamage(damage);
                temp.SetActive(true);
            }

            GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion_2, this.transform.position, false);

            isFire = false;
            rigid.velocity = Vector3.zero;
            coll.enabled = false;
            isDestroyed = true;

            turnSpeed = 0;
        }
    }

    virtual protected void ActiveFalse()
    {
        isFire = false;
        rigid.velocity = Vector3.zero;
        coll.enabled = false;
        isDestroyed = true;

        turnSpeed = 0;

        StopAllCoroutines();
    }

    virtual protected void ResetInfo()
    {
        rigid.velocity = Vector3.zero;
        isFire = false;
        coll.enabled = true;
        isDestroyed = false;
        StopAllCoroutines();
        //this.GetComponent<MeshRenderer>().enabled = true;
        this.gameObject.SetActive(false);
    }
}

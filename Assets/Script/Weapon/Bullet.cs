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
    protected float currentLifeTime = 0;
    [SerializeField] protected bool isDestroyed;
    protected SphereCollider coll;
    [SerializeField] protected TrailRenderer trail;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform target;
    [SerializeField] protected float stunTime;
    private float turnSpeed;
    private float rndlimitTurnSpeed;

    public float GetDamage() { return damage; }

    protected void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        coll = this.GetComponent<SphereCollider>();

        if (trail == null)
            trail = this.GetComponent<TrailRenderer>();

        rndlimitTurnSpeed = Random.Range(1.0f, 2.5f);
    }

    virtual protected void Update()
    {
        if (isFire)
        {

            if (target)
            {
                turnSpeed += Time.deltaTime * 4;
                turnSpeed = Mathf.Clamp(turnSpeed, 0, rndlimitTurnSpeed);

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
        }
        else
        {
            currentLifeTime += Time.deltaTime;
            if (currentLifeTime >= lifeTime)
            {
                ActiveFalse();
            }
        }
    }

    public void SetFire(Vector3 pos, Vector3 direction, float speed, float damage)
    {
        isFire = true;
        this.dir = direction;
        this.speed = speed;
        this.damage = damage;

        this.transform.position = pos;
        this.transform.rotation = Quaternion.LookRotation(direction);

        this.gameObject.SetActive(true);
    }

    public void SetFire(Vector3 pos, Vector3 direction, Transform target, float speed, float damage)
    {
        isFire = true;
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.dir = direction;
        tempDir = direction;

        this.transform.position = pos;
        this.transform.rotation = Quaternion.LookRotation((target.position - pos).normalized);

        this.gameObject.SetActive(true);
    }

    public void SetFire(Vector3 pos, Vector3 direction, float speed, float damage, float stunTime)
    {
        isFire = true;
        this.dir = direction;
        this.speed = speed;
        this.damage = damage;
        this.stunTime = stunTime;

        this.transform.position = pos;
        this.transform.rotation = Quaternion.LookRotation(direction);

        this.gameObject.SetActive(true);
    }

    public void SetFire(Vector3 pos, Transform target, float speed, float damage, float stunTime)
    {
        isFire = true;
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.stunTime = stunTime;
        this.dir = (target.position - this.transform.position).normalized;

        this.transform.position = pos;
        this.transform.rotation = Quaternion.LookRotation((target.position - pos).normalized);

        this.gameObject.SetActive(true);
    }

    virtual protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            other.GetComponent<PlayerController>().DecreaseHp(damage);
            GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.BulletHit_normal);
            temp.transform.position = this.transform.position;
            temp.SetActive(true);
            GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion_2, this.transform.position, false);
            ActiveFalse();
        }
        else if (/*LayerMask.LayerToName(other.gameObject.layer).Equals("Default") ||*/ LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Wall"))
        //else if (other.CompareTag("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment"))
        {

            if (!other.isTrigger)
            {
                GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.BulletHit_normal);
                if (temp != null)
                {
                    temp.transform.position = this.transform.position;
                    temp.SetActive(true);
                }

                GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion_2, this.transform.position, false);
                ActiveFalse();
            }
        }
    }

    virtual protected void ActiveFalse()
    {
        isFire = false;
        rigid.velocity = Vector3.zero;
        coll.enabled = false;
        isDestroyed = true;

        currentLifeTime = 0;
        turnSpeed = 0;
        //this.GetComponent<MeshRenderer>().enabled = false;

    }

    virtual protected void ResetInfo()
    {
        currentLifeTime = 0;
        rigid.velocity = Vector3.zero;
        isFire = false;
        coll.enabled = true;
        isDestroyed = false;

        //this.GetComponent<MeshRenderer>().enabled = true;
        this.gameObject.SetActive(false);
    }
}

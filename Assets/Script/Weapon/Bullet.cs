using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected Vector3 dir;
    [SerializeField] protected float speed;
    protected float damage;
    [SerializeField] protected bool isFire;
    protected Rigidbody rigid;
    [SerializeField] protected float lifeTime = 8.0f;
    protected float currentLifeTime = 0;
    [SerializeField] protected bool isDestroyed;
    protected SphereCollider coll;
    [SerializeField] protected TrailRenderer trail;
    [SerializeField] protected Transform target;
    [SerializeField] protected float stunTime;

    public float GetDamage() { return damage; }

    protected void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        coll = this.GetComponent<SphereCollider>();

        if(trail == null)
         trail = this.GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    virtual protected void FixedUpdate()
    {

        if(isFire)
        {
            rigid.velocity = dir * speed;
        }

        if(isDestroyed)
        {
            if(trail.positionCount <= 0)
            {
                ResetInfo();
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
    }

    public void SetFire(Vector3 pos, Transform target, float speed, float damage)
    {
        isFire = true;
        this.target = target;
        this.speed = speed;
        this.damage = damage;

        this.transform.position = pos;
        this.transform.rotation = Quaternion.LookRotation((target.position - pos).normalized);
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
    }

    public void SetFire(Vector3 pos, Transform target, float speed, float damage, float stunTime)
    {
        isFire = true;
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.stunTime = stunTime;

        this.transform.position = pos;
        this.transform.rotation = Quaternion.LookRotation((target.position - pos).normalized);
    }

    virtual protected void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DecreaseHp(damage);
            ActiveFalse();
        }
        else if (LayerMask.LayerToName(other.gameObject.layer).Equals("Default") || LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment"))
        //else if (other.CompareTag("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment"))
        {
            
            if (!other.isTrigger)
                ActiveFalse();
        }
    }

    virtual protected void ActiveFalse()
    {
        isFire = false;
        rigid.velocity = Vector3.zero;
        coll.enabled = false;
        isDestroyed = true;
        
        currentLifeTime = 0;
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    virtual protected void ResetInfo()
    {
        currentLifeTime = 0;
        rigid.velocity = Vector3.zero;
        isFire = false;
        coll.enabled = true;
        isDestroyed = false;
        this.GetComponent<MeshRenderer>().enabled = true;
        this.gameObject.SetActive(false);
    }
}

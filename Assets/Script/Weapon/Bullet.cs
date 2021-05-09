using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 dir;
    private float speed;
    private float damage;
    private bool isFire;
    private Rigidbody rigid;
    private float lifeTime = 15.0f;
    private float currentLifeTime = 0;
    private bool isDestroyed;
    private SphereCollider coll;
    private TrailRenderer trail;

    private void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        coll = this.GetComponent<SphereCollider>();
        trail = this.GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isFire)
        {
            rigid.velocity = dir * speed;
        }

        if(isDestroyed)
        {
            ActiveFalse();
            if(trail.positionCount <= 0)
            {
                ResetInfo();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DecreaseHp(damage);
            ActiveFalse();
        }
        else if (!LayerMask.LayerToName(other.gameObject.layer).Equals("Ignore Raycast"))
        //else if (other.CompareTag("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment"))
        {
            ActiveFalse();
        }
    }

    void ActiveFalse()
    {
        isFire = false;
        rigid.velocity = Vector3.zero;
        coll.enabled = false;
        isDestroyed = true;
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    void ResetInfo()
    {
        currentLifeTime = lifeTime;
        rigid.velocity = Vector3.zero;
        isFire = false;
        coll.enabled = true;
        isDestroyed = false;
        this.GetComponent<MeshRenderer>().enabled = true;
        this.gameObject.SetActive(false);
    }
}

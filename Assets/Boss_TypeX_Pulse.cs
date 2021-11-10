using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Pulse : MonoBehaviour
{
    private SphereCollider coll;
    private ParticleSystem p;
    private float damage;

    public void SetActiveTrue(float damage)
    {
        this.gameObject.SetActive(true);
        this.damage = damage;

        if (coll == null || p == null)
        {
            coll = this.GetComponent<SphereCollider>();
            p = this.GetComponent<ParticleSystem>();
        }
    }

    public void SetActiveFalse()
    {
        this.gameObject.SetActive(false);
        coll.radius = 0;
        p.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        coll.radius = coll.radius + Time.deltaTime * 23.5f / 2;

        if (coll.radius >= 23.5f)
        {
            p.Play();
            coll.radius = 0;
        }
        else
        {
            if (!p.isPlaying)
                p.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(other.transform.position.y - this.transform.position.y <= 0)
            {
                other.GetComponent<PlayerController>().DecreaseHp(damage);
            }
        }
    }
}

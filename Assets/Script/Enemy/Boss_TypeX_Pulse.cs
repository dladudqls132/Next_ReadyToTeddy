using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Pulse : MonoBehaviour
{
    private SphereCollider coll;
    private ParticleSystem p;
    private float damage;
    [SerializeField] private float delay;
    private float currentDelay;

    public void SetDelay(float value) { delay = value; }

    public void SetActiveTrue(float damage, float delay)
    {
        this.gameObject.SetActive(true);
        this.damage = damage;
        this.delay = delay;

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
        if (coll.radius >= 33f)
        {
            currentDelay += Time.deltaTime;

            if (currentDelay >= delay)
            {
                p.Play();
                coll.radius = 0;
                currentDelay = 0;
            }
         
        }
        else
        {
            coll.radius = coll.radius + Time.deltaTime * 33f / 2;

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

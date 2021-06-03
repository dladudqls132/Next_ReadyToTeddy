using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_CL : Bullet
{
    public GameObject chain;
    public GameObject hitEffect;

    // Update is called once per frame
    override protected void FixedUpdate()
    {



        if (isDestroyed)
        {
            if (trail.positionCount <= 0)
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

        if (isFire)
        {
            if (target != null)
            {
                dir = (target.GetComponent<Collider>().bounds.center - this.transform.position).normalized;
                this.transform.rotation = Quaternion.LookRotation(dir);
            }

            rigid.velocity = dir * speed;
        }

    }

    override protected void ActiveFalse()
    {
        base.ActiveFalse();
    }

    protected override void ResetInfo()
    {
        base.ResetInfo();

        //temp = false;
        //ignoreTrs = null;
        //target = null;
    }

    override protected void OnTriggerEnter(Collider other)
    {

        if (LayerMask.LayerToName(other.gameObject.layer).Equals("Enemy"))
        {
            Collider[] coll = Physics.OverlapSphere(this.transform.position, 10, 1 << LayerMask.NameToLayer("Root"));

            int num = 0;
            for (int i = 0; i < coll.Length; i++)
            {

                if(coll[i].CompareTag("Enemy") && other.transform.root != coll[i].transform && !coll[i].GetComponent<Enemy>().GetIsDead())
                {
                    num++;

                    GameObject temp = Instantiate(chain);
                    temp.GetComponent<Chain>().SetLine(other.transform.root, coll[i].transform, stunTime);

                    coll[i].GetComponent<Enemy>().DecreaseHp(damage, other.transform.root.GetComponent<Enemy_RagdollController>().spineRigid.position, other.transform.root.GetComponent<Enemy_RagdollController>().spineRigid.transform, rigid.velocity, EffectType.Lightning, stunTime);
                    //coll[i].GetComponent<Enemy>().SetRigidity(true, stunTime);
                    //GameObject tempHit = Instantiate(hitEffect, coll[i].transform);
                    //tempHit.GetComponent<HitEffect>().SetHitEffect(coll[i].GetComponent<Enemy_RagdollController>().spineRigid.position, 3.0f);



                    if (num == 2)
                        break;
                }
            }
            //GameObject hit = Instantiate(hitEffect, other.GetComponent<Collider>().bounds.center, Quaternion.identity, other.transform);
            //hit.GetComponent<HitEffect>().SetHitEffect(other.GetComponent<Enemy_RagdollController>().spineRigid.position, 3.0f);

            other.transform.root.GetComponent<Enemy>().DecreaseHp(damage, other.transform.root.GetComponent<Enemy_RagdollController>().spineRigid.position, other.transform.root.GetComponent<Enemy_RagdollController>().spineRigid.transform, rigid.velocity, EffectType.Lightning, stunTime);
            //other.GetComponent<Enemy>().SetRigidity(true, stunTime);

            ActiveFalse();
        }
        else if (LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Shield"))
        //else if (other.CompareTag("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment"))
        {
            if (LayerMask.LayerToName(other.gameObject.layer).Equals("Shield"))
            {
                other.GetComponent<Boss_EnergyShield>().DecreaseHp();
            }

            if(!other.isTrigger)
                ActiveFalse();
        }
    }
}

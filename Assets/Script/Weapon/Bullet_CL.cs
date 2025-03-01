﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_CL : Bullet
{
    public GameObject chain;
    public GameObject hitEffect;

    // Update is called once per frame
    override protected void Update()
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

                    GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Hit);
                    coll[i].GetComponent<Enemy>().DecreaseHp(damage, coll[i].transform.root.position, other.transform.root, rigid.velocity, EffectType.Damaged_lightning, stunTime);

                    GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Electric, coll[i].transform, false);

                    if (num == 2)
                        break;
                }
            }
            //GameObject hit = Instantiate(hitEffect, other.GetComponent<Collider>().bounds.center, Quaternion.identity, other.transform);
            //hit.GetComponent<HitEffect>().SetHitEffect(other.GetComponent<Enemy_RagdollController>().spineRigid.position, 3.0f);

            other.transform.root.GetComponent<Enemy>().DecreaseHp(damage, other.transform.root.position, other.transform.root, rigid.velocity, EffectType.Damaged_lightning, stunTime);
            //other.GetComponent<Enemy>().SetRigidity(true, stunTime);

            GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Electric, other.transform, false);
            GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Hit);
            ActiveFalse();
        }
        else if (LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Shield"))
        //else if (other.CompareTag("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment"))
        {
            if (LayerMask.LayerToName(other.gameObject.layer).Equals("Shield"))
            {
                if(other.GetComponent<Boss_EnergyShield>() != null)
                    other.GetComponent<Boss_EnergyShield>().DecreaseHp();
                ActiveFalse();
            }
            else
            {
                if (!other.isTrigger)
                    ActiveFalse();
            }
        }
    }
}

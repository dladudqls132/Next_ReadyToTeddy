using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_CL : Bullet
{
    public GameObject chain;
    public Transform ignoreTrs;
    public bool temp;

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

        if (other.CompareTag("Enemy"))
        {
            Collider[] coll = Physics.OverlapSphere(this.transform.position, 10, 1 << LayerMask.NameToLayer("Enemy"));

            int num = 0;
            for (int i = 0; i < coll.Length; i++)
            {

                if(coll[i].CompareTag("Enemy"))
                {
                    num++;
                    //Bullet tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.CL).GetComponent<Bullet>();
                    //tempBullet.gameObject.SetActive(true);
                    //tempBullet.SetFire(other.GetComponent<Collider>().bounds.center, coll[i].transform, 150, 0);
                    //tempBullet.GetComponent<Bullet_CL>().ignoreTrs = other.transform;
                    //tempBullet.GetComponent<Bullet_CL>().temp = true;

                    GameObject temp = Instantiate(chain);
                    temp.GetComponent<Chain>().SetLine(other.transform, coll[i].transform, 1.0f);
                    coll[i].GetComponent<Enemy>().SetRigidity(true, 1);

                    if (num == 2)
                        break;
                }
            }

            other.GetComponent<Enemy>().SetRigidity(true, 1);

            if (ignoreTrs != other.transform)
                ActiveFalse();
        }
        else if (LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment"))
        //else if (other.CompareTag("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment"))
        {
            ActiveFalse();
        }
    }
}

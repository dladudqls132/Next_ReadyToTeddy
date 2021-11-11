using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Plasma : Bullet
{
    private Vector3 targetOffset;

    protected override void Update()
    {
        if (isFire)
        {

            if (target)
            {

                dir = ((target.position + targetOffset) - this.transform.position).normalized;
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
    }

    protected override void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag("Player") || /*LayerMask.LayerToName(other.gameObject.layer).Equals("Default") ||*/ LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Wall"))
        //else if (other.CompareTag("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment"))
        {
            if (!other.isTrigger)
            {
                GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.BulletHit_Trap);
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

    protected override void ResetInfo()
    {
        base.ResetInfo();

        targetOffset = Random.insideUnitSphere;
    }

    protected override void ActiveFalse()
    {
        base.ActiveFalse();
    }

    protected override IEnumerator ActiveFalse_LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        if (!isDestroyed)
        {
            GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.BulletHit_Trap);
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
        }
    }
}

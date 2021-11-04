using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Trap : Bullet
{
    protected override void Update()
    {
        base.Update();
    }

    override protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DecreaseHp(damage);
            GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.BulletHit_Trap);
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

    protected override void ActiveFalse()
    {
        base.ActiveFalse();
    }

    protected override void ResetInfo()
    {
        base.ResetInfo();
    }
}

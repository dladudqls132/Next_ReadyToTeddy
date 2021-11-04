using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject_LightningBall : InteractiveObject
{
    public override void DecreaseHp(float damage, Vector3 damagedPos, Transform damagedTrs, Vector3 damagedVelocity, EffectType effectType)
    {
        base.DecreaseHp(damage, damagedPos, damagedTrs, damagedVelocity, effectType);
    }

    protected override void ActiveFalse()
    {
        GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.BulletHit_Trap);
        if (temp != null)
        {
            temp.transform.position = this.transform.position;
            temp.GetComponent<Explosion>().SetDamage(2);
            temp.SetActive(true);
        }

        GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion_2, this.transform.position, false);

        isDestroyed = false;
        currentHp = maxHp;
        this.transform.parent.gameObject.SetActive(false);
    }
}

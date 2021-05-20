using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float damage;

    private void OnParticleCollision(GameObject other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).Equals("Enemy"))
        {
            other.transform.root.GetComponent<Enemy>().DecreaseHp(damage, other.GetComponent<Collider>().bounds.center, other.transform, this.transform.forward, EffectType.Normal);
        }
    }
}

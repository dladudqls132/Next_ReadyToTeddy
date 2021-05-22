using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float damage;

    private int tempNum = 0;

    private void OnParticleCollision(GameObject other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).Equals("Enemy"))
        {
            tempNum++;

            if(tempNum % 10 == 0)
                other.transform.root.GetComponent<Enemy>().DecreaseHp(damage, other.GetComponent<Collider>().bounds.center, other.transform, this.transform.forward, EffectType.Normal);
            else
                other.transform.root.GetComponent<Enemy>().DecreaseHp(damage, other.GetComponent<Collider>().bounds.center, other.transform, this.transform.forward, EffectType.None);
        }
    }
}

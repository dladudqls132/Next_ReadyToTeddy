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
            Transform other_root = other.GetComponent<Enemy_Bone>().root;
            tempNum++;



            if (tempNum % 10 == 0)
            {
                //GameManager.Instance.GetSoundManager().AudioPlayOneShot(AudioSourceType.SFX, SoundType.Hit);
                other_root.GetComponent<Enemy>().DecreaseHp(damage, other.GetComponent<Collider>().bounds.center, other.transform, this.transform.forward, EffectType.Damaged_normal);
            }
            else
                other_root.GetComponent<Enemy>().DecreaseHp(damage, other.GetComponent<Collider>().bounds.center, other.transform, this.transform.forward, EffectType.None);
        }
    }
}

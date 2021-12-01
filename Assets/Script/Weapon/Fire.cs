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

            if (tempNum % 30 == 0)
            {
                GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Hit);
                other_root.GetComponent<Enemy>().DecreaseHp(damage, other.GetComponent<Collider>().bounds.center, other.transform, this.transform.forward, EffectType.Damaged_normal);
                if (!other_root.GetComponent<Enemy>().GetIsDead())
                    GameManager.Instance.GetCrosshairController().SetAttack_Normal(true);
                else
                    GameManager.Instance.GetCrosshairController().SetAttack_Kill(true);
            }
            else
                other_root.GetComponent<Enemy>().DecreaseHp(damage, other.GetComponent<Collider>().bounds.center, other.transform, this.transform.forward, EffectType.None);
        }
        else if(LayerMask.LayerToName(other.gameObject.layer).Equals("InteractiveObject"))
        {
            tempNum++;

            if (tempNum % 5 == 0)
            {
                GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.Hit);
                other.GetComponent<InteractiveObject>().DecreaseHp(damage, other.GetComponent<Collider>().bounds.center, other.transform, this.transform.forward, EffectType.Damaged_normal);

                if (!other.GetComponent<InteractiveObject>().GetIsDestroy())
                    GameManager.Instance.GetCrosshairController().SetAttack_Normal(true);
                else
                    GameManager.Instance.GetCrosshairController().SetAttack_Kill(true);
            }
            else
                other.GetComponent<InteractiveObject>().DecreaseHp(damage, other.GetComponent<Collider>().bounds.center, other.transform, this.transform.forward, EffectType.None);
        }
    }
}

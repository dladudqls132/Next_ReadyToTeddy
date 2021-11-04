using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] protected bool isDestroyed;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;

    public bool GetIsDestroy() { return isDestroyed; }

    private void Start()
    {
        currentHp = maxHp;
    }

    virtual public void DecreaseHp(float damage, Vector3 damagedPos, Transform damagedTrs, Vector3 damagedVelocity, EffectType effectType)
    {
        if (!isDestroyed)
        {
            currentHp -= damage;

            GameObject effect = GameManager.Instance.GetPoolEffect().GetEffect(effectType);

            if (effect != null)
            {
                if (effectType != EffectType.Damaged_lightning)
                    effect.transform.SetParent(null);
                else
                    effect.GetComponent<HitEffect>().SetHitEffect(this.transform, 3.0f);

                effect.transform.position = damagedPos;
                effect.transform.rotation = Quaternion.LookRotation(damagedVelocity.normalized);
                effect.transform.rotation = Quaternion.Euler(effect.transform.eulerAngles.x - 90, effect.transform.eulerAngles.y, effect.transform.eulerAngles.z);
                effect.SetActive(true);
            }

            if (currentHp <= 0)
            {
                ActiveFalse();
            }
        }
    }

    virtual protected void ActiveFalse()
    {
        isDestroyed = true;
        currentHp = 0;
    }
}

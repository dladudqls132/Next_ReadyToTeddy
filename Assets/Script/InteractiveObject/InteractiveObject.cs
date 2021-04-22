using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] protected bool canDestroyed;
    [SerializeField] protected bool isDestroyed;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;

    public bool GetCanDetroy() { return canDestroyed; }
    public bool GetIsDestroy() { return isDestroyed; }

    virtual public void DecreaseHp(float value)
    {
        if (!isDestroyed && canDestroyed)
        {
            currentHp -= value;

            if (currentHp <= 0)
            {
                isDestroyed = true;
                currentHp = 0;
            }
        }
    }
}

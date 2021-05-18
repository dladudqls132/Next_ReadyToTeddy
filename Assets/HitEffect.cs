using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private bool isSet;
    [SerializeField] private float lifeTime;
    private float currentLifeTime;

    public void SetHitEffect(float time)
    {
        isSet = true;
        lifeTime = time;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSet)
        {
            currentLifeTime += Time.deltaTime;

            if(currentLifeTime >= lifeTime)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

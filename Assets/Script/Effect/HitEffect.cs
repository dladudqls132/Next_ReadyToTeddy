using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private bool isSet;
    [SerializeField] private float lifeTime;
    private float currentLifeTime;

    public void SetHitEffect(Transform parent, float time)
    {
        this.transform.SetParent(parent);
        isSet = true;
        lifeTime = time;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSet)
        {
            //this.transform.position = this.transform.root.GetComponent<Enemy_RagdollController>().spineRigid.position;

            currentLifeTime += Time.deltaTime;

            if(currentLifeTime >= lifeTime)
            {
                currentLifeTime = 0;
                this.transform.SetParent(null);
                this.gameObject.SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDrum : InteractiveObject
{
    [SerializeField] private float explosionRadius = 5.0f;
    [SerializeField] private float explosionPower = 10.0f;

    public override void DecreaseHp(float value)
    {
        base.DecreaseHp(value);

        if(isDestroyed)
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 1.0F, ForceMode.VelocityChange);

                    if (rb.CompareTag("Player"))
                    {
                        PlayerController temp = rb.GetComponent<PlayerController>();
                        temp.SetIsPushed(true);
                    }
                    else if(rb.CompareTag("Enemy"))
                    {
                        Enemy temp = rb.GetComponent<Enemy>();
                        temp.DecreaseHp(50, false);
                    }
                }
            }

            this.gameObject.SetActive(false);
        }
    }
}

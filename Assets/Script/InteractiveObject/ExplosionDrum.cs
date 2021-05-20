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

                    RaycastHit hit2;

                    if (rb.CompareTag("Player"))
                    {
                        if (Physics.Raycast(this.transform.position, (hit.ClosestPoint(explosionPos) - this.transform.position).normalized, out hit2, explosionRadius))
                        {
                            if (!hit2.transform.CompareTag("Player"))
                                continue;

                            PlayerController temp = rb.GetComponent<PlayerController>();
                            //rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 1.0F, ForceMode.VelocityChange);
                            //temp.SetIsPushed(true);
                        }
                    }
                    else if (rb.CompareTag("Enemy"))
                    {

                        if (Physics.Raycast(this.transform.position, (hit.ClosestPoint(explosionPos) - this.transform.position).normalized, out hit2, explosionRadius))
                        {
                            if (!LayerMask.LayerToName(hit2.transform.gameObject.layer).Equals("Enemy"))
                            {

                                continue;
                            }

                            if (GameManager.Instance.GetIsCombat())
                            {
                                Enemy temp = rb.GetComponent<Enemy>();

                                temp.DecreaseHp(/*this.gameObject, */10000, hit.ClosestPoint(explosionPos), rb.transform, (hit.ClosestPoint(explosionPos) - explosionPos).normalized * 500, EffectType.None);
                                //rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 1.0F, ForceMode.VelocityChange);
                            }
                        }
                    }
                }
            }

            this.gameObject.SetActive(false);
        }
    }
}

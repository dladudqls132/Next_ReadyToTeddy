using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    Grenade_normal
}

public class Projectile_Grenade : Projectile
{
    [SerializeField] private ProjectileType projectileType;

    override protected void Awake()
    {
        base.Awake();
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (LayerMask.LayerToName(collision.gameObject.layer).Equals("Enviroment"))
    //    {
    //       rigid.drag += Time.deltaTime * 2;
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        if(isThrown)
        {
            currentRemainingTime -= Time.deltaTime;

            if(currentRemainingTime <= 0)
            {
                currentRemainingTime = remainingTime;
                Instantiate(particle, this.transform.position, particle.gameObject.transform.rotation);

                Vector3 explosionPos = transform.position;
                Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius, (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Root")), QueryTriggerInteraction.Ignore);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        RaycastHit hit2;

                        if (rb.CompareTag("Player"))
                        {
                            if (Physics.Raycast(this.transform.position, (hit.ClosestPoint(explosionPos) - this.transform.position).normalized, out hit2, explosionRadius, 1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Player")))
                            {
                                if (LayerMask.LayerToName(hit2.transform.gameObject.layer).Equals("Enviroment"))
                                    continue;

                                PlayerController temp = rb.GetComponent<PlayerController>();
                                //rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 1.0F, ForceMode.VelocityChange);
                                //temp.SetIsPushed(true);
                            }
                        }
                        else if(rb.CompareTag("Enemy"))
                        {
                    
                            if (Physics.Raycast(this.transform.position, (hit.ClosestPoint(explosionPos) - this.transform.position).normalized, out hit2, explosionRadius, 1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Root")))
                            {
                                if (LayerMask.LayerToName(hit2.transform.gameObject.layer).Equals("Enviroment"))
                                {
                                    continue;
                                }
                                
                                    if (GameManager.Instance.GetIsCombat())
                                    {
                                        Enemy temp = rb.transform.root.GetComponent<Enemy>();

                                        temp.DecreaseHp(/*this.gameObject, */10000, hit.ClosestPoint(explosionPos), temp.GetComponent<Enemy_RagdollController>().spineRigid.transform, Vector3.ClampMagnitude((hit.ClosestPoint(explosionPos) - explosionPos).normalized * 100, 100), EffectType.Normal);

                                        //rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 1.0F, ForceMode.VelocityChange);
                                    }
                                
                            }
                        }
                    }
                }

                Destroy(this.gameObject);
            }
        }
    }
}

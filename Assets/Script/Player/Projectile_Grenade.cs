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

    override protected void Start()
    {
        base.Start();
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

                                //temp.SetIsPushed(true);
                            }
                        }
                        else if (rb.CompareTag("Enemy"))
                        {
                           
                            if (Physics.Raycast(this.transform.position, (hit.ClosestPoint(explosionPos) - this.transform.position).normalized, out hit2, explosionRadius))
                            {
                                if (!hit2.transform.CompareTag("Enemy"))
                                {
                                    continue;
                                }

                                if (GameManager.Instance.GetIsCombat())
                                {
                                    Enemy temp = rb.GetComponent<Enemy>();
                                    temp.DecreaseHp(this.gameObject, 10000, hit.ClosestPoint(explosionPos), (hit.ClosestPoint(explosionPos) - explosionPos).normalized * 500);
                                    rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 1.0F, ForceMode.VelocityChange);
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

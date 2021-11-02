using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Boss : MonoBehaviour
{
    private float damage;
    private Rigidbody rigid;
    // Start is called before the first frame update

    private void Start()
    {
        
    }

    public void Fire(Vector3 startPos, Vector3 dir, float speed, float damage)
    {
        this.damage = damage;
        this.transform.position = startPos;
        rigid = this.GetComponent<Rigidbody>();
        //rigid.velocity = dir * speed;
        this.transform.rotation = Quaternion.LookRotation(dir);
        this.gameObject.SetActive(true);
        rigid.velocity = Vector3.zero;
        rigid.AddForce(dir * speed, ForceMode.Impulse);

        StartCoroutine(ActiveFalse());
    }

    IEnumerator ActiveFalse()
    {
        yield return new WaitForSeconds(3);

        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment")/* || LayerMask.LayerToName(other.gameObject.layer).Equals("Default")*/ || LayerMask.LayerToName(other.gameObject.layer).Equals("Player"))
        {
            
            //Collider[] colliders = Physics.OverlapSphere(this.transform.position, 4, (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Root")), QueryTriggerInteraction.Ignore);
            //foreach (Collider hit in colliders)
            //{
            //    Rigidbody rb = hit.GetComponent<Rigidbody>();

            //    if (rb != null)
            //    {
            //        RaycastHit hit2;

            //        if (rb.CompareTag("Player"))
            //        {
            //            if (Physics.Raycast(this.transform.position, (hit.ClosestPoint(this.transform.position) - this.transform.position).normalized, out hit2, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enviroment") | 1 << LayerMask.NameToLayer("Player")))
            //            {
            //                if (LayerMask.LayerToName(hit2.transform.gameObject.layer).Equals("Enviroment") || LayerMask.LayerToName(hit2.transform.gameObject.layer).Equals("Default"))
            //                    continue;

            //                PlayerController temp = rb.GetComponent<PlayerController>();
            //                temp.DecreaseHp(10);
            //                //rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 1.0F, ForceMode.VelocityChange);
            //                //temp.SetIsPushed(true);
            //            }
            //        }
            //    }
            //}

            GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.EnergyBall_Bomb, this.transform.position, false);
            GameObject tempBomb = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Explosion_EnergyBall);
            tempBomb.GetComponent<Explosion>().SetDamage(damage);

            tempBomb.transform.position = other.ClosestPoint(this.transform.position);
            tempBomb.gameObject.SetActive(true);

            this.gameObject.SetActive(false);
        }
    }
}

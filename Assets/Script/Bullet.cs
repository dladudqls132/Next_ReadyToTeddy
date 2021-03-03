using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigid;
    protected float speed;
    protected float damage;

    private bool isFire;
    Quaternion rot;


    public void Init()
    {
        rigid = this.GetComponent<Rigidbody>();
        rot = Quaternion.Euler(Vector3.zero);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(rigid.position, this.transform.right, out hit, speed * Time.deltaTime) && isFire)
        {
            if (hit.transform.CompareTag("Enviroment"))
            {
                isFire = false;
                rigid.velocity = Vector3.zero;

                this.gameObject.SetActive(false);
            }
        }
    }

    public void SetFire(Vector3 shotPos, float damage, float speed, float spreadAnagle)
    {
        this.gameObject.SetActive(true);
        this.isFire = true;
        this.speed = speed;
        this.damage = damage;

        rot = Random.rotation;

        this.transform.position = shotPos;
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rot, spreadAnagle);
        rigid.AddForce(this.transform.forward * speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enviroment"))
        {
            isFire = false;
            rigid.velocity = Vector3.zero;

            this.gameObject.SetActive(false);
        }
    }
}

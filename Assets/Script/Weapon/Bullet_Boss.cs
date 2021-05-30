using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Boss : MonoBehaviour
{
    private Rigidbody rigid;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(Vector3 dir, float speed)
    {
        rigid = this.GetComponent<Rigidbody>();
        rigid.velocity = dir * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(LayerMask.LayerToName(other.gameObject.layer).Equals("Enviroment") || LayerMask.LayerToName(other.gameObject.layer).Equals("Defalut") || LayerMask.LayerToName(other.gameObject.layer).Equals("Player"))
        {
            
        }
    }
}

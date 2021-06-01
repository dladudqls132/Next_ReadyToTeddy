using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Teddy_Container : MonoBehaviour
{
    private Transform target;
    [SerializeField] private ParticleSystem floorRange;
    private bool isDrop;
    private Rigidbody rigid;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        floorRange = Instantiate(floorRange);
        //floorRange.gameObject.SetActive(false);
        target = GameManager.Instance.GetPlayer().transform;
        rigid = this.GetComponent<Rigidbody>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDrop)        
            SetDrop(target.position);
    }

    public void SetDrop(Vector3 pos)
    {
        anim.enabled = false;
        rigid.useGravity = true;
        isDrop = true;
        rigid.AddForce((pos - this.transform.position).normalized * 60, ForceMode.VelocityChange);
        this.transform.rotation = Quaternion.LookRotation((pos - this.transform.position).normalized);

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, (pos - this.transform.position).normalized, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enviroment")))
        {
            floorRange.transform.position = hit.point;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer).Equals("Enviroment"))
        {
            rigid.useGravity = false;
            rigid.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}

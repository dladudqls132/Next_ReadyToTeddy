using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Teddy_Container : MonoBehaviour
{
    private Transform target;
    [SerializeField] private ParticleSystem floorRange;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private bool isDrop_ready;
    [SerializeField] private bool isDrop;
    private Rigidbody rigid;
    private Vector3 dropPos;
    private float dropReadyTime;
    private float currentDropReadyTime;
    private float temp;
    //[SerializeField] private BoxCollider checkingColl;
    //[SerializeField] private BoxCollider realColl;
    private bool isReset;
    private Vector3 originPos;
    private Quaternion originRot;

    // Start is called before the first frame update
    void Start()
    {
        floorRange = Instantiate(floorRange);
        explosion = Instantiate(explosion);
        floorRange.gameObject.SetActive(false);
        explosion.gameObject.SetActive(false);
        target = GameManager.Instance.GetPlayer().transform;
        rigid = this.GetComponent<Rigidbody>();
        originPos = this.transform.position;
        originRot = this.transform.rotation;
    }

    private void FixedUpdate()
    {
        if(isReset)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            this.transform.position = Vector3.Lerp(this.transform.position, originPos, Time.deltaTime);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, originRot, Time.deltaTime);

            if(Vector3.Distance(this.transform.position, originPos) <= 0.5f)
            {
                isReset = false;
            }
        }

        if(isDrop_ready)
        {
            currentDropReadyTime += Time.deltaTime;

            if(currentDropReadyTime >= dropReadyTime)
            {
                SetDrop();
            }

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation((dropPos - this.transform.position).normalized), Time.deltaTime * 5);
        }
        else if(!isDrop)
        {
            temp += Time.deltaTime * 2.0f;

            this.transform.position = transform.position + new Vector3(0, Mathf.Cos(temp) / 170, 0);
        }
        else
        {
            rigid.velocity = rigid.velocity + rigid.velocity.normalized * 10 * Time.deltaTime;
        }
    }

    public bool SetDropReady(Vector3 dropPos, float time)
    {
        if (isDrop || isDrop_ready) return false;

        floorRange.gameObject.SetActive(true);
        isDrop_ready = true;
        this.dropPos = dropPos;
        dropReadyTime = time;

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, (dropPos - this.transform.position).normalized, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enviroment")))
        {
            floorRange.transform.position = hit.point;
            floorRange.Play();
        }

        return true;
    }

    public void SetNone()
    {
        rigid.constraints = RigidbodyConstraints.None;
        floorRange.Stop();
        floorRange.gameObject.SetActive(false);
        isDrop = false;
        isDrop_ready = false;
        rigid.useGravity = true;
        this.GetComponent<Boss_Teddy_Container>().enabled = false;
    }

    public void SetDrop()
    {
        rigid.constraints = RigidbodyConstraints.None;
        currentDropReadyTime = 0;
        floorRange.Stop();
        floorRange.gameObject.SetActive(false);
        isDrop = true;
        isDrop_ready = false;
        rigid.useGravity = true;
        rigid.AddForce((dropPos - this.transform.position).normalized * 60, ForceMode.VelocityChange);
        this.transform.rotation = Quaternion.LookRotation((dropPos - this.transform.position).normalized);
    }

    public void ResetPos()
    {
        currentDropReadyTime = 0;
        
        isDrop = false;
        isDrop_ready = false;
        isReset = true;

        rigid.useGravity = false;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.constraints = RigidbodyConstraints.FreezeAll;

        floorRange.Stop();
        floorRange.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDrop)
        {
            if (LayerMask.LayerToName(collision.gameObject.layer).Equals("Enviroment") || LayerMask.LayerToName(collision.gameObject.layer).Equals("Player") || LayerMask.LayerToName(collision.gameObject.layer).Equals("Root") || LayerMask.LayerToName(collision.gameObject.layer).Equals("Default"))
            {
                if (rigid.velocity.magnitude > 6)
                {
                    explosion.transform.position = collision.contacts[0].point;
                    explosion.gameObject.SetActive(true);
                    explosion.Play();
                    Camera.main.GetComponent<FPPCamController>().Shake(0.5f, 0.125f, true);

                    if(LayerMask.LayerToName(collision.gameObject.layer).Equals("Player"))
                    {
                        collision.transform.GetComponent<PlayerController>().DecreaseHp(20);
                    }
                    if(LayerMask.LayerToName(collision.gameObject.layer).Equals("Root"))
                    {
                        if(!collision.gameObject.CompareTag("Boss"))
                            collision.transform.GetComponent<Enemy>().DecreaseHp(60);
                    }
                }
                rigid.velocity = Vector3.zero;
                rigid.angularVelocity = Vector3.zero;
            }
        }
    }
}

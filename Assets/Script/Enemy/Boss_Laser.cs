using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Laser : MonoBehaviour
{
    private Transform parent;
    private BoxCollider coll;

    private void Awake()
    {
        coll = this.GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        Invoke("SetCollEnableTrue", 1.5f);
        this.transform.rotation = Quaternion.LookRotation(GameManager.Instance.GetPlayer().transform.position - this.transform.position);
    }

    private void OnDisable()
    {
        coll.enabled = false;
    }

    private void SetCollEnableTrue()
    {
        if(this.gameObject.activeSelf)
            coll.enabled = true;
    }

    public void SetParent(Transform t)
    {
        parent = t;
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(GameManager.Instance.GetPlayer().transform.position - this.transform.position), 0.2f);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(GameManager.Instance.GetPlayer().GetAimPos().position - this.transform.position), Time.deltaTime * 8);
        this.transform.position = parent.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DecreaseHp(20 * Time.deltaTime);
        }
    }
}

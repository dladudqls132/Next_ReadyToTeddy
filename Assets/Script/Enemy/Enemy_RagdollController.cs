using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct BodyPart
{
    public Transform transform;
    public Quaternion originRot;
    public Vector3 originPos;
}

public class Enemy_RagdollController : MonoBehaviour
{
    [SerializeField] private Rigidbody spineRigid;
    private float disappearTime = 10.0f;
    private float currentDisappearTime;
    [SerializeField] private List<BodyPart> bodyParts  = new List<BodyPart>();

    public void Init()
    {
        Component[] components = GetComponentsInChildren(typeof(Transform));

        //For each of the transforms, create a BodyPart instance and store the transform 
        foreach (Component c in components)
        {
            if (c.GetComponent<Rigidbody>() != null)
            {
                BodyPart bodyPart = new BodyPart();
                bodyPart.transform = c as Transform;
                bodyPart.originRot = bodyPart.transform.localRotation;
                bodyPart.originPos = bodyPart.transform.localPosition;
                bodyParts.Add(bodyPart);
            }
        }
    }


    public void Start()
    {
        Component[] components = GetComponentsInChildren(typeof(Transform));

        //For each of the transforms, create a BodyPart instance and store the transform 
        foreach (Component c in components)
        {
            if (c.GetComponent<Rigidbody>() != null)
            {
                BodyPart bodyPart = new BodyPart();
                bodyPart.transform = c as Transform;
                bodyPart.originRot = bodyPart.transform.localRotation;
                bodyPart.originPos = bodyPart.transform.localPosition;
                bodyParts.Add(bodyPart);
            }
        }
    }

    private void Update()
    {
        if (this.GetComponent<Enemy>().GetIsDead())
        {
            currentDisappearTime += Time.deltaTime;

            if (currentDisappearTime >= disappearTime)
            {
                currentDisappearTime = 0;
                ResetRagdoll();
            }
        }
    }

    void ResetRagdoll()
    {
        for(int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].transform.localPosition = bodyParts[i].originPos;
            bodyParts[i].transform.localRotation = bodyParts[i].originRot;
            bodyParts[i].transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bodyParts[i].transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        this.GetComponent<Animator>().enabled = true;
        this.gameObject.SetActive(false);
    }

    public void AddForce(Transform trs, Vector3 force)
    {
        //if(!trs.GetComponent<Enemy_RagdollController>())
        //    trs.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
        //else  
        //    spineRigid.AddForce(force, ForceMode.Impulse);
        
        for(int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bodyParts[i].transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        if(trs.CompareTag("Head"))
            spineRigid.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
        else
            trs.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
    }
}

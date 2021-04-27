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

    private void Update()
    {
        currentDisappearTime += Time.deltaTime;

        if(currentDisappearTime >= disappearTime)
        {
            currentDisappearTime = 0;
            ResetRagdoll();
        }
    }

    void ResetRagdoll()
    {
        for(int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].transform.localPosition = bodyParts[i].originPos;
            bodyParts[i].transform.localRotation = bodyParts[i].originRot;
        }

        this.gameObject.SetActive(false);
    }

    public void AddForce(Vector3 force)
    {
        spineRigid.AddForce(force, ForceMode.Impulse);
    }
}

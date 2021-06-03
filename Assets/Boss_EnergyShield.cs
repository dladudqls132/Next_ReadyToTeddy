using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_EnergyShield : MonoBehaviour
{
    [SerializeField] private int shieldHp;
    [SerializeField] private int currentShieldHp;
    [SerializeField] private List<Rigidbody> rigidbodies = new List<Rigidbody>();

    [SerializeField]  Transform parent;

    private void Start()
    {
        currentShieldHp = shieldHp;

        Component[] components = GetComponentsInChildren(typeof(Transform));

        //For each of the transforms, create a BodyPart instance and store the transform 
        foreach (Component c in components)
        {
            if (c.GetComponent<Rigidbody>() != null)
            {
                rigidbodies.Add(c.GetComponent<Rigidbody>());
            }
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < rigidbodies.Count; i++)
        {
            rigidbodies[i].isKinematic = true;
        }

        currentShieldHp = shieldHp;
        this.GetComponent<SphereCollider>().enabled = true;

        if(parent != null)
            this.transform.SetParent(parent);
    }

    void InvokeDestroy()
    {
        Destroy(this.gameObject);
    }

    public void DecreaseHp()
    {
        currentShieldHp -= 1;

        if(currentShieldHp <= 0)
        {
            this.GetComponent<SphereCollider>().enabled = false;
            parent = this.transform.parent;
            this.transform.SetParent(null);

            Invoke("InvokeDestroy", 10);

            for(int i = 0; i < rigidbodies.Count; i++)
            {
                rigidbodies[i].isKinematic = false;
                rigidbodies[i].AddForce((rigidbodies[i].transform.position - parent.position).normalized * 10, ForceMode.VelocityChange);
            }
        }
    }
}

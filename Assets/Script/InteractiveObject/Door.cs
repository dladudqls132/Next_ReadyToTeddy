using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private List<Transform> enemies = new List<Transform>();
    [SerializeField] private bool isOpened;
    private Collider[] coll;
    private Animator anim;

    private void Start()
    {
        coll = this.GetComponents<Collider>();
        anim = this.transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isAllDead = true;

        for(int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].gameObject.activeSelf)
            {
                isAllDead = false;
            }
        }
        
        if(isAllDead)
        {
            for(int i = 0; i < coll.Length; i++)
            {
                coll[i].enabled = false;
            }

            isOpened = true;
            anim.SetBool("isOpened", true);
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
           if(!enemies.Contains(other.transform))
            {
                enemies.Add(other.transform);
            }
        }
    }
}

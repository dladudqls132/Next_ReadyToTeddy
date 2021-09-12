using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Magma : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float tick;
    private float currentTick;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTick += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (currentTick >= tick)
            {
                GameManager.Instance.GetPlayer().DecreaseHp(damage);
                currentTick = 0;
            }
        }
    }
}

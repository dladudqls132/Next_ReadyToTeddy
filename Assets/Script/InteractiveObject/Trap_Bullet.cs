using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Bullet : MonoBehaviour
{
    [SerializeField] private Transform firePos;
    [SerializeField] private float fireRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float currentFireRate;
    private bool cansee;
    private bool isDetect;
    private float activeTime = 6;
    private float currentActiveTime;

    private void Start()
    {
        if(currentFireRate == 0)
            currentFireRate = Random.Range(0.0f, fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; 
        if(Physics.Raycast(this.transform.position, (GameManager.Instance.GetPlayer().transform.position - this.transform.position).normalized, out hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Trap") | 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Root"))))
        {
            if(hit.transform.CompareTag("Player"))
            {
                cansee = true;
            }
            else
            {
                if (Vector3.Distance(this.transform.position, GameManager.Instance.GetPlayer().transform.position) > 50.0f)
                {
                    cansee = false;
                }
            }
        }

        if (Vector3.Distance(this.transform.position, GameManager.Instance.GetPlayer().transform.position) <= 50.0f)
        {
            cansee = true;
        }

        if(cansee)
        {
            currentActiveTime = activeTime;
            isDetect = true;
        }
        else
        {
            currentActiveTime -= Time.deltaTime;

            if(currentActiveTime <= 0)
            {
                isDetect = false;
            }
        }

        if (!isDetect) return;

        currentFireRate += Time.deltaTime;

        if(currentFireRate >= fireRate)
        {
            Bullet tempBullet1 = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Normal).GetComponent<Bullet>();
            tempBullet1.gameObject.SetActive(true);
            tempBullet1.SetFire(firePos.position, firePos.forward, bulletSpeed, damage);

            currentFireRate = 0;
        }
    }
}

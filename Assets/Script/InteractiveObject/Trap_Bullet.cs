using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Bullet : MonoBehaviour
{
    [SerializeField] private Transform firePos;
    [SerializeField] private float fireRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float damage;
    private float currentFireRate;

    private void Start()
    {
        currentFireRate = Random.Range(0.0f, fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, GameManager.Instance.GetPlayer().transform.position) > 30.0f) return;

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

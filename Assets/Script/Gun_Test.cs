using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Test : Gun
{
    [SerializeField] private GameObject ammoHit;
    [SerializeField] private float spreadAngle;
    [SerializeField] private float fireNum;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < fireNum; i++)
            {
                float rndDmg = Random.Range(minDamage, maxDamage);
                GameObject bulletTemp = GameManager.Instance.GetBulletManager().GetBullet();

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 direction = (hit.point - shotPos.position).normalized;
                    float temp = Random.Range(-Mathf.PI, Mathf.PI);

                    Vector3 moveDir = direction + (Vector3.Cross(direction, Vector3.Cross(direction, Vector3.up)) * Mathf.Sin(temp) + Vector3.Cross(direction, Vector3.up) * Mathf.Cos(temp)) * Random.Range(0.0f, spreadAngle * Mathf.Deg2Rad);

                    RaycastHit hit2;
                    if(Physics.Raycast(shotPos.position, moveDir, out hit2, Mathf.Infinity))
                    {
                        Instantiate(ammoHit, hit2.point, Quaternion.identity);
                    }
                    //bulletTemp.GetComponent<Bullet>().SetFire(shotPos.position, direction, rndDmg, speed);
                }
            }

            //currentAmmo -= 1;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isReload = true;
        }

        if (isReload)
        {
            currentReloadTime -= Time.deltaTime;

            if (currentReloadTime <= 0)
            {
                isReload = false;
                currentReloadTime = reloadTime;
            }
        }
    }
}

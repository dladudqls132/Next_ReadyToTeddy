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
        if(Input.GetMouseButtonDown(0) && canShot)
        {
            isReload = false;

            for (int i = 0; i < fireNum; i++)
            {
                float rndDmg = Random.Range(minDamage, maxDamage);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    direction = (hit.point - shotPos.position).normalized;

                    //float temp = Random.Range(-Mathf.PI, Mathf.PI);
                    //Vector3 shotDir = direction + (Vector3.Cross(direction, Vector3.Cross(direction, Vector3.up)) * Mathf.Sin(temp) + Vector3.Cross(direction, Vector3.up) * Mathf.Cos(temp)) * Random.Range(0.0f, spreadAngle / 90);

                    //RaycastHit hit2;
                    //if(Physics.Raycast(shotPos.position, shotDir, out hit2, Mathf.Infinity))
                    //{
                    //    Instantiate(ammoHit, hit2.point, Quaternion.identity);
                    //}
                }
                else
                {
                    direction = Camera.main.transform.forward;
                }

                float temp = Random.Range(-Mathf.PI, Mathf.PI);
                Vector3 shotDir = direction + (Vector3.Cross(direction, Vector3.Cross(direction, Vector3.up)) * Mathf.Sin(temp) + Vector3.Cross(direction, Vector3.up) * Mathf.Cos(temp)) * Random.Range(0.0f, spreadAngle / 90);

                RaycastHit hit2;
                if (Physics.Raycast(shotPos.position, shotDir, out hit2, Mathf.Infinity))
                {
                    Instantiate(ammoHit, hit2.point, Quaternion.identity);
                }

                isShot = true;
            }

            currentAmmo--;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isReload = true;
        }

        if(isShot)
        {
            currentShotDelay -= Time.deltaTime;

            if(currentShotDelay <= 0)
            {
                isShot = false;
                currentShotDelay = shotDelay;
            }
        }

        if (isReload)
        {
            currentReloadTime -= Time.deltaTime;

            if (currentReloadTime <= 0)
            {                
                currentReloadTime = reloadTime;
                currentAmmo++;

                if(currentAmmo >= maxAmmo)
                {
                    isReload = false;
                }
            }
        }
        else
        {
            currentReloadTime = reloadTime;
        }

        if(currentAmmo <= 0)
        {
            isReload = true;
        }

        if (currentAmmo > 0 && !isShot)
            canShot = true;
        else
            canShot = false;
    }
}

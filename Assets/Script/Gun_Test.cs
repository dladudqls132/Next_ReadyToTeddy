using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Test : Gun
{
    [SerializeField] private GameObject bulletHit = null;
    [SerializeField] private float spreadAngle = 0;
    [SerializeField] private float fireNum = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(isShot)
        {
            currentShotDelay -= Time.deltaTime;

            if(currentShotDelay <= 0)
            {
                isShot = false;
                isRecoil = false;
                //Debug.Log("asd");
                currentShotDelay = shotDelay;
            }

            if(currentShotDelay <= shotDelay / 1.6f)
            {
                isRecoil = true;
            }
        }

        if (isReload)
        {
            currentReloadTime -= Time.deltaTime;

            if (currentReloadTime <= 0)
            {                
                currentReloadTime = reloadTime;
                currentAmmo = maxAmmo;

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

        if(currentAmmo <= 0 && !isShot)
        {
            isReload = true;
        }

        if (currentAmmo > 0 && !isShot)
            canShot = true;
        else
            canShot = false;
    }

    override public bool Fire()
    {
        if (canShot)
        {
            isReload = false;
            isRecoil = false;

            for (int i = 0; i < fireNum; i++)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Ignore Raycast"))))
                {
                    //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    //{
                    //    direction = (hit.point - shotPos.position).normalized;
                    //}
                    //else
                    //{
                    //    direction = Camera.main.transform.forward;
                    //}
                    direction = (hit.point - shotPos.position).normalized;
                }
                else
                    direction = Camera.main.transform.forward;

                float temp = Random.Range(-Mathf.PI, Mathf.PI);
                //Vector3 shotDir = direction + (Vector3.Cross(direction, Vector3.Cross(direction, Vector3.up)).normalized * Mathf.Sin(temp) + Vector3.Cross(direction, Vector3.up).normalized * Mathf.Cos(temp)) * Random.Range(0.0f, spreadAngle / 90);
                Vector3 shotDir = direction + (Camera.main.transform.up * Mathf.Sin(temp) + Camera.main.transform.right * Mathf.Cos(temp)) * Random.Range(0.0f, GameManager.Instance.GetPlayer().GetIsAiming() ? spreadAngle / 180 : spreadAngle / 180 * 2);

                Debug.DrawRay(shotPos.position, shotDir * 1000);

                RaycastHit hit2;
                if (Physics.Raycast(shotPos.position, shotDir, out hit2, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Collide))
                {
                    if (hit2.transform.CompareTag("Enemy"))
                    {
                        Enemy enemy = hit2.transform.GetComponent<Enemy>();
                        //enemy.SetCurrentHP(enemy.GetCurrentHP() - damagePerBullet);
                        enemy.DecreaseHp(owner, damagePerBullet, hit2.point, true);
                    }
                    else
                    {
                        GameObject tempObect = Instantiate(bulletHit, hit2.point, Quaternion.identity);
                        tempObect.transform.rotation = Quaternion.LookRotation(hit2.normal);
                        //tempObect.transform.SetParent(hit2.transform, true);
                        //tempObect.transform.localScale = new Vector3(1 / hit2.transform.localScale.x * bulletHit.transform.localScale.x, 1 / hit2.transform.localScale.y * bulletHit.transform.localScale.y, 1 / hit2.transform.localScale.z * bulletHit.transform.localScale.z);
                        tempObect.transform.SetParent(hit2.transform);
                    }
                }

      
                muzzleFlash.Play();
                isShot = true;
            }

            currentAmmo--;

            return true;
        }

        return false;
    }
}

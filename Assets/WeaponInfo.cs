using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "Scriptable Object Asset/Weapon Info")]
public class WeaponInfo : ScriptableObject
{
    [System.Serializable]
    public struct GunInfos
    {
        public GameObject prefab;
        public GunType gunType;
        public float damagePerBullet;
        public int maxAmmo;
        public int fireNum;
        public float spreadAngle_normal;
        public float spreadAngle_aiming;
        public float shotDelay;
    }

    [System.Serializable]
    public struct ProjecileInfos
    {
        public GameObject prefab;
        public ProjectileType projectileType;
        public float damage;
        public int maxHaveNum;
        public float remainingTime;
        public float explosionRadius;
        public float explosionPower;
    }

    public void SetInfo()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            Gun temp = guns[i].prefab.GetComponent<Gun>();
            temp.SetInfo(guns[i].gunType, guns[i].damagePerBullet, guns[i].maxAmmo, guns[i].fireNum, guns[i].spreadAngle_normal, guns[i].spreadAngle_aiming, guns[i].shotDelay);
        }

        for(int i = 0; i < projeciles.Length; i++)
        {
            Projectile temp = projeciles[i].prefab.GetComponent<Projectile>();
            temp.SetInfo(projeciles[i].damage, projeciles[i].explosionPower, projeciles[i].explosionRadius, projeciles[i].maxHaveNum, projeciles[i].remainingTime);
        }
    }

    public GunInfos[] guns;
    public ProjecileInfos[] projeciles;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float spawnRate;
    private float currentSpawnRate;
    private bool isSpawn;

    public GameObject GetMob() { return prefab; }

    private void Start()
    {
        prefab = Instantiate(prefab);
        prefab.transform.position = this.transform.position;
        prefab.transform.rotation = this.transform.rotation;
        prefab.SetActive(false);
    }

    public void SpawnMob()
    {
        prefab.transform.position = this.transform.position;
        prefab.transform.rotation = this.transform.rotation;
        prefab.GetComponent<Enemy>().SetDead(false);
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if(!isSpawn)
    //        currentSpawnRate += Time.deltaTime;
    //    else
    //    {
    //        if(prefab.GetComponent<Enemy>().GetIsDead())
    //        {
    //            isSpawn = false;
    //        }
    //    }

    //    if(currentSpawnRate >= spawnRate)
    //    {
    //        SpawnMob();

    //        isSpawn = true;
    //        currentSpawnRate = 0;
    //    }
    //}
}

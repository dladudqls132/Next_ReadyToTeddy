using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_ItemBox : MonoBehaviour
{
    [SerializeField] private GameObject[] boxes_prefab;
    [SerializeField] private List<GameObject> boxes = new List<GameObject>();
    [SerializeField] private Transform[] spawnPos;
    [SerializeField] private float spawnTime;
    [SerializeField] private Transform dest;
    [SerializeField] private float[] speed = new float[2];
    private int spawnerNum;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < boxes_prefab.Length; i++)
        {

            GameObject temp = Instantiate(boxes_prefab[i]);
            temp.transform.position = this.transform.position;
            boxes.Add(temp);
            temp.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            if (!boxes[i].activeSelf)
            {
                StartCoroutine(SpawnBox(i));
            }
            else
            {
                if (Vector3.Distance(boxes[i].transform.position, dest.position) < 10)
                {
                    speed[i] -= Time.deltaTime * 7;

                    speed[i] = Mathf.Clamp(speed[i], 0, 3);
                }

                boxes[i].transform.position = boxes[i].transform.position + (dest.position - boxes[i].transform.position).normalized * Time.deltaTime * speed[i];
            }
        }
    }

    IEnumerator SpawnBox(int num)
    {
        yield return new WaitForSeconds(spawnTime);

        speed[num] = 3;
        boxes[num].transform.position = spawnPos[spawnerNum].position;
        boxes[num].GetComponent<Item_Box>().Spawn();
        spawnerNum++;
        spawnerNum %= spawnPos.Length;
    }
}

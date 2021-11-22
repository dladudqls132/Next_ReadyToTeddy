using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private List<Transform> enemies = new List<Transform>();
    [SerializeField] private bool isOpened;
    private Collider[] coll;
    private Animator anim;
    private bool isEnter;
    [SerializeField] private int stageNum;

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

            GameManager.Instance.GetPlayerSaveData().SaveData(stageNum);
            GameManager.Instance.GetRemainingEnemy().gameObject.SetActive(false);
            isEnter = false;
            isOpened = true;
            anim.SetBool("isOpened", true);
            this.enabled = false;
        }

        if(isEnter)
        {
            int aliveNum = 0;

            for(int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i].gameObject.activeSelf)
                {
                    aliveNum++;
                }
            }

            GameManager.Instance.GetRemainingEnemy().gameObject.SetActive(true);
            GameManager.Instance.GetRemainingEnemy().SetNum(aliveNum);
        }
    }

    void ShakeCam()
    {
        GameManager.Instance.GetPlayer().GetCam().Shake(1.3f, 0.2f, true);
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
        if(other.CompareTag("Player"))
        {
            isEnter = true;
        }
    }
}

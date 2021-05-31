using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private int roomNum;
    [SerializeField] private bool isCleared;
    [SerializeField] private bool isEnterPlayer;

    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    public int GetRoomNum() { return roomNum; }
    public bool GetIsEnterPlayer() { return isEnterPlayer; }

    private void Update()
    {
        bool tempCleared = true;

        for(int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].GetIsDead())
                tempCleared = false;
        }

        isCleared = tempCleared;

        if (isEnterPlayer)
        {
            if (!isCleared)
                GameManager.Instance.SetIsCombat(true);
            else
                GameManager.Instance.SetIsCombat(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<RoomInfo>() != null)
            other.GetComponent<RoomInfo>().SetRoom(this.transform.root.GetComponent<Room>());

        if(other.CompareTag("Enemy"))
        {
            if(!enemies.Contains(other.GetComponent<Enemy>()))
                enemies.Add(other.GetComponent<Enemy>());
        }
        else if(other.CompareTag("Player"))
        {
            isEnterPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isEnterPlayer = false;
        }
    }
}

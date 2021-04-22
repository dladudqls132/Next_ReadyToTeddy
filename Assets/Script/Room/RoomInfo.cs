using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    [SerializeField] private Room currentRoom;

    public void SetRoom(Room room) { currentRoom = room; }
    public Room GetRoom() { return currentRoom; }
}

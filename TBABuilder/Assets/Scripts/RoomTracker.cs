using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTracker : MonoBehaviour
{
    [SerializeField] Room[] rooms = null;
    Room currentRoom = null;

    private void Start()
    {
        currentRoom = rooms[0];
        foreach (Room room in rooms)
        {
            // Instantiates all RoomObjects so the original doesn't get changed during runtime
            room.prepareRoomObjects();
        }
    }

    public Room getCurrentRoom()
    {
        return currentRoom;
    }

    public RoomObject findObjectInCurrentRoom(string objName)
    {
        return currentRoom.findObjectInRoom(objName);
    }
}

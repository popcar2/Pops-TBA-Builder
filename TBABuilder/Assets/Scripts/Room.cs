using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Room", order = 0)]
public class Room : ScriptableObject
{
    // realRoomObjects is what the game starts with but the issue is that the game alters objects permanently.
    // Therefore, realRoomObjects is copied into roomObjects 
    [SerializeField] List<RoomObject> realRoomObjects;
    [SerializeField] List<RoomObject> roomObjects;

    [TextArea(1, 10)] [SerializeField] public string roomText; 

    public void prepareRoomObjects()
    {
        roomObjects = new List<RoomObject>();
        foreach (RoomObject obj in realRoomObjects)
        {
            RoomObject temp = Instantiate(obj);
            temp.name = obj.name;
            roomObjects.Add(temp);
        }
    }

    public List<RoomObject> getRoomObjects()
    {
        return roomObjects;
    }

    public RoomObject findObjectInRoom(string objName)
    {
        Player player = FindObjectOfType<Player>();

        // Look in player's inventory first
        foreach (RoomObject obj in player.getInventory())
        {
            if (obj.name.ToLower() == objName)
            {
                return obj;
            }
        }

        // Look in this room
        foreach (RoomObject obj in roomObjects)
        {
            if (obj.name.ToLower() == objName)
            {
                return obj;
            }
        }

        return null;
    }

    public void removeRoomObject(RoomObject obj)
    {
        roomObjects.Remove(obj);
    }

    public void addRoomObject(RoomObject obj)
    {
        roomObjects.Add(obj);
    }
}

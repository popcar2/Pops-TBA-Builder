using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Room", order = 0)]
public class Room : ScriptableObject
{
    // roomObjects is what the game starts with but the issue is that the game alters objects permanently.
    // Therefore, roomObjects is copied into copiedRoomObjects 
    [SerializeField] List<RoomObject> roomObjects;
    List<RoomObject> copiedRoomObjects;

    [TextArea(1, 10)] [SerializeField] public string roomText;
    [HideInInspector] public List<RoomConnectionVars> roomConnections = new List<RoomConnectionVars>();


    [Serializable]
    public class RoomConnectionVars
    {
        public Room room;
        public string roomAlias;
    }

    public void prepareRoomObjects()
    {
        copiedRoomObjects = new List<RoomObject>();
        foreach (RoomObject obj in roomObjects)
        {
            RoomObject temp = Instantiate(obj);
            temp.name = obj.name;
            copiedRoomObjects.Add(temp);
        }
    }

    public List<RoomObject> getRoomObjects()
    {
        return copiedRoomObjects;
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
        foreach (RoomObject obj in copiedRoomObjects)
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
        copiedRoomObjects.Remove(obj);
    }

    public void addRoomObject(RoomObject obj)
    {
        copiedRoomObjects.Add(obj);
    }
}

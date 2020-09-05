using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Room", order = 0)]
public class Room : ScriptableObject
{
    [NonSerialized] public bool isInitialized = false;

    [SerializeField] public List<RoomObject> roomObjects;
    [TextArea(1, 10)] [SerializeField] public string roomText;
    [HideInInspector] public List<RoomConnectionVars> roomConnections = new List<RoomConnectionVars>();

    [HideInInspector] public List<RoomObject> runtimeRoomObjects = new List<RoomObject>();
    [HideInInspector] public string runtimeRoomText;
    [HideInInspector] public List<RoomConnectionVars> runtimeRoomConnections = new List<RoomConnectionVars>();

    public void initializeRuntimeVariables()
    {
        isInitialized = true;
        runtimeRoomObjects = roomObjects.ToList();
        runtimeRoomText = roomText;
        runtimeRoomConnections = roomConnections;

        foreach (RoomObject obj in runtimeRoomObjects)
        {
            if (obj.isInitialized == false)
                obj.initializeRuntimeVariables();
        }
    }


    [Serializable]
    public class RoomConnectionVars
    {
        public Room room;
        public string roomAlias;
    }

    public void removeRoomObject(RoomObject obj)
    {
        runtimeRoomObjects.Remove(obj);
    }

    public void addRoomObject(RoomObject obj)
    {
        runtimeRoomObjects.Add(obj);
    }
}

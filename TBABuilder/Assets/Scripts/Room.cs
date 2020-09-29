using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Room", order = 0)]
public class Room : ScriptableObject
{
    [NonSerialized] public bool isInitialized = false;

    [SerializeField] public List<RoomObject> roomObjects;
    [TextArea(1, 10)] [SerializeField] public string roomEntryText;
    [TextArea(1, 10)] [SerializeField] public string lookText;
    [HideInInspector] public List<RoomConnectionVars> roomConnections = new List<RoomConnectionVars>();

    [HideInInspector] public List<RoomObject> runtimeRoomObjects = new List<RoomObject>();
    [HideInInspector] public string runtimeRoomEntryText;
    [HideInInspector] public string runtimeLookText;
    [HideInInspector] public List<RoomConnectionVars> runtimeRoomConnections = new List<RoomConnectionVars>();

    [HideInInspector] public List<RoomObject.EditorVariables> roomEntryActions = new List<RoomObject.EditorVariables>();

    /// <summary>
    /// Initializes runtime variables, is called when the player enters the room.
    /// </summary>
    public void initializeRuntimeVariables()
    {
        isInitialized = true;
        runtimeRoomObjects = roomObjects.ToList();

        // In case the room text was changed via an action before initialization
        if (String.IsNullOrEmpty(runtimeLookText))
            runtimeLookText = lookText;
        if (String.IsNullOrEmpty(runtimeRoomEntryText))
            runtimeRoomEntryText = roomEntryText;

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
        public string roomInactiveText;
        public bool isActive = true;
    }

    public void removeRoomObject(RoomObject obj)
    {
        runtimeRoomObjects.Remove(obj);
    }

    public void addRoomObject(RoomObject obj)
    {
        runtimeRoomObjects.Add(obj);
    }

    // Is needed to reset runtime room text when the game starts.
    private void OnEnable()
    {
        runtimeLookText = lookText;
        runtimeRoomEntryText = roomEntryText;
    }
}

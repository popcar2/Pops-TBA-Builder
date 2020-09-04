using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTracker : MonoBehaviour
{
    [SerializeField] Room startingRoom;
    Room currentRoom = null;

    TextPrompt textPrompt;
    ObjectFinder objectFinder;
    Player player;

    private void Start()
    {
        textPrompt = FindObjectOfType<TextPrompt>();
        objectFinder = FindObjectOfType<ObjectFinder>();
        player = FindObjectOfType<Player>();

        currentRoom = startingRoom;

        currentRoom.initializeRuntimeVariables();
    }

    public Room getCurrentRoom()
    {
        return currentRoom;
    }

    public RoomObject findObjectInCurrentRoom(string objName)
    {
        // Look in player's inventory first
        foreach (RoomObject obj in player.getInventory())
        {
            if (obj.name == objName)
            {
                return obj;
            }
        }

        // Look in player's equipped items
        foreach (RoomObject obj in player.getEquippedItems())
        {
            if (obj.name == objName)
            {
                return obj;
            }
        }

        // Look in this room
        foreach (RoomObject obj in currentRoom.runtimeRoomObjects)
        {
            if (obj.name == objName)
            {
                return obj;
            }
        }

        return null;
    }

    public void forceChangeRoom(Room room)
    {
        if (room == null)
        {
            textPrompt.printText("\nThere's nothing in that direction.");
            return;
        }

        if (room.isInitialized == false)
            room.initializeRuntimeVariables();

        currentRoom = room;
        textPrompt.printText("\n" + currentRoom.runtimeRoomText);
    }

    public void changeRoomViaRoomConnection(string userInput)
    {
        Room newRoom = findRoomConnection(userInput);

        if (newRoom == null)
        {
            textPrompt.printText($"\nThere's nothing in that direction.");
            return;
        }

        if (newRoom.isInitialized == false)
            newRoom.initializeRuntimeVariables();

        currentRoom = newRoom;
        textPrompt.printText("\n" + currentRoom.runtimeRoomText);
    }

    private Room findRoomConnection(string userInput)
    {
        Room newRoom = null;

        foreach (Room.RoomConnectionVars roomConnection in currentRoom.runtimeRoomConnections)
        {
            // No aliases
            if (string.IsNullOrWhiteSpace(roomConnection.roomAlias))
            {
                if (userInput.ToLower().Contains(roomConnection.room.name.ToLower()))
                {
                    newRoom = roomConnection.room;
                }
            }

            // Has aliases
            else
            {
                string[] roomAliases = roomConnection.roomAlias.Split(',');
                foreach (string roomAlias in roomAliases)
                {
                    if (userInput.ToLower().Contains(roomAlias.ToLower()))
                    {
                        newRoom = roomConnection.room;
                        break;
                    }
                }
            }

            if (newRoom != null)
            {
                break;
            }
        }

        return newRoom;
    }
}

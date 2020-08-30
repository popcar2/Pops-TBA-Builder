using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTracker : MonoBehaviour
{
    [SerializeField] Room[] rooms = null;
    Room currentRoom = null;

    TextPrompt textPrompt;

    private void Start()
    {
        currentRoom = rooms[0];
        foreach (Room room in rooms)
        {
            // Instantiates all RoomObjects so the original doesn't get changed during runtime
            room.prepareRoomObjects();
        }

        textPrompt = FindObjectOfType<TextPrompt>();
    }

    public Room getCurrentRoom()
    {
        return currentRoom;
    }

    public RoomObject findObjectInCurrentRoom(string objName)
    {
        return currentRoom.findObjectInRoom(objName);
    }

    public void findRoomFromInput(string input)
    {
        Room newRoom = null;
        
        foreach (Room.RoomConnectionVars roomConnection in currentRoom.roomConnections)
        {
            // No aliases
            if (string.IsNullOrWhiteSpace(roomConnection.roomAlias))
            {
                if (input.ToLower().Contains(roomConnection.room.name.ToLower()))
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
                    if (input.ToLower().Contains(roomAlias.ToLower()))
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

        if (newRoom == null)
        {
            textPrompt.printText($"\nThere's nothing in that direction.");
        }
        else
        {
            currentRoom = newRoom;
            textPrompt.printText("\n" + currentRoom.roomText);
        }
    }

    public void forceChangeRoom(Room room)
    {
        currentRoom = room;
        textPrompt.printText("\n" + currentRoom.roomText);
    }
}

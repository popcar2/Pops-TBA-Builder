using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTracker : MonoBehaviour
{
    [SerializeField] Room startingRoom = null;
    Room currentRoom = null;

    TextPrompt textPrompt;
    Player player;

    private void Start()
    {
        textPrompt = FindObjectOfType<TextPrompt>();
        player = FindObjectOfType<Player>();
        currentRoom = startingRoom;
        currentRoom.initializeRuntimeVariables();
        textPrompt.printText(currentRoom.roomText);
    }

    public void printCurrentRoomText()
    {
        string roomText = currentRoom.runtimeRoomText;

        if (System.String.IsNullOrEmpty(roomText))
        {
            textPrompt.printText("\n(You forgot to add Room Text for this current room)");
        }
        else
        {
            textPrompt.printText("\n" + roomText);
        }
    }

    public Room getCurrentRoom()
    {
        return currentRoom;
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

        // Is delayed so the flavor text can be printed before room text.
        StartCoroutine(textPrompt.printTextAfterTime("\n" + currentRoom.runtimeRoomText, 0.1f));
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

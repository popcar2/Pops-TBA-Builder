using UnityEngine;

public class RoomTracker : MonoBehaviour
{
    [SerializeField] Room startingRoom = null;
    Room currentRoom = null;

    TextPrompt textPrompt;
    DefaultValues defaultValues;
    ActionHandler actionHandler;

    private void Start()
    {
        textPrompt = FindObjectOfType<TextPrompt>();
        defaultValues = FindObjectOfType<DefaultValues>();
        actionHandler = FindObjectOfType<ActionHandler>();

        currentRoom = startingRoom;
        currentRoom.initializeRuntimeVariables();
        textPrompt.printText(currentRoom.roomEntryText);
        actionHandler.executeActions(currentRoom, currentRoom.roomEntryActions);
    }

    public void printCurrentRoomText()
    {
        string roomText = currentRoom.runtimeLookText;

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
            Debug.Log("You forgot to add a target room to move in this action.");
            return;
        }

        if (room.isInitialized == false)
            room.initializeRuntimeVariables();

        currentRoom = room;

        // Is delayed so the flavor text can be printed before room text.
        actionHandler.executeActions(currentRoom, currentRoom.roomEntryActions);
        StartCoroutine(textPrompt.printTextAfterDelay("\n" + currentRoom.runtimeRoomEntryText, 0.1f));
    }

    public void changeRoomViaRoomConnection(string userInput)
    {
        Room newRoom = findRoomConnection(userInput);

        if (newRoom == null)
            return;

        if (newRoom.isInitialized == false)
            newRoom.initializeRuntimeVariables();

        currentRoom = newRoom;
        actionHandler.executeActions(currentRoom, currentRoom.roomEntryActions);
        textPrompt.printText("\n" + currentRoom.runtimeRoomEntryText);
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
                    // If room inactive
                    if (!roomConnection.isActive)
                    {
                        textPrompt.printText("\n" + roomConnection.roomInactiveText);
                        return null;
                    }
                    newRoom = roomConnection.room;
                    break;
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
                        // If room inactive
                        if (!roomConnection.isActive)
                        {
                            textPrompt.printText("\n" + roomConnection.roomInactiveText);
                            return null;
                        }
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
            textPrompt.printText("\n" + defaultValues.roomNotFoundText);
        }

        return newRoom;
    }
}

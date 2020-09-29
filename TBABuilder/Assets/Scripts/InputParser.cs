using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;

public class InputParser : MonoBehaviour
{
    TextPrompt textPrompt;
    RoomTracker roomTracker;
    Player player;
    ActionHandler actionHandler;
    DefaultValues defaultValues;

    private void Start()
    {
        textPrompt = FindObjectOfType<TextPrompt>();
        roomTracker = FindObjectOfType<RoomTracker>();
        player = FindObjectOfType<Player>();
        actionHandler = FindObjectOfType<ActionHandler>();
        defaultValues = FindObjectOfType<DefaultValues>();
    }

    /// <summary>
    /// Returns all words in user input separated by a space as a string array. Also removes additional whitespace. 
    /// </summary>
    /// <param name="text"></param>
    private string[] trimText(string text)
    {
        StringBuilder sb = new StringBuilder();
        bool spaceFlag = false;

        // Remove repeated whitespace
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == ' ')
            {
                if (spaceFlag != true)
                {
                    sb.Append(' ');
                }
                spaceFlag = true;
            }
            else
            {
                sb.Append(text[i]);
                spaceFlag = false;
            }
        }

        // Remove starting and ending whitespace
        if (sb.Length > 2)
        {
            if (sb[0] == ' ')
            {
                sb.Remove(0, 1);
            }
            if (sb[sb.Length - 1] == ' ')
            {
                sb.Remove(sb.Length - 1, 1);
            }
        }
        

        // Transform it into a string array, make another string array to remove unnecessary words
        string[] words = sb.ToString().ToLower().Split(' ');

        return words;
    }

    // Helper function for findObjectFromInput, to easily iterate through inventory, equipment, and objects in the room
    private RoomObject findObjectFromList(string input, List<RoomObject> roomObjects)
    {
        foreach (RoomObject roomObject in roomObjects)
        {
            // No aliases
            if (String.IsNullOrWhiteSpace(roomObject.objectAliases))
            {
                if (input.ToLower().Contains(roomObject.name.ToLower()))
                {
                    return roomObject;
                }
            }

            else
            {
                // Has aliases
                string[] roomObjectAliases = roomObject.objectAliases.Split(',');
                foreach (string alias in roomObjectAliases)
                {
                    if (input.ToLower().Contains(alias.ToLower()))
                    {
                        return roomObject;
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Finds and returns the RoomObject with the same name or alias in input. Searches in current room, inventory, and equipment. Returns null if nothing found.
    /// </summary>
    /// <param name="input">The user's input</param>
    private RoomObject findObjectFromInput(string input)
    {
        RoomObject foundObject;

        foundObject = findObjectFromList(input, roomTracker.getCurrentRoom().runtimeRoomObjects);

        if (foundObject != null)
            return foundObject;

        foundObject = findObjectFromList(input, player.getInventory());

        if (foundObject != null)
            return foundObject;

        foundObject = findObjectFromList(input, player.getEquippedItems());

        return foundObject;
    }

    /// <summary>
    /// Scrapes user input to find commands and the object referenced and directs it to ActionHandler.
    /// </summary>
    /// <param name="input">The user's input</param>
    public void parseInput(string input)
    {
        string[] words = trimText(input);

        // Player commands are separate from regular commands because actions are in the Player script rather than ActionHandler
        // and regular commands need a minimum of 2 words to work whereas player commands (like "inv") can be one word.
        // Check player commands. If one activates, return.

        string failText = "\n" + defaultValues.unknownCommand;

        if (words.Length == 0)
        {
            textPrompt.printText(failText);
            return;
        }

        string command = words[0];
        RoomObject targetObject = findObjectFromInput(input);

        // parsePlayerCommands needs input to scrape room name from input when player moves.
        bool playerCommandSuccess = parsePlayerCommands(command, input);
        if (playerCommandSuccess)
            return;

        if (words.Length == 1)
        {
            textPrompt.printText(failText);
            return;
        }

        // Regular Commands: Eat, Talk, Kill, Sit, Use, Pickup, Wear
        parseRegularCommands(command, targetObject);
        
    }

    /// <summary>
    /// Checks if the string is a regular command and forwards targetObject to ActionHandler.
    /// </summary>
    /// <param name="command">The first word in the user's input</param>
    /// <param name="targetObject">The object found in user's input</param>
    private void parseRegularCommands(string command, RoomObject targetObject)
    {
        switch (command)
        {
            // eat
            case "eat":
            case "devour":
            case "consume":
                actionHandler.eatObject(targetObject);
                break;
            // drink
            case "drink":
                actionHandler.drinkObject(targetObject);
                break;
            // talk
            case "talk":
            case "speak":
            case "say":
                actionHandler.talkToPerson(targetObject);
                break;
            // kill
            case "kill":
            case "stab":
            case "cleave":
            case "murder":
                actionHandler.killPerson(targetObject);
                break;
            // break
            case "break":
            case "smash":
            case "destroy":
                actionHandler.BreakObject(targetObject);
                break;
            // sit
            case "sit":
            case "seat":
                actionHandler.sitOnObject(targetObject);
                break;
            // use
            case "use":
            case "activate":
                actionHandler.useObject(targetObject);
                break;
            // pickup
            case "pickup":
            case "grab":
            case "pick":
            case "take":
            case "steal":
                actionHandler.pickupObject(targetObject);
                break;
            // wear
            case "wear":
            case "equip":
            case "put":
                actionHandler.wearObject(targetObject);
                break;
            // open
            case "open":
            case "unlock":
            case "reveal":
                actionHandler.openObject(targetObject);
                break;
            // look at
            case "look":
                actionHandler.lookAtObject(targetObject);
                break;
            // unknown command
            default:
                textPrompt.printText("\n" + defaultValues.unknownCommand);
                break;
        }
    }

    /// <summary>
    /// Checks if the command is a player command and calls appropriate methods. Returns false if unsuccessful.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="input"></param>
    /// <returns>Returns true if successful</returns>
    private bool parsePlayerCommands(string command, string input)
    {
        bool successFlag = true;

        switch (command)
        {
            case "inv":
            case "inventory":
            case "i":
                player.openInventory();
                break;
            case "look":
                if (input.Split(' ').Length != 1)
                {
                    successFlag = false;
                    break;
                }
                roomTracker.printCurrentRoomText();
                break;
            case "move":
            case "go":
                roomTracker.changeRoomViaRoomConnection(input);
                break;
            // Fluff/secret commands
            case "die":
                textPrompt.printText("\n\"Eh, guess I'll die\". You suffocate yourself to death. Not sure why you'd want to do that.");
                textPrompt.killPlayer();
                break;
            default:
                successFlag = false;
                break;
        }

        return successFlag;
    }

    /// <summary>
    /// Prints flavor text or default success/failure of a generic command.
    /// </summary>
    /// <param name="successBool">Whether the command was successful or not</param>
    /// <param name="defaultSuccess">Default success flavor text</param>
    /// <param name="defaultFailure">Default failure flavor text</param>
    /// <param name="objFlavorText">The object's custom flavor text. Leave empty or null if it doesn't exist.</param>
    public void printResponse(bool successBool, string defaultSuccess, string defaultFailure, ref string objFlavorText)
    {
        bool hasFlavorText = !String.IsNullOrEmpty(objFlavorText);

        if (successBool)
        {
            // Success
            if (!hasFlavorText)
            {
                textPrompt.printText("\n" + defaultSuccess);
            }
        }
        else
        {
            // Failure 
            if (!hasFlavorText)
            {
                textPrompt.printText("\n" + defaultFailure);
            }
        }

        // print flavor text regardless
        if (hasFlavorText)
        {
            textPrompt.printText("\n" + objFlavorText);
        }
    }
}

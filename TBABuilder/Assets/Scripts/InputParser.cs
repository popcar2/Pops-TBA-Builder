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

    private string[] trimText(string text)
    {
        // TODO: Remove unnecessary elements of trimText since text parsing was rewritten
        StringBuilder sb = new StringBuilder();
        bool spaceFlag = false;

        // Remove repeated whitespace so no error is caused
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
        string[] wordsTemp = sb.ToString().ToLower().Split(' ');
        string[] words = new string[wordsTemp.Length];
        int wordsLength = 0;

        // Remove fluff words
        foreach (string s in wordsTemp)
        {
            switch (s)
            {
                case "to":
                case "as":
                case "the":
                case "on":
                case "is":
                case "was":
                case "in":
                case "at":
                case "a":
                case "of":
                    break;
                default:
                    words[wordsLength] = s;
                    wordsLength++;
                    break;
            }
        }
        Array.Resize(ref words, wordsLength);
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

    private RoomObject findObjectFromInput(string input)
    {
        RoomObject foundObject = null;

        foundObject = findObjectFromList(input, roomTracker.getCurrentRoom().runtimeRoomObjects);

        if (foundObject != null)
            return foundObject;

        foundObject = findObjectFromList(input, player.getInventory());

        if (foundObject != null)
            return foundObject;

        foundObject = findObjectFromList(input, player.getEquippedItems());

        return foundObject;
    }

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

    // Is a bool because the main InputParser function would know whether to keep going or return
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

    // Returns whether the operation was a success or a failure. objCheck is the bool you use to check objects, such as obj.IsEdible
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

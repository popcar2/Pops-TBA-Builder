using System.Collections;
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
    private void Start()
    {
        textPrompt = FindObjectOfType<TextPrompt>();
        roomTracker = FindObjectOfType<RoomTracker>();
        player = FindObjectOfType<Player>();
        actionHandler = FindObjectOfType<ActionHandler>();
    }

    private string[] trimText(string text)
    {
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

    public void parseInput(string input)
    {
        string[] words = trimText(input);

        // Player commands are separate from regular commands because actions are in the Player script rather than ActionHandler
        // and regular commands need a minimum of 2 words to work whereas player commands (like "inv") can be one word.
        // Check player commands. If one activates, return.

        string failText = "\nUnknown Command, type \"help\" for the list of commands";
        if (words.Length == 0)
        {
            textPrompt.printText(failText);
            return;
        }

        bool playerCommandSuccess = parsePlayerCommands(words);
        if (playerCommandSuccess)
            return;

        if (words.Length == 1)
        {
            textPrompt.printText(failText);
            return;
        }

        // Regular Commands: Eat, Talk, Kill, Sit, Use, Pickup, Wear
        parseRegularCommands(words);
        
    }

    private void parseRegularCommands(string[] words)
    {
        switch (words[0])
        {
            // eat
            case "eat":
            case "devour":
            case "consume":
            case "drink":
                actionHandler.eatObject(words[1]);
                break;
            // talk
            case "talk":
            case "speak":
            case "say":
                actionHandler.talkToPerson(words[1]);
                break;
            // kill
            case "kill":
            case "destroy":
            case "break":
            case "stab":
            case "cleave":
            case "murder":
                actionHandler.killPerson(words[1]);
                break;
            // sit
            case "sit":
            case "seat":
                actionHandler.sitOnObject(words[1]);
                break;
            // use
            case "use":
            case "activate":
                actionHandler.useObject(words[1]);
                break;
            // pickup
            case "pickup":
            case "grab":
            case "pick":
            case "steal":
                if (words[1] == "up")
                {
                    actionHandler.pickupObject(words[2]);
                }
                else
                {
                    actionHandler.pickupObject(words[1]);
                }
                break;
            // wear
            case "wear":
            case "equip":
            case "put":
                actionHandler.wearObject(words[1]);
                break;
            // unknown command
            default:
                textPrompt.printText("\nWhat is \"" + words[0] + "\"?");
                break;
        }
    }

    // Is a bool because the main InputParser function would know whether to keep going or return
    private bool parsePlayerCommands(string[] words)
    {
        // It's easier to set successflag as true then set it as false if it went to switch's default rather than set it to true every other case
        bool successFlag = true;

        switch (words[0])
        {
            case "inv":
            case "inventory":
            case "i":
                player.openInventory();
                break;
            case "look":
                actionHandler.printCurrentRoomText();
                break;
            case "move":
            case "go":
                roomTracker.changeRoom(words[1]);
                break;
            // Fluff/secret commands
            case "die":
                textPrompt.printText("\n\"Eh, guess I'll die\". You suffocate yourself to death. Not sure why you'd want to do that.");
                textPrompt.killPlayer();
                break;
            case "dab":
                textPrompt.printText("\nYou unleash a sick dab similar to the ones you find on the youtubes, " +
                    "and suddenly you begin to hear the ground quake. The earth crumbles before you and flies into the sun. This is all your fault.");
                textPrompt.killPlayer();
                break;
            default:
                successFlag = false;
                break;
        }

        return successFlag;
    }

    // Returns whether the operation was a success or a failure. objCheck is the bool you use to check objects, such as obj.IsEdible
    public bool printResponse(RoomObject obj, bool objCheck, string defaultSuccess, string defaultFailure, string objFlavorText)
    {
        bool success = false;
        bool hasFlavorText = !String.IsNullOrEmpty(objFlavorText);

        if (objCheck)
        {
            // Success
            if (!hasFlavorText)
            {
                textPrompt.printText("\n" + defaultSuccess);
            }
            success = true;
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

        return success;
    }

    public RoomObject findRoomObjectPrintOtherwise(string objName)
    {
        RoomObject obj = roomTracker.findObjectInCurrentRoom(objName);

        if (obj == null)
        {
            textPrompt.printText("\nThere is no " + objName + " in this room");
        }

        return obj;
    }
}

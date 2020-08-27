using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    InputParser inputParser;
    RoomTracker roomTracker;
    Player player;
    TextPrompt textPrompt;

    private void Start()
    {
        inputParser = FindObjectOfType<InputParser>();
        roomTracker = FindObjectOfType<RoomTracker>();
        player = FindObjectOfType<Player>();
        textPrompt = FindObjectOfType<TextPrompt>();
    }

    public void executeActions(RoomObject obj, List<RoomObject.EditorVariables> objVars)
    {
        string objName = obj.name;
        for (int i = 0; i < objVars.Count; i++)
        {
            RoomObject.ActionCategory category = objVars[i].actionCategory;

            // Execute player actions
            if (category == RoomObject.ActionCategory.PlayerActions)
            {
                RoomObject.PlayerAction playerAction = objVars[i].playerAction;

                switch (playerAction)
                {
                    case RoomObject.PlayerAction.AddToInventory:
                        player.addItemToInventory(obj);
                        break;
                    case RoomObject.PlayerAction.RemoveFromInventory:
                        player.removeItemFromInventory(obj);
                        break;
                    case RoomObject.PlayerAction.KillPlayer:
                        textPrompt.killPlayer();
                        textPrompt.printText("\n--YOU DIED-- (Press any button to continue)");
                        break;
                    case RoomObject.PlayerAction.EquipItem:
                        player.equipItem(obj);
                        break;
                    case RoomObject.PlayerAction.RemoveEquippedItem:
                        player.removeEquippedItem(obj);
                        break;
                    default:
                        Debug.Log($"Unknown PlayerAction enum at {obj.name}: you forgot to add what to do in ActionHandler!");
                        break;
                }
            }
            else if (category == RoomObject.ActionCategory.ObjectActions)
            {
                RoomObject.ObjectAction objectAction = objVars[i].objectAction;

                // Execute object actions
                switch (objectAction)
                {
                    case RoomObject.ObjectAction.DestroyThisObject:
                        player.removeItemFromInventory(obj);
                        roomTracker.getCurrentRoom().removeRoomObject(obj);
                        break;
                    case RoomObject.ObjectAction.SetIsEdible:
                        obj.isEdible = objVars[i].varsToChange.isEdible;
                        break;
                    case RoomObject.ObjectAction.SetIsTalkable:
                        obj.isTalkable = objVars[i].varsToChange.isTalkable;
                        break;
                    case RoomObject.ObjectAction.SetIsKillable:
                        obj.isKillable = objVars[i].varsToChange.isKillable;
                        break;
                    case RoomObject.ObjectAction.SetIsSittable:
                        obj.isSittable = objVars[i].varsToChange.isSittable;
                        break;
                    case RoomObject.ObjectAction.SetIsUsable:
                        obj.isUsable = objVars[i].varsToChange.isUsable;
                        break;
                    case RoomObject.ObjectAction.SetIsWearable:
                        obj.isWearable = objVars[i].varsToChange.isWearable;
                        break;

                    case RoomObject.ObjectAction.ChangeEdibleFlavorText:
                        obj.EdibleFlavorText = objVars[i].varsToChange.edibleFlavorText;
                        break;
                    case RoomObject.ObjectAction.ChangeKillableFlavorText:
                        obj.KillableFlavorText = objVars[i].varsToChange.killableFlavorText;
                        break;
                    case RoomObject.ObjectAction.ChangePickupableFlavorText:
                        obj.PickupableFlavorText = objVars[i].varsToChange.pickupableFlavorText;
                        break;
                    case RoomObject.ObjectAction.ChangeSittableFlavorText:
                        obj.SittableFlavorText = objVars[i].varsToChange.sittableFlavorText;
                        break;
                    case RoomObject.ObjectAction.ChangeTalkableFlavorText:
                        obj.TalkableFlavorText = objVars[i].varsToChange.talkableFlavorText;
                        break;
                    case RoomObject.ObjectAction.ChangeUsableFlavorText:
                        obj.UsableFlavorText = objVars[i].varsToChange.usableFlavorText;
                        break;
                    case RoomObject.ObjectAction.ChangeWearableFlavorText:
                        obj.WearableFlavorText = objVars[i].varsToChange.wearableFlavorText;
                        break;

                    default:
                        Debug.Log($"Unknown ObjectAction enum at {obj.name}: you forgot to add what to do in ActionHandler!");
                        break;
                }
            }
            else if (category == RoomObject.ActionCategory.RoomActions)
            {
                RoomObject.RoomAction roomAction = objVars[i].roomAction;
                Room currentRoom = roomTracker.getCurrentRoom();

                if (roomAction == RoomObject.RoomAction.AddObjectToRoom)
                {
                    currentRoom.addRoomObject(obj);
                }
                else if (roomAction == RoomObject.RoomAction.RemoveObjectFromRoom)
                {
                    currentRoom.removeRoomObject(obj);
                }
            }
        }
    }

    public void eatObject(string objName)
    {
        RoomObject obj = inputParser.findRoomObjectPrintOtherwise(objName);

        if (obj == null)
            return;

        string defaultSuccessText = "You eat the " + objName + ".";
        string defaultFailText = "You can't eat the " + objName + ".";
        bool successful = inputParser.printResponse(obj, obj.isEdible, defaultSuccessText, defaultFailText, obj.EdibleFlavorText);

        if (successful)
            executeActions(obj, obj.edibleVars);

    }

    public void talkToPerson(string objName)
    {
        RoomObject obj = inputParser.findRoomObjectPrintOtherwise(objName);

        if (obj == null)
            return;

        string defaultSuccessText = "You talk to " + objName + ".";
        string defaultFailText = "You can't talk to " + objName + ".";
        bool successful = inputParser.printResponse(obj, obj.isTalkable, defaultSuccessText, defaultFailText, obj.TalkableFlavorText);

        if (successful)
            executeActions(obj, obj.talkableVars);
    }

    public void killPerson(string objName)
    {
        RoomObject obj = inputParser.findRoomObjectPrintOtherwise(objName);

        if (obj == null)
            return;

        string defaultSuccessText = "You kill " + objName + ".";
        string defaultFailText = "You can't kill " + objName + ".";
        bool successful = inputParser.printResponse(obj, obj.isKillable, defaultSuccessText, defaultFailText, obj.KillableFlavorText);

        if (successful)
            executeActions(obj, obj.killableVars);
    }

    public void sitOnObject(string objName)
    {
        RoomObject obj = inputParser.findRoomObjectPrintOtherwise(objName);

        if (obj == null)
            return;

        string defaultSuccessText = "You sit on the " + objName + ".";
        string defaultFailText = "You can't sit on the the " + objName + ".";
        bool successful = inputParser.printResponse(obj, obj.isSittable, defaultSuccessText, defaultFailText, obj.SittableFlavorText);

        if (successful)
            executeActions(obj, obj.sittableVars);
    }

    public void useObject(string objName)
    {
        RoomObject obj = inputParser.findRoomObjectPrintOtherwise(objName);

        if (obj == null)
            return;

        string defaultSuccessText = "You use the " + objName + ".";
        string defaultFailText = "You can't use the " + objName + ".";
        bool successful = inputParser.printResponse(obj, obj.isUsable, defaultSuccessText, defaultFailText, obj.UsableFlavorText);

        if (successful)
            executeActions(obj, obj.usableVars);
    }

    public void pickupObject(string objName)
    {
        RoomObject obj = inputParser.findRoomObjectPrintOtherwise(objName);

        if (obj == null)
            return;

        string defaultSuccessText = "You pick up the " + objName + ".";
        string defaultFailText = "You can't pick up the " + objName + ".";
        bool successful = inputParser.printResponse(obj, obj.isPickupable, defaultSuccessText, defaultFailText, obj.PickupableFlavorText);

        if (successful)
            executeActions(obj, obj.pickupVars);
    }

    public void wearObject(string objName)
    {
        RoomObject obj = inputParser.findRoomObjectPrintOtherwise(objName);

        if (obj == null)
            return;

        string defaultSuccessText = "You equip the " + objName + ".";
        string defaultFailText = "You can't equip the " + objName + ".";
        bool successful = inputParser.printResponse(obj, obj.isWearable, defaultSuccessText, defaultFailText, obj.WearableFlavorText);

        if (successful)
            executeActions(obj, obj.wearableVars);
    }

    public void printCurrentRoomText()
    {
        string roomText = roomTracker.getCurrentRoom().roomText;
        if (System.String.IsNullOrEmpty(roomText))
        {
            textPrompt.printText("\n(You forgot to add Room Text for this current room)");
        }
        else
        {
            textPrompt.printText("\n" + roomText);
        }
    }
}

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
        // NOTE ON USING OBJECTS SET IN THE INSPECTOR: Objects that are ADDED have to be copies to retain the original variables of the scriptable object.
        // Objects that are REMOVED/EDITED use the name as a way to find the same object, since comparing it to the OBJECT set to the inspector compares two different objects
        // (they are different objects since they are copies of the Scriptable Object and are not the same)

        // TODO: Make actions perform on ALL objects of the same time? 

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
                        player.addItemToInventory(copyRoomObject(objVars[i].varsToChange.targetObject));
                        break;
                    case RoomObject.PlayerAction.RemoveFromInventory:
                        RoomObject objToRemove = roomTracker.findObjectInCurrentRoom(objVars[i].varsToChange.targetObject.name);
                        Debug.Log(objToRemove.name);
                        player.removeItemFromInventory(objToRemove);
                        break;
                    case RoomObject.PlayerAction.KillPlayer:
                        textPrompt.killPlayer();
                        textPrompt.printText("\n--YOU DIED-- (Press any button to continue)");
                        break;
                    case RoomObject.PlayerAction.EquipItem:
                        player.equipItem(copyRoomObject(objVars[i].varsToChange.targetObject));
                        break;
                    case RoomObject.PlayerAction.RemoveEquippedItem:
                        player.removeEquippedItem(roomTracker.findObjectInCurrentRoom(objVars[i].varsToChange.targetObject.name));
                        break;
                    default:
                        Debug.Log($"Unknown PlayerAction enum at {obj.name}: you forgot to add what to do in ActionHandler!");
                        break;
                }
            }
            else if (category == RoomObject.ActionCategory.ObjectActions)
            {
                RoomObject.ObjectAction objectAction = objVars[i].objectAction;

                RoomObject targetObject = roomTracker.findObjectInCurrentRoom(objVars[i].varsToChange.targetObject.name);

                if (targetObject == null)
                {
                    Debug.Log($"{objName}: couldn't find {objVars[i].varsToChange.targetObject.name} in the current room");
                    continue;
                }

                // Execute object actions
                switch (objectAction)
                {
                    case RoomObject.ObjectAction.DestroyThisObject:
                        player.removeItemFromInventory(targetObject);
                        player.removeEquippedItem(targetObject);
                        roomTracker.getCurrentRoom().removeRoomObject(targetObject);
                        break;
                    case RoomObject.ObjectAction.SetIsEdible:
                        targetObject.isEdible = objVars[i].varsToChange.isEdible;
                        break;
                    case RoomObject.ObjectAction.SetIsTalkable:
                        targetObject.isTalkable = objVars[i].varsToChange.isTalkable;
                        break;
                    case RoomObject.ObjectAction.SetIsKillable:
                        targetObject.isKillable = objVars[i].varsToChange.isKillable;
                        break;
                    case RoomObject.ObjectAction.SetIsSittable:
                        targetObject.isSittable = objVars[i].varsToChange.isSittable;
                        break;
                    case RoomObject.ObjectAction.SetIsUsable:
                        targetObject.isUsable = objVars[i].varsToChange.isUsable;
                        break;
                    case RoomObject.ObjectAction.SetIsWearable:
                        targetObject.isWearable = objVars[i].varsToChange.isWearable;
                        break;
                    case RoomObject.ObjectAction.SetIsOpenable:
                        targetObject.isOpenable = objVars[i].varsToChange.isOpenable;
                        break;

                    case RoomObject.ObjectAction.ChangeEdibleFlavorText:
                        targetObject.EdibleFlavorText = objVars[i].varsToChange.edibleFlavorText;
                        break;
                    case RoomObject.ObjectAction.ChangeKillableFlavorText:
                        targetObject.KillableFlavorText = objVars[i].varsToChange.killableFlavorText;
                        break;
                    case RoomObject.ObjectAction.ChangePickupableFlavorText:
                        targetObject.PickupableFlavorText = objVars[i].varsToChange.pickupableFlavorText;
                        break;
                    case RoomObject.ObjectAction.ChangeSittableFlavorText:
                        targetObject.SittableFlavorText = objVars[i].varsToChange.sittableFlavorText;
                        break;
                    case RoomObject.ObjectAction.ChangeTalkableFlavorText:
                        targetObject.TalkableFlavorText = objVars[i].varsToChange.talkableFlavorText;
                        break;
                    case RoomObject.ObjectAction.ChangeUsableFlavorText:
                        targetObject.UsableFlavorText = objVars[i].varsToChange.usableFlavorText;
                        break;
                    case RoomObject.ObjectAction.ChangeWearableFlavorText:
                        targetObject.WearableFlavorText = objVars[i].varsToChange.wearableFlavorText;
                        break;
                    case RoomObject.ObjectAction.ChangeOpenableFlavorText:
                        targetObject.OpenableFlavorText = objVars[i].varsToChange.openableFlavorText;
                        break;

                    default:
                        Debug.Log($"Unknown ObjectAction enum at {obj.name}: you forgot to add what to do in ActionHandler!");
                        break;
                }
            }
            else if (category == RoomObject.ActionCategory.RoomActions)
            {
                RoomObject.RoomAction roomAction = objVars[i].roomAction;

                if (roomAction == RoomObject.RoomAction.AddObjectToRoom)
                {
                    roomTracker.getCurrentRoom().addRoomObject(copyRoomObject(objVars[i].varsToChange.targetObject));
                }
                else if (roomAction == RoomObject.RoomAction.RemoveObjectFromRoom)
                {
                    roomTracker.getCurrentRoom().removeRoomObject(roomTracker.findObjectInCurrentRoom(objVars[i].varsToChange.targetObject.name));
                }
                else if (roomAction == RoomObject.RoomAction.ChangeRoom)
                {
                    roomTracker.forceChangeRoom(objVars[i].varsToChange.targetRoom);
                }
            }
        }
    }

    private void printIfObjectIsNull()
    {
        textPrompt.printText("\nThere is no object with that name in this room.");  
    }

    // Used to preserve original objects when adding new ones to the scene
    private RoomObject copyRoomObject(RoomObject originalObject)
    {
        RoomObject copiedObject = Instantiate(originalObject);
        copiedObject.name = originalObject.name;
        return copiedObject;
    }

    public void eatObject(RoomObject obj)
    {
        if (obj == null)
        {
            printIfObjectIsNull();
            return;
        }

        string defaultSuccessText = "You eat the " + obj.name + ".";
        string defaultFailText = "You can't eat the " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.isEdible, defaultSuccessText, defaultFailText, obj.EdibleFlavorText);

        if (successful)
            executeActions(obj, obj.edibleVars);

    }

    public void talkToPerson(RoomObject obj)
    {
        if (obj == null)
        {
            printIfObjectIsNull();
            return;
        }

        string defaultSuccessText = "You talk to " + obj.name + ".";
        string defaultFailText = "You can't talk to " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.isTalkable, defaultSuccessText, defaultFailText, obj.TalkableFlavorText);

        if (successful)
            executeActions(obj, obj.talkableVars);
    }

    public void killPerson(RoomObject obj)
    {
        if (obj == null)
        {
            printIfObjectIsNull();
            return;
        }

        string defaultSuccessText = "You kill " + obj.name + ".";
        string defaultFailText = "You can't kill " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.isKillable, defaultSuccessText, defaultFailText, obj.KillableFlavorText);

        if (successful)
            executeActions(obj, obj.killableVars);
    }

    public void sitOnObject(RoomObject obj)
    {
        if (obj == null)
        {
            printIfObjectIsNull();
            return;
        }

        string defaultSuccessText = "You sit on the " + obj.name + ".";
        string defaultFailText = "You can't sit on the the " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.isSittable, defaultSuccessText, defaultFailText, obj.SittableFlavorText);

        if (successful)
            executeActions(obj, obj.sittableVars);
    }

    public void useObject(RoomObject obj)
    {
        if (obj == null)
        {
            printIfObjectIsNull();
            return;
        }

        string defaultSuccessText = "You use the " + obj.name + ".";
        string defaultFailText = "You can't use the " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.isUsable, defaultSuccessText, defaultFailText, obj.UsableFlavorText);

        if (successful)
            executeActions(obj, obj.usableVars);
    }

    public void pickupObject(RoomObject obj)
    {

        if (obj == null)
        {
            printIfObjectIsNull();
            return;
        }

        string defaultSuccessText = "You pick up the " + obj.name + ".";
        string defaultFailText = "You can't pick up the " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.isPickupable, defaultSuccessText, defaultFailText, obj.PickupableFlavorText);

        if (successful)
            executeActions(obj, obj.pickupVars);
    }

    public void wearObject(RoomObject obj)
    {

        if (obj == null)
        {
            printIfObjectIsNull();
            return;
        }

        string defaultSuccessText = "You equip the " + obj.name + ".";
        string defaultFailText = "You can't equip the " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.isWearable, defaultSuccessText, defaultFailText, obj.WearableFlavorText);

        if (successful)
            executeActions(obj, obj.wearableVars);
    }

    public void openObject(RoomObject obj)
    {

        if (obj == null)
        {
            printIfObjectIsNull();
            return;
        }

        string defaultSuccessText = "You open the " + obj.name + ".";
        string defaultFailText = "You can't open the " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.isOpenable, defaultSuccessText, defaultFailText, obj.OpenableFlavorText);

        if (successful)
            executeActions(obj, obj.openableVars);
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

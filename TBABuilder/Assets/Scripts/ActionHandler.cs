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

                RoomObject targetObject = objVars[i].varsToChange.targetObject;

                if (targetObject == null)
                {
                    Debug.Log($"{objName}: There's an object that wasn't set in an action");
                    continue;
                }

                switch (playerAction)
                {
                    case RoomObject.PlayerAction.KillPlayer:
                        textPrompt.killPlayer();
                        textPrompt.printText("\nYOU DIED (Press any button to restart)");
                        break;

                    case RoomObject.PlayerAction.WinGame:
                        textPrompt.winGame();
                        textPrompt.printText("\nYOU WON! (Press any button to restart)");
                        break;

                    case RoomObject.PlayerAction.AddToInventory:
                        player.addItemToInventory(targetObject);
                        roomTracker.getCurrentRoom().removeRoomObject(targetObject);
                        player.removeEquippedItem(targetObject);
                        break;

                    case RoomObject.PlayerAction.RemoveFromInventory:
                        player.removeItemFromInventory(targetObject);
                        break;

                    case RoomObject.PlayerAction.EquipItem:
                        player.equipItem(targetObject);
                        break;

                    case RoomObject.PlayerAction.RemoveEquippedItem:
                        player.removeEquippedItem(targetObject);
                        break;

                    default:
                        Debug.Log($"Unknown PlayerAction enum at {obj.name}: you forgot to add what to do in ActionHandler!");
                        break;
                }
            }
            else if (category == RoomObject.ActionCategory.ObjectActions)
            {
                RoomObject.ObjectAction objectAction = objVars[i].objectAction;

                RoomObject targetObject = objVars[i].varsToChange.targetObject;

                if (targetObject == null)
                {
                    Debug.Log($"{objName}: There's an object that wasn't set in an action");
                    continue;
                }

                // Execute object actions
                switch (objectAction)
                {
                    case RoomObject.ObjectAction.DestroyObject:
                        player.removeItemFromInventory(targetObject);
                        player.removeEquippedItem(targetObject);
                        roomTracker.getCurrentRoom().removeRoomObject(targetObject);
                        break;

                    case RoomObject.ObjectAction.SetIsEdible:
                        targetObject.runtimeIsEdible = objVars[i].varsToChange.isEdible;
                        break;

                    case RoomObject.ObjectAction.SetIsTalkable:
                        targetObject.runtimeIsTalkable = objVars[i].varsToChange.isTalkable;
                        break;

                    case RoomObject.ObjectAction.SetIsKillable:
                        targetObject.runtimeIsKillable = objVars[i].varsToChange.isKillable;
                        break;

                    case RoomObject.ObjectAction.SetIsSittable:
                        targetObject.runtimeIsSittable = objVars[i].varsToChange.isSittable;
                        break;

                    case RoomObject.ObjectAction.SetIsUsable:
                        targetObject.runtimeIsUsable = objVars[i].varsToChange.isUsable;
                        break;

                    case RoomObject.ObjectAction.SetIsWearable:
                        targetObject.runtimeIsWearable = objVars[i].varsToChange.isWearable;
                        break;

                    case RoomObject.ObjectAction.SetIsOpenable:
                        targetObject.runtimeIsOpenable = objVars[i].varsToChange.isOpenable;
                        break;


                    case RoomObject.ObjectAction.ChangeEdibleFlavorText:
                        targetObject.runtimeEdibleFlavorText = objVars[i].varsToChange.edibleFlavorText;
                        break;

                    case RoomObject.ObjectAction.ChangeKillableFlavorText:
                        targetObject.runtimeKillableFlavorText = objVars[i].varsToChange.killableFlavorText;
                        break;

                    case RoomObject.ObjectAction.ChangePickupableFlavorText:
                        targetObject.runtimePickupableFlavorText = objVars[i].varsToChange.pickupableFlavorText;
                        break;

                    case RoomObject.ObjectAction.ChangeSittableFlavorText:
                        targetObject.runtimeSittableFlavorText = objVars[i].varsToChange.sittableFlavorText;
                        break;

                    case RoomObject.ObjectAction.ChangeTalkableFlavorText:
                        targetObject.runtimeTalkableFlavorText = objVars[i].varsToChange.talkableFlavorText;
                        break;

                    case RoomObject.ObjectAction.ChangeUsableFlavorText:
                        targetObject.runtimeUsableFlavorText = objVars[i].varsToChange.usableFlavorText;
                        break;

                    case RoomObject.ObjectAction.ChangeWearableFlavorText:
                        targetObject.runtimeWearableFlavorText = objVars[i].varsToChange.wearableFlavorText;
                        break;

                    case RoomObject.ObjectAction.ChangeOpenableFlavorText:
                        targetObject.runtimeOpenableFlavorText = objVars[i].varsToChange.openableFlavorText;
                        break;

                    case RoomObject.ObjectAction.ChangeLookAtFlavorText:
                        targetObject.runtimeLookAtFlavorText = objVars[i].varsToChange.lookAtFlavorText;
                        break;

                    default:
                        Debug.Log($"Unknown ObjectAction enum at {obj.name}: you forgot to add what to do in ActionHandler!");
                        break;
                }
            }
            else if (category == RoomObject.ActionCategory.RoomActions)
            {
                RoomObject.RoomAction roomAction = objVars[i].roomAction;
                Room targetRoom = objVars[i].varsToChange.targetRoom;

                switch (roomAction) {
                    case RoomObject.RoomAction.AddObjectToRoom:
                        if (objVars[i].varsToChange.targetObject.isInitialized == false)
                            objVars[i].varsToChange.targetObject.initializeRuntimeVariables();
                        roomTracker.getCurrentRoom().addRoomObject(objVars[i].varsToChange.targetObject);
                        break;

                    case RoomObject.RoomAction.RemoveObjectFromRoom:
                        roomTracker.getCurrentRoom().removeRoomObject(objVars[i].varsToChange.targetObject);
                        break;

                    case RoomObject.RoomAction.SwitchRooms:
                        roomTracker.forceChangeRoom(objVars[i].varsToChange.targetRoom);
                        break;

                    case RoomObject.RoomAction.ChangeRoomText:
                        targetRoom.runtimeRoomText = objVars[i].varsToChange.roomText;
                        break;

                    default:
                        Debug.Log($"Unknown RoomAction enum at {obj.name}: you forgot to add what to do in ActionHandler!");
                        break;
                }
            }
        }
    }

    public void eatObject(RoomObject obj)
    {
        if (obj == null)
        {
            textPrompt.printText("\nEat what?");
            return;
        }

        string defaultSuccessText = "You eat the " + obj.name + ".";
        string defaultFailText = "You can't eat the " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.runtimeIsEdible, defaultSuccessText, defaultFailText, obj.runtimeEdibleFlavorText);

        if (successful)
            executeActions(obj, obj.edibleVars);

    }

    public void talkToPerson(RoomObject obj)
    {
        if (obj == null)
        {
            textPrompt.printText("\nTalk to who?");
            return;
        }

        string defaultSuccessText = "You talk to " + obj.name + ".";
        string defaultFailText = "You can't talk to " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.runtimeIsTalkable, defaultSuccessText, defaultFailText, obj.runtimeTalkableFlavorText);

        if (successful)
            executeActions(obj, obj.talkableVars);
    }

    public void killPerson(RoomObject obj)
    {
        if (obj == null)
        {
            textPrompt.printText("\nKill who?");
            return;
        }

        string defaultSuccessText = "You kill " + obj.name + ".";
        string defaultFailText = "You can't kill " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.runtimeIsKillable, defaultSuccessText, defaultFailText, obj.runtimeKillableFlavorText);

        if (successful)
            executeActions(obj, obj.killableVars);
    }

    public void sitOnObject(RoomObject obj)
    {
        if (obj == null)
        {
            textPrompt.printText("\nSit on what?");
            return;
        }

        string defaultSuccessText = "You sit on the " + obj.name + ".";
        string defaultFailText = "You can't sit on the the " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.runtimeIsSittable, defaultSuccessText, defaultFailText, obj.runtimeSittableFlavorText);

        if (successful)
            executeActions(obj, obj.sittableVars);
    }

    public void useObject(RoomObject obj)
    {
        if (obj == null)
        {
            textPrompt.printText("\nUse what?");
            return;
        }

        string defaultSuccessText = "You use the " + obj.name + ".";
        string defaultFailText = "You can't use the " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.runtimeIsUsable, defaultSuccessText, defaultFailText, obj.runtimeUsableFlavorText);

        if (successful)
            executeActions(obj, obj.usableVars);
    }

    public void pickupObject(RoomObject obj)
    {

        if (obj == null)
        {
            textPrompt.printText("\nPick up what?");
            return;
        }

        string defaultSuccessText = "You pick up the " + obj.name + ".";
        string defaultFailText = "You can't pick up the " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.runtimeIsPickupable, defaultSuccessText, defaultFailText, obj.runtimePickupableFlavorText);

        if (successful)
            executeActions(obj, obj.pickupVars);
    }

    public void wearObject(RoomObject obj)
    {

        if (obj == null)
        {
            textPrompt.printText("\nEquip what?");
            return;
        }

        string defaultSuccessText = "You equip the " + obj.name + ".";
        string defaultFailText = "You can't equip the " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.runtimeIsWearable, defaultSuccessText, defaultFailText, obj.runtimeWearableFlavorText);

        if (successful)
            executeActions(obj, obj.wearableVars);
    }

    public void openObject(RoomObject obj)
    {

        if (obj == null)
        {
            textPrompt.printText("\nOpen what?");
            return;
        }

        string defaultSuccessText = "You open the " + obj.name + ".";
        string defaultFailText = "You can't open the " + obj.name + ".";
        bool successful = inputParser.printResponse(obj, obj.runtimeIsOpenable, defaultSuccessText, defaultFailText, obj.runtimeOpenableFlavorText);

        if (successful)
            executeActions(obj, obj.openableVars);
    }

    public void lookAtObject(RoomObject obj)
    {
        if (obj == null)
        {
            textPrompt.printText("\nLook at what?");
            return;
        }

        if (System.String.IsNullOrWhiteSpace(obj.runtimeLookAtFlavorText))
        {
            textPrompt.printText("\nNothing interesting here.");
            return;
        }

        textPrompt.printText("\n" + obj.runtimeLookAtFlavorText);
    }

    public void printCurrentRoomText()
    {
        string roomText = roomTracker.getCurrentRoom().runtimeRoomText;

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

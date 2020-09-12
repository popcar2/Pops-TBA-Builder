using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    InputParser inputParser;
    RoomTracker roomTracker;
    Player player;
    TextPrompt textPrompt;
    DefaultValues defaultValues;

    private void Start()
    {
        inputParser = FindObjectOfType<InputParser>();
        roomTracker = FindObjectOfType<RoomTracker>();
        player = FindObjectOfType<Player>();
        textPrompt = FindObjectOfType<TextPrompt>();
        defaultValues = FindObjectOfType<DefaultValues>();
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
                        textPrompt.printText("\n" + defaultValues.deathText);
                        break;

                    case RoomObject.PlayerAction.WinGame:
                        textPrompt.winGame();
                        textPrompt.printText("\n" + defaultValues.winText);
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

                    case RoomObject.ObjectAction.SetIsDrinkable:
                        targetObject.runtimeIsDrinkable = objVars[i].varsToChange.isDrinkable;
                        break;

                    case RoomObject.ObjectAction.SetIsTalkable:
                        targetObject.runtimeIsTalkable = objVars[i].varsToChange.isTalkable;
                        break;

                    case RoomObject.ObjectAction.SetIsKillable:
                        targetObject.runtimeIsKillable = objVars[i].varsToChange.isKillable;
                        break;

                    case RoomObject.ObjectAction.SetIsBreakable:
                        targetObject.runtimeIsBreakable = objVars[i].varsToChange.isBreakable;
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

                    case RoomObject.ObjectAction.ChangeDrinkableFlavorText:
                        targetObject.runtimeDrinkableFlavorText = objVars[i].varsToChange.drinkableFlavorText;
                        break;

                    case RoomObject.ObjectAction.ChangeKillableFlavorText:
                        targetObject.runtimeKillableFlavorText = objVars[i].varsToChange.killableFlavorText;
                        break;

                    case RoomObject.ObjectAction.ChangeBreakableFlavorText:
                        targetObject.runtimeBreakableFlavorText = objVars[i].varsToChange.breakableFlavorText;
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

    // Needs an unholy amount of variables but still better than copy pasting the same thing to each method
    public void doGenericAction(RoomObject obj, string defaultSuccessText, string defaultFailText, string flavorText, bool successBool, List<RoomObject.EditorVariables> objVars)
    {
        bool successful = inputParser.printResponse(obj, successBool, defaultSuccessText, defaultFailText, flavorText);

        if (successful)
            executeActions(obj, objVars);
    }

    public void eatObject(RoomObject obj)
    {
        if (defaultValues.eatActive)
        {
            if (obj == null)
            {
                textPrompt.printText("\n" + defaultValues.edibleNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.edibleSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.edibleFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, obj.runtimeEdibleFlavorText, obj.runtimeIsEdible, obj.edibleVars);
        }
        else
        {
            textPrompt.printText("\n" + defaultValues.unknownCommand);
        }
    }

    public void drinkObject(RoomObject obj)
    {
        if (defaultValues.drinkActive)
        {
            if (obj == null)
            {
                textPrompt.printText("\n" + defaultValues.drinkableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.drinkableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.drinkableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, obj.runtimeDrinkableFlavorText, obj.runtimeIsDrinkable, obj.drinkableVars);
        }
        else
        {
            textPrompt.printText("\n" + defaultValues.unknownCommand);
        }
    }

    public void talkToPerson(RoomObject obj)
    {
        if (defaultValues.talkActive)
        {
            if (obj == null)
            {
                textPrompt.printText("\n" + defaultValues.talkableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.talkableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.talkableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, obj.runtimeTalkableFlavorText, obj.runtimeIsTalkable, obj.talkableVars);
        }
        else
        {
            textPrompt.printText("\n" + defaultValues.unknownCommand);
        }
    }

    public void killPerson(RoomObject obj)
    {
        if (defaultValues.killActive)
        {
            if (obj == null)
            {
                textPrompt.printText("\n" + defaultValues.killableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.killableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.killableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, obj.runtimeKillableFlavorText, obj.runtimeIsKillable, obj.killableVars);
        }
        else
        {
            textPrompt.printText("\n" + defaultValues.unknownCommand);
        }
    }

    public void BreakObject(RoomObject obj)
    {
        if (defaultValues.breakActive)
        {
            if (obj == null)
            {
                textPrompt.printText("\n" + defaultValues.breakableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.breakableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.breakableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, obj.runtimeBreakableFlavorText, obj.runtimeIsBreakable, obj.breakableVars);
        }
        else
        {
            textPrompt.printText("\n" + defaultValues.unknownCommand);
        }
    }

    public void sitOnObject(RoomObject obj)
    {
        if (defaultValues.sitActive)
        {
            if (obj == null)
            {
                textPrompt.printText("\n" + defaultValues.sittableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.sittableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.sittableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, obj.runtimeSittableFlavorText, obj.runtimeIsSittable, obj.sittableVars);
        }
        else
        {
            textPrompt.printText("\n" + defaultValues.unknownCommand);
        }
    }

    public void useObject(RoomObject obj)
    {
        if (defaultValues.useActive)
        {
            if (obj == null)
            {
                textPrompt.printText("\n" + defaultValues.usableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.usableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.usableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, obj.runtimeUsableFlavorText, obj.runtimeIsUsable, obj.usableVars);
        }
        else
        {
            textPrompt.printText("\n" + defaultValues.unknownCommand);
        }
    }

    public void pickupObject(RoomObject obj)
    {
        if (defaultValues.pickupActive)
        {
            if (obj == null)
            {
                textPrompt.printText("\n" + defaultValues.pickupableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.pickupableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.pickupableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, obj.runtimePickupableFlavorText, obj.runtimeIsPickupable, obj.pickupableVars);
        }
        else
        {
            textPrompt.printText("\n" + defaultValues.unknownCommand);
        }
    }

    public void wearObject(RoomObject obj)
    {
        if (defaultValues.wearActive)
        {
            if (obj == null)
            {
                textPrompt.printText("\n" + defaultValues.wearableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.wearableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.wearableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, obj.runtimeWearableFlavorText, obj.runtimeIsWearable, obj.wearableVars);
        }
        else
        {
            textPrompt.printText("\n" + defaultValues.unknownCommand);
        }
    }

    public void openObject(RoomObject obj)
    {
        if (defaultValues.openActive)
        {
            if (obj == null)
            {
                textPrompt.printText("\n" + defaultValues.openableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.openableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.openableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, obj.runtimeOpenableFlavorText, obj.runtimeIsOpenable, obj.openableVars);
        }
        else
        {
            textPrompt.printText("\n" + defaultValues.unknownCommand);
        }
    }

    public void lookAtObject(RoomObject obj)
    {
        if (defaultValues.lookAtActive)
        {
            if (obj == null)
            {
                textPrompt.printText("\n" + defaultValues.lookAtNotFoundText);
                return;
            }

            if (System.String.IsNullOrWhiteSpace(obj.runtimeLookAtFlavorText))
            {
                textPrompt.printText("\n" + defaultValues.lookAtDefaultText.Replace("(NAME)", obj.name));
                return;
            }

            textPrompt.printText("\n" + obj.runtimeLookAtFlavorText);
        }
        else
        {
            textPrompt.printText("\n" + defaultValues.unknownCommand);
        }
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

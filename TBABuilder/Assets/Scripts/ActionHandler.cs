using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    InputParser inputParser;
    RoomTracker roomTracker;
    Player player;
    TextPrompt textPrompt;
    DefaultValues defaultValues;

    List<RoomObject.EditorVariables> delayedActions;
    bool checkDelayed = true;

    private void Awake()
    {
        inputParser = FindObjectOfType<InputParser>();
        roomTracker = FindObjectOfType<RoomTracker>();
        player = FindObjectOfType<Player>();
        textPrompt = FindObjectOfType<TextPrompt>();
        defaultValues = FindObjectOfType<DefaultValues>();

        delayedActions = new List<RoomObject.EditorVariables>();
    }

    /// <summary>
    /// Execute the action list in an object.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="objVars">The editor variables list (actions) inside of the target object</param>
    public void executeActions(Object obj, List<RoomObject.EditorVariables> objVars)
    {
        for (int i = 0; i < objVars.Count; i++)
        {
            if (objVars[i].conditionalVars.Count == 0)
            {
                // This isn't a conditional, execute action normally
                executeOneAction(obj, objVars[i]);
            }
            else
            {
                // This is a conditional, loop over every action inside of it through recursion
                Room targetRoom = objVars[i].varsToChange.targetRoom;
                RoomObject targetObject = objVars[i].varsToChange.targetObject;
                bool conditionalBool = objVars[i].conditionalBool;

                switch (objVars[i].conditional)
                {
                    case RoomObject.Conditional.ObjectExistsInRoom:
                        if (targetRoom.runtimeRoomObjects.Contains(targetObject) == conditionalBool)
                        {
                            executeActions(obj, objVars[i].conditionalVars);
                        }
                        break;
                    case RoomObject.Conditional.ObjectExistsInInventory:
                        if (player.getInventory().Contains(targetObject) == conditionalBool)
                        {
                            executeActions(obj, objVars[i].conditionalVars);
                        }
                        break;
                    case RoomObject.Conditional.ObjectExistsInEquipment:
                        if (player.getEquippedItems().Contains(targetObject) == conditionalBool)
                        {
                            executeActions(obj, objVars[i].conditionalVars);
                        }
                        break;
                    case RoomObject.Conditional.RoomWasVisited:
                        if (objVars[i].varsToChange.targetRoom.isInitialized == conditionalBool)
                        {
                            executeActions(obj, objVars[i].conditionalVars);
                        }
                        break;
                    default:
                        Debug.Log($"Unknown Conditional enum at {obj.name}: you forgot to add what to do in ActionHandler!");
                        break;
                }
            }
        }
    }
    
    /// <summary>
    /// Execute one action.
    /// </summary>
    /// <param name="obj">The parent object</param>
    /// <param name="action">The action to execute</param>
    public void executeOneAction(Object obj, RoomObject.EditorVariables action)
    {
        if (checkDelayed)
        {
            if (action.isDelayed)
            {
                delayedActions.Add(action);
                return;
            }
        }

        RoomObject.ActionCategory category = action.actionCategory;

        // Execute player actions
        if (category == RoomObject.ActionCategory.PlayerActions)
        {
            RoomObject.PlayerAction playerAction = action.playerAction;

            RoomObject targetObject = action.varsToChange.targetObject;

            switch (playerAction)
            {
                case RoomObject.PlayerAction.KillPlayer:
                    textPrompt.killPlayer();
                    // The delay for printText is so the flavor text is printed first before death/win text.
                    StartCoroutine(textPrompt.printTextAfterDelay(defaultValues.deathText, 0.1f));
                    break;

                case RoomObject.PlayerAction.WinGame:
                    textPrompt.winGame();
                    // The delay for printText is so the flavor text is printed first before death/win text.
                    StartCoroutine(textPrompt.printTextAfterDelay(defaultValues.winText, 0.1f));
                    break;

                case RoomObject.PlayerAction.AddToInventory:
                    player.addItemToInventory(targetObject);
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
            RoomObject.ObjectAction objectAction = action.objectAction;

            RoomObject targetObject = action.varsToChange.targetObject;

            if (targetObject == null)
            {
                Debug.Log($"{obj.name}: There's an object that wasn't set in an action");
                return;
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
                    targetObject.runtimeIsEdible = action.varsToChange.isEdible;
                    break;

                case RoomObject.ObjectAction.SetIsDrinkable:
                    targetObject.runtimeIsDrinkable = action.varsToChange.isDrinkable;
                    break;

                case RoomObject.ObjectAction.SetIsTalkable:
                    targetObject.runtimeIsTalkable = action.varsToChange.isTalkable;
                    break;

                case RoomObject.ObjectAction.SetIsKillable:
                    targetObject.runtimeIsKillable = action.varsToChange.isKillable;
                    break;

                case RoomObject.ObjectAction.SetIsBreakable:
                    targetObject.runtimeIsBreakable = action.varsToChange.isBreakable;
                    break;

                case RoomObject.ObjectAction.SetIsSittable:
                    targetObject.runtimeIsSittable = action.varsToChange.isSittable;
                    break;

                case RoomObject.ObjectAction.SetIsUsable:
                    targetObject.runtimeIsUsable = action.varsToChange.isUsable;
                    break;

                case RoomObject.ObjectAction.SetIsPickupable:
                    targetObject.runtimeIsPickupable = action.varsToChange.isPickupable;
                    break;

                case RoomObject.ObjectAction.SetIsWearable:
                    targetObject.runtimeIsWearable = action.varsToChange.isWearable;
                    break;

                case RoomObject.ObjectAction.SetIsOpenable:
                    targetObject.runtimeIsOpenable = action.varsToChange.isOpenable;
                    break;


                case RoomObject.ObjectAction.ChangeEdibleFlavorText:
                    targetObject.runtimeEdibleFlavorText = action.varsToChange.edibleFlavorText;
                    break;

                case RoomObject.ObjectAction.ChangeDrinkableFlavorText:
                    targetObject.runtimeDrinkableFlavorText = action.varsToChange.drinkableFlavorText;
                    break;

                case RoomObject.ObjectAction.ChangeKillableFlavorText:
                    targetObject.runtimeKillableFlavorText = action.varsToChange.killableFlavorText;
                    break;

                case RoomObject.ObjectAction.ChangeBreakableFlavorText:
                    targetObject.runtimeBreakableFlavorText = action.varsToChange.breakableFlavorText;
                    break;

                case RoomObject.ObjectAction.ChangePickupableFlavorText:
                    targetObject.runtimePickupableFlavorText = action.varsToChange.pickupableFlavorText;
                    break;

                case RoomObject.ObjectAction.ChangeSittableFlavorText:
                    targetObject.runtimeSittableFlavorText = action.varsToChange.sittableFlavorText;
                    break;

                case RoomObject.ObjectAction.ChangeTalkableFlavorText:
                    targetObject.runtimeTalkableFlavorText = action.varsToChange.talkableFlavorText;
                    break;

                case RoomObject.ObjectAction.ChangeUsableFlavorText:
                    targetObject.runtimeUsableFlavorText = action.varsToChange.usableFlavorText;
                    break;

                case RoomObject.ObjectAction.ChangeWearableFlavorText:
                    targetObject.runtimeWearableFlavorText = action.varsToChange.wearableFlavorText;
                    break;

                case RoomObject.ObjectAction.ChangeOpenableFlavorText:
                    targetObject.runtimeOpenableFlavorText = action.varsToChange.openableFlavorText;
                    break;

                case RoomObject.ObjectAction.ChangeLookAtFlavorText:
                    targetObject.runtimeLookAtFlavorText = action.varsToChange.lookAtFlavorText;
                    break;

                default:
                    Debug.Log($"Unknown ObjectAction enum at {obj.name}: you forgot to add what to do in ActionHandler!");
                    break;
            }
        }
        else if (category == RoomObject.ActionCategory.RoomActions)
        {
            RoomObject.RoomAction roomAction = action.roomAction;
            Room targetRoom = action.varsToChange.targetRoom;

            switch (roomAction)
            {
                case RoomObject.RoomAction.AddObjectToRoom:
                    if (action.varsToChange.targetObject.isInitialized == false)
                        action.varsToChange.targetObject.initializeRuntimeVariables();
                    roomTracker.getCurrentRoom().addRoomObject(action.varsToChange.targetObject);
                    break;

                case RoomObject.RoomAction.RemoveObjectFromRoom:
                    roomTracker.getCurrentRoom().removeRoomObject(action.varsToChange.targetObject);
                    break;

                case RoomObject.RoomAction.SwitchRooms:
                    roomTracker.forceChangeRoom(action.varsToChange.targetRoom);
                    break;

                case RoomObject.RoomAction.ChangeRoomEntryText:
                    targetRoom.runtimeRoomEntryText = action.varsToChange.roomEntryText;
                    break;

                case RoomObject.RoomAction.ChangeRoomLookText:
                    targetRoom.runtimeLookText = action.varsToChange.roomLookText;
                    break;

                case RoomObject.RoomAction.ActivateRoomConnection:
                    setActiveRoomConnection(action.varsToChange.targetRoom, action.varsToChange.targetRoomToActivate, true);
                    break;
                case RoomObject.RoomAction.DisableRoomConnection:
                    setActiveRoomConnection(action.varsToChange.targetRoom, action.varsToChange.targetRoomToActivate, false);
                    break;

                default:
                    Debug.Log($"Unknown RoomAction enum at {obj.name}: you forgot to add what to do in ActionHandler!");
                    break;
            }
        }
    }

    // Needs an unholy amount of variables but still better than copy pasting the same thing to each method
    /// <summary>
    /// Execute actions in a generic command and prints response.
    /// </summary>
    /// <param name="obj">The object which called the action</param>
    /// <param name="defaultSuccessText">Default success flavor text</param>
    /// <param name="defaultFailText">Default failure flavor text</param>
    /// <param name="flavorText">Custom flavor text, leave empty or null if it doesn't exist</param>
    /// <param name="successBool">Whether the action is successful or not</param>
    /// <param name="objVars">The list of actions</param>
    public void doGenericAction(Object obj, string defaultSuccessText, string defaultFailText, ref string flavorText, bool successBool, List<RoomObject.EditorVariables> objVars)
    {
        if (successBool)
            executeActions(obj, objVars);

        inputParser.printResponse(successBool, defaultSuccessText, defaultFailText, ref flavorText);
        checkDelayed = false;

        if (successBool)
        {
            executeActions(obj, delayedActions);
            delayedActions.Clear();
        }

        checkDelayed = true;
    }

    public void eatObject(RoomObject obj)
    {
        if (defaultValues.eatActive)
        {
            if (obj == null)
            {
                textPrompt.printText(defaultValues.edibleNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.edibleSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.edibleFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeEdibleFlavorText, obj.runtimeIsEdible, obj.edibleVars);
        }
        else
        {
            textPrompt.printText(defaultValues.unknownCommand);
        }
    }

    public void drinkObject(RoomObject obj)
    {
        if (defaultValues.drinkActive)
        {
            if (obj == null)
            {
                textPrompt.printText(defaultValues.drinkableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.drinkableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.drinkableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeDrinkableFlavorText, obj.runtimeIsDrinkable, obj.drinkableVars);
        }
        else
        {
            textPrompt.printText(defaultValues.unknownCommand);
        }
    }

    public void talkToPerson(RoomObject obj)
    {
        if (defaultValues.talkActive)
        {
            if (obj == null)
            {
                textPrompt.printText(defaultValues.talkableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.talkableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.talkableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeTalkableFlavorText, obj.runtimeIsTalkable, obj.talkableVars);
        }
        else
        {
            textPrompt.printText(defaultValues.unknownCommand);
        }
    }

    public void killPerson(RoomObject obj)
    {
        if (defaultValues.killActive)
        {
            if (obj == null)
            {
                textPrompt.printText(defaultValues.killableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.killableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.killableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeKillableFlavorText, obj.runtimeIsKillable, obj.killableVars);
        }
        else
        {
            textPrompt.printText(defaultValues.unknownCommand);
        }
    }

    public void BreakObject(RoomObject obj)
    {
        if (defaultValues.breakActive)
        {
            if (obj == null)
            {
                textPrompt.printText(defaultValues.breakableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.breakableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.breakableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeBreakableFlavorText, obj.runtimeIsBreakable, obj.breakableVars);
        }
        else
        {
            textPrompt.printText(defaultValues.unknownCommand);
        }
    }

    public void sitOnObject(RoomObject obj)
    {
        if (defaultValues.sitActive)
        {
            if (obj == null)
            {
                textPrompt.printText(defaultValues.sittableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.sittableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.sittableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeSittableFlavorText, obj.runtimeIsSittable, obj.sittableVars);
        }
        else
        {
            textPrompt.printText(defaultValues.unknownCommand);
        }
    }

    public void useObject(RoomObject obj)
    {
        if (defaultValues.useActive)
        {
            if (obj == null)
            {
                textPrompt.printText(defaultValues.usableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.usableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.usableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeUsableFlavorText, obj.runtimeIsUsable, obj.usableVars);
        }
        else
        {
            textPrompt.printText(defaultValues.unknownCommand);
        }
    }

    public void pickupObject(RoomObject obj)
    {
        if (defaultValues.pickupActive)
        {
            if (obj == null)
            {
                textPrompt.printText(defaultValues.pickupableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.pickupableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.pickupableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimePickupableFlavorText, obj.runtimeIsPickupable, obj.pickupableVars);
        }
        else
        {
            textPrompt.printText(defaultValues.unknownCommand);
        }
    }

    public void wearObject(RoomObject obj)
    {
        if (defaultValues.wearActive)
        {
            if (obj == null)
            {
                textPrompt.printText(defaultValues.wearableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.wearableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.wearableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeWearableFlavorText, obj.runtimeIsWearable, obj.wearableVars);
        }
        else
        {
            textPrompt.printText(defaultValues.unknownCommand);
        }
    }

    public void openObject(RoomObject obj)
    {
        if (defaultValues.openActive)
        {
            if (obj == null)
            {
                textPrompt.printText(defaultValues.openableNotFoundText);
                return;
            }

            string defaultSuccessText = defaultValues.openableSuccessText.Replace("(NAME)", obj.name);
            string defaultFailText = defaultValues.openableFailText.Replace("(NAME)", obj.name);
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeOpenableFlavorText, obj.runtimeIsOpenable, obj.openableVars);
        }
        else
        {
            textPrompt.printText(defaultValues.unknownCommand);
        }
    }

    public void lookAtObject(RoomObject obj)
    {
        if (defaultValues.lookAtActive)
        {
            if (obj == null)
            {
                textPrompt.printText(defaultValues.lookAtNotFoundText);
                return;
            }

            if (System.String.IsNullOrWhiteSpace(obj.runtimeLookAtFlavorText))
            {
                textPrompt.printText(defaultValues.lookAtDefaultText.Replace("(NAME)", obj.name));
                return;
            }

            textPrompt.printText(obj.runtimeLookAtFlavorText);
        }
        else
        {
            textPrompt.printText(defaultValues.unknownCommand);
        }
    }

    /// <summary>
    /// Sets whether a room connection is accessible or not
    /// </summary>
    /// <param name="targetRoom">The room you want to alter</param>
    /// <param name="roomConnection">The room connection</param>
    /// <param name="activeBool"></param>
    private void setActiveRoomConnection(Room targetRoom, Room roomConnection, bool activeBool)
    {
        foreach (Room.RoomConnectionVars roomConnectionVars in targetRoom.roomConnections)
        {
            if (roomConnectionVars.room == roomConnection)
            {
                roomConnectionVars.isActive = activeBool;
            }
        }
    }
}

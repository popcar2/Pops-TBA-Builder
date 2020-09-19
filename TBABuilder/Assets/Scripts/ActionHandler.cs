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

    public void executeOneAction(RoomObject obj, RoomObject.EditorVariables action)
    {
        RoomObject.ActionCategory category = action.actionCategory;

        // Execute player actions
        if (category == RoomObject.ActionCategory.PlayerActions)
        {
            RoomObject.PlayerAction playerAction = action.playerAction;

            RoomObject targetObject = action.varsToChange.targetObject;

            if (targetObject == null)
            {
                Debug.Log($"{obj.name}: There's an object that wasn't set in an action");
                return;
            }

            switch (playerAction)
            {
                case RoomObject.PlayerAction.KillPlayer:
                    textPrompt.killPlayer();
                    // The delay for printText is so the flavor text is printed first before death/win text.
                    StartCoroutine(textPrompt.printTextAfterDelay("\n" + defaultValues.deathText, 0.1f));
                    break;

                case RoomObject.PlayerAction.WinGame:
                    textPrompt.winGame();
                    // The delay for printText is so the flavor text is printed first before death/win text.
                    StartCoroutine(textPrompt.printTextAfterDelay("\n" + defaultValues.winText, 0.1f));
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

                case RoomObject.RoomAction.ChangeRoomText:
                    targetRoom.runtimeRoomText = action.varsToChange.roomText;
                    break;

                default:
                    Debug.Log($"Unknown RoomAction enum at {obj.name}: you forgot to add what to do in ActionHandler!");
                    break;
            }
        }
    }

    // Needs an unholy amount of variables but still better than copy pasting the same thing to each method
    public void doGenericAction(RoomObject obj, string defaultSuccessText, string defaultFailText, ref string flavorText, bool successBool, List<RoomObject.EditorVariables> objVars)
    {
        if (successBool)
            executeActions(obj, objVars);

        inputParser.printResponse(obj, successBool, defaultSuccessText, defaultFailText, ref flavorText);
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
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeEdibleFlavorText, obj.runtimeIsEdible, obj.edibleVars);
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
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeDrinkableFlavorText, obj.runtimeIsDrinkable, obj.drinkableVars);
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
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeTalkableFlavorText, obj.runtimeIsTalkable, obj.talkableVars);
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
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeKillableFlavorText, obj.runtimeIsKillable, obj.killableVars);
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
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeBreakableFlavorText, obj.runtimeIsBreakable, obj.breakableVars);
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
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeSittableFlavorText, obj.runtimeIsSittable, obj.sittableVars);
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
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeUsableFlavorText, obj.runtimeIsUsable, obj.usableVars);
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
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimePickupableFlavorText, obj.runtimeIsPickupable, obj.pickupableVars);
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
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeWearableFlavorText, obj.runtimeIsWearable, obj.wearableVars);
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
            doGenericAction(obj, defaultSuccessText, defaultFailText, ref obj.runtimeOpenableFlavorText, obj.runtimeIsOpenable, obj.openableVars);
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
}

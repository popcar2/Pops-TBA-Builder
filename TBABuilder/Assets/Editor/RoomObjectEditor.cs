using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(RoomObject))]
public class RoomObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoomObject obj = (RoomObject)target;
        DefaultValues defaultValues = FindObjectOfType<DefaultValues>();

        // Using this method makes the variables in the object save properly when exiting Unity.
        // Otherwise, the enum lists reset on restart for whatever reason.
        EditorUtility.SetDirty(obj);

        showActionBooleans(obj, defaultValues);
        showFlavorTexts(obj, defaultValues);
        showMiscTab(obj, defaultValues);
    }

    private void showActionBooleans(RoomObject obj, DefaultValues defaultValues)
    {
        EditorGUILayout.LabelField(new GUIContent("Action booleans", "Set whether an action can happen on this object and its commands."), EditorStyles.boldLabel);

        if (defaultValues.eatActive)
        {
            obj.isEdible = GUILayout.Toggle(obj.isEdible, new GUIContent("Is Edible", "Can you eat this object?"));
            showActionTab(ref obj.isEdible, ref obj.edibleVars);
        }
        if (defaultValues.drinkActive)
        {
            obj.isDrinkable = GUILayout.Toggle(obj.isDrinkable, new GUIContent("Is Drinkable", "Can you drink this object?"));
            showActionTab(ref obj.isDrinkable, ref obj.drinkableVars);
        }
        if (defaultValues.talkActive)
        {
            obj.isTalkable = GUILayout.Toggle(obj.isTalkable, new GUIContent("Is Talkable", "Can you talk to this object/person?"));
            showActionTab(ref obj.isTalkable, ref obj.talkableVars);
        }
        if (defaultValues.killActive)
        {
            obj.isKillable = GUILayout.Toggle(obj.isKillable, new GUIContent("Is Killable", "Can you kill this object/person?"));
            showActionTab(ref obj.isKillable, ref obj.killableVars);
        }
        if (defaultValues.breakActive)
        {
            obj.isBreakable = GUILayout.Toggle(obj.isBreakable, new GUIContent("Is Breakable", "Can you break this object?"));
            showActionTab(ref obj.isBreakable, ref obj.breakableVars);
        }
        if (defaultValues.sitActive)
        {
            obj.isSittable = GUILayout.Toggle(obj.isSittable, new GUIContent("Is Sittable", "Can you sit on this object?"));
            showActionTab(ref obj.isSittable, ref obj.sittableVars);
        }
        if (defaultValues.useActive)
        {
            obj.isUsable = GUILayout.Toggle(obj.isUsable, new GUIContent("Is Usable", "Can you use this object?"));
            showActionTab(ref obj.isUsable, ref obj.usableVars);
        }
        if (defaultValues.pickupActive)
        {
            obj.isPickupable = GUILayout.Toggle(obj.isPickupable, new GUIContent("Is Pickupable", "Can you pick up this object?"));
            showActionTab(ref obj.isPickupable, ref obj.pickupableVars);
        }
        if (defaultValues.wearActive)
        {
            obj.isWearable = GUILayout.Toggle(obj.isWearable, new GUIContent("Is Wearable", "Can you wear/equip this object?"));
            showActionTab(ref obj.isWearable, ref obj.wearableVars);
        }
        if (defaultValues.openActive)
        {
            obj.isOpenable = GUILayout.Toggle(obj.isOpenable, new GUIContent("Is Openable", "Can you open this object?"));
            showActionTab(ref obj.isOpenable, ref obj.openableVars);
        }
    }

    private static void showFlavorTexts(RoomObject obj, DefaultValues defaultValues)
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(new GUIContent("Flavor Texts", "Set the responses when something happens to this object. " +
            "Flavor texts get printed regardless of the action's success or not."), EditorStyles.boldLabel);

        if (defaultValues.eatActive)
        {
            EditorGUILayout.PrefixLabel(new GUIContent("Edible Flavor Text", "The response that's printed when the player attempts to eat this object."));
            obj.edibleFlavorText = EditorGUILayout.TextArea(obj.edibleFlavorText, EditorStyles.textArea);
        }
        if (defaultValues.drinkActive)
        {
            EditorGUILayout.PrefixLabel(new GUIContent("Drinkable Flavor Text", "The response that's printed when the player attempts to drink this object."));
            obj.drinkableFlavorText = EditorGUILayout.TextArea(obj.drinkableFlavorText, EditorStyles.textArea);
        }
        if (defaultValues.talkActive)
        {
            EditorGUILayout.PrefixLabel(new GUIContent("Talkable Flavor Text", "The response that's printed when the player attempts to talk to this object."));
            obj.talkableFlavorText = EditorGUILayout.TextArea(obj.talkableFlavorText, EditorStyles.textArea);
        }
        if (defaultValues.killActive)
        {
            EditorGUILayout.PrefixLabel(new GUIContent("Killable Flavor Text", "The response that's printed when the player attemps to kill this object."));
            obj.killableFlavorText = EditorGUILayout.TextArea(obj.killableFlavorText, EditorStyles.textArea);
        }
        if (defaultValues.breakActive)
        {
            EditorGUILayout.PrefixLabel(new GUIContent("Breakable Flavor Text", "The response that's printed when the player attempts to break this object."));
            obj.breakableFlavorText = EditorGUILayout.TextArea(obj.breakableFlavorText, EditorStyles.textArea);
        }
        if (defaultValues.sitActive)
        {
            EditorGUILayout.PrefixLabel(new GUIContent("Sittable Flavor Text", "The response that's printed when the player attempts to sit on this object."));
            obj.sittableFlavorText = EditorGUILayout.TextArea(obj.sittableFlavorText, EditorStyles.textArea);
        }
        if (defaultValues.useActive)
        {
            EditorGUILayout.PrefixLabel(new GUIContent("Usable Flavor Text", "The response that's printed when the player attempts to use this object."));
            obj.usableFlavorText = EditorGUILayout.TextArea(obj.usableFlavorText, EditorStyles.textArea);
        }
        if (defaultValues.pickupActive)
        {
            EditorGUILayout.PrefixLabel(new GUIContent("Pickupable Flavor Text", "The response that's printed when the player attempts to pick up this object."));
            obj.pickupableFlavorText = EditorGUILayout.TextArea(obj.pickupableFlavorText, EditorStyles.textArea);
        }
        if (defaultValues.wearActive)
        {
            EditorGUILayout.PrefixLabel(new GUIContent("Wearable Flavor Text", "The response that's printed when the player attempts to wear this object."));
            obj.wearableFlavorText = EditorGUILayout.TextArea(obj.wearableFlavorText, EditorStyles.textArea);
        }
        if (defaultValues.openActive)
        {
            EditorGUILayout.PrefixLabel(new GUIContent("Openable Flavor Text", "The response that's printed when the player attempts to open this object."));
            obj.openableFlavorText = EditorGUILayout.TextArea(obj.openableFlavorText, EditorStyles.textArea);
        }
    }

    private void showMiscTab(RoomObject obj, DefaultValues defaultValues)
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(new GUIContent("Misc Variables", "Other options for this object"), EditorStyles.boldLabel);

        GUIContent aliasContent = new GUIContent("RoomObject Aliases", "The different names of the object in-game, which the player types to interact with." +
                " Aliases are separated by a comma. Leave empty to set it to the object's name in the editor by default.\nEx: Potion of Healing,Healing Potion,Red Potion");
        EditorGUILayout.PrefixLabel(aliasContent);
        showAdditionalTextArea(ref obj.objectAliases);

        if (defaultValues.lookAtActive)
        {
            GUIContent lookAtContent = new GUIContent("Look At Flavor Text", "The text that's printed when the player looks at the object");
            EditorGUILayout.PrefixLabel(lookAtContent);
            obj.lookAtFlavorText = EditorGUILayout.TextArea(obj.lookAtFlavorText, EditorStyles.textArea);
        }
    }

    private void showActionTab(ref bool toggleBool, ref List<RoomObject.EditorVariables> objVars, int indentation = 0, int depth = 0)
    {
        GUILayout.BeginVertical(); 
        if (toggleBool)
        {
            for (int i = 0; i < objVars.Count; i++)
            {
                showOneAction(ref objVars, i, indentation, depth);
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(20 + indentation);

            if (GUILayout.Button($"Add new action ({depth})"))
            {
                RoomObject.EditorVariables newVar = new RoomObject.EditorVariables();
                newVar.varsToChange.targetObject = (RoomObject)target;
                objVars.Add(newVar);

            }
            GUILayout.EndHorizontal();
        }
        else
        {
            objVars.Clear();
        }
        GUILayout.EndVertical();
    }

    private void showOneAction(ref List<RoomObject.EditorVariables> objVars , int i, int indentation = 0, int depth = 0)
    {
        GUILayout.BeginHorizontal();

        GUILayout.Space(20 + indentation);

        // Delete an action
        if (GUILayout.Button("-", GUILayout.MaxWidth(25)))
        {
            objVars.Remove(objVars[i]);

            // Break out so it doesn't get an index out of range error when continuing
            return;
        }

        objVars[i].actionCategory = (RoomObject.ActionCategory)EditorGUILayout.EnumPopup(GUIContent.none, objVars[i].actionCategory, GUILayout.MaxWidth(150));

        // Show Player Actions
        if (objVars[i].actionCategory == RoomObject.ActionCategory.PlayerActions)
        {
            objVars[i].playerAction = (RoomObject.PlayerAction)EditorGUILayout.EnumPopup(GUIContent.none, objVars[i].playerAction);
        }

        // Show Object Actions
        else if (objVars[i].actionCategory == RoomObject.ActionCategory.ObjectActions)
        {
            objVars[i].objectAction = (RoomObject.ObjectAction)EditorGUILayout.EnumPopup(GUIContent.none, objVars[i].objectAction);
        }

        // Show Room Actions
        else if (objVars[i].actionCategory == RoomObject.ActionCategory.RoomActions)
        {
            objVars[i].roomAction = (RoomObject.RoomAction)EditorGUILayout.EnumPopup(GUIContent.none, objVars[i].roomAction);
        }

        // Show a conditional and all of the actions inside it
        else if (objVars[i].actionCategory == RoomObject.ActionCategory.Conditionals)
        {
            objVars[i].conditional = (RoomObject.Conditional)EditorGUILayout.EnumPopup(GUIContent.none, objVars[i].conditional);

            GUILayout.EndHorizontal();
            showAllAdditionalVariables(ref objVars, i, indentation);
            bool needThisForRef = true;
            showActionTab(ref needThisForRef, ref objVars[i].conditionalVars, indentation + 30, depth + 1);
            return;
        }

        GUILayout.EndHorizontal();

        // Show additional values that are required for some actions
        showAllAdditionalVariables(ref objVars, i, indentation);
    }

    private void showAllAdditionalVariables(ref List<RoomObject.EditorVariables> objVars, int i, int indentation = 0)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(50 + indentation);
        GUILayout.BeginVertical();

        GUIContent targetObjectLabel = new GUIContent("Target Object", "The object that is affected by the selected action command. Set to the current object by default.");
        GUIContent targetRoomLabel = new GUIContent("Target Room", "The room that is affected by the selected action command.");
        // Change this to a Switch statement later

        if (objVars[i].actionCategory == RoomObject.ActionCategory.PlayerActions)
        {
            if (objVars[i].playerAction == RoomObject.PlayerAction.AddToInventory)
            {
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].playerAction == RoomObject.PlayerAction.RemoveFromInventory)
            {
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].playerAction == RoomObject.PlayerAction.EquipItem)
            {
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].playerAction == RoomObject.PlayerAction.RemoveEquippedItem)
            {
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
        }
        else if (objVars[i].actionCategory == RoomObject.ActionCategory.ObjectActions)
        {
            if (objVars[i].objectAction == RoomObject.ObjectAction.DestroyObject)
            {
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsEdible)
            {
                objVars[i].varsToChange.isEdible = GUILayout.Toggle(objVars[i].varsToChange.isEdible, "Become Edible");
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsDrinkable)
            {
                objVars[i].varsToChange.isDrinkable = GUILayout.Toggle(objVars[i].varsToChange.isDrinkable, "Become Drinkable");
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsTalkable)
            {
                objVars[i].varsToChange.isTalkable = GUILayout.Toggle(objVars[i].varsToChange.isTalkable, "Become Talkable");
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsKillable)
            {
                objVars[i].varsToChange.isKillable = GUILayout.Toggle(objVars[i].varsToChange.isKillable, "Become Killable");
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsBreakable)
            {
                objVars[i].varsToChange.isBreakable = GUILayout.Toggle(objVars[i].varsToChange.isBreakable, "Become Breakable");
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsSittable)
            {
                objVars[i].varsToChange.isSittable = GUILayout.Toggle(objVars[i].varsToChange.isSittable, "Become Sittable");
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsUsable)
            {
                objVars[i].varsToChange.isUsable = GUILayout.Toggle(objVars[i].varsToChange.isUsable, "Become Usable");
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsPickupable)
            {
                objVars[i].varsToChange.isPickupable = GUILayout.Toggle(objVars[i].varsToChange.isPickupable, "Become Pickupable");
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsWearable)
            {
                objVars[i].varsToChange.isWearable = GUILayout.Toggle(objVars[i].varsToChange.isWearable, "Become Wearable");
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsOpenable)
            {
                objVars[i].varsToChange.isOpenable = GUILayout.Toggle(objVars[i].varsToChange.isWearable, "Become Openable");
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }

            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeEdibleFlavorText)
            {
                showAdditionalTextArea(ref objVars[i].varsToChange.edibleFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeDrinkableFlavorText)
            {
                showAdditionalTextArea(ref objVars[i].varsToChange.drinkableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeKillableFlavorText)
            {
                showAdditionalTextArea(ref objVars[i].varsToChange.killableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeBreakableFlavorText)
            {
                showAdditionalTextArea(ref objVars[i].varsToChange.breakableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangePickupableFlavorText)
            {
                showAdditionalTextArea(ref objVars[i].varsToChange.pickupableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeSittableFlavorText)
            {
                showAdditionalTextArea(ref objVars[i].varsToChange.sittableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeTalkableFlavorText)
            {
                showAdditionalTextArea(ref objVars[i].varsToChange.talkableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeUsableFlavorText)
            {
                showAdditionalTextArea(ref objVars[i].varsToChange.usableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeWearableFlavorText)
            {
                showAdditionalTextArea(ref objVars[i].varsToChange.wearableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeOpenableFlavorText)
            {
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
                showAdditionalTextArea(ref objVars[i].varsToChange.openableFlavorText);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeLookAtFlavorText)
            {
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
                showAdditionalTextArea(ref objVars[i].varsToChange.lookAtFlavorText);
            }
        }
        else if (objVars[i].actionCategory == RoomObject.ActionCategory.RoomActions)
        {
            if (objVars[i].roomAction == RoomObject.RoomAction.SwitchRooms)
            {
                // TODO: Change this later
                GUIContent content = new GUIContent("Move to room", "Instantly move to the room you've selected, regardless of where it is. However, it has to be in roomTracker's room list.");
                objVars[i].varsToChange.targetRoom = (Room)EditorGUILayout.ObjectField(content, objVars[i].varsToChange.targetRoom, typeof(Room), true);
            }
            else if (objVars[i].roomAction == RoomObject.RoomAction.AddObjectToRoom)
            {
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].roomAction == RoomObject.RoomAction.RemoveObjectFromRoom)
            {
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].roomAction == RoomObject.RoomAction.ChangeRoomEntryText)
            {
                showSelectableRoom(targetRoomLabel, ref objVars[i].varsToChange.targetRoom);
                showAdditionalTextArea(ref objVars[i].varsToChange.roomEntryText);
            }
            else if (objVars[i].roomAction == RoomObject.RoomAction.ChangeRoomLookText)
            {
                showSelectableRoom(targetRoomLabel, ref objVars[i].varsToChange.targetRoom);
                showAdditionalTextArea(ref objVars[i].varsToChange.roomLookText);
            }
            else if (objVars[i].roomAction == RoomObject.RoomAction.ActivateRoomConnection)
            {
                showSelectableRoom(targetRoomLabel, ref objVars[i].varsToChange.targetRoom);
                showSelectableRoom(new GUIContent("Room To Activate", "The room that's going to be activated in Target Room."), ref objVars[i].varsToChange.targetRoomToActivate);
            }
            else if (objVars[i].roomAction == RoomObject.RoomAction.DisableRoomConnection)
            {
                showSelectableRoom(targetRoomLabel, ref objVars[i].varsToChange.targetRoom);
                showSelectableRoom(new GUIContent("Room To Activate", "The room that's going to be activated in Target Room."), ref objVars[i].varsToChange.targetRoomToActivate);
            }
        }
        else if (objVars[i].actionCategory == RoomObject.ActionCategory.Conditionals)
        {
            GUIContent conditionalBoolContent = new GUIContent("Activate on true", "Determines whether the commands in this conditional will activate when it's true or false");
            objVars[i].conditionalBool = GUILayout.Toggle(objVars[i].conditionalBool, conditionalBoolContent);

            if (objVars[i].conditional == RoomObject.Conditional.ObjectExistsInRoom)
            {
                showSelectableRoom(targetRoomLabel, ref objVars[i].varsToChange.targetRoom);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].conditional == RoomObject.Conditional.ObjectExistsInInventory)
            {
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].conditional == RoomObject.Conditional.ObjectExistsInEquipment)
            {
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].conditional == RoomObject.Conditional.RoomWasVisited)
            {
                showSelectableRoom(targetRoomLabel, ref objVars[i].varsToChange.targetRoom);
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private void showAdditionalTextArea(ref string text)
    {
        text = EditorGUILayout.TextArea(text, EditorStyles.textArea);
    }

    private void showSelectableRoomObject(GUIContent label, ref RoomObject obj)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);
        obj = (RoomObject)EditorGUILayout.ObjectField(obj, typeof(RoomObject), true, GUILayout.MaxWidth(200f));
        GUILayout.EndHorizontal();
    }

    private void showSelectableRoom(GUIContent label, ref Room room)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);
        room = (Room)EditorGUILayout.ObjectField(room, typeof(Room), true, GUILayout.MaxWidth(200f));
        GUILayout.EndHorizontal();
    }
}

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(RoomObject))]
public class RoomObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoomObject obj = (RoomObject)target;

        // Using this method makes the variables in the object save properly when exiting Unity.
        // Otherwise, the enum lists reset on restart for whatever reason.
        EditorUtility.SetDirty(obj);

        obj.isEdible = GUILayout.Toggle(obj.isEdible, "Is Edible");
        showActionTab(ref obj.isEdible, ref obj.edibleVars);
        obj.isTalkable = GUILayout.Toggle(obj.isTalkable, "Is Talkable");
        showActionTab(ref obj.isTalkable, ref obj.talkableVars);
        obj.isKillable = GUILayout.Toggle(obj.isKillable, "Is Killable");
        showActionTab(ref obj.isKillable, ref obj.killableVars);
        obj.isSittable = GUILayout.Toggle(obj.isSittable, "Is Sittable");
        showActionTab(ref obj.isSittable, ref obj.sittableVars);
        obj.isUsable = GUILayout.Toggle(obj.isUsable, "Is Usable");
        showActionTab(ref obj.isUsable, ref obj.usableVars);
        obj.isPickupable = GUILayout.Toggle(obj.isPickupable, "Is Pickupable");
        showActionTab(ref obj.isPickupable, ref obj.pickupVars);
        obj.isWearable = GUILayout.Toggle(obj.isWearable, "Is Wearable");
        showActionTab(ref obj.isWearable, ref obj.wearableVars);
        obj.isOpenable = GUILayout.Toggle(obj.isOpenable, "Is Openable");
        showActionTab(ref obj.isOpenable, ref obj.openableVars);

        float textAreaHeight = 30f;

        GUILayout.Label("Edible Flavor Text");
        obj.EdibleFlavorText = EditorGUILayout.TextArea(obj.EdibleFlavorText, GUILayout.MinHeight(textAreaHeight));
        GUILayout.Label("Talkable Flavor Text");
        obj.TalkableFlavorText = EditorGUILayout.TextArea(obj.TalkableFlavorText, GUILayout.MinHeight(textAreaHeight));
        GUILayout.Label("Killable Flavor Text");
        obj.KillableFlavorText = EditorGUILayout.TextArea(obj.KillableFlavorText, GUILayout.MinHeight(textAreaHeight));
        GUILayout.Label("Sittable Flavor Text");
        obj.SittableFlavorText = EditorGUILayout.TextArea(obj.SittableFlavorText, GUILayout.MinHeight(textAreaHeight));
        GUILayout.Label("Usable Flavor Text");
        obj.UsableFlavorText = EditorGUILayout.TextArea(obj.UsableFlavorText, GUILayout.MinHeight(textAreaHeight));
        GUILayout.Label("Pickupable Flavor Text");
        obj.PickupableFlavorText = EditorGUILayout.TextArea(obj.PickupableFlavorText, GUILayout.MinHeight(textAreaHeight));
        GUILayout.Label("Wearable Flavor Text");
        obj.WearableFlavorText = EditorGUILayout.TextArea(obj.WearableFlavorText, GUILayout.MinHeight(textAreaHeight));
        GUILayout.Label("Openable Flavor Text");
        obj.OpenableFlavorText = EditorGUILayout.TextArea(obj.OpenableFlavorText, GUILayout.MinHeight(textAreaHeight));

        showMiscTab(obj);

        base.OnInspectorGUI();
    }

    private void showMiscTab(RoomObject obj)
    {
        GUILayout.BeginHorizontal();

        GUIContent content = new GUIContent("RoomObject Aliases", "The different names of the object in-game, which the player types to interact with." +
                " Aliases are separated by a comma. Leave empty to set it to the object's name in the editor by default.\nEx: Potion of Healing,Healing Potion,Red Potion");
        EditorGUILayout.PrefixLabel(content);
        showAdditionalTextField(ref obj.objectAliases);

        GUILayout.EndHorizontal();
    }

    private void showActionTab(ref bool toggleBool, ref List<RoomObject.EditorVariables> objVars)
    {
        GUILayout.BeginVertical(); 
        if (toggleBool)
        {
            for (int i = 0; i < objVars.Count; i++)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Space(20);

                // Delete an action
                if (GUILayout.Button("-", GUILayout.MaxWidth(25))){
                    objVars.Remove(objVars[i]);

                    // Break out so it doesn't get an index out of range error when continuing
                    break;
                }

                objVars[i].actionCategory = (RoomObject.ActionCategory)EditorGUILayout.EnumPopup(GUIContent.none, objVars[i].actionCategory, GUILayout.MaxWidth(150));

                // Show Player Actions
                if (objVars[i].actionCategory == RoomObject.ActionCategory.PlayerActions)
                {
                    objVars[i].playerAction = (RoomObject.PlayerAction)EditorGUILayout.EnumPopup(GUIContent.none, objVars[i].playerAction);
                }

                // Show Object Actions
                if (objVars[i].actionCategory == RoomObject.ActionCategory.ObjectActions)
                {
                    objVars[i].objectAction = (RoomObject.ObjectAction)EditorGUILayout.EnumPopup(GUIContent.none, objVars[i].objectAction);
                }

                // Show Room Actions
                if (objVars[i].actionCategory == RoomObject.ActionCategory.RoomActions)
                {
                    objVars[i].roomAction = (RoomObject.RoomAction)EditorGUILayout.EnumPopup(GUIContent.none, objVars[i].roomAction);
                }

                GUILayout.EndHorizontal();

                // Show additional values that are required for some actions
                showAllAdditionalVariables(ref objVars, i);
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            if (GUILayout.Button("Add new action"))
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

    private void showAllAdditionalVariables(ref List<RoomObject.EditorVariables> objVars, int i)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(50);
        GUILayout.BeginVertical();

        GUIContent targetObjectLabel = new GUIContent("Target Object", "The object that is affected by the selected action command. Set to the current object by default.");
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
            if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsEdible)
            {
                objVars[i].varsToChange.isEdible = GUILayout.Toggle(objVars[i].varsToChange.isEdible, "Become Edible");
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
                showAdditionalTextField(ref objVars[i].varsToChange.edibleFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeKillableFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.killableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangePickupableFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.pickupableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeSittableFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.sittableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeTalkableFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.talkableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeUsableFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.usableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeWearableFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.wearableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeOpenableFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.openableFlavorText);
                showSelectableRoomObject(targetObjectLabel, ref objVars[i].varsToChange.targetObject);
            }
        }
        else if (objVars[i].actionCategory == RoomObject.ActionCategory.RoomActions)
        {
            if (objVars[i].roomAction == RoomObject.RoomAction.ChangeRoom)
            {
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
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private void showAdditionalTextField(ref string textField)
    {
        textField = EditorGUILayout.TextArea(textField);
    }

    private void showSelectableRoomObject(GUIContent label, ref RoomObject obj)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);
        obj = (RoomObject)EditorGUILayout.ObjectField(obj, typeof(RoomObject), true, GUILayout.MaxWidth(200f));
        GUILayout.EndHorizontal();
    }
}

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

        
        GUILayout.Label("Edible Flavor Text");
        obj.EdibleFlavorText = EditorGUILayout.TextArea(obj.EdibleFlavorText, GUILayout.MinHeight(30f));
        GUILayout.Label("Talkable Flavor Text");
        obj.TalkableFlavorText = EditorGUILayout.TextArea(obj.TalkableFlavorText, GUILayout.MinHeight(30f));
        GUILayout.Label("Killable Flavor Text");
        obj.KillableFlavorText = EditorGUILayout.TextArea(obj.KillableFlavorText, GUILayout.MinHeight(30f));
        GUILayout.Label("Sittable Flavor Text");
        obj.SittableFlavorText = EditorGUILayout.TextArea(obj.SittableFlavorText, GUILayout.MinHeight(30f));
        GUILayout.Label("Usable Flavor Text");
        obj.UsableFlavorText = EditorGUILayout.TextArea(obj.UsableFlavorText, GUILayout.MinHeight(30f));
        GUILayout.Label("Pickupable Flavor Text");
        obj.PickupableFlavorText = EditorGUILayout.TextArea(obj.PickupableFlavorText, GUILayout.MinHeight(30f));
        GUILayout.Label("Wearable Flavor Text");
        obj.WearableFlavorText = EditorGUILayout.TextArea(obj.WearableFlavorText, GUILayout.MinHeight(30f));

        base.OnInspectorGUI();
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

                objVars[i].actionCategory = (RoomObject.ActionCategory)EditorGUILayout.EnumPopup("", objVars[i].actionCategory, GUILayout.MaxWidth(150));

                // Show Player Actions
                if (objVars[i].actionCategory == RoomObject.ActionCategory.PlayerActions)
                {
                    objVars[i].playerAction = (RoomObject.PlayerAction)EditorGUILayout.EnumPopup("", objVars[i].playerAction);
                }

                // Show Object Actions
                if (objVars[i].actionCategory == RoomObject.ActionCategory.ObjectActions)
                {
                    objVars[i].objectAction = (RoomObject.ObjectAction)EditorGUILayout.EnumPopup("", objVars[i].objectAction);
                }

                // Show Room Actions
                if (objVars[i].actionCategory == RoomObject.ActionCategory.RoomActions)
                {
                    objVars[i].roomAction = (RoomObject.RoomAction)EditorGUILayout.EnumPopup("", objVars[i].roomAction);
                }

                GUILayout.EndHorizontal();

                // Show additional values that are required for some actions
                showAllAdditionalVariables(ref objVars, i);
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            if (GUILayout.Button("Add new action"))
            {
                // Fills all lists with actions. While this means unnecessary values exist, it makes indexing a whole lot easier
                // when they're all on the same index.
                //fillListsWithValues(ref objVars);

                RoomObject.EditorVariables newVar = new RoomObject.EditorVariables();
                objVars.Add(newVar);
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            objVars = new List<RoomObject.EditorVariables>();
        }
        GUILayout.EndVertical();
    }

    private void showAllAdditionalVariables(ref List<RoomObject.EditorVariables> objVars, int i)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(50);

        // Change this to a Switch statement later
        if (objVars[i].actionCategory == RoomObject.ActionCategory.ObjectActions)
        {
            if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsEdible)
            {
                showAdditionalBool(ref objVars[i].varsToChange.isEdible, "Become Edible");
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsTalkable)
            {
                showAdditionalBool(ref objVars[i].varsToChange.isTalkable, "Become Talkable");
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsKillable)
            {
                showAdditionalBool(ref objVars[i].varsToChange.isKillable, "Become Killable");
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsSittable)
            {
                showAdditionalBool(ref objVars[i].varsToChange.isSittable, "Become Sittable");
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsUsable)
            {
                showAdditionalBool(ref objVars[i].varsToChange.isUsable, "Become Usable");
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsPickupable)
            {
                showAdditionalBool(ref objVars[i].varsToChange.isPickupable, "Become Pickupable");
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.SetIsWearable)
            {
                showAdditionalBool(ref objVars[i].varsToChange.isWearable, "Become Wearable");
            }

            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeEdibleFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.edibleFlavorText);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeKillableFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.killableFlavorText);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangePickupableFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.pickupableFlavorText);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeSittableFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.sittableFlavorText);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeTalkableFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.talkableFlavorText);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeUsableFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.usableFlavorText);
            }
            else if (objVars[i].objectAction == RoomObject.ObjectAction.ChangeWearableFlavorText)
            {
                showAdditionalTextField(ref objVars[i].varsToChange.wearableFlavorText);
            }

        }
        GUILayout.EndHorizontal();
    }

    private void showAdditionalBool(ref bool toggleBool, string boolText)
    {
        toggleBool = GUILayout.Toggle(toggleBool, boolText);
    }

    private void showAdditionalTextField(ref string textField)
    {
        textField = GUILayout.TextArea(textField);
    }

    /* DELETE THIS LATER
    private void fillListsWithValues(ref RoomObject.EditorVariables objVars)
    {
        RoomObject.ActionCategory category = RoomObject.ActionCategory.PlayerActions;
        objVars.actionCategories.Add(category);
        RoomObject.PlayerAction playerAction = RoomObject.PlayerAction.AddToInventory;
        objVars.playerActions.Add(playerAction);
        RoomObject.ObjectAction objectAction = RoomObject.ObjectAction.IsSittable;
        objVars.objectActions.Add(objectAction);
        RoomObject.RoomAction roomAction = RoomObject.RoomAction.AddObjectToRoom;
        objVars.roomActions.Add(roomAction);
    }*/ 
}

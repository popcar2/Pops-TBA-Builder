using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{
    bool foldRoomConnections = true;
    bool foldRoomEntryActions = true;

    public override void OnInspectorGUI()
    {
        Room room = (Room)target;
        EditorUtility.SetDirty(room);

        base.OnInspectorGUI();
        showConnectedRooms(room.roomConnections);
        showRoomEntryActions(room);
    }

    private void showRoomEntryActions(Room room)
    {
        foldRoomEntryActions = EditorGUILayout.Foldout(foldRoomEntryActions, "Room Entry Actions", true);
        if (!foldRoomEntryActions)
            return;

        bool isTrue = true;
        RoomObjectEditor.showActionTab(ref isTrue, ref room.roomEntryActions, target, -20);
    }

    private void showConnectedRooms(List<Room.RoomConnectionVars> roomConnections)
    {
        foldRoomConnections = EditorGUILayout.Foldout(foldRoomConnections, "Room Connections", true);
        if (!foldRoomConnections)
            return;

        // Loop through connections
        for (int i = 0; i < roomConnections.Count; i++)
        {
            showOneConnectedRoom(roomConnections, roomConnections[i]);
        }

        // Add a connection
        if (GUILayout.Button("Add new room connection"))
        {
            Room.RoomConnectionVars newVar = new Room.RoomConnectionVars();
            roomConnections.Add(newVar);
        }
    }

    private void showOneConnectedRoom(List<Room.RoomConnectionVars> roomConnections, Room.RoomConnectionVars roomConnection)
    {
        GUILayout.BeginHorizontal();

        // Delete a connection
        if (GUILayout.Button("-", GUILayout.MaxWidth(25)))
        {
            roomConnections.Remove(roomConnection);
            return;
        }

        roomConnection.room = (Room)EditorGUILayout.ObjectField(roomConnection.room, typeof(Room), true, GUILayout.MaxWidth(200f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(30f);
        GUILayout.BeginVertical();

        GUIContent isActiveContent = new GUIContent("Is Active", "Whether or not the player can access this room by default or not. " +
            "Inactive rooms will be inaccessible until they're activated through an action command.");
        roomConnection.isActive = GUILayout.Toggle(roomConnection.isActive, isActiveContent);

        GUIContent roomInactiveTextContent = new GUIContent("Room Inactive Text", "The flavor text that's printed when the user attempts to move to the room while it's inactive");
        EditorGUILayout.PrefixLabel(roomInactiveTextContent);
        roomConnection.roomInactiveText = EditorGUILayout.TextArea(roomConnection.roomInactiveText, EditorStyles.textArea);

        GUILayout.BeginHorizontal();

        GUIContent roomAliasesContent = new GUIContent("Room Aliases", "The different names of the room in-game, which the player types to move to." +
            " Aliases are separated by a comma. Leave empty to set to the room's name in the editor by default.\nEx: Dungeon,Dungeon Room,Cell,Prison Cell");
        EditorGUILayout.PrefixLabel(roomAliasesContent);
        roomConnection.roomAlias = EditorGUILayout.TextArea(roomConnection.roomAlias);

        GUILayout.EndHorizontal();

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}

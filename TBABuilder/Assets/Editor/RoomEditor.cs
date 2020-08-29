using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{
    bool foldRoomConnections = true;
    public override void OnInspectorGUI()
    {
        Room room = (Room)target;
        EditorUtility.SetDirty(room);

        base.OnInspectorGUI();
        showConnectedRooms(ref room.roomConnections);
    }

    private void showConnectedRooms(ref List<Room.RoomConnectionVars> roomConnections)
    {
        foldRoomConnections = EditorGUILayout.Foldout(foldRoomConnections, "Room Connections", true);
        if (!foldRoomConnections)
            return;

        // Loop through connections
        for (int i = 0; i < roomConnections.Count; i++)
        {
            GUILayout.BeginHorizontal();
            // Delete a connection
            if (GUILayout.Button("-", GUILayout.MaxWidth(25)))
            {
                roomConnections.Remove(roomConnections[i]);
                break;
            }

            roomConnections[i].room = (Room)EditorGUILayout.ObjectField(roomConnections[i].room, typeof(Room), true, GUILayout.MaxWidth(200f));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(30f);

            GUIContent content = new GUIContent("Room Aliases", "The different names of the room in-game, which the player types to move to." +
                " Aliases are separated by a comma. Leave empty to set to the room's name in the editor by default.\nEx: Dungeon,Dungeon Room,Cell,Prison Cell");
            EditorGUILayout.PrefixLabel(content);
            roomConnections[i].roomAlias = EditorGUILayout.TextArea(roomConnections[i].roomAlias);

            GUILayout.EndHorizontal();
        }

        // Add a connection
        if (GUILayout.Button("Add new room connection"))
        {
            Room.RoomConnectionVars newVar = new Room.RoomConnectionVars();
            roomConnections.Add(newVar);
        }
    }
}

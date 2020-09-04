using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RoomObject", order = 0)]
[Serializable]
public class RoomObject : ScriptableObject
{
    // You may be wondering why these variables are public rather than have getters and setters
    // This is because properties can't be passed by reference and that makes it more difficult to manipulate outside the class
    // Since you're free to get and set them ANYWAYS and there isn't a special rule for either, I decided to make them public

    [NonSerialized] public bool isInitialized = false;

    public bool isEdible = false;
    public bool isTalkable = false;
    public bool isKillable = false;
    public bool isSittable = false;
    public bool isUsable = false;
    public bool isPickupable = false;
    public bool isWearable = false;
    public bool isOpenable = false;

    public bool runtimeIsEdible;
    public bool runtimeIsTalkable;
    public bool runtimeIsKillable;
    public bool runtimeIsSittable;
    public bool runtimeIsUsable;
    public bool runtimeIsPickupable;
    public bool runtimeIsWearable;
    public bool runtimeIsOpenable;

    public string edibleFlavorText;
    public string talkableFlavorText;
    public string killableFlavorText;
    public string sittableFlavorText;
    public string usableFlavorText;
    public string pickupableFlavorText;
    public string wearableFlavorText;
    public string openableFlavorText;
    public string lookAtFlavorText;

    public string runtimeEdibleFlavorText;
    public string runtimeTalkableFlavorText;
    public string runtimeKillableFlavorText;
    public string runtimeSittableFlavorText;
    public string runtimeUsableFlavorText;
    public string runtimePickupableFlavorText;
    public string runtimeWearableFlavorText;
    public string runtimeOpenableFlavorText;
    public string runtimeLookAtFlavorText;

    public void initializeRuntimeVariables() {
        isInitialized = true;

        runtimeIsEdible = isEdible;
        runtimeIsTalkable = isTalkable;
        runtimeIsKillable = isKillable;
        runtimeIsSittable = isSittable;
        runtimeIsUsable = isUsable;
        runtimeIsPickupable = isPickupable;
        runtimeIsWearable = isWearable;
        runtimeIsOpenable = isOpenable;

        runtimeEdibleFlavorText = edibleFlavorText;
        runtimeTalkableFlavorText = talkableFlavorText;
        runtimeKillableFlavorText = killableFlavorText;
        runtimeSittableFlavorText = sittableFlavorText;
        runtimeUsableFlavorText = usableFlavorText;
        runtimePickupableFlavorText = pickupableFlavorText;
        runtimeWearableFlavorText = wearableFlavorText;
        runtimeOpenableFlavorText = openableFlavorText;
        runtimeLookAtFlavorText = lookAtFlavorText;
    }

    // Each index of the list represents one action.
    public List<EditorVariables> edibleVars = new List<EditorVariables>();
    public List<EditorVariables> talkableVars = new List<EditorVariables>();
    public List<EditorVariables> killableVars = new List<EditorVariables>();
    public List<EditorVariables> sittableVars = new List<EditorVariables>();
    public List<EditorVariables> usableVars = new List<EditorVariables>();
    public List<EditorVariables> pickupVars = new List<EditorVariables>();
    public List<EditorVariables> wearableVars = new List<EditorVariables>();
    public List<EditorVariables> openableVars = new List<EditorVariables>();

    // Misc variables
    public string objectAliases;

    [Serializable]
    public class EditorVariables
    {
        public ActionCategory actionCategory = new ActionCategory();
        public PlayerAction playerAction = new PlayerAction();
        public ObjectAction objectAction = new ObjectAction();
        public RoomAction roomAction = new RoomAction();
        public CurrentVarsToChange varsToChange = new CurrentVarsToChange();
    }

    // Exists in EditorVariables. Just here for tidying up the large amount of variables.
    [Serializable]
    public class CurrentVarsToChange
    {
        public bool isEdible;
        public bool isTalkable;
        public bool isKillable;
        public bool isSittable;
        public bool isUsable;
        public bool isPickupable;
        public bool isWearable;
        public bool isOpenable;

        public string edibleFlavorText;
        public string talkableFlavorText;
        public string killableFlavorText;
        public string sittableFlavorText;
        public string usableFlavorText;
        public string pickupableFlavorText;
        public string wearableFlavorText;
        public string openableFlavorText;
        public string lookAtFlavorText;

        public Room targetRoom;

        public string roomText;

        public RoomObject targetObject;
    }

    [Serializable]
    public enum ActionCategory
    {
        PlayerActions,
        ObjectActions,
        RoomActions
    }

    [Serializable]
    public enum PlayerAction
    {
        AddToInventory,
        RemoveFromInventory,
        KillPlayer,
        EquipItem,
        RemoveEquippedItem
    }

    [Serializable]
    public enum ObjectAction
    {
        DestroyObject,

        SetIsEdible,
        SetIsTalkable,
        SetIsKillable,
        SetIsSittable,
        SetIsUsable,
        SetIsPickupable,
        SetIsWearable,
        SetIsOpenable,

        ChangeEdibleFlavorText,
        ChangeTalkableFlavorText,
        ChangeKillableFlavorText,
        ChangeSittableFlavorText,
        ChangeUsableFlavorText,
        ChangePickupableFlavorText,
        ChangeWearableFlavorText,
        ChangeOpenableFlavorText,
        ChangeLookAtFlavorText
    }

    [Serializable]
    public enum RoomAction
    {
        RemoveObjectFromRoom,
        AddObjectToRoom,
        SwitchRooms,
        ChangeRoomText
    }
}
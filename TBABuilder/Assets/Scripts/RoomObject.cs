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
    public bool isDrinkable = false;
    public bool isTalkable = false;
    public bool isKillable = false;
    public bool isBreakable = false;
    public bool isSittable = false;
    public bool isUsable = false;
    public bool isPickupable = false;
    public bool isWearable = false;
    public bool isOpenable = false;

    public bool runtimeIsEdible;
    public bool runtimeIsDrinkable;
    public bool runtimeIsTalkable;
    public bool runtimeIsKillable;
    public bool runtimeIsBreakable;
    public bool runtimeIsSittable;
    public bool runtimeIsUsable;
    public bool runtimeIsPickupable;
    public bool runtimeIsWearable;
    public bool runtimeIsOpenable;

    public string edibleFlavorText;
    public string drinkableFlavorText;
    public string talkableFlavorText;
    public string killableFlavorText;
    public string breakableFlavorText;
    public string sittableFlavorText;
    public string usableFlavorText;
    public string pickupableFlavorText;
    public string wearableFlavorText;
    public string openableFlavorText;
    public string lookAtFlavorText;

    public string runtimeEdibleFlavorText;
    public string runtimeDrinkableFlavorText;
    public string runtimeTalkableFlavorText;
    public string runtimeKillableFlavorText;
    public string runtimeBreakableFlavorText;
    public string runtimeSittableFlavorText;
    public string runtimeUsableFlavorText;
    public string runtimePickupableFlavorText;
    public string runtimeWearableFlavorText;
    public string runtimeOpenableFlavorText;
    public string runtimeLookAtFlavorText;

    /// <summary>
    /// Initializes runtime variables. Runs whenever the object is called. 
    /// </summary>
    public void initializeRuntimeVariables() {
        isInitialized = true;

        runtimeIsEdible = isEdible;
        runtimeIsDrinkable = isDrinkable;
        runtimeIsTalkable = isTalkable;
        runtimeIsKillable = isKillable;
        runtimeIsBreakable = isBreakable;
        runtimeIsSittable = isSittable;
        runtimeIsUsable = isUsable;
        runtimeIsPickupable = isPickupable;
        runtimeIsWearable = isWearable;
        runtimeIsOpenable = isOpenable;

        runtimeEdibleFlavorText = edibleFlavorText;
        runtimeDrinkableFlavorText = drinkableFlavorText;
        runtimeTalkableFlavorText = talkableFlavorText;
        runtimeKillableFlavorText = killableFlavorText;
        runtimeBreakableFlavorText = breakableFlavorText;
        runtimeSittableFlavorText = sittableFlavorText;
        runtimeUsableFlavorText = usableFlavorText;
        runtimePickupableFlavorText = pickupableFlavorText;
        runtimeWearableFlavorText = wearableFlavorText;
        runtimeOpenableFlavorText = openableFlavorText;
        runtimeLookAtFlavorText = lookAtFlavorText;
    }

    // Each index of the list represents one action.
    public List<EditorVariables> edibleVars = new List<EditorVariables>();
    public List<EditorVariables> drinkableVars = new List<EditorVariables>();
    public List<EditorVariables> talkableVars = new List<EditorVariables>();
    public List<EditorVariables> killableVars = new List<EditorVariables>();
    public List<EditorVariables> breakableVars = new List<EditorVariables>();
    public List<EditorVariables> sittableVars = new List<EditorVariables>();
    public List<EditorVariables> usableVars = new List<EditorVariables>();
    public List<EditorVariables> pickupableVars = new List<EditorVariables>();
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

        public bool isDelayed = false; // Delays the action to happen after flavor text is printed, not before

        // Conditionals
        public Conditional conditional = new Conditional();
        public List<EditorVariables> conditionalVars = new List<EditorVariables>();
        public bool conditionalBool = true;
    }

    // Exists in EditorVariables. Just here for tidying up the large amount of variables.
    [Serializable]
    public class CurrentVarsToChange
    {
        public bool isEdible;
        public bool isDrinkable;
        public bool isTalkable;
        public bool isKillable;
        public bool isBreakable;
        public bool isSittable;
        public bool isUsable;
        public bool isPickupable;
        public bool isWearable;
        public bool isOpenable;

        public string edibleFlavorText;
        public string drinkableFlavorText;
        public string talkableFlavorText;
        public string killableFlavorText;
        public string breakableFlavorText;
        public string sittableFlavorText;
        public string usableFlavorText;
        public string pickupableFlavorText;
        public string wearableFlavorText;
        public string openableFlavorText;
        public string lookAtFlavorText;

        public Room targetRoom;
        public Room targetRoomConnection; // Used in activating/deactivating room connections

        public string roomLookText;
        public string roomEntryText;

        public RoomObject targetObject;
    }

    [Serializable]
    public enum ActionCategory
    {
        PlayerActions,
        ObjectActions,
        RoomActions,
        Conditionals
    }

    [Serializable]
    public enum PlayerAction
    {
        KillPlayer,
        WinGame,
        AddToInventory,
        RemoveFromInventory,
        EquipItem,
        RemoveEquippedItem
    }

    [Serializable]
    public enum ObjectAction
    {
        DestroyObject,

        SetIsEdible,
        SetIsDrinkable,
        SetIsTalkable,
        SetIsKillable,
        SetIsBreakable,
        SetIsSittable,
        SetIsUsable,
        SetIsPickupable,
        SetIsWearable,
        SetIsOpenable,

        ChangeEdibleFlavorText,
        ChangeDrinkableFlavorText,
        ChangeTalkableFlavorText,
        ChangeKillableFlavorText,
        ChangeBreakableFlavorText,
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
        AddObjectToRoom,
        RemoveObjectFromRoom,
        ActivateRoomConnection,
        DisableRoomConnection,
        SwitchRooms,
        ChangeRoomLookText,
        ChangeRoomEntryText
    }

    [Serializable]
    public enum Conditional
    {
        ObjectExistsInRoom,
        ObjectExistsInInventory,
        ObjectExistsInEquipment,
        RoomWasVisited,
        RoomConnectionIsActive
    }
}
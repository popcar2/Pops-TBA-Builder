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

    [Header("Booleans")]
    [SerializeField] public bool isEdible = false;
    [SerializeField] public bool isTalkable = false;
    [SerializeField] public bool isKillable = false;
    [SerializeField] public bool isSittable = false;
    [SerializeField] public bool isUsable = false;
    [SerializeField] public bool isPickupable = false;
    [SerializeField] public bool isWearable = false;
    [SerializeField] public bool isOpenable = false;

    [Header("Flavor Text")]
    [SerializeField] [TextArea(1, 5)] string edibleFlavorText;
    [SerializeField] [TextArea(1, 5)] string talkableFlavorText;
    [SerializeField] [TextArea(1, 5)] string killableFlavorText;
    [SerializeField] [TextArea(1, 5)] string sittableFlavorText;
    [SerializeField] [TextArea(1, 5)] string usableFlavorText;
    [SerializeField] [TextArea(1, 5)] string pickupableFlavorText;
    [SerializeField] [TextArea(1, 5)] string wearableFlavorText;
    [SerializeField] [TextArea(1, 5)] string openableFlavorText;

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

        public Room targetRoom;
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
        DestroyThisObject,

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
        ChangeOpenableFlavorText
    }

    [Serializable]
    public enum RoomAction
    {
        RemoveObjectFromRoom,
        AddObjectToRoom,
        ChangeRoom
    }

    public string EdibleFlavorText { get => edibleFlavorText; set => edibleFlavorText = value; }
    public string TalkableFlavorText { get => talkableFlavorText; set => talkableFlavorText = value; }
    public string KillableFlavorText { get => killableFlavorText; set => killableFlavorText = value; }
    public string SittableFlavorText { get => sittableFlavorText; set => sittableFlavorText = value; }
    public string UsableFlavorText { get => usableFlavorText; set => usableFlavorText = value; }
    public string PickupableFlavorText { get => pickupableFlavorText; set => pickupableFlavorText = value; }
    public string WearableFlavorText { get => wearableFlavorText; set => wearableFlavorText = value; }
    public string OpenableFlavorText { get => openableFlavorText; set => openableFlavorText = value; }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Player : MonoBehaviour
{
    enum Status
    {
        Healthy,
        Poisoned,
        Trapped
    }

    [SerializeField] private Status playerStatus;
    [SerializeField] private List<RoomObject> inventory;

    TextPrompt textPrompt;

    private void Start()
    {
        playerStatus = Status.Healthy;
        inventory = new List<RoomObject>();
        textPrompt = FindObjectOfType<TextPrompt>();
    }

    public void addItemToInventory(RoomObject item)
    {
        item.isPickupable = false;
        item.PickupableFlavorText = "You're already holding the " + item.name.ToLower() + ".";
        inventory.Add(item);
    }

    public void removeItemFromInventory(RoomObject item)
    {
        inventory.Remove(item);
    }

    public List<RoomObject> getInventory()
    {
        return inventory;
    }

    // Might want to move to InputParser? Makes little sense making a command that only prints here, even though the inventory is in player
    public void openInventory()
    {
        if (inventory.Count == 0)
        {
            textPrompt.printText("\nYou open your pouch of items, and find that it is empty.");
            return;
        }

        StringBuilder inv = new StringBuilder("\nYou open your pouch of items, its contents are: ");
        foreach (RoomObject obj in inventory)
        {
            inv.Append(obj.name + ", ");
        }
        inv.Remove(inv.Length - 2, 2);
        inv.Append(".");
        textPrompt.printText(inv.ToString());
    }
}

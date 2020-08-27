using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Player : MonoBehaviour
{
    [SerializeField] private List<RoomObject> inventory;
    [SerializeField] private List<RoomObject> equippedItems;

    TextPrompt textPrompt;

    private void Start()
    {
        inventory = new List<RoomObject>();
        equippedItems = new List<RoomObject>();
        textPrompt = FindObjectOfType<TextPrompt>();
    }

    public void addItemToInventory(RoomObject item)
    {
        //item.isPickupable = false;
        //item.PickupableFlavorText = "You're already holding the " + item.name.ToLower() + ".";
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
            openEquipment();
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

        openEquipment();
    }

    public void openEquipment()
    {
        StringBuilder equipment = new StringBuilder("\nYour current equipped items are: ");
        if (equippedItems.Count > 0)
        {
            foreach (RoomObject obj in equippedItems)
            {
                equipment.Append(obj.name + ", ");
            }
            equipment.Remove(equipment.Length - 2, 2);
            equipment.Append(".");
            textPrompt.printText(equipment.ToString());
        }
        else
        {
            textPrompt.printText("\nYou have no items equipped.");
        }
    }

    public void equipItem(RoomObject equipment)
    {
        equippedItems.Add(equipment);
    }

    public void removeEquippedItem(RoomObject equipment)
    {
        equippedItems.Remove(equipment);
    }

    public List<RoomObject> getEquippedItems()
    {
        return equippedItems;
    }
}

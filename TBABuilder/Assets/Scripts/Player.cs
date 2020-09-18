using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Player : MonoBehaviour
{
    [SerializeField] private List<RoomObject> inventory;
    [SerializeField] private List<RoomObject> equippedItems;

    TextPrompt textPrompt;
    DefaultValues defaultValues;

    private void Start()
    {
        inventory = new List<RoomObject>();
        equippedItems = new List<RoomObject>();
        textPrompt = FindObjectOfType<TextPrompt>();
        defaultValues = FindObjectOfType<DefaultValues>();
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

    public void openInventory()
    {
        if (inventory.Count == 0)
        {
            textPrompt.printText("\n" + defaultValues.emptyInventoryText);
            openEquipment();
            return;
        }

        StringBuilder itemList = new StringBuilder();
        foreach (RoomObject obj in inventory)
        {
            itemList.Append(obj.name + ", ");
        }
        itemList.Remove(itemList.Length - 2, 2);

        string inv = "\n" + defaultValues.occupiedInventoryText.Replace("(ITEMS)", itemList.ToString());
        textPrompt.printText(inv);

        openEquipment();
    }

    public void openEquipment()
    {
        if (equippedItems.Count == 0)
        {
            textPrompt.printText("\n" + defaultValues.emptyEquipmentText);
            return;
        }

        StringBuilder equipmentList = new StringBuilder();
        foreach (RoomObject obj in equippedItems)
        {
            equipmentList.Append(obj.name + ", ");
        }
        equipmentList.Remove(equipmentList.Length - 2, 2);

        string equipment = "\n" + defaultValues.occupiedEquipmentText.Replace("(ITEMS)", equipmentList.ToString());

        textPrompt.printText(equipment);
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

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

    private void Awake()
    {
        inventory = new List<RoomObject>();
        equippedItems = new List<RoomObject>();
        textPrompt = FindObjectOfType<TextPrompt>();
        defaultValues = FindObjectOfType<DefaultValues>();
    }

    /// <summary>
    /// Adds a RoomObject to the inventory list.
    /// </summary>
    public void addItemToInventory(RoomObject item)
    {
        if (item == null)
        {
            Debug.LogError($"{item.name}: There's an object that wasn't set in an action");
            return;
        }
        inventory.Add(item);
    }

    /// <summary>
    /// Removes a RoomObject from the inventory list.
    /// </summary>
    /// <param name="item"></param>
    public void removeItemFromInventory(RoomObject item)
    {
        if (item == null)
        {
            Debug.LogError($"{item.name}: There's an object that wasn't set in an action");
            return;
        }
        inventory.Remove(item);
    }

    public List<RoomObject> getInventory()
    {
        return inventory;
    }

    /// <summary>
    /// Prints all objects in the inventory. Listed objects replace (ITEMS) in the default string.
    /// </summary>
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

    /// <summary>
    /// Prints all objects that are currently equipped. Listed objects replace (ITEMS) in the default string.
    /// </summary>
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

    /// <summary>
    /// Adds a RoomObject to the equipment list.
    /// </summary>
    /// <param name="item"></param>
    public void equipItem(RoomObject item)
    {
        if (item == null)
        {
            Debug.LogError($"{item.name}: There's an object that wasn't set in an action");
            return;
        }
        equippedItems.Add(item);
    }
    
    /// <summary>
    /// Removes a RoomObject from the equipment list.
    /// </summary>
    /// <param name="item"></param>
    public void removeEquippedItem(RoomObject item)
    {
        if (item == null)
        {
            Debug.LogError($"{item.name}: There's an object that wasn't set in an action");
            return;
        }
        equippedItems.Remove(item);
    }

    public List<RoomObject> getEquippedItems()
    {
        return equippedItems;
    }
}

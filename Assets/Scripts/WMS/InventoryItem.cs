using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public string ItemID { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public string Location { get; set; }

    public InventoryItem(string itemID, string name, int quantity, string location)
    {
        ItemID = itemID;
        Name = name;
        Quantity = quantity;
        Location = location;
    }
}

public class InventoryManager
{
    private Dictionary<string, InventoryItem> _inventory = new Dictionary<string, InventoryItem>();

    public void AddItem(string itemID, string name, int quantity, string location)
    {
        if (_inventory.ContainsKey(itemID))
        {
            _inventory[itemID].Quantity += quantity;
        }
        else
        {
            _inventory[itemID] = new InventoryItem(itemID, name, quantity, location);
        }
    }

    public void RemoveItem(string itemID, int quantity)
    {
        if (_inventory.ContainsKey(itemID) && _inventory[itemID].Quantity >= quantity)
        {
            _inventory[itemID].Quantity -= quantity;
            if (_inventory[itemID].Quantity == 0)
            {
                _inventory.Remove(itemID);
            }
        }
    }

    public InventoryItem GetItem(string itemID)
    {
        return _inventory.ContainsKey(itemID) ? _inventory[itemID] : null;
    }

    // 新增方法，用于获取所有库存物品的列表
    public List<InventoryItem> GetInventoryItems()
    {
        return new List<InventoryItem>(_inventory.Values);
    }
}


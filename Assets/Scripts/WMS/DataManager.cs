using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{

    public static void SaveInventory(List<InventoryItem> items)
    {
        string json = JsonUtility.ToJson(items);
        File.WriteAllText(Application.persistentDataPath + "/Inventory.json", json);
    }

    public static List<InventoryItem> LoadInventory()
    {
        string path = Application.persistentDataPath + "/Inventory.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<List<InventoryItem>>(json);
        }
        return new List<InventoryItem>();
    }
}


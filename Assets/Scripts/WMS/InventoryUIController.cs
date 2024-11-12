using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class InventoryUIController : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text quantityText;

    public void AddItemToInventory()
    {
        WMSManager.Instance.InventoryManager.AddItem("001", "Sample Item", 10, "A1");
        UpdateUI();
    }

    public void RemoveItemFromInventory()
    {
        WMSManager.Instance.InventoryManager.RemoveItem("001", 5);
        UpdateUI();
    }

    private void UpdateUI()
    {
        InventoryItem item = WMSManager.Instance.InventoryManager.GetItem("001");
        if (item != null)
        {
            itemNameText.text = item.Name;
            quantityText.text = item.Quantity.ToString();
        }
    }
}


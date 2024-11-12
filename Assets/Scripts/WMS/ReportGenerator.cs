using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReportGenerator
{
    // 生成库存报告并保存为 JSON 文件
    public void GenerateInventoryReport(List<InventoryItem> inventoryItems)
    {
        // 将库存数据转换为 JSON
        string jsonReport = JsonUtility.ToJson(new InventoryReport(inventoryItems), true);
        
        // 文件路径
        string filePath = Application.persistentDataPath + "/InventoryReport.json";
        
        // 写入文件
        File.WriteAllText(filePath, jsonReport);
        Debug.Log($"Inventory report saved at {filePath}");
    }

    // 生成订单完成报告并保存为 JSON 文件
    public void GenerateOrderReport(List<Order> completedOrders)
    {
        // 将订单数据转换为 JSON
        string jsonReport = JsonUtility.ToJson(new OrderReport(completedOrders), true);
        
        // 文件路径
        string filePath = Application.persistentDataPath + "/OrderReport.json";
        
        // 写入文件
        File.WriteAllText(filePath, jsonReport);
        Debug.Log($"Order report saved at {filePath}");
    }

    // 内部类，用于包装库存报告数据
    private class InventoryReport
    {
        public List<InventoryItem> Items { get; set; }

        public InventoryReport(List<InventoryItem> items)
        {
            Items = items;
        }
    }

    // 内部类，用于包装订单报告数据
    private class OrderReport
    {
        public List<Order> Orders { get; set; }

        public OrderReport(List<Order> orders)
        {
            Orders = orders;
        }
    }
}


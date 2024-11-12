using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 在某个 UI 按钮点击事件中调用生成报表
public class ReportButtonController : MonoBehaviour
{
    private ReportGenerator reportGenerator;

    private void Start()
    {
        reportGenerator = new ReportGenerator();
    }

    public void OnGenerateInventoryReport()
    {
        List<InventoryItem> inventoryItems = WMSManager.Instance.InventoryManager.GetInventoryItems(); // 假设 GetInventoryItems() 是一个获取所有库存物品的方法
        reportGenerator.GenerateInventoryReport(inventoryItems);
    }

    public void OnGenerateOrderReport()
    {
        List<Order> completedOrders = WMSManager.Instance.OrderManager.GetCompletedOrders(); // 假设 GetCompletedOrders() 是一个获取所有已完成订单的方法
        reportGenerator.GenerateOrderReport(completedOrders);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public string OrderID { get; private set; }
    public List<InventoryItem> Items { get; private set; }
    public bool IsCompleted { get; private set; } // 新增属性，用于标记订单是否完成

    public Order(string orderID)
    {
        OrderID = orderID;
        Items = new List<InventoryItem>();
    }

    public void AddItem(InventoryItem item)
    {
        Items.Add(item);
    }

    // 完成订单
    public void CompleteOrder()
    {
        IsCompleted = true;
    }
}

public class OrderManager
{
    private List<Order> _orders = new List<Order>();

    public void CreateOrder(string orderID)
    {
        _orders.Add(new Order(orderID));
    }

    public Order GetOrder(string orderID)
    {
        return _orders.Find(o => o.OrderID == orderID);
    }

    public void CompleteOrder(string orderID)
    {
        Order order = GetOrder(orderID);
        if (order != null)
        {
            order.CompleteOrder(); // 标记订单为完成
            // 通知 InventoryManager 进行库存更新
        }
    }

    // 新增方法，用于获取所有已完成的订单
    public List<Order> GetCompletedOrders()
    {
        return _orders.FindAll(o => o.IsCompleted);
    }
}


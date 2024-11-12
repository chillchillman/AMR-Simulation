using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WMSManager : MonoBehaviour
{
    private static WMSManager _instance;

    public static WMSManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<WMSManager>();
            }
            return _instance;
        }
    }

    public InventoryManager InventoryManager { get; private set; }
    public VehicleManager VehicleManager { get; private set; }
    public OrderManager OrderManager { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        // 初始化各模块
        InventoryManager = new InventoryManager();
        VehicleManager = new VehicleManager();
        OrderManager = new OrderManager();
    }
}


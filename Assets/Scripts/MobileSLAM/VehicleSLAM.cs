using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;

public class VehicleSLAM : MonoBehaviour
{
    private IVehicleMovement vehicleMovement;
    private IEnvironmentScanner environmentScanner;
    private IMapDrawer mapDrawer;

    void Awake()
    {
        vehicleMovement = new VehicleMovement(5f, 100f);
        environmentScanner = new EnvironmentScanner(16, 10f);
        mapDrawer = new MapDrawer();
    }

    void Update()
    {
        // 控制車輛的移動
        vehicleMovement.Move(transform);

        // 發射射線來檢測周圍環境
        environmentScanner.Scan(transform);

        // 在場景中顯示環境地圖
        mapDrawer.Draw(environmentScanner.GetObstacleMap());
    }
}
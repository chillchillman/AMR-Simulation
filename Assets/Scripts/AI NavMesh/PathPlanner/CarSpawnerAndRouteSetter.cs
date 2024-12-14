using System.Collections.Generic;
using UnityEngine;

public class CarSpawnerAndRouteSetter : MonoBehaviour
{
    public GameObject carPrefab; // 車輛Prefab
    public Transform carParent; // 車輛父物件
    private List<CarNavigationController> carControllers = new List<CarNavigationController>(); // 車輛控制器列表

    private PopulationManager populationManager;

    private void Start()
    {
        // 獲取 PopulationManager 的參考
        populationManager = FindObjectOfType<PopulationManager>();
        if (populationManager == null)
        {
            Debug.LogError("PopulationManager not found in the scene!");
            return;
        }
    }

    /// <summary>
    /// 根據最佳基因生成車輛並設置導航路徑
    /// </summary>
    public void GenerateCarsAndSetRoutes()
    {
        // 檢查 PopulationManager 和最佳基因
        if (populationManager == null || populationManager.optimalDNA == null)
        {
            Debug.LogError("PopulationManager or optimalDNA is not ready.");
            return;
        }

        // 清除之前生成的車輛
        foreach (var car in carControllers)
        {
            Destroy(car.gameObject);
        }
        carControllers.Clear();

        // 獲取最佳車輛數量和最佳基因
        int optimalAMRCount = populationManager.optimalAMRCount; // 車輛數量
        DNA optimalDNA = populationManager.optimalDNA; // 最佳基因

        // 分配基因到導航路徑
        List<List<int>> amrRoutes = AssignWaypointsToCars(optimalDNA, optimalAMRCount);

        // 生成車輛並設置導航路徑
        for (int i = 0; i < optimalAMRCount; i++)
        {
            // 計算生成點位置
            Vector3 spawnPosition = WaypointManager.Instance.GetStartPoint();
            spawnPosition.x += Random.Range(-38f, -30f); // 隨機偏移 X 軸生成點

            // 創建車輛
            GameObject car = Instantiate(carPrefab, spawnPosition, Quaternion.identity, carParent);
            car.name = $"Car_{i}";

            CarNavigationController controller = car.GetComponent<CarNavigationController>();
            if (controller != null)
            {
                // 設置導航路徑：起點 -> 中間站點 -> 終點
                List<Vector3> route = new List<Vector3> { WaypointManager.Instance.GetStartPoint() };
                foreach (int waypointIndex in amrRoutes[i])
                {
                    route.Add(WaypointManager.Instance.GetWaypointPositions()[waypointIndex - 1]);
                }
                route.Add(WaypointManager.Instance.GetEndPoint());

                controller.SetNavigationRoute(route); // 設置路徑到控制器
                controller.SetCarIndex(i); // 設置車輛的索引
                carControllers.Add(controller); // 添加到控制器列表
            }
        }

        Debug.Log($"{optimalAMRCount} cars generated and routes assigned successfully.");
    }


    public void StartAllCars()
    {
        foreach (var controller in carControllers)
        {
            if (controller != null)
            {
                controller.StartNavigation(); // 調用每輛車的導航方法
            }
        }
        Debug.Log("All cars started navigation.");
    }

    /// <summary>
    /// 根據 DNA 和車輛數量，分配 waypoints 到每輛車
    /// </summary>
    private List<List<int>> AssignWaypointsToCars(DNA dna, int carCount)
    {
        List<List<int>> amrRoutes = new List<List<int>>();

        // 初始化路徑分配列表
        for (int i = 0; i < carCount; i++)
        {
            amrRoutes.Add(new List<int>());
        }

        // 根據 DNA 將 waypoint 分配到對應車輛
        for (int i = 0; i < dna.GetGenes().Count; i++)
        {
            int assignedCarIndex = dna.GetGenes()[i];
            if (assignedCarIndex < carCount)
            {
                amrRoutes[assignedCarIndex].Add(i + 1); // 分配 waypoint (基於索引+1)
            }
            else
            {
                Debug.LogError($"DNA assignment error: Car index {assignedCarIndex} exceeds car count.");
            }
        }

        return amrRoutes;
    }
}

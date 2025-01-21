using System.Collections.Generic;
using UnityEngine;

public class CarCargoHandler : MonoBehaviour
{
    [Header("Cargo Configuration")]
    public GameObject cargoPrefab; // 貨物的Prefab
    public Transform cargoParent; // 貨物的父物件，用於附加到車輛上
    private GameObject currentCargo; // 當前的貨物物件

    private List<Vector3> waypoints; // 車輛的路徑點
    private int currentWaypointIndex = 0; // 當前路徑點索引

    private void Start()
    {
        // 初始化導航路徑
        CarNavigationController controller = GetComponent<CarNavigationController>();
        if (controller != null)
        {
            waypoints = controller.GetNavigationRoute();
        }
        else
        {
            Debug.LogError("CarNavigationController not found on the Car!");
        }
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        // 確認車輛是否接近當前路徑點
        float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypointIndex]);
        if (distanceToWaypoint < 1.0f) // 設定為 1.0 的接近範圍
        {
            HandleWaypointInteraction();
            AdvanceToNextWaypoint();
        }
    }

    private void HandleWaypointInteraction()
    {
        if (currentWaypointIndex == 0 || currentWaypointIndex%2 == 0)
        {
            // 在起始點生成貨物
            GenerateCargo();
        }
        else if (currentWaypointIndex == waypoints.Count - 1 || currentWaypointIndex%2 != 0)
        {
            // 在終點卸貨
            RemoveCargo();
        }
        else
        {
            // 在其他路徑點卸載舊貨物並生成新貨物
            RemoveCargo();
            GenerateCargo();
        }
    }

    private void AdvanceToNextWaypoint()
    {
        currentWaypointIndex++;
        if (currentWaypointIndex >= waypoints.Count)
        {
            currentWaypointIndex = waypoints.Count - 1; // 確保索引不越界
        }
    }

    private void GenerateCargo()
    {
        if (currentCargo != null) return; // 如果已有貨物則不生成
        currentCargo = Instantiate(cargoPrefab, cargoParent);
        currentCargo.transform.localPosition = Vector3.zero; // 設置相對車輛的中心位置
        Debug.Log($"Cargo generated at Waypoint {currentWaypointIndex}");
    }

    private void RemoveCargo()
    {
        if (currentCargo != null)
        {
            Destroy(currentCargo);
            currentCargo = null;
            Debug.Log($"Cargo removed at Waypoint {currentWaypointIndex}");
        }
    }
}

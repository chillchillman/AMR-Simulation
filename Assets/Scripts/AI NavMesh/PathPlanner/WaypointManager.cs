using System;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance { get; private set; }

    private List<Vector3> waypoints = new List<Vector3>(); //所有目標點 (包含起點 終點 waypoint)
    public event Action OnWaypointsUpdated;

    [Header("Preset Points")]
    [SerializeField] private Transform startPoint; 
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform[] targetPoints; // 其他目標point(不包括start final 的 waypoint)


    //[SerializeField] private LayerMask waypointLayerMask; // 用於檢測可點擊的目標層

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0)) // 檢測鼠標左鍵點擊
    //     {
    //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //         if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, waypointLayerMask))
    //         {
    //             AddWaypoint(hit.point);
    //         }
    //     }
    // }

    // public void AddWaypoint(Vector3 position)
    // {
    //     if (waypoints.Contains(position))
    //     {
    //         Debug.LogWarning($"Waypoint already exists at position: {position}");
    //         return;
    //     }

    //     waypoints.Add(position);
    //     OnWaypointsUpdated?.Invoke();
    //     Debug.Log($"Waypoint added at position: {position}");

    //     // 顯示當前所有 Waypoints 的記錄
    //     DisplayWaypoints();
    // }

    private void Start()
    {
        InitializeWaypoints();
    }

    private void InitializeWaypoints()
    {
        //清空舊的 Waypoints
        waypoints.Clear();

        //添加 Startpoint
        if(startPoint != null)
        {
            waypoints.Add(startPoint.position);
            Debug.Log($"Start added at position: {startPoint.position}");
        }

        //添加 WaypointsList
        if(targetPoints != null && targetPoints.Length > 0)
        {
            foreach (var target in targetPoints)
            {
                if(target != null)
                {
                    waypoints.Add(target.position);
                    Debug.Log($"Waypoint added at position: {target.position}");
                }
            }
        }

        //添加 Endpoint
        if(endPoint != null)
        {
            waypoints.Add(endPoint.position);
            Debug.Log($"Waypoint added at position: {endPoint.position}");
        }

        //通知更新
        OnWaypointsUpdated?.Invoke();

        //顯示所有的 Waypoints
        DisplayWaypoints();

    }



    public List<Vector3> GetWaypointPositions()
    {
        return new List<Vector3>(waypoints);
    }

    public void ClearWaypoints()
    {
        waypoints.Clear();
        OnWaypointsUpdated?.Invoke();
        Debug.Log("All waypoints cleared.");
    }

    private void DisplayWaypoints()
    {
        Debug.Log("Current Waypoints:");
        for (int i = 0; i < waypoints.Count; i++)
        {
            Debug.Log($"Waypoint {i + 1}: {waypoints[i]}");
        }
    }
}

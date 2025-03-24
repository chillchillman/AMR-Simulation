using System;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance { get; private set; }

    private List<Vector3> waypoints = new List<Vector3>(); //所有目標點 (包含起點 終點 waypoint)
    private List<Vector3> startPoints = new List<Vector3>();
    private List<Vector3> endPoints = new List<Vector3>();
    private HashSet<int> usedStartPointIndices = new HashSet<int>();
    private HashSet<int> usedEndPointIndices = new HashSet<int>();
    
    public delegate void WaypointsUpdated();
    public event WaypointsUpdated OnWaypointsUpdated;

    [Header("Preset Points")]
    [SerializeField] private Transform[] startPointTransform; 
    [SerializeField] private Transform[] endPointTransform;
    [SerializeField] private Transform[] targetPoints; // 其他目標point(不包括start final 的 waypoint)


    //[SerializeField] private LayerMask waypointLayerMask; // 用於檢測可點擊的目標層

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    private void Start()
    {
        InitializeWaypoints();
    }

    private void InitializeWaypoints()
    {
        // 初始化 startPoints
        startPoints.Clear();
        usedStartPointIndices.Clear();
        if (startPointTransform != null && startPointTransform.Length > 0)
        {
            foreach (var startTransform in startPointTransform)
            {
                if (startTransform != null)
                    startPoints.Add(startTransform.position);
            }
        }

        //if (endPointTransform != null) endPoint = endPointTransform.position;
        endPoints.Clear();
        usedEndPointIndices.Clear();
        if (endPointTransform != null && endPointTransform.Length > 0)
        {
            foreach (var endTransform in endPointTransform)
            {
                if (endTransform != null)
                    endPoints.Add(endTransform.position);
            }
        }

        //清空舊的 Waypoints
        waypoints.Clear();

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

        //通知更新
        OnWaypointsUpdated?.Invoke();

        //顯示所有的 Waypoints
        DisplayWaypoints();

    }

    public void AddWaypoint(Vector3 position, string tag)
    {

        waypoints.Add(position); // 添加到主列表

        OnWaypointsUpdated?.Invoke();
        Debug.Log($"Waypoint added: {position} with tag {tag}");
    }

    public List<Vector3> GetWaypointPositions()
    {
        return new List<Vector3>(waypoints);
    }

     public Vector3 GetStartPoint()
    {
        if (startPoints.Count == 0)
            return Vector3.zero;

        List<int> availableIndices = new List<int>();
        for (int i = 0; i < startPoints.Count; i++)
        {
            if (!usedStartPointIndices.Contains(i))
                availableIndices.Add(i);
        }

        if (availableIndices.Count == 0)
        {
            usedStartPointIndices.Clear();
            availableIndices.AddRange(System.Linq.Enumerable.Range(0, startPoints.Count));
        }

        int selectedIndex = availableIndices[UnityEngine.Random.Range(0, availableIndices.Count)];
        usedStartPointIndices.Add(selectedIndex);
        return startPoints[selectedIndex];
    }

    public Vector3 GetEndPoint()
    {

        if (endPoints.Count == 0)
            return Vector3.zero;

        List<int> availableIndices = new List<int>();
        for (int i = 0; i < endPoints.Count; i++)
        {
            if (!usedEndPointIndices.Contains(i))
                availableIndices.Add(i);
        }

        if (availableIndices.Count == 0)
        {
            usedEndPointIndices.Clear();
            availableIndices.AddRange(System.Linq.Enumerable.Range(0, endPoints.Count));
        }

        int selectedIndex = availableIndices[UnityEngine.Random.Range(0, availableIndices.Count)];
        usedEndPointIndices.Add(selectedIndex);
        return endPoints[selectedIndex];
    //    if (endPoints.Count == 0)
    //         return Vector3.zero;
    //     int randomIndex = UnityEngine.Random.Range(0, endPoints.Count);
    //     return endPoints[randomIndex];
    }


    public float[,] GenerateFullDistanceMatrix()
    {
        Vector3 startPoint = GetStartPoint();
        Vector3 endPoint = GetEndPoint();
        List<Vector3> allPoints = new List<Vector3> { startPoint };
        allPoints.AddRange(waypoints);
        allPoints.Add(endPoint);

        int count = allPoints.Count;
        float[,] distanceMatrix = new float[count, count];

        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                distanceMatrix[i, j] = (i == j) ? 0 : Vector3.Distance(allPoints[i], allPoints[j]);
            }
        }

        return distanceMatrix;
    }

    // /// <summary>
    // /// 生成距離矩陣
    // /// </summary>
    // public float[,] GenerateDistanceMatrix()
    // {
    //     int count = waypoints.Count;
    //     float[,] distanceMatrix = new float[count, count];

    //     for (int i = 0; i < count; i++)
    //     {
    //         for (int j = 0; j < count; j++)
    //         {
    //             if (i == j)
    //             {
    //                 distanceMatrix[i, j] = 0; // 自身距離為0
    //             }
    //             else
    //             {
    //                 Vector3 posA = waypoints[i];
    //                 Vector3 posB = waypoints[j];
    //                 distanceMatrix[i, j] = Vector3.Distance(posA, posB);
    //             }
    //         }
    //     }

    //     return distanceMatrix;
    // }

    //----------------------------------------------------------------


    public void ClearWaypoints()
    {
        waypoints.Clear();
        OnWaypointsUpdated?.Invoke();
        Debug.Log("All waypoints cleared.");
    }

    private void DisplayWaypoints()
    {
        Debug.Log("Current Waypoints:");
        Debug.Log($"Start Point: {startPoints}");
        for (int i = 0; i < waypoints.Count; i++)
        {
            Debug.Log($"Waypoint {i + 1}: {waypoints[i]}");
        }
        Debug.Log($"End Point: {endPoints}");
    }
}

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class WaypointPlacementManager : MonoBehaviour
{
    [Header("Waypoint Prefabs")]
    public GameObject loadPointPrefab;
    public GameObject unloadPointPrefab;

    [Header("UI Elements")]
    public Button loadButton;
    public Button unloadButton;

    private GameObject currentPoint; // 當前生成但未固定的位置
    private Camera mainCamera;

    private string currentTag; // 當前生成點的類型 (load/unload)

    private void Start()
    {
        mainCamera = Camera.main;

        // 綁定按鈕點擊事件
        loadButton.onClick.AddListener(() => StartPlacingWaypoint("load"));
        unloadButton.onClick.AddListener(() => StartPlacingWaypoint("unload"));
    }

    private void Update()
    {
        // 如果當前沒有正在放置的點，直接返回
        if (currentPoint == null) return;

        // 鼠標跟隨功能
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 移動當前碰撞點到鼠標位置
            currentPoint.transform.position = hit.point;
        }

        // 固定點位置
        if (Input.GetMouseButtonDown(0))
        {
            FinalizePlacement();
        }
    }

    private void StartPlacingWaypoint(string tag)
    {
        // 避免多次生成未固定的點
        if (currentPoint != null) return;

        currentTag = tag;

        // 根據 tag 生成對應的點
        if (tag == "load")
        {
            currentPoint = Instantiate(loadPointPrefab);
            currentPoint.tag = "load";
        }
        else if (tag == "unload")
        {
            currentPoint = Instantiate(unloadPointPrefab);
            currentPoint.tag = "unload";
        }

        Debug.Log($"Started placing waypoint with tag: {tag}");
    }

    private void FinalizePlacement()
    {
        if (currentPoint != null)
        {
            // 將 Waypoint 添加到 WaypointManager
            WaypointManager.Instance.AddWaypoint(currentPoint.transform.position, currentTag);

            Debug.Log($"Waypoint with tag {currentTag} placed at {currentPoint.transform.position}");
            currentPoint = null; // 清空當前操作點
        }
    }
}

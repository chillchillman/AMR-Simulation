using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class NavMeshVisualizer : MonoBehaviour
{
    public Color navMeshColor = new Color(0, 0.5f, 1, 0.3f); // 可導航區域顏色
    public NavMeshSurface navMeshSurface;
    private Mesh navMesh;
    private int clickCount = 0;

    void Start()
    {
        navMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = navMesh;
        UpdateNavMeshVisualization();
    }

    public void UpdateNavMeshVisualization()
    {
        // 清除舊的導航區域
        ClearNavMeshVisualization();

        // 獲取最新的導航網格資料
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

        // 更新 Mesh 的頂點和三角形，並重新生成導航區域顯示
        navMesh.vertices = triangulation.vertices;
        navMesh.triangles = triangulation.indices;
        navMesh.RecalculateNormals();

        // 設置顏色和材質
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer.material == null)
        {
            meshRenderer.material = new Material(Shader.Find("Standard"));
        }
        meshRenderer.material.color = navMeshColor;
    }

    // 清除舊的導航網格渲染數據
    private void ClearNavMeshVisualization()
    {
        // 將 navMesh 的數據清空，釋放上一個 Bake 的顯示結果
        navMesh.Clear();
    }

    public void OnBakeButtonClick()
    {
        if (navMeshSurface != null)
        {
            // 清除之前的導航區域顯示
            ClearNavMeshVisualization();

            // 重新 Bake 並更新可視化顯示
            navMeshSurface.BuildNavMesh();
            UpdateNavMeshVisualization();
            Debug.Log("NavMesh baked and visualized successfully!");
        }
        else
        {
            Debug.LogError("No NavMeshSurface component found!");
        }
    }

    public void ShowNavMesh ()
    {
        // 增加點擊次數
        clickCount += 1;

        // 使用取模運算控制 NavMesh 的啟用狀態
        MeshRenderer meshRenderer = navMeshSurface.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = (clickCount % 2 == 0);
        }
        else
        {
            Debug.LogError("MeshRenderer not found on NavMeshSurface object.");
        }
        
    }

}

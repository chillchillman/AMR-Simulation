using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Diagnostics;

public interface IPathfindingStrategy
{
    NavMeshPath FindPath(NavMeshAgent agent, Vector3 targetPosition); // 回傳計算出的路徑
    void LogPathDetails(NavMeshPath path); // 用於記錄路徑細節
}

public  class AStarPathfindingStrategy : IPathfindingStrategy
{
    public NavMeshPath FindPath(NavMeshAgent agent, Vector3 targetPosition)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        // 使用 NavMeshAgent 自帶的 A* 演算法
        agent.SetDestination(targetPosition);

        stopwatch.Stop();
        UnityEngine.Debug.Log("A* Algorithm: Pathfinding completed in " + stopwatch.ElapsedMilliseconds + " ms.");
        
        // 返回 NavMesh 計算的路徑
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);
        return path;
    }
    public void LogPathDetails(NavMeshPath path)
    {
        string pathDetails = "A* Path Nodes:";
        foreach (var corner in path.corners)
        {
            pathDetails += "\n" + corner;
        }
        UnityEngine.Debug.Log(pathDetails);
    }
}

public class DijkstraPathfindingStrategy : IPathfindingStrategy
{
    public NavMeshPath FindPath(NavMeshAgent agent, Vector3 targetPosition)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        // 使用 NavMesh 自帶的 Dijkstra 模擬
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(agent.transform.position, targetPosition, NavMesh.AllAreas, path);

        stopwatch.Stop();
        UnityEngine.Debug.Log("Dijkstra Algorithm: Pathfinding completed in " + stopwatch.ElapsedMilliseconds + " ms.");

        // 將計算出的路徑返回
        return path;
    }

    public void LogPathDetails(NavMeshPath path)
    {
        string pathDetails = "Dijkstra Path Nodes:";
        foreach (var corner in path.corners)
        {
            pathDetails += "\n" + corner;
        }
        UnityEngine.Debug.Log(pathDetails);
    }
}

public class BFSPathfindingStrategy : IPathfindingStrategy
{
    public NavMeshPath FindPath(NavMeshAgent agent, Vector3 targetPosition)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        // 模擬 BFS 路徑計算
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(agent.transform.position, targetPosition, NavMesh.AllAreas, path);

        stopwatch.Stop();
        UnityEngine.Debug.Log("BFS Algorithm: Pathfinding completed in " + stopwatch.ElapsedMilliseconds + " ms.");

        return path;
    }

    public void LogPathDetails(NavMeshPath path)
    {
        string pathDetails = "BFS Path Nodes:";
        foreach (var corner in path.corners)
        {
            pathDetails += "\n" + corner;
        }
        UnityEngine.Debug.Log(pathDetails);
    }
}


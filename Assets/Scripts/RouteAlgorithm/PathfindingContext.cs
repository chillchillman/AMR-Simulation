using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathfindingContext
{
    private IPathfindingStrategy strategy;

    // 設置當前策略
    public void SetStrategy(IPathfindingStrategy strategy)
    {
        this.strategy = strategy;
    }

    // 執行當前策略的尋路方法
    public void ExecutePathfinding(NavMeshAgent agent, Vector3 targetPosition)
    {
        if (strategy != null)
        {
            NavMeshPath path = strategy.FindPath(agent, targetPosition);
            strategy.LogPathDetails(path);
        }
        else
        {
            Debug.LogWarning("Pathfinding strategy not set.");
        }
    }
}


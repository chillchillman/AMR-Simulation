using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarController : MonoBehaviour
{
    public PathPlanner pathPlanner; // 路径规划模块
    public UserInputManager inputManager; // 用户输入管理模块
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // 初始化NavMeshAgent参数
        ApplyUserDefinedParameters();
    }

    public void StartMovement()
    {
        List<Vector3> waypoints = pathPlanner.GetWaypoints();
        if (waypoints.Count > 0)
        {
            StartCoroutine(MoveAlongPath(waypoints));
        }
    }

    private IEnumerator MoveAlongPath(List<Vector3> waypoints)
    {
        foreach (Vector3 point in waypoints)
        {
            navMeshAgent.SetDestination(point);
            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > 0.1f)
            {
                yield return null; // 等待到達當前目標點
            }
        }
    }

    private void ApplyUserDefinedParameters()
    {
        // 从输入管理模块获取用户参数
        navMeshAgent.speed = inputManager.GetSpeed();
        navMeshAgent.angularSpeed = inputManager.GetRotationSpeed();
        float loadWeight = inputManager.GetLoadWeight();

        // 根据负载权重动态调整速度
        navMeshAgent.speed = Mathf.Max(1, navMeshAgent.speed * (1 - (loadWeight / inputManager.GetMaxLoad())));
    }
}

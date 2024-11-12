using UnityEngine;
using UnityEngine.AI;

public class MultiAgentPathfinding : MonoBehaviour
{
    public Transform[] targetPoints; // 目標位置陣列
    public GameObject agentPrefab;   // 智能体預製件
    public int numberOfAgents = 5;   // 智能体數量

    void Start()
    {
        for (int i = 0; i < numberOfAgents; i++)
        {
            // 創建智能体
            GameObject newAgent = Instantiate(agentPrefab, GetRandomStartPosition(), Quaternion.identity);
            NavMeshAgent agentComponent = newAgent.GetComponent<NavMeshAgent>();

            // 隨機分配目標
            Transform target = targetPoints[Random.Range(0, targetPoints.Length)];
            agentComponent.SetDestination(target.position);
        }
    }

    Vector3 GetRandomStartPosition()
    {
        // 生成隨機初始位置
        float x = Random.Range(-30.0f, 15.0f);
        float z = Random.Range(-14.0f, -15.0f);
        return new Vector3(x, 0.66f, z);
    }
}

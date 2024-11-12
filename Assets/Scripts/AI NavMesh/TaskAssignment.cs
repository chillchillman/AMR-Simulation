using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskAssignment : MonoBehaviour
{
    public GameObject agentPrefab;
    public Transform[] taskTargets; // 任务目标列表
    private Queue<Transform> taskQueue = new Queue<Transform>();
    private List<NavMeshAgent> agents = new List<NavMeshAgent>();

    void Start()
    {
        // 初始化任務隊列
        foreach (Transform target in taskTargets)
        {
            taskQueue.Enqueue(target);
        }

        // 創建智能体並分配任務
        for (int i = 0; i < taskTargets.Length; i++)
        {
            GameObject newAgent = Instantiate(agentPrefab, GetRandomStartPosition(), Quaternion.identity);
            NavMeshAgent agentComponent = newAgent.GetComponent<NavMeshAgent>();
            agents.Add(agentComponent);
        }

        AssignTasks();
    }

    void AssignTasks()
    {
        foreach (NavMeshAgent agent in agents)
        {
            if (taskQueue.Count > 0)
            {
                Transform nextTask = taskQueue.Dequeue();
                agent.SetDestination(nextTask.position);
            }
        }
    }

    Vector3 GetRandomStartPosition()
    {
        float x = Random.Range(-30.0f, -10.0f);
        float z = Random.Range(-14.0f, 15.0f);
        return new Vector3(x, 0.66f, z);
    }
}

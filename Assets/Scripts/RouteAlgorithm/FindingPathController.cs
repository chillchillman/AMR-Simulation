using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindingPathController : MonoBehaviour
{
   public Transform targetPoint;
   private NavMeshAgent agent;
   private PathfindingContext pathfindingContext;

   private void Start()
   {
       agent = GetComponent<NavMeshAgent>();
       pathfindingContext = new PathfindingContext();

       //預設A*
       pathfindingContext.SetStrategy(new AStarPathfindingStrategy());
       pathfindingContext.ExecutePathfinding(agent, targetPoint.position);
   }

   private void Update()
   {
       if(Input.GetKeyDown(KeyCode.A))
       {
        pathfindingContext.SetStrategy(new AStarPathfindingStrategy());
        pathfindingContext.ExecutePathfinding(agent, targetPoint.position);
        Debug.Log("Switched to A* Pathfinding Strategy");
       }

       else if(Input.GetKeyDown(KeyCode.D))
       {
        pathfindingContext.SetStrategy(new DijkstraPathfindingStrategy());
        pathfindingContext.ExecutePathfinding(agent, targetPoint.position);
        Debug.Log("Switched to Dijkstra Pathfinding Strategy");
       }
       else if (Input.GetKeyDown(KeyCode.B))
       {
        pathfindingContext.SetStrategy(new BFSPathfindingStrategy());
        pathfindingContext.ExecutePathfinding(agent, targetPoint.position);
        Debug.Log("Switched to BFS Pathfinding Strategy");
       }
   }
   
}

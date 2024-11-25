using System.Collections.Generic;
using UnityEngine;

public class PathPlanner : MonoBehaviour
{
    private List<Vector3> waypoints = new List<Vector3>();

    public void AddWaypoint(Vector3 waypoint)
    {
        waypoints.Add(waypoint);
    }

    public List<Vector3> GetWaypoints()
    {
        return waypoints;
    }

    public void ClearWaypoints()
    {
        waypoints.Clear();
    }
}

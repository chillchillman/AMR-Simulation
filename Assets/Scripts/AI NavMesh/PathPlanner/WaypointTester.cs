using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointTester : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Press P to print waypoint position, and D to print the distance Matrix");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            List<Vector3> position = WaypointManager.Instance.GetWaypointPositions();
            Debug.Log("Waypoint positions: ");
            foreach(var pos in position)
            {
                Debug.Log(pos);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            float[,] distanceMatrix = WaypointManager.Instance.GenerateFullDistanceMatrix();
            Debug.Log("Distance Matrix:");

            for (int i = 0; i < distanceMatrix.GetLength(0); i++)
            {
                string row = "";
                for (int j = 0; j < distanceMatrix.GetLength(1); j++)
                {
                    row += distanceMatrix[i, j].ToString("F2") + "\t";
                }
                Debug.Log(row);
            }
        }
    }
}

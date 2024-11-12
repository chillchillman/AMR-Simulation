using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public interface IVehicleMovement
{
    void Move(Transform vehicleTransform);
}

public class VehicleMovement : IVehicleMovement
{
    private float vehicleSpeed;
    private float rotationSpeed;

    public VehicleMovement(float vehicleSpeed, float rotationSpeed)
    {
        this.vehicleSpeed = vehicleSpeed;
        this.rotationSpeed = rotationSpeed;
    }

    public void Move(Transform vehicleTransform)
    {
        // 前進和後退移動
        float moveDirection = Input.GetAxis("Vertical") * vehicleSpeed * Time.deltaTime;
        vehicleTransform.Translate(Vector3.forward * moveDirection);

        // 左右轉向
        float turnDirection = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        vehicleTransform.Rotate(Vector3.up, turnDirection);
    }
}

public interface IEnvironmentScanner
{
    void Scan(Transform vehicleTransform);
    Dictionary<Vector2, float> GetObstacleMap();
}

public class EnvironmentScanner : IEnvironmentScanner
{
    private int numRays;
    private float rayDistance;
    private Dictionary<Vector2, float> obstacleMap;

    public EnvironmentScanner(int numRays, float rayDistance)
    {
        this.numRays = numRays;
        this.rayDistance = rayDistance;
        this.obstacleMap = new Dictionary<Vector2, float>();
    }

    public void Scan(Transform vehicleTransform)
    {
        for (int i = 0; i < numRays; i++)
        {
            // 計算射線的方向
            float angle = i * (360f / numRays);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * vehicleTransform.forward;

            Ray ray = new Ray(vehicleTransform.position, direction);
            RaycastHit hit;

            // 發射射線檢測障礙物
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                // 儲存障礙物的坐標和距離信息
                Vector2 obstaclePosition = new Vector2(hit.point.x, hit.point.z);
                if (!obstacleMap.ContainsKey(obstaclePosition))
                {
                    obstacleMap.Add(obstaclePosition, hit.distance);
                }
                Debug.DrawRay(vehicleTransform.position, direction * hit.distance, Color.red);
                Debug.Log($"Hit {hit.collider.gameObject.name} at distance: {hit.distance}");
            }
            else
            {
                Debug.DrawRay(vehicleTransform.position, direction * rayDistance, Color.green);
            }
        }
    }

    public Dictionary<Vector2, float> GetObstacleMap()
    {
        return obstacleMap;
    }
}

public interface IMapDrawer
{
    void Draw(Dictionary<Vector2, float> obstacleMap);
}

public class MapDrawer : IMapDrawer
{
    public void Draw(Dictionary<Vector2, float> obstacleMap)
    {
        foreach (KeyValuePair<Vector2, float> obstacle in obstacleMap)
        {
            Vector3 position = new Vector3(obstacle.Key.x, 0, obstacle.Key.y);
            Debug.DrawLine(position, position + Vector3.up * 2, Color.blue);
        }
    }
}


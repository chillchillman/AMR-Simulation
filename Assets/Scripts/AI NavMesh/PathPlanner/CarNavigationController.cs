using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarNavigationController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private bool isMoving = false;
//----------------------------------------------------------------
    public Animator carAnimator; // 連結Animator
    public string animationTrigger = "Pickup"; // 動畫參數名稱
    private Transform currentTargetObject; // 目標貨物的Transform
//----------------------------------------------------------------

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        WaypointManager.Instance.OnWaypointsUpdated += OnWaypointsUpdated;
    }

    private void OnDestroy()
    {
        WaypointManager.Instance.OnWaypointsUpdated -= OnWaypointsUpdated;
    }

    public void SetSpeed(float speed)
    {
        navMeshAgent.speed = Mathf.Clamp(speed, 0, 20); // 限制速度範圍
        Debug.Log($"Speed set to {speed}");
    }

    public void SetAngularSpeed(float angularSpeed)
    {
        navMeshAgent.angularSpeed = Mathf.Clamp(angularSpeed, 0, 720); // 限制旋轉速度範圍
        Debug.Log($"Angular speed set to {angularSpeed}");
    }

    public void SetAcceleration(float acceleration)
    {
        navMeshAgent.acceleration = Mathf.Clamp(acceleration, 0, 50); // 限制加速度範圍
        Debug.Log($"Acceleration set to {acceleration}");
    }

    public void StartNavigation()
    {
        List<Vector3> waypoints = WaypointManager.Instance.GetWaypointPositions();
        if (waypoints.Count == 0)
        {
            Debug.LogWarning("No waypoints available for navigation.");
            return;
        }

        isMoving = true;
        StartCoroutine(NavigateThroughWaypoints(waypoints));
    }

    public void StopNavigation()
    {
        if (isMoving)
        {
            StopAllCoroutines();
            navMeshAgent.ResetPath();
            isMoving = false;
            Debug.Log("Navigation stopped.");
        }
    }

    private IEnumerator NavigateThroughWaypoints(List<Vector3> waypoints)
    {
        foreach (var point in waypoints)
        {
            navMeshAgent.SetDestination(point);
            Debug.Log($"Navigating to waypoint: {point}");

            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > 0.1f)
            {
                yield return null;
            }

            //----------------------------------------------------------------
            if(currentTargetObject != null)
            {
                FaceTarget(currentTargetObject);
                PlayPickupAnimation();
                yield return new WaitForSeconds(2.0f);
            }
            //----------------------------------------------------------------
        }

        isMoving = false;
        Debug.Log("Navigation completed!");
    }

//----------------------------------------------------------------
    private void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * navMeshAgent.angularSpeed);
        Debug.Log($"Facing target: {target.name}");
    }
//---------------------------------------------------------------

    private void PlayPickupAnimation()
    {
        if (carAnimator != null)
        {
            carAnimator.SetTrigger(animationTrigger);
            Debug.Log("Pickup animation triggered.");
        }
        else
        {
            Debug.LogWarning("Animator not assigned to Car.");
        }
    }
 //------------------------------------------------------------   

    private void OnWaypointsUpdated()
    {
        Debug.Log("Waypoints updated.");
    }

    public void SetTargetObject(Transform targetObject)
    {
        currentTargetObject = targetObject;
        Debug.Log($"Target object set to: {targetObject.name}");
    }
}

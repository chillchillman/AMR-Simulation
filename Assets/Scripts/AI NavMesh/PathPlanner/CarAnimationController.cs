using UnityEngine;

public class CarAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the Car object!");
        }
    }

    public void TriggerAnimation(string animationState)
    {
        if (animator != null)
        {
            animator.SetTrigger(animationState);
            Debug.Log($"Triggered animation: {animationState}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waypoint"))
        {
            Waypoint waypoint = other.GetComponent<Waypoint>();
            if (waypoint != null && waypoint.AssignedCar == gameObject)
            {
                TriggerAnimation("WaypointEnter");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Waypoint"))
        {
            Waypoint waypoint = other.GetComponent<Waypoint>();
            if (waypoint != null && waypoint.AssignedCar == gameObject)
            {
                TriggerAnimation("WaypointExit");
            }
        }
    }
}

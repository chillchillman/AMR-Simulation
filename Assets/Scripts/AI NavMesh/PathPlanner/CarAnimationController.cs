using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimationController : MonoBehaviour
{
    private Animator animator;
    private bool isTriggeredUP = false;
    private bool isTriggeredDOWN = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the Car object!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Waypoint"))
        {
            Debug.Log("Car has entered the trigger zone!");
            isTriggeredUP = true;   
            isTriggeredDOWN = false;
            UpdateAnimationState(); 
        }
        Debug.Log($"OnTriggerEnter called with object: {other.name}");
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Waypoint"))
        {
            Debug.Log("Car has left the trigger zone~");
            isTriggeredUP = false; 
            isTriggeredDOWN = true;
            UpdateAnimationState(); 
        }
        Debug.Log($"OnTriggerExit called with object: {other.name}");
    }

    private void UpdateAnimationState()
    {
        // 更新Animator参数
        if (animator != null)
        {
            animator.SetBool("isTriggeredUP", isTriggeredUP);
            animator.SetBool("isTriggeredDown", isTriggeredDOWN);
        }
    }
}

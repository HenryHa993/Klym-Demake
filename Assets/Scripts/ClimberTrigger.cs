using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*Detect climbable surfaces. Attached to a trigger on the player. Stores current climbable.*/
public class ClimberTrigger : MonoBehaviour
{
    public bool IsClimbableDetected;
    public GameObject DetectedClimbable;

    /*The player will not move if the trigger exits a climbable.*/
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Climbable"))
        {
            return;
        }
        
        IsClimbableDetected = false;
        DetectedClimbable = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Climbable"))
        {
            return;
        }

        IsClimbableDetected = true;
        DetectedClimbable = other.gameObject;
    }
}

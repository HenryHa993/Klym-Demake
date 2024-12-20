using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*Detect climbable surfaces. Attached to a trigger on the player. Stores current climbable.*/
public class ClimbingTriggerBox : MonoBehaviour
{
    public bool IsClimbableDetected;
    public GameObject DetectedClimbable;

    /*The player will not move if the trigger exits a climbable.*/
    private void OnTriggerExit(Collider other)
    {
        IsClimbableDetected = false;
        DetectedClimbable = null;
    }

    /*Detect and register climbable surface.*/
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Climbable"))
        {
            return;
        }

        IsClimbableDetected = true;
        DetectedClimbable = other.gameObject;
    }
}

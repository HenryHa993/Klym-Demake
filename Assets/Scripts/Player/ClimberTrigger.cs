using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*Detect climbable surfaces. Attached to a trigger on the player. Stores current climbable.*/
public class ClimberTrigger : MonoBehaviour
{
    public bool IsClimbableDetected;
    public bool IsPickupDetected;
    public GameObject DetectedClimbable;
    public GameObject DetectedPickup;

    /*The player will not move if the trigger exits a climbable.*/
    private void OnTriggerExit(Collider other)
    {
        /*if ((!other.gameObject.CompareTag("Climbable") || other.gameObject != DetectedClimbable) || (!other.gameObject.CompareTag("Pickup") || other.gameObject != DetectedPickup))
        {
            return;
        }*/
        if (other.gameObject == DetectedClimbable)
        {
            IsClimbableDetected = false;
            DetectedClimbable = null;
        }

        if (other.gameObject == DetectedPickup)
        {
            IsPickupDetected = false;
            DetectedPickup = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.layer != LayerMask.NameToLayer("Climbable"))
        {
            return;
        }*/
        Debug.Log(other.gameObject.tag);
        
        if (other.gameObject.CompareTag("Climbable"))
        {
            IsClimbableDetected = true;
            DetectedClimbable = other.gameObject;
        }

        if (other.gameObject.CompareTag("Pickup"))
        {
            IsPickupDetected = true;
            DetectedPickup = other.gameObject;
        }
        
    }

    public Vector3 GetGrabPoint()
    {
        RaycastHit hit;
        //Physics.ra
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}

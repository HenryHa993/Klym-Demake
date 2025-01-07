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

    public LayerMask CollisionLayerMask;

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
        //Physics.Raycast(transform.position - (transform.forward * 0.1f) , transform.forward, out hit, 5.0f, CollisionLayerMask)
        if (Physics.SphereCast(transform.position - (transform.forward * 0.1f) ,0.2f, transform.forward, out hit, 3.0f, CollisionLayerMask))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}

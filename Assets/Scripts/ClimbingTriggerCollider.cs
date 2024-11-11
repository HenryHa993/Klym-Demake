using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*Passive collision detection for detecting climbable surfaces.*/
public class ClimbingTriggerBox : MonoBehaviour
{
    public Climber _climber;

    private bool _isClimbableDetected;
    public bool IsClimbableDetected
    {
        get { return _isClimbableDetected; }
    }

    private GameObject _detectedClimbable;
    public GameObject DetectedClimbable
    {
        get { return _detectedClimbable; }
    }

    /*Nothing should actually happen here,
     the player would not move.
    It would be still cool if the hands can still move as if it is looking for a climbable surface.*/
    private void OnTriggerExit(Collider other)
    {
        _isClimbableDetected = false;
        _detectedClimbable = null;
    }

    /*Detect if surface detected is climbable.
     If yes, enable climbing.*/
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Climbable"))
        {
            return;
        }
        //Debug.Log("DETECTED: Climbable surface");
        _isClimbableDetected = true;
        _detectedClimbable = other.gameObject;
    }
}

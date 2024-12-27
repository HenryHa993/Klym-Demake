using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

// Todo: Refactor this to not use climber controller at all
public class ClimberInputs : MonoBehaviour
{
    public Vector2 Direction;
    public Vector2 ReachDirection;

    public bool ClimbEnabled;
    public bool ClimbDisabled;
    
    public bool GrabEnabled;
    public bool GrabDisabled;
    
    /*Player input mappings. Might be better to insert into the player controller.*/
    public void OnClimbEnabled(InputValue value)
    {
        ClimbEnabledInput(value.isPressed);
    }

    public void ClimbEnabledInput(bool climb)
    {
        ClimbEnabled = climb;
    }
    
    /*Input action functions. Allows the player to let go.*/
    public void OnClimbDisabled(InputValue value)
    {
        ClimbDisabledInput(value.isPressed);
    }

    public void ClimbDisabledInput(bool letGo)
    {
        ClimbDisabled = letGo;
    }

    /*Two functions, also input mapping functions. Used to try detect climbable using trigger.*/
    public void OnDetectClimbable(InputValue value)
    {
        DetectClimbableInput(value.Get<Vector2>());
    }

    public void DetectClimbableInput(Vector2 newClimbDirection)
    {
        Direction = newClimbDirection;
    }

    public void OnReach(InputValue value)
    {
        ReachInput(value.Get<Vector2>());
    }

    public void ReachInput(Vector2 newReachDirection)
    {
        ReachDirection = newReachDirection;
    }

    public void OnGrabEnable(InputValue value)
    {
        if (ClimbEnabled)
        {
            return;
        }
        GrabEnableInput();
    }

    public void GrabEnableInput()
    {
        GrabEnabled = true;
    }
    
    public void OnGrabDisable(InputValue value)
    {
        GrabDisableInput();
    }

    public void GrabDisableInput()
    {
        GrabDisabled = true;
    }
}

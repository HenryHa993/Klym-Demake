using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

// Todo: Refactor this to not use climber controller at all
public class ClimberInputs : MonoBehaviour
{
    public bool EnableClimb;
    public Vector2 DetectDirection;
    public bool Grab;
    public bool LetGo;

    /*Player input mappings. Might be better to insert into the player controller.*/
    public void OnClimb(InputValue value)
    {
        ClimbInput(value.isPressed);
    }

    public void ClimbInput(bool climb)
    {
        EnableClimb = climb;
    }

    /*Two functions, also input mapping functions. Used to try detect climbable using trigger.*/
    public void OnDetectClimbable(InputValue value)
    {
        DetectClimbableInput(value.Get<Vector2>());
    }

    public void DetectClimbableInput(Vector2 newClimbDirection)
    {
        DetectDirection = newClimbDirection;
    }

    /*Grabbing inputs to choose to move to another trigger, if there is one.*/
    public void OnGrab(InputValue value)
    {
        GrabInput(value.isPressed);
    }

    public void GrabInput(bool grab)
    {
        Grab = grab;
    }

    /*Input action functions. Allows the player to let go.*/
    public void OnLetGo(InputValue value)
    {
        LetGoInput(value.isPressed);
    }

    public void LetGoInput(bool letGo)
    {
        LetGo = letGo;
    }

}

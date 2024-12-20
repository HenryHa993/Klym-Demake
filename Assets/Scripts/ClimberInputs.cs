using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ClimberInputs : MonoBehaviour
{
    private ClimberController _climberController;

    private void Start()
    {
        _climberController = GetComponent<ClimberController>();
    }

    /*Player input mappings. Might be better to insert into the player controller.*/
    public void OnClimb(InputValue value)
    {
        ClimbInput(value.isPressed);
    }

    public void ClimbInput(bool climb)
    {
        _climberController.EnableClimbing();
    }

    /*Two functions, also input mapping functions. Used to try detect climbable using trigger.*/
    public void OnDetectClimbable(InputValue value)
    {
        DetectClimbableInput(value.Get<Vector2>());
    }

    public void DetectClimbableInput(Vector2 newClimbDirection)
    {
        //ClimbingTriggerCollider.transform.localPosition = Vector3.Lerp(ClimbingTriggerCollider.transform.localPosition, newClimbDirection.normalized,Time.deltaTime * 100f);// This should really be applied to the collider
        _climberController.ClimbingTriggerCollider.transform.localPosition = newClimbDirection.normalized;// This should really be applied to the collider
    }

    /*Grabbing inputs to choose to move to another trigger, if there is one.*/
    public void OnGrab(InputValue value)
    {
        GrabInput(value.isPressed);
    }

    public void GrabInput(bool grab)
    {
        _climberController.GrabClimbable();
    }

    /*Input action functions. Allows the player to let go.*/
    public void OnLetGo(InputValue value)
    {
        LetGoInput(value.isPressed);
    }

    public void LetGoInput(bool letGo)
    {
        _climberController.DisableClimbing();
    }

}

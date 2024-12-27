using System;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/*Climber script. Implements the details of how
 * the player latches onto a given ledge.*/
public class ClimberController : MonoBehaviour
{
    public ClimberTrigger ClimbingTriggerCollider;
    public bool IsClimbing;
    public float TransitionSpeed = 3.0f;
    public float TargetOffset = 1.2f;

    private ThirdPersonController _thirdPersonController; // For gravity/vertical velocity, player rotation
    private CharacterController _characterController; // For player transform (AND NOT ROTATION)
    private PlayerInput _playerInput; // Input management
    private ClimberInputs _climberInputs;
    
    private Vector3 _targetPosition;
    
    void Start()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _climberInputs = GetComponent<ClimberInputs>();
    }

    private void Update()
    {

        if (_climberInputs.EnableClimb)
        {
            SetClimbingEnabled(true);
            _climberInputs.EnableClimb = false;
        }

        if (_climberInputs.LetGo)
        {
            SetClimbingEnabled(false);
            _climberInputs.LetGo = false;
        }
        
        GrabClimbable();
        
    }

    /*Lerps between the player's current and target position.*/
    private void FixedUpdate()
    {
        if (!IsClimbing)
        {
            return;
        }

        // Lerping between current position and target
        // TODO: use a range instead of !=
        if (transform.position != _targetPosition)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * TransitionSpeed); // magic numbers
        }
        
        // Move around ledge detector
        Vector3 newLocalPosition = ClimbingTriggerCollider.transform.localPosition + _climberInputs.DetectDirection.ConvertTo<Vector3>();
        ClimbingTriggerCollider.transform.localPosition = Vector3.Lerp(ClimbingTriggerCollider.transform.localPosition, newLocalPosition, Time.deltaTime);
    }

    public void SetClimbingEnabled(bool enabled)
    {
        if (enabled)
        {
            if(!ClimbingTriggerCollider.IsClimbableDetected)
            {
                return;
            }
            
            _playerInput.actions.FindAction("Move").Disable();
            _playerInput.actions.FindAction("Climb").Disable();
        
            _playerInput.actions.FindActionMap("Climbing").Enable();

            // Align player with wall
            _thirdPersonController.transform.rotation = Quaternion.LookRotation(ClimbingTriggerCollider.DetectedClimbable.transform.forward);

            // Set player target position
            _targetPosition = ClimbingTriggerCollider.transform.position;
            _targetPosition.y -= TargetOffset; // Magic number
        }
        else
        {
            _playerInput.actions.FindAction("Move").Enable();
            _playerInput.actions.FindAction("Climb").Enable();
            _playerInput.actions.FindActionMap("Climbing").Disable();
        }
        
        IsClimbing = enabled;
        _thirdPersonController.SetGravityEnabled(!enabled);

        ClimbingTriggerCollider.transform.localPosition = Vector3.zero;
    }

    public void GrabClimbable()
    {
        if (!_climberInputs.Grab || !ClimbingTriggerCollider.IsClimbableDetected)
        {
            return;
        }

        _targetPosition = ClimbingTriggerCollider.transform.position;
        _targetPosition.y -= TargetOffset;
        ClimbingTriggerCollider.transform.localPosition = Vector3.zero;
        
        _climberInputs.Grab = false;
    }
}

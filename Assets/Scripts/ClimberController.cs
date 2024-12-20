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
    private Vector3 _targetPosition;
    
    void Start()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
    }

    /*Lerps between the player's current and target position.*/
    private void FixedUpdate()
    {
        if (!IsClimbing)
        {
            return;
        }

        if (transform.position != _targetPosition)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * TransitionSpeed); // magic numbers
        }
        
    }

    /*Set up climbing. Enable appropriate action mappings. Position player on hold.*/
    public void EnableClimbing()
    {
        if(IsClimbing || !ClimbingTriggerCollider.IsClimbableDetected)
        {
            return;
        }

        IsClimbing = true;
        
        _playerInput.actions.FindAction("Move").Disable();
        _playerInput.actions.FindAction("Climb").Disable();
        
        _playerInput.actions.FindActionMap("Climbing").Enable();

        // Align player with wall
        _thirdPersonController.transform.rotation = Quaternion.LookRotation(ClimbingTriggerCollider.DetectedClimbable.transform.forward);

        // Set player target position
        _targetPosition = ClimbingTriggerCollider.transform.position;
        _targetPosition.y -= TargetOffset; // Magic number

        // Disable gravity on controller
        _thirdPersonController.SetGravityEnabled(false);
    }

    public void DisableClimbing()
    {
        if (!IsClimbing)
        {
            return;
        }
        
        IsClimbing = false;

        _playerInput.actions.FindAction("Move").Enable();
        _playerInput.actions.FindAction("Climb").Enable();
        _playerInput.actions.FindActionMap("Climbing").Disable();

        _thirdPersonController.SetGravityEnabled(true);
    }

    public void GrabClimbable()
    {
        if (ClimbingTriggerCollider.IsClimbableDetected)
        {
            _targetPosition = ClimbingTriggerCollider.transform.position;
            _targetPosition.y -= TargetOffset; // Magic number
        }
    }
    
    /*Magic must be used in order to directly change the player's transform.*/
    private void Warp(Vector3 position)
    {
        _characterController.enabled = false;
        _characterController.transform.position = position;
        _characterController.enabled = true;
    }
}

using System;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

/*Climber script. Implements the details of how
 * the player latches onto a given ledge.*/
public class ClimberController : MonoBehaviour
{
    public ClimberTrigger ClimbTrigger;
    public ClimberTrigger GrabTrigger;

    public ClimberHand ActiveHand;
    public ClimberHand InactiveHand;
    
    public bool IsClimbing;
    public bool IsGrabbing;
    
    public float TransitionSpeed = 3.0f;
    public float TargetOffset = 1.2f;
    public float SoftReachRange = 1.0f;
    public float LongReachRange = 1.0f;

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

        if (_climberInputs.ClimbEnabled)
        {
            SetClimbingEnabled(true);
            _climberInputs.ClimbEnabled = false;
        }

        if (_climberInputs.ClimbDisabled)
        {
            SetClimbingEnabled(false);
            _climberInputs.ClimbDisabled = false;
        }
        
        if (_climberInputs.GrabEnabled)
        {
            SetGrabModeEnabled(true);
            _climberInputs.GrabEnabled = false;
        }

        if (_climberInputs.GrabDisabled)
        {
            SetGrabModeEnabled(false);
            _climberInputs.GrabDisabled = false;
        }
        
        MoveReach();
    }

    /*Lerps between the player's current and target position.*/
    private void FixedUpdate()
    {
        if (!IsClimbing)
        {
            return;
        }
        
        // Move around ledge detector but if grab is disabled in this frame, do not 
        if (_climberInputs.Direction != Vector2.zero && !IsGrabbing)
        {
            ClimbTrigger.transform.localPosition = _climberInputs.Direction * SoftReachRange;
            GrabClimbable(ClimbTrigger);
        }

        // Lerping between current position and target
        if ((transform.position - _targetPosition).magnitude > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.fixedDeltaTime * TransitionSpeed); // magic numbers
        }
        else
        {
            IsGrabbing = false;
        }
    }

    public void SetClimbingEnabled(bool enabled)
    {
        if (enabled)
        {
            if(!ClimbTrigger.IsClimbableDetected)
            {
                return;
            }
            
            _playerInput.actions.FindAction("Move").Disable();
            _playerInput.actions.FindAction("ClimbEnabled").Disable();
        
            _playerInput.actions.FindActionMap("Climbing").Enable();
            _playerInput.actions.FindAction("Reach").Disable();

            // Align player with wall
            _thirdPersonController.transform.rotation = Quaternion.LookRotation(ClimbTrigger.DetectedClimbable.transform.forward);

            /*// Set player target position
            _targetPosition = ClimbingTriggerCollider.transform.position;
            _targetPosition.y -= TargetOffset; // Magic number*/
            GrabClimbable(ClimbTrigger);
        }
        else
        {
            _playerInput.actions.FindAction("Move").Enable();
            _playerInput.actions.FindAction("ClimbEnabled").Enable();
            _playerInput.actions.FindActionMap("Climbing").Disable();
            
            SetGrabModeEnabled(false);
        }
        
        IsClimbing = enabled;
        _thirdPersonController.SetGravityEnabled(!enabled);

        ClimbTrigger.transform.localPosition = Vector3.zero;
        GrabTrigger.transform.localPosition = Vector3.zero;
    }

    public void GrabClimbable(ClimberTrigger trigger)
    {
        if (!trigger.IsClimbableDetected)
        {
            return;
        }

        _targetPosition = trigger.transform.position;
        
        ActiveHand.SetHandPosition(_targetPosition);
        
        _targetPosition.y -= TargetOffset;
        
        trigger.transform.localPosition = Vector3.zero;

        (ActiveHand, InactiveHand) = (InactiveHand, ActiveHand);
        InactiveHand.SetHandActive(false);
    }

    // Initiate grabbing controls if the player is holding the mouse button that frame
    public void SetGrabModeEnabled(bool enabled)
    {
        if (enabled)
        {
            _playerInput.actions.FindAction("Reach").Enable();
            _playerInput.actions.FindAction("Look").Disable();
        }
        else
        {
            _playerInput.actions.FindAction("Reach").Disable();
            _playerInput.actions.FindAction("Look").Enable();
            
            IsGrabbing = true;
            
            GrabClimbable(GrabTrigger);
            GrabTrigger.transform.localPosition = Vector3.zero;
        }
    }

    /*Move player's reach according to mouse movements*/
    public void MoveReach()
    {
        if (_climberInputs.ReachDirection == Vector2.zero)
        {
            return;
        }
        
        Vector3 newLocalPosition = GrabTrigger.transform.localPosition + _climberInputs.ReachDirection.ConvertTo<Vector3>();
        GrabTrigger.transform.localPosition = Vector3.Lerp(GrabTrigger.transform.localPosition, newLocalPosition, Time.deltaTime);
        GrabTrigger.transform.localPosition =
            Vector3.ClampMagnitude(GrabTrigger.transform.localPosition, LongReachRange);
        
        ActiveHand.SetHandPosition(GrabTrigger.transform.position - ActiveHand.Offset);
        ActiveHand.SetHandActive(true);
        ActiveHand.transform.localRotation = Quaternion.Euler(0, 0,
            (Mathf.Atan2(GrabTrigger.transform.localPosition.y, GrabTrigger.transform.localPosition.x) * Mathf.Rad2Deg) - 90.0f);
    }
}

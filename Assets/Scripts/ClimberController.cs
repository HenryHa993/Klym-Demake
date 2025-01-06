using System;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
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
    public CinemachineVirtualCamera _virtualCamera;
    public CameraRoot PlayerCameraRoot;
    
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
    
    public Vector3 ClimbCameraDamping = new Vector3(5,5,5);
    public Vector3 NavigationDamping;

    private ThirdPersonController _thirdPersonController; // For gravity/vertical velocity, player rotation
    private CharacterController _characterController; // For player transform (AND NOT ROTATION)
    private PlayerInput _playerInput; // Input management
    private ClimberInputs _climberInputs;
    private Animator _animator;

    private Cinemachine3rdPersonFollow _thirdPersonFollow;
    private IEnumerator CameraZoomCoroutine;
    
    private Vector3 _targetPosition;
    
    void Start()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _climberInputs = GetComponent<ClimberInputs>();
        _animator = GetComponent<Animator>();
        
        _playerInput.actions.FindAction("Reach").Disable();
        
        _thirdPersonFollow =
            _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        NavigationDamping = _thirdPersonFollow.Damping;
    }

    /*Listens for potential inputs.*/
    private void Update()
    {
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

    /*Lerps between the player's current and target climbing position.*/
    private void FixedUpdate()
    {
        if (!IsClimbing)
        {
            return;
        }
        
        // WASD will allow the player to shift across their current platform, only if they are not transitioning from a grab motion.
        if (_climberInputs.Direction != Vector2.zero && !IsGrabbing)
        {
            ClimbTrigger.transform.localPosition = _climberInputs.Direction * SoftReachRange;
            GrabClimbable(ClimbTrigger);
        }

        // Interpolates between current position and target.
        if ((transform.position - _targetPosition).magnitude > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.fixedDeltaTime * TransitionSpeed); // magic numbers
        }
        else
        {
            IsGrabbing = false;
        }
    }

    /*Setup for action mappings and player rotation for climbing.*/
    public void SetClimbingEnabled(bool enabled)
    {
        if (enabled)
        {
            _playerInput.actions.FindAction("Move").Disable();
            _playerInput.actions.FindActionMap("Climbing").Enable();
            _playerInput.actions.FindAction("Reach").Disable();

            // Align player with climbable
            _thirdPersonController.transform.rotation = Quaternion.LookRotation(GrabTrigger.DetectedClimbable.transform.forward);
            
            // Virtual camera settings
            _thirdPersonFollow.Damping = ClimbCameraDamping;
            
            if (CameraZoomCoroutine != null)
            {
                StopCoroutine(CameraZoomCoroutine);
            }
            
            CameraZoomCoroutine = ZoomCameraOut(7);
            StartCoroutine(CameraZoomCoroutine);
        }
        else
        {
            _playerInput.actions.FindAction("Move").Enable();
            _playerInput.actions.FindActionMap("Climbing").Disable();
            
            _playerInput.actions.FindAction("Reach").Disable();
            _playerInput.actions.FindAction("Look").Enable();
            
            // Reset hand states and grab trigger.
            ActiveHand.SetHandState(ClimberHand.HandState.Idle);
            InactiveHand.SetHandState(ClimberHand.HandState.Idle);
            
            //GrabTrigger.transform.localPosition = Vector3.zero; // todo: I feel like triggers should be reset else where?

            _thirdPersonFollow.Damping = NavigationDamping;

            if (CameraZoomCoroutine != null)
            {
                StopCoroutine(CameraZoomCoroutine);
            }
            
            CameraZoomCoroutine = ZoomCameraIn(5);
            StartCoroutine(CameraZoomCoroutine);
        }
        
        IsClimbing = enabled;
        
        _thirdPersonController.SetGravityEnabled(!enabled);
        
        _animator.SetBool("IsClimbing", enabled); // todo: Cache ID

        ClimbTrigger.transform.localPosition = Vector3.zero;
    }

    /*Set target position, depending on if a climbable is detected on the input trigger*/
    public void GrabClimbable(ClimberTrigger trigger)
    {
        // Do nothing if no climbable
        if (!trigger.IsClimbableDetected)
        {
            if (!IsClimbing)
            {
                SetClimbingEnabled(false);
            }
            GrabTrigger.transform.localPosition = Vector3.zero;
            return;
        }
        
        // If not climbing, initiate climbing mode.
        if (!IsClimbing)
        {
            SetClimbingEnabled(true);
        }
        
        _targetPosition = trigger.transform.position;
        
        // Set new ActiveHand target position, and swap hands.
        ActiveHand.SetTargetPosition(_targetPosition);
        ActiveHand.SetHandState(ClimberHand.HandState.Grab);
        (ActiveHand, InactiveHand) = (InactiveHand, ActiveHand);
        
        _targetPosition.y -= TargetOffset;
        
        // Reset position.
        // todo: should this be somewhere else? What if it fails?
        trigger.transform.localPosition = Vector3.zero;

    }

    /*Settings for initiating/exiting grabbing mode when the LMB is up or down.*/
    public void SetGrabModeEnabled(bool enabled)
    {
        // Grab mode will disable camera looking function, and mouse input will instead move the hands.
        if (enabled)
        {
            _playerInput.actions.FindAction("Reach").Enable();
            _playerInput.actions.FindAction("Look").Disable();
            SetGrabCameraActive(true);
        }
        // Exiting grab mode will attempt to grab a detectable. It will also reset grab trigger.
        else
        {
            _playerInput.actions.FindAction("Reach").Disable();
            _playerInput.actions.FindAction("Look").Enable();
            
            // todo: should this be in an if detected statement?
            IsGrabbing = true;
            GrabClimbable(GrabTrigger);
            
            // todo: should I rely on grab trigger to reset it?
            //GrabTrigger.transform.localPosition = Vector3.zero;
            SetGrabCameraActive(false);
        }
    }

    /*Grab mechanic, move the arm according to mouse if LMB is pressed.*/
    public void MoveReach()
    {
        // todo: State for holding LMB
        if (!_climberInputs.GrabActive)
        {
            return;
        }
        
        // Set ActiveHand target position to GrabTrigger's position and clamp to range.
        Vector3 newLocalPosition = GrabTrigger.transform.localPosition + _climberInputs.ReachDirection.ConvertTo<Vector3>();
        GrabTrigger.transform.localPosition = Vector3.Lerp(GrabTrigger.transform.localPosition, newLocalPosition, Time.deltaTime);
        GrabTrigger.transform.localPosition =
            Vector3.ClampMagnitude(GrabTrigger.transform.localPosition, LongReachRange);
        
        ActiveHand.SetTargetPosition(GrabTrigger.transform.position);
        
        // Set appropriate hand state.
        if (GrabTrigger.IsClimbableDetected)
        {
            ActiveHand.SetHandState(ClimberHand.HandState.DetectLedge);
        }
        else
        {
            ActiveHand.SetHandState(ClimberHand.HandState.Reach);
        }
    }

    /*Grab camera to focus on hands when grabbing.*/
    public void SetGrabCameraActive(bool enabled)
    {
        if (!IsClimbing)
        {
            return;
        }
        
        if (enabled)
        {
            PlayerCameraRoot.SetTarget(ActiveHand.gameObject);
        }
        else
        {
            PlayerCameraRoot.Reset();
        }
    }

    public IEnumerator ZoomCameraOut(float zoom)
    {
        for (float z = _thirdPersonFollow.CameraDistance; z < zoom; z += Time.deltaTime)
        {
            _thirdPersonFollow.CameraDistance = z;
            yield return null;
        }
    }
    
    public IEnumerator ZoomCameraIn(float zoom)
    {
        for (float z = _thirdPersonFollow.CameraDistance; z > zoom; z -= Time.deltaTime)
        {
            _thirdPersonFollow.CameraDistance = z;
            yield return null;
        }
    }
}

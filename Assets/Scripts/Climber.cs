using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

/*Climber script. Implements the details of how
 * the player latches onto a given ledge.*/
public class Climber : MonoBehaviour
{
    public ClimbingTriggerBox ClimbingTriggerCollider;

    private ThirdPersonController _thirdPersonController; // For gravity/vertical velocity, player rotation
    private CharacterController _characterController; // For player transform (AND NOT ROTATION)
    private PlayerInput _playerInput; // Input management
    private Vector3 _targetPosition;

    private bool _isClimbing = false;

    // Start is called before the first frame update
    void Start()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
    }

    /*Handles lerping between holds to make it seem smoother.*/
    private void FixedUpdate()
    {
        if (!_isClimbing)
        {
            return;
        }

        if (transform.position != _targetPosition)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * 3.0f); // magic numbers
        }
        
    }

    /*Activate Action Map for Climbing,
     Setup initial climbing position.
    I opted to seperate the logic of enabling and disabling for readability.*/
    public void EnableClimbing()
    {
        if(_isClimbing)
        {
            return;
        }

        _isClimbing = true;

        /*Edit Input Maps
        Move into separate function*/
        _playerInput.actions.FindAction("Move").Disable();
        _playerInput.actions.FindAction("Climb").Disable();
        _playerInput.actions.FindActionMap("Climbing").Enable();

        /*Align player to wall here*/
        _thirdPersonController.transform.rotation = Quaternion.LookRotation(ClimbingTriggerCollider.DetectedClimbable.transform.forward);

        /*Refine the position we warp to*/
        _targetPosition = ClimbingTriggerCollider.transform.position;
        _targetPosition.y = _targetPosition.y - 1.2f; // Magic number

        _thirdPersonController.SetGravityEnabled(false);
    }

    public void DisableClimbing()
    {
        if (!_isClimbing)
        {
            return;
        }
        
        _isClimbing = false;

        _playerInput.actions.FindAction("Move").Enable();
        _playerInput.actions.FindActionMap("Climbing").Disable();

        _thirdPersonController.SetGravityEnabled(true);
    }

    public void OnClimb(InputValue value)
    {
        ClimbInput(value.isPressed);
    }

    public void ClimbInput(bool climb)
    {
        EnableClimbing();
    }

    /*Occurs when climbing enabled and Input Actions called.*/
    public void OnDetectLedge(InputValue value)
    {
        DetectLedgeInput(value.Get<Vector2>());
    }

    /*Moves the collider around, which is used to detect climbables.*/
    public void DetectLedgeInput(Vector2 newClimbDirection)
    {
        //ClimbingTriggerCollider.transform.localPosition = Vector3.Lerp(ClimbingTriggerCollider.transform.localPosition, newClimbDirection.normalized,Time.deltaTime * 100f);// This should really be applied to the collider
        ClimbingTriggerCollider.transform.localPosition = newClimbDirection.normalized;// This should really be applied to the collider
    }

    /*Grab input.*/
    public void OnGrab(InputValue value)
    {
        GrabInput(value.isPressed);
    }

    /*Allows players to choose their point of movement.*/
    public void GrabInput(bool grab)
    {
        if (ClimbingTriggerCollider.IsClimbableDetected)
        {
            _targetPosition = ClimbingTriggerCollider.transform.position;
            _targetPosition.y = _targetPosition.y - 1.2f; // Magic number
        }
    }

    /*Magic must be used in order to directly change the player's transform.*/
    private void Warp(Vector3 position)
    {
        _characterController.enabled = false;
        _characterController.transform.position = position;// Vector3.Lerp(_characterController.transform.position, position, 0.1f);
        _characterController.enabled = true;
    }


}

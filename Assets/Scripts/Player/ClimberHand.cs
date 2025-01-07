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

public class ClimberHand : MonoBehaviour
{
    public enum HandState : int
    {
        Idle,
        Reach,
        Grab,
        DetectLedge
    }

    public Sprite[] HandSprites;
    private HandState _currentState = HandState.Idle;
    
    public Vector3 Offset;
    public float TransitionSpeed = 15.0f;

    public SpriteRenderer SpriteRenderer;
    public Vector3 _targetPosition;

    private void FixedUpdate()
    {
        // If idle, hands should not be moving
        if (_currentState == HandState.Idle)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.fixedDeltaTime * TransitionSpeed);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.fixedTime * TransitionSpeed);
            return;
        }
        
        // Otherwise, go to target position
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.fixedDeltaTime * TransitionSpeed);
        transform.localRotation = Quaternion.Euler(0, 0,
            (Mathf.Atan2(transform.localPosition.y, transform.localPosition.x) * Mathf.Rad2Deg) - 90.0f);
    }

    public void SetHandState(HandState state)
    {
        SpriteRenderer.sprite = HandSprites[(int)state];
        _currentState = state;
    }

    public void SetTargetPosition(Vector3 position)
    {
        _targetPosition = position + Offset;
    }
}

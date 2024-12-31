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
    public GameObject Body;
    public Sprite OpenHand;
    public Sprite ClosedHand;

    public Vector3 Offset;
    public float TransitionSpeed = 15.0f;

    private SpriteRenderer _spriteRenderer;
    private Vector3 _targetPosition;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.fixedDeltaTime * TransitionSpeed);
        transform.localRotation = Quaternion.Euler(0, 0,
            (Mathf.Atan2(transform.localPosition.y, transform.localPosition.x) * Mathf.Rad2Deg) - 90.0f);
    }

    public void SetHandActive(bool enabled)
    {
        if (enabled)
        {
            _spriteRenderer.sprite = OpenHand;
        }
        else
        {
            _spriteRenderer.sprite = ClosedHand;
        }
    }

    public void SetHandPosition(Vector3 position)
    {
        _targetPosition = position + Offset;
    }
}

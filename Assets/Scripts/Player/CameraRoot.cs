using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoot : MonoBehaviour
{
    private GameObject _targetGameObject;
    private bool _isFollowing;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_isFollowing)
        {
            return;
        }

        transform.position = _targetGameObject.transform.position;
    }

    public void SetTarget(GameObject target)
    {
        _isFollowing = true;

        _targetGameObject = target;
    }

    public void Reset()
    {
        _isFollowing = false;
        transform.localPosition = Vector3.zero;
    }
}

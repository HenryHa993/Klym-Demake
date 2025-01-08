using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using StarterAssets;
using UnityEngine;

/*This manages bird. I am opting to flick on/off instead of fading
 because you will not see the animation anyways when dialogue is showing.*/
public class BirdManager : MonoBehaviour
{
    public GameObject Bird;
    public string[] HasBirdDialogue;
    public float BirdGravity = -2.0f;
    
    private ThirdPersonController _thirdPersonController;
    private Subtitles _subtitleComponent;

    private bool _hasBird;
    private float _defaultGravity;

    private void Start()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _subtitleComponent = GetComponent<Subtitles>();
        
        _defaultGravity = _thirdPersonController.DefaultGravity;
    }

    /*Return true if successfully gave bird. Otherwise, return false.*/
    public bool TakeBird()
    {
        // Play dialogue.
        if (_hasBird)
        {
            _subtitleComponent.StartDialogue(HasBirdDialogue);
            return false;
        }

        Bird.SetActive(true);
        _hasBird = true;
        _thirdPersonController.Gravity = BirdGravity;
        _thirdPersonController.DefaultGravity = BirdGravity;
        return true;
    }

    public bool GiveBird()
    {
        if (!_hasBird)
        {
            return false;
        }

        Bird.SetActive(false);
        _hasBird = false;
        _thirdPersonController.Gravity = _defaultGravity;
        _thirdPersonController.DefaultGravity = _defaultGravity;
        return true;
    }
}

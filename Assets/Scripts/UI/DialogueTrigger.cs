using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This triggers dialogue, by passing an array of strings to the player's Subtitles component.*/
public class DialogueTrigger : MonoBehaviour
{
    public string[] Dialogue;
    
    private Subtitles _subtitleSystem;
    
    private void Start()
    {
        _subtitleSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<Subtitles>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _subtitleSystem.StartDialogue(Dialogue);
        Destroy(gameObject);
    }
}

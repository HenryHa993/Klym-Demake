using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : Pickup
{
    public string[] dialogue;
    
    public override IEnumerator Interaction()
    {
        yield return base.Interaction();

        BirdManager _birdManager = _player.GetComponent<BirdManager>();

        // If player does not have bird, play dialogue
        if (_birdManager.TakeBird())
        {
            Subtitles _subtitleComponent = _player.GetComponent<Subtitles>();
            _subtitleComponent.StartDialogue(dialogue);   
            Destroy(gameObject);
        }
        
    }
}

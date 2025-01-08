using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pants : Pickup
{
    public string[] dialogue;
    
    public override IEnumerator Interaction()
    {
        yield return base.Interaction();

        Clothing _clothingComponent = _player.GetComponent<Clothing>();
        _clothingComponent.Wear(_clothingComponent.Bottom);
        yield return _fadeComponent.FadeOut();
        
        Subtitles _subtitleComponent = _player.GetComponent<Subtitles>();
        _subtitleComponent.StartDialogue(dialogue);
        
        Destroy(gameObject);
    }
}

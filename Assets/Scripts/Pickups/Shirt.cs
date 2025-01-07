using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shirt : Pickup
{
    public override IEnumerator Interaction()
    {
        yield return base.Interaction();
        
        Clothing _clothingComponent = _player.GetComponent<Clothing>();
        _clothingComponent.Wear(_clothingComponent.Shirt);
        yield return _fadeComponent.FadeOut();
        Destroy(gameObject);
    }
}

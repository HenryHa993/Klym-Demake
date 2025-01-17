using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*All pickups are intended to interact with the player object,
 so we find and cache the player game object. It would be more appropriate
 to call this an interactable because I use it for basic interactions.*/
public class Pickup : MonoBehaviour
{
    protected GameObject _player;
    public Fade _fadeComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public virtual IEnumerator Interaction()
    {
        yield return null;
    }
}

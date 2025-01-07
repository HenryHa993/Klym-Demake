using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprint : MonoBehaviour
{
    private Fade _fadeComponent;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        _fadeComponent = GetComponent<Fade>();
        
        yield return _fadeComponent.FadeIn();
        yield return _fadeComponent.FadeOut();
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprint : MonoBehaviour
{
    private Color _color;
    
    public float FadeInSpeed = 1.0f;
    public float FadeOutSpeed = 1.0f;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        _color = GetComponent<SpriteRenderer>().material.color;
        yield return FadeIn();
        yield return FadeOut();
    }

    public IEnumerator FadeOut()
    {
        for (float alpha = 1f; alpha >= 0; alpha -= (Time.deltaTime * FadeOutSpeed))
        {
            _color.a = alpha;
            GetComponent<SpriteRenderer>().material.SetColor("_Color", _color);
            yield return null;
        }
        Debug.Log("Faded out");
        
        Destroy(this);
    }
    
    public IEnumerator FadeIn()
    {
        for (float alpha = 0f; alpha <= 1; alpha += (Time.deltaTime * FadeInSpeed))
        {
            _color.a = alpha;
            GetComponent<SpriteRenderer>().material.SetColor("_Color", _color);
            yield return null;
        }
        Debug.Log("Faded in");
    }
}

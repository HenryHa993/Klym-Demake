using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Color _color;
    
    public float FadeInSpeed = 1.0f;
    public float FadeOutSpeed = 1.0f;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _color = _spriteRenderer.material.color;
    }

    public IEnumerator FadeOut()
    {
        for (float alpha = 1f; alpha >= 0; alpha -= (Time.deltaTime * FadeOutSpeed))
        {
            _color.a = alpha;
            GetComponent<SpriteRenderer>().material.SetColor("_Color", _color);
            yield return null;
        }
    }
    
    public IEnumerator FadeIn()
    {
        for (float alpha = 0f; alpha <= 1; alpha += (Time.deltaTime * FadeInSpeed))
        {
            _color.a = alpha;
            GetComponent<SpriteRenderer>().material.SetColor("_Color", _color);
            yield return null;
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numbers : MonoBehaviour
{
    public float fadeSpeed;
    
    private SpriteRenderer _spriteRenderer;
    private bool _fade;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _fade = true;
        _spriteRenderer.color = Color.white;
    }

    private void Update()
    {
        if (_fade)
        {
            _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, Color.clear, Time.deltaTime * fadeSpeed);
            if (_spriteRenderer.color.a < 0.05)
            {
                _fade = false;
            }
        }
    }
}

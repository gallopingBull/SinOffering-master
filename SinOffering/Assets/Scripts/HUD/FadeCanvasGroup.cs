using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class that handles fade in/out behavior for canvas UI elements.
/// </summary>

public class FadeCanvasGroup : MonoBehaviour
{
    [SerializeField] float _fadeDuration = 2;
    
    [Range(.5f, 1)]
    [SerializeField] float _fadeSpeed = 1f;
    private float _maxDelta = 0;

    private bool _fadeEnabled = false;
    private float _targetAlpha;
    private float _currentAlpha;
    private CanvasGroup _canvas;
    
    [SerializeField] 
    Queue<CanvasGroup> _canvases = new Queue<CanvasGroup>();

    private void OnEnable()
    {
        UIEvents.OnHUDDisplay += FadeInCanvasGroup;
        UIEvents.OnHUDHide += FadeOutCanvasGroup;
        UIEvents.OnMenuOpened += FadeInCanvasGroup;
        UIEvents.OnMenuClosed += FadeOutCanvasGroup;
    }
    
    private void OnDisable()
    {
        UIEvents.OnHUDDisplay -= FadeInCanvasGroup;
        UIEvents.OnHUDHide -= FadeOutCanvasGroup;
        UIEvents.OnMenuOpened -= FadeInCanvasGroup;
        UIEvents.OnMenuClosed -= FadeOutCanvasGroup;
    }
    
    private void FixedUpdate()
    {
        if (_fadeEnabled)
            Fade(Time.fixedDeltaTime);
    }
    
    private void Fade(float deltaTime)
    {   
        _maxDelta = _fadeDuration * deltaTime * _fadeSpeed;
        _currentAlpha = Mathf.MoveTowards(_currentAlpha, _targetAlpha, _maxDelta);
        _canvas.alpha = _currentAlpha;
        if (_currentAlpha == _targetAlpha)
            DisableFade();
    }
    
    private void FadeInCanvasGroup(CanvasGroup canvas)
    {
        if (!_canvases.Contains(canvas))
            _canvases.Enqueue(canvas);
        if (_canvas == null)
        {
            _canvas = canvas;
            _currentAlpha = 0;
            _targetAlpha = 1;
            _fadeEnabled = true;
        }
    }
    
    private void FadeOutCanvasGroup(CanvasGroup canvas)
    {
        if (!_canvases.Contains(canvas))
            _canvases.Enqueue(canvas);
        if (_canvas == null)
        {
            _canvas = canvas;
            _currentAlpha = 1;
            _targetAlpha = 0;
            _fadeEnabled = true;
        }
    }
    
    private void DisableFade()
    {
        _fadeEnabled = false;
        _canvases.Dequeue();
        _canvas = null;
        if (_canvases.Count > 0)
        {
            if (_canvases.Peek().alpha == 0)
                FadeInCanvasGroup(_canvases.Peek());
            else
                FadeOutCanvasGroup(_canvases.Peek());
        }
    }
}

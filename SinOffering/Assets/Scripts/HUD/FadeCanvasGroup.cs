using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// singleton class that handles fade in/out behavior for canvas UI elements.
/// </summary>
public class FadeCanvasGroup : MonoBehaviour
{
    static private FadeCanvasGroup _instance;
    [SerializeField] float _fadeDuration = 2;
    
    [Range(.5f, 1)]
    [SerializeField] float _fadeSpeed = 1f;
    private float _maxDelta = 0;

    private bool _fadeEnabled = false;
    private float _targetAlpha;
    private float _currentAlpha;
    static private CanvasGroup _canvas;
    [SerializeField]
    static private Queue<CanvasGroup> _canvases = new Queue<CanvasGroup>();
    static public FadeCanvasGroup Instance { get => _instance; }

    private void OnEnable()
    {
        if (_instance == null)
            _instance = this;
    }
    private void OnDisable()
    {
        _instance = null;
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
    public void FadeInCanvasGroup(CanvasGroup canvas)
    {
        Debug.Log($"FadeInCanvasGroup({canvas.gameObject.name})");
        if (!_canvases.Contains(canvas))
            _canvases.Enqueue(canvas);
        if (_canvas == null)
            _canvas = canvas;
        _currentAlpha = 0;
        _targetAlpha = 1;
        _fadeEnabled = true;
    }
    public void FadeOutCanvasGroup(CanvasGroup canvas)
    {
        Debug.Log($"FadeOutCanvasGroup({canvas.gameObject.name})");
        if (!_canvases.Contains(canvas))
            _canvases.Enqueue(canvas);
        if (_canvas == null)
            _canvas = canvas;
        _currentAlpha = 1;
        _targetAlpha = 0;
        _fadeEnabled = true;
    }
}

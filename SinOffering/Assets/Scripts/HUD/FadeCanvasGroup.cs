using System.Collections.Generic;
using UnityEngine;

public class FadeCanvasGroup : MonoBehaviour
{
    [SerializeField]
    private float _fadeDuration = 2;
    
    static private bool _fadeEnabled = false;
    static private float _targetAlpha;
    static private float _currentAlpha;
    static private CanvasGroup _canvasGroup;
 
    private void Awake()
    {
        //_canvasGroup = GetComponent<CanvasGroup>();
        //canvasGroups = new Queue<CanvasGroup>();
    }

    private void FixedUpdate()
    {
        if (_fadeEnabled)
        {
            // fade in/out calculation
            // magic number, test to see if it reeally makes a difference when scaling faderate time
            _currentAlpha = Mathf.MoveTowards(_currentAlpha, _targetAlpha, _fadeDuration * Time.fixedDeltaTime *1.15f); 
            _canvasGroup.alpha = _currentAlpha;
            if (_currentAlpha == _targetAlpha)
            {
                _fadeEnabled = false;
                _canvasGroup = null;
            }      
        } 
    }

    public void FadeInCanvasGroup()
    {
        
        _currentAlpha = 0;
        _targetAlpha = 1;
        _fadeEnabled = true;
    }
    public void FadeOutCanvasGroup()
    {
        _currentAlpha = 1;
        _targetAlpha = 0;
        _fadeEnabled = true;
    }
    static public void FadeInCanvasGroup(CanvasGroup canvas)
    {
        Debug.Log("calling FadeIn from: " + canvas.name);
        _canvasGroup = canvas;
        _currentAlpha = 0;
        _targetAlpha = 1;
        _fadeEnabled = true;
    }

    static public void FadeOutCanvasGroup(CanvasGroup canvas) 
    {
        Debug.Log("calling FadeIn from: " + canvas.name);
        _canvasGroup = canvas;
        _currentAlpha = 1;
        _targetAlpha = 0;
        _fadeEnabled = true;
    }

    #region queue testing
    public void AddCanvasGroupToFadeQueue(CanvasGroup canvas)
    {
        //canvasGroups.Enqueue(canvas);
    }

    public void RemoveCanvasGroupToFadeQueue(CanvasGroup canvas)
    {
        //canvasGroups.Enqueue(canvas);
    }
    #endregion

}

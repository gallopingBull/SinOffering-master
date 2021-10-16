using System.Collections.Generic;
using UnityEngine;

public class FadeCanvasGroup : MonoBehaviour
{
    [HideInInspector]
    public static FadeCanvasGroup _instance;
    private CanvasGroup canvasGroup;
    private List<CanvasGroup> canvasGroups;

    private bool fadeEnabled = false;
  
    private float targetAlpha;
    private float currentAlpha;
    [SerializeField]
    private float fadeDuration = 2;

    private void Awake()
    {
        if (_instance != null)
            return;
        _instance = this;
        canvasGroup = new CanvasGroup();
        canvasGroups = new List<CanvasGroup>();
    }

    private void FixedUpdate()
    {
        if (fadeEnabled)
        {
            // fade in/out calculation
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, 2.0f * Time.deltaTime);
            canvasGroup.alpha = currentAlpha;
            if (currentAlpha == targetAlpha)
                fadeEnabled = false;
        } 
    }



    public void FadeInCanvasGroup(CanvasGroup canvas)
    {
        currentAlpha = 0;
        targetAlpha = 1;
        canvasGroup = canvas;
        fadeEnabled = true;
    }

    public void FadeOutCanvasGroup(CanvasGroup canvas) 
    {
        currentAlpha = 1;
        targetAlpha = 0;
        canvasGroup = canvas;
        fadeEnabled = true;
    }
}

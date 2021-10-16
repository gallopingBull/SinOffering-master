using System.Collections.Generic;
using UnityEngine;

public class FadeCanvasGroup : MonoBehaviour
{
    [HideInInspector]
    //public static FadeCanvasGroup _instance;
    private CanvasGroup canvasGroup;
    //private Queue<CanvasGroup> canvasGroups;

    private bool fadeEnabled = false;
  
    private float targetAlpha;
    private float currentAlpha;
    [SerializeField]
    private float fadeDuration = 2;

    private void Awake()
    {
        // if (_instance != null)
        //   return;
        //_instance = this;
        if (canvasGroup != null)
            return;

        canvasGroup = GetComponent<CanvasGroup>();
        //canvasGroups = new Queue<CanvasGroup>();
    }

    private void FixedUpdate()
    {
        if (fadeEnabled)
        {
            // fade in/out calculation
            // magic number, test to see if it reeally makes a difference when scaling faderate time
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeDuration * Time.fixedDeltaTime *1.15f); 
            canvasGroup.alpha = currentAlpha;
            if (currentAlpha == targetAlpha)
                fadeEnabled = false;
        } 
    }

    public void FadeInCanvasGroup()
    {
        currentAlpha = 0;
        targetAlpha = 1;
        fadeEnabled = true;
    }
    public void FadeOutCanvasGroup()
    {
        currentAlpha = 1;
        targetAlpha = 0;
        fadeEnabled = true;
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

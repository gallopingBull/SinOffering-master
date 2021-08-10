//https://youtu.be/nNbM40HFyCs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicBars : MonoBehaviour
{
    #region variables
    [HideInInspector]
    public bool EnabledBars = false;
    [HideInInspector]
    public bool isActive = false;
    public float BarSize = 300;
    
    private float TargetSize;
    private float ChangeSizeAmmount;
    private RectTransform topBar, bottomBar;
    
    #endregion

    #region functions   
    // Start is called before the first frame update
    private void Awake()
    {
        GameObject gameObject = new GameObject("topBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        topBar = gameObject.GetComponent<RectTransform>();
        topBar.anchorMin = new Vector2(0, 1);
        topBar.anchorMax = new Vector2(1, 1);
        topBar.sizeDelta = new Vector2(0, 300);

        gameObject = new GameObject("bottomBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        bottomBar = gameObject.GetComponent<RectTransform>();
        bottomBar.anchorMin = new Vector2(0, 0);
        bottomBar.anchorMax = new Vector2(1, 0);
        bottomBar.sizeDelta = new Vector2(0, 300);

        Hide(0f);
    }

    private void Update()
    {
        if (EnabledBars)
        {
            Vector2 sizeDelta = topBar.sizeDelta;
            sizeDelta.y += ChangeSizeAmmount * Time.deltaTime;
            if (ChangeSizeAmmount > 0)
            {
                if (sizeDelta.y >= TargetSize)
                {
                    sizeDelta.y = TargetSize;
                    EnabledBars = false;
                }
            }
            else
            {
                if (sizeDelta.y <= TargetSize)
                {
                    sizeDelta.y = TargetSize;
                    EnabledBars = false;
                }
            }
            topBar.sizeDelta = sizeDelta;
            bottomBar.sizeDelta = sizeDelta;
        }
    }

    public void Show(float time)
    {
        TargetSize = BarSize;
        ChangeSizeAmmount = (TargetSize - topBar.sizeDelta.y) / time;
        EnabledBars = true;
        isActive = true;
    }

    public void Hide(float time)
    {
        TargetSize = 0;
        ChangeSizeAmmount = (TargetSize - topBar.sizeDelta.y) / time;
        EnabledBars = true;
        isActive = false;
    }
    #endregion

}

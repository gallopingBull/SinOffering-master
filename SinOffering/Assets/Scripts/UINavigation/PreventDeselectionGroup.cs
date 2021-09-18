// original author: Dylan Wolf
// https://www.gamasutra.com/blogs/DylanWolf/20190128/335228/Stupid_Unity_UI_Navigation_Tricks.php

using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

/// <summary>
/// 
/// prevents a button from being deselected and remains selected
/// if the player clicks outside the button interactable region
/// 
/// </summary>
public class PreventDeselectionGroup : MonoBehaviour
{
    private EventSystem eventSystem;
    private GameObject selectedObject;

    private void Start()
    {
        eventSystem = EventSystem.current;
    }
            
    private void Update()
    {
        if (eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject != selectedObject)
            selectedObject = eventSystem.currentSelectedGameObject;
        else if (selectedObject != null && eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(selectedObject);
    }
}

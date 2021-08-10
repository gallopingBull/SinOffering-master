// original author: Dylan Wolf
// https://www.gamasutra.com/blogs/DylanWolf/20190128/335228/Stupid_Unity_UI_Navigation_Tricks.php

// prevents a button from being deselected and remains selected
// if the player clicks out of the button 
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
public class PreventDeselectionGroup : MonoBehaviour
{
    EventSystem evt;

    private void Start()
    {
        evt = EventSystem.current;
    }

    public  GameObject sel;

    private void Update()
    {
        if (evt.currentSelectedGameObject != null && evt.currentSelectedGameObject != sel)
            sel = evt.currentSelectedGameObject;
        else if (sel != null && evt.currentSelectedGameObject == null)
            evt.SetSelectedGameObject(sel);
    }
}

using System;
using UnityEngine;

/// <summary>
/// static class that store generic UI action events. 
/// </summary>

public static class UIEvents
{
    static public Action<CanvasGroup> OnHUDDisplay;
    static public Action<CanvasGroup> OnHUDHide;
    
    // this is a unique event to hide/display HUD without
    // requiring a parameter. 
    static public Action OnStoreMenuOpened;
    static public Action OnStoreMenuClosed;

    static public Action<CanvasGroup> OnMenuOpened;
    static public Action<CanvasGroup> OnMenuClosed;
}

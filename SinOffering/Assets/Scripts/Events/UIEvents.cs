using System;
using UnityEngine;
public static class UIEvents
{
    static public Action<CanvasGroup> OnHUDDisplay;
    static public Action<CanvasGroup> OnHUDHide;

    static public Action<CanvasGroup> OnMenuOpened;
    static public Action OnStoreMenuOpened;

    static public Action<CanvasGroup> OnMenuClosed;
    static public Action OnStoreMenuClosed;
}

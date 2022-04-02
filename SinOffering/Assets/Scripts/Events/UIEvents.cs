using System;
public static class UIEvents
{
    static public Action OnHUDDisplay;
    static public Action OnHUDHide;

    static public Action<int> OnMenuOpened;
    static public Action<int> OnMenuClosed;
}

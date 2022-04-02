using System;
public static class GameEvents
{
    static public Action<float> OnDamageEvent;
    static public Action OnKilledEvent;
    static public Action<float> OnManaUpdateEvent;
    static public Action<int> OnCurrencyUpdateEvent;
    static public Action<int> OnFaithUpdateEvent;
}

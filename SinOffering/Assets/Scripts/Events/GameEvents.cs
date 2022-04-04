using System;

/// <summary>
/// static class that store generic Game Events. 
/// </summary>

public static class GameEvents
{
    static public Action<float> OnDamageEvent;
    static public Action OnKilledEvent;
    static public Action<float> OnManaUpdateEvent;
    static public Action<int> OnCurrencyUpdateEvent; // OnGetSilver???
    static public Action<int> OnFaithUpdateEvent;

    static public Action OnEnemyKilled;

    static public Action OnAddCameraTarget;
    static public Action OnRemoveCameraTarget;

    static public Action OnSaveGame;
    static public Action OnLoadGame;
}

using System;
using UnitySpriteCutter;

/// <summary>
/// static class that store generic Game Events. 
/// </summary>

public static class GameEvents
{
    public static Action<float> OnDamageEvent;
    public static Action OnKilledEvent;
                  
    public static Action<float> OnManaUpdateEvent;
    public static Action<int> OnCurrencyUpdateEvent; // OnGetSilver???
    public static Action<int> OnFaithUpdateEvent;
                  
    public static Action OnEnemyKilled;
    public static Action<SpriteCutterOutput> OnEnemySliced;
                  
    public static Action OnAddCameraTarget;
    public static Action OnRemoveCameraTarget;
                  
    public static Action OnSaveGame;
    public static Action OnLoadGame;
}

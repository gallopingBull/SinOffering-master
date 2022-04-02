using System;
using UnityEngine;
/// <summary>
/// Utility function that's used by non-monobehavior objects that need to
/// use monobehavior callback methods.
/// </summary>
public class MonobehaviorUtility : MonoBehaviour
{
    [SerializeField]
    private static MonobehaviorUtility _instance;
    private Action OnFixedUpdate;
    public static void AddUpdateCallback(Action updateMethod)
    {
        Debug.Log("AddUpdateCallback invoked");
        if (_instance == null)
            _instance = new GameObject("[Monobehavior Utility]").AddComponent<MonobehaviorUtility>();
        
        _instance.OnFixedUpdate += updateMethod;
    }

    void FixedUpdate() => OnFixedUpdate?.Invoke();
}

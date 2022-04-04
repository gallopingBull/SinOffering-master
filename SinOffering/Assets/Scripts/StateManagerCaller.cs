using UnityEngine;

/// <summary>
/// helper class to StateManager.cs for notifications for animation events
/// </summary>

public class StateManagerCaller : MonoBehaviour
{
    public void CallStateEnter(Entity.State state) =>
        PlayerController.instance.StateManager.EnterState(state);
}

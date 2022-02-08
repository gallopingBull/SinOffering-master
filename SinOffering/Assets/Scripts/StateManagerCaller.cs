/**
 * helper class to StateManager.cs for animation events
 */
using UnityEngine;

public class StateManagerCaller : MonoBehaviour
{
    public void CallStateEnter(Entity.State state)
    {
        PlayerController.instance.StateManager.EnterState(state);
    }
}

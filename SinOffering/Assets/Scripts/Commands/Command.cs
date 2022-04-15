using UnityEngine;

/// <summary>
/// command class base. derived classes will be invoked in InputHandler.cs.
/// </summary>

public abstract class Command : MonoBehaviour
{
    protected PlayerController _pc;

    protected virtual void Start() => 
        _pc = PlayerController.instance;
    public abstract void Execute();
    public abstract void Redo();
}

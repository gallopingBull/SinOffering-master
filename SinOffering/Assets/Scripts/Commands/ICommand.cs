using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICommand : MonoBehaviour{
    protected PlayerController pc;

    public virtual void Execute() { }
    public virtual void Redo() { }
}

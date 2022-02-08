using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ICommand : MonoBehaviour{
    protected PlayerController pc;

    protected virtual void Start()
    {
        pc = PlayerController.instance;
    }
    public virtual void Execute() { }
    public virtual void Redo() { }
}

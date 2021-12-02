using UnityEngine;

[System.Serializable]
public class GameModeAttributes : ScriptableObject
{
    // game mode
    // rules 
    // objective
    // match time
 
    public GameModeModifier GameModeModifier;

    virtual public bool CheckGameStatus() { return false; }
}

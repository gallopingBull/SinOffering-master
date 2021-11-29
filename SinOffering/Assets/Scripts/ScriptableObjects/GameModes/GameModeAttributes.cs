using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//#region testing


public interface IGameModeData<T>
{
    public void CheckGameStatus();
    T GetData();
}
[System.Serializable]
public class GameModeAttributes : ScriptableObject
{
    // game mode
    // rules 
    // objective
    // match time

    //public int MaxPoints = 10;
    //public bool SpawnCrates = false;
    // crate spawn  locations
    //public Transform[] spawnLocs;
    //private int lastSpawnLoc;
    //public GameObject Crate;
    virtual public bool CheckGameStatus() { return false;}

}


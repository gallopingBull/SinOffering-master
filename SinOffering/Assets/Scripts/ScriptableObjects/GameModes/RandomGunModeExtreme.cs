using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "RandomGunExtremeMode", menuName = "GameModes/GameMode_RandomGunsExtreme")]
public class RandomGunModeExtreme: GameModeAttributes
{
    public int MaxPoints = 10;
    public bool SpawnCrates = false;
    // crate spawn  locations
    public Transform[] spawnLocs;
    private int lastSpawnLoc;
    public GameObject Crates;

    public override bool CheckGameStatus()  
    {
        bool completed = false;

        Debug.Log("gameMode: ");
        return completed;
        //base.CheckGameStatus();
    }
}
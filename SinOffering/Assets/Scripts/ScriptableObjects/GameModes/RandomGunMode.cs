using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "RandomGunMode", menuName = "GameModes/GameMode_RandomGuns")]
public class RandomGunMode : GameModeAttributes
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


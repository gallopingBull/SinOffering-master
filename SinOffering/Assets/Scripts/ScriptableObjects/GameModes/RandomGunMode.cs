using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "RandomGunMode", menuName = "GameModes/GameMode_RandomGuns")]
public class RandomGunMode : GameModeAttributes
{
    public int MaxPoints = 10;
    public bool SpawnCrates = true;
    
    public Transform[] spawnLocs; // crate spawn locations.

    public GameObject Crates;

    public override bool CheckGameStatus()
    {
        bool completed = false;
        Debug.Log("gameMode: ");
        return completed;
    }
}


using UnityEngine;

[CreateAssetMenu(fileName = "SurvivalMode", menuName = "GameModes/GameMode_Survival")]
public class SurvivalMode : GameModeAttributes  
{
    public int WaveCount = 10;
    //public bool SpawnCrates = true;
    
    public override bool CheckGameStatus()
    {
        bool completed = false;
        Debug.Log("gameMode: ");
        return completed;
    }
}

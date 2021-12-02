using UnityEngine;

[CreateAssetMenu(fileName = "HighScoreMode", menuName = "GameModes/GameMode_HighScore")]
public class HighScoreMode : GameModeAttributes
{
    public override bool CheckGameStatus()
    {
        bool completed = false;
        Debug.Log("gameMode: ");
        return completed;
    }
}

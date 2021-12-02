using UnityEngine;

[CreateAssetMenu(fileName = "TimeAttackMode", menuName = "GameModes/GameMode_TimeAttack")]
public class TimeAttackMode : GameModeAttributes
{
    public float MatchTime = 10f;
    public override bool CheckGameStatus()
    {
        bool completed = false;
        Debug.Log("gameMode: ");
        return completed;
    }
}

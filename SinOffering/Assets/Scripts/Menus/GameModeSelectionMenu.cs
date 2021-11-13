using UnityEngine;

public class GameModeSelectionMenu : MonoBehaviour
{
    private IGameModeSelectionMenu _client = null;
    [SerializeField]
    private GameMode _gameMode;

    public void SetClient(IGameModeSelectionMenu client)
    {
        _client = client;
    }
    public void SendGameMode()
    {
        _client.SetGameMode(_gameMode);
    }

    public void InitMenu()
    {

    }
}

public interface IGameModeSelectionMenu
{
    void SetGameMode(GameMode gameMode);

}

